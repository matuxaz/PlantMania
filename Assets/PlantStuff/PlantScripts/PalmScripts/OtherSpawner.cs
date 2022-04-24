using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class OtherSpawner : MonoBehaviour
{
    public GameObject leaf;
    public GameObject coconut;
    public int leafOffsetAngle = 30;
    public float posOffset = 0.3f;

    float angleStep;

    // Start is called before the first frame update
    public void StartSpawnOther(Transform trunk)
    {

        angleStep = Random.Range(-10, 10);

        while (angleStep < 360)
        {
            SpawnLeaf(trunk);

            angleStep += leafOffsetAngle + Random.Range(-10, 10);
        }

        int cocoCount = Random.Range(0, 3);

        for(int i = 0; i <= cocoCount; i++)
            SpawnCoconut(trunk);

    }

    void SpawnLeaf(Transform trunk)
    {
        GameObject leafGO = GameObject.Instantiate(leaf);
        leafGO.transform.position = trunk.position + new Vector3(0, 0.5f, 0);

        leafGO.transform.rotation = trunk.rotation * Quaternion.Euler(new Vector3(Random.Range(-15, 15), angleStep, Random.Range(-30, 0)));

        SplineComputer leafSC = leafGO.GetComponent<SplineComputer>();

        SplinePoint[] points = leafSC.GetPoints();

        for (int i = 1; i < points.Length; i++)
        {
            points[i].position += RandomVector(-posOffset, posOffset);
        }

        leafSC.SetPoints(points);

        foreach (Transform child in leafGO.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void SpawnCoconut(Transform trunk)
    {
        Quaternion rotation = trunk.rotation * Quaternion.Euler(RandomVector(0f, 360f));
        float randomAngle = Random.Range(0f, 360f);
        Vector3 position = trunk.position + Quaternion.AngleAxis(randomAngle, trunk.up) * trunk.forward * 0.85f + new Vector3(0f, -0.4f, 0f);

        Instantiate(coconut, position, rotation);
    }

    static Vector3 RandomVector(float min, float max)
    {
        return new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
    }

}
