using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedProjectile : MonoBehaviour
{
    public PlantType plantType = new PlantType();

    public enum PlantType
    {
        Palm,
        Vine,
        Rose
    };

    public GameObject palmPart;
    public float palmMaxAngle = 12.5f;
    public int vinePointCount = 100;
    public int vineMaxBranches = 5;
    public int vineMinBranches = 3;

    void Start()
    {
        Destroy(gameObject, 15);
    }
    private bool collided;
    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag != "Bullet" && !collided)
        {
            collided = true;
            Destroy(gameObject, 5);

            // SAPWNS ROSE
            if (plantType.ToString() == "Rose")
            {
                SpawnRose(c);
            }

            // SPAWNS PALM
            else if (plantType.ToString() == "Palm")
            {
                SpawnPalm(c);
            }

            // SPAWNS VINE
            else if (plantType.ToString() == "Vine")
            {
                SpawnVine(c);
            }
        }
    }

    void SpawnRose(Collision collision)
    {
        gameObject.GetComponent<FlowerGrow_v2>().spawnFlower(collision.contacts[0].point);
    }

    void SpawnPalm(Collision collision)
    {
        GameObject newPart = Instantiate(palmPart, collision.contacts[0].point, Quaternion.Euler(RandomVectorYPlane(palmMaxAngle)));
        newPart.GetComponent<PartSpawner>().SpawnNextPart(0);
    }

    void SpawnVine(Collision collision)
    {
        int branchCount = Random.Range(vineMinBranches, vineMaxBranches + 1);
        float angle;
        Vector3 direction;
        Vector3 spawnPoint;

        for (int i = 0; i <= branchCount; i++)
        {
            angle = Random.Range(0, 360);
            direction = Quaternion.AngleAxis(90, new Vector3(1, 0, 1)) * collision.contacts[0].normal;
            direction = Quaternion.AngleAxis(angle, collision.contacts[0].normal) * direction;
            spawnPoint = collision.contacts[0].point + collision.contacts[0].normal * 0.5f;
            gameObject.GetComponent<Brancher_v3>().SpawnNextPart(spawnPoint, collision.contacts[0].normal, vinePointCount, direction);
        }
    }

    static Vector3 RandomVectorYPlane(float offset)
    {
        return new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset));
    }
}
