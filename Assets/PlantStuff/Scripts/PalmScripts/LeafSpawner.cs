using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class LeafSpawner : MonoBehaviour
{
    public GameObject leaf;
    public int leafCount = 12;
    public float posOffset = 0.3f;

    public List<GameObject> leaves = new List<GameObject>();

    public void SpawnLeaf(Transform trunk)
    {

        for (float angle = Random.Range(-10, 10); angle < 360; angle += 360 / leafCount + Random.Range(-10, 10))
        {
            leaves.Add(SpawnLeaf(trunk, angle));
        }

    }

    public void UpdateLeaf(Transform info)
    {
        for (int i = 0; i < leaves.Count; i++)
        {
            leaves[i].transform.position = info.position + info.up * 0.5f;
        }
    }

    GameObject SpawnLeaf(Transform trunk, float angle)
    {
        GameObject leafGO = GameObject.Instantiate(leaf, gameObject.transform);
        leafGO.transform.position = trunk.position + trunk.up * 1.75f;

        leafGO.transform.rotation = trunk.rotation * Quaternion.Euler(new Vector3(Random.Range(-15, 15), angle, Random.Range(-30, 0)));

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

        return leafGO;
    }

    static Vector3 RandomVector(float min, float max)
    {
        return new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
    }

}
