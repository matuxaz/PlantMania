using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSeed : MonoBehaviour
{
    public Vector3 spawnCoords = Vector3.zero;

    public void GrowFlower()
    {
        gameObject.GetComponent<FlowerGrow_v2>().spawnFlower(spawnCoords);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
