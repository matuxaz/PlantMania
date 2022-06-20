using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineSeed : MonoBehaviour
{
    public int vinePointCount = 100;
    public int vineMaxBranches = 5;
    public int vineMinBranches = 3;

    public void SpawnVine(Vector3 point, Vector3 normal)
    {
        Debug.Log("at spawn vine");

        int branchCount = Random.Range(vineMinBranches, vineMaxBranches + 1);
        float angle;
        Vector3 direction;
        Vector3 spawnPoint;

        for (int i = 0; i <= branchCount; i++)
        {
            angle = Random.Range(0, 360);
            direction = Quaternion.AngleAxis(90, new Vector3(1, 0, 1)) * normal;
            direction = Quaternion.AngleAxis(angle, normal) * direction;
            spawnPoint = point + normal* 0.5f;
            StartCoroutine(gameObject.GetComponent<Brancher_v3>().SpawnNextPart(spawnPoint, normal, vinePointCount, direction));
        }
    }

    private void Update()
    {
        
    }

}
