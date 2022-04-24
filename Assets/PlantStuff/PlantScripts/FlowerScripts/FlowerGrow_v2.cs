using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class FlowerGrow_v2 : MonoBehaviour
{
    public GameObject flowerPart;
    public GameObject flowerBud;
    public GameObject flowerPetal;
    public GameObject flowerThorn;
    public GameObject[] flowerLeaf;
    public float growSpeed = 1;
    public float incrementSize = 0.05f;
    public float size = 1;
    public float pointRadiusMultiplier = 1;
    public float minPointDistance = 1f;
    public float maxPointDistance = 2f;
    public int petalCount = 40;
    public float petalSpread = 60;
    public int thornProbabilityDivisor = 100;
    public int leafProbabilityDivisor = 100;

    float time = 0;
    float growth = 0;
    int pointCount = 4;
    GameObject flowerGO;
    SplineComputer flowerSC;
    int budCount = 4;
    GameObject[] buds;
    GameObject[] petals;
    bool ready = false;
    float[] leanRatio;

    Quaternion rotation;

    public void spawnFlower(Vector3 coords)
    {
        flowerGO = Instantiate(flowerPart); // change back later
        flowerSC = flowerGO.GetComponent<SplineComputer>();

        buds = new GameObject[budCount];
        petals = new GameObject[petalCount];
        leanRatio = new float[petalCount];

        SplinePoint[] points = flowerSC.GetPoints();
        Vector3 position = coords;
        float distance = 0;

        flowerPetal.GetComponent<MeshRenderer>().material = Resources.Load("RoseColors/Rose" + RandomRoseColor(), typeof(Material)) as Material;

        for (int i = 0; i < pointCount; i++)
        {
            points[i].position = position;
            Debug.Log(position);
            points[i].normal = Vector3.forward;

            distance += Random.Range(minPointDistance, maxPointDistance);
            position = coords + Random.insideUnitSphere * pointRadiusMultiplier;
            position.y = distance + coords.y;
        }

        flowerSC.SetPoints(points);

        for (int b = 0; b < budCount; b++)
        {
            buds[b] = Instantiate(flowerBud, coords, flowerBud.transform.rotation * Quaternion.AngleAxis(b * 360 / budCount, Vector3.up));
            buds[b].transform.localScale *= 0.3f;
        }

        for (int p = 0; p < petalCount; p++)
        {
            leanRatio[p] = Random.Range(1, 10) / 10f;

            petals[p] = Instantiate(flowerPetal, coords - new Vector3(0, 0.3f, 0), flowerPetal.transform.rotation * Quaternion.AngleAxis(p * 360 / petalCount, Vector3.up));
            petals[p].transform.localScale *= 0;
        }

        ready = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - time > 1 / growSpeed && growth < 1.0 - incrementSize && ready)
        {
            time = Time.time;
            growth += incrementSize;

            SplineMesh flowerSM = flowerGO.GetComponent<SplineMesh>();
            flowerSM.GetChannel(0).clipTo = growth;

            rotation = flowerSC.Evaluate(growth).rotation;

            for (int b = 0; b < budCount; b++)
            {
                buds[b].transform.position = flowerSC.EvaluatePosition(growth);
                buds[b].transform.rotation = rotation;
                buds[b].transform.Rotate(0, 0, b * 360 / budCount);
                buds[b].transform.Rotate(90 - 75 * growth, 0, 0);
            }

            for (int p = 0; p < petalCount; p++)
            {
                petals[p].transform.position = flowerSC.EvaluatePosition(growth) - rotation * Vector3.forward * 0.3f;
                petals[p].transform.rotation = rotation;
                petals[p].transform.Rotate(0, 0, p * 360 / petalCount);
                petals[p].transform.Rotate(90 - petalSpread * leanRatio[p] * growth, 0, 0);
                petals[p].transform.localScale = new Vector3(2f * leanRatio[p], 1f, 1.5f * leanRatio[p]) * growth / 3.5f;
            }

            if(Random.Range(0, thornProbabilityDivisor) == 0)
            {
                SpawnThorn(flowerSC.EvaluatePosition(growth), rotation, flowerSC.Evaluate(growth).size);

                //float thornSizeMiltiplier = Random.Range(0.5f, 1.5f);
                //Quaternion thornRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
                //Vector3 thornPosition = flowerSC.EvaluatePosition(growth) + rotation * thornRotation * Vector3.up * - flowerSC.Evaluate(growth).size / 10; //-0.35f * (1f - Mathf.Pow(growth, 2.5f))

                //var thorn = Instantiate(flowerThorn, thornPosition, rotation * thornRotation);
                //thorn.transform.localScale = thorn.transform.localScale * size * thornSizeMiltiplier / 8;
                //thorn.transform.Rotate(90, 0, 90);
            }

            if(Random.Range(0, leafProbabilityDivisor) == 0 && 0.1f < growth && growth < 0.8f)
            {
                SpawnLeaf(flowerSC.EvaluatePosition(growth), rotation, flowerSC.Evaluate(growth).size);

                //float leafSizeMiltiplier = Random.Range(0.5f, 1.5f);
                //Quaternion leafRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
                //Vector3 leafPosition = flowerSC.EvaluatePosition(growth) + rotation * leafRotation * Vector3.up * -flowerSC.Evaluate(growth).size / 10; //-0.35f * (1f - Mathf.Pow(growth, 2.5f))

                //var leaf = Instantiate(flowerLeaf[Random.Range(0, flowerLeaf.Length)], leafPosition, rotation * leafRotation);
                //leaf.transform.localScale = leaf.transform.localScale * size * leafSizeMiltiplier / 5;
                //leaf.transform.Rotate(-180, 0, 0);
                //leaf.transform.Rotate(0, -180, 0);
                //leaf.transform.Rotate(0, 0, 90);
            }
        }
    }

    string RandomRoseColor()
    {
        int colorNumber = Random.Range(0, 13125);
        string color;

        Debug.Log(colorNumber);

        if (colorNumber < 5000)
            color = "Red";
        else if (colorNumber < 10000)
            if (Random.Range(0, 2) == 0)
                color = "White";
            else
                color = "Yellow";
        else if (colorNumber < 11250)
            color = "Pink";
        else if (colorNumber < 12500)
            if (Random.Range(0, 2) == 0)
                color = "Purple";
            else
                color = "Orange";
        else
            color = "Blue";

        return color;
    }

    void SpawnThorn(Vector3 position, Quaternion rotation, float stemSize)
    {
        float thornSizeMiltiplier = Random.Range(0.5f, 1.5f);
        Quaternion thornRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
        Vector3 thornPosition = position + rotation * thornRotation * Vector3.up * -stemSize / 10;

        var thorn = Instantiate(flowerThorn, thornPosition, rotation * thornRotation);
        thorn.transform.localScale = thorn.transform.localScale * size * thornSizeMiltiplier / 8;
        thorn.transform.Rotate(90, 0, 90);
    }

    void SpawnLeaf(Vector3 position, Quaternion rotation, float stemSize)
    {
        float leafSizeMiltiplier = Random.Range(0.5f, 1.5f);
        Quaternion leafRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
        Vector3 leafPosition = position + rotation * leafRotation * Vector3.up * -stemSize / 10;

        var leaf = Instantiate(flowerLeaf[Random.Range(0, flowerLeaf.Length)], leafPosition, rotation * leafRotation);
        leaf.transform.localScale = leaf.transform.localScale * size * leafSizeMiltiplier / 5;
        leaf.transform.Rotate(-180, 0, 0);
        leaf.transform.Rotate(0, -180, 0);
        leaf.transform.Rotate(0, 0, 90);
    }
}
