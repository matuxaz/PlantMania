using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveScript : MonoBehaviour
{
    public GameObject bee;
    public Transform spawnPoint;
    public float spawnTime = 1000;

    public float i = 0;

    void Update()
    {
       
        if(i >= spawnTime)
        {
            InstantiateBee(spawnPoint);
            i = 0;
        }
        i++;
    }

    private void InstantiateBee(Transform spawnPoint) //create bee at spawnpoint
    {
        var projectileObj = Instantiate(bee, spawnPoint.position, Quaternion.identity) as GameObject;
    }
}
