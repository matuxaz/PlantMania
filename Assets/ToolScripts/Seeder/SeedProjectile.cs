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
        Flower
    };

    public GameObject palmSeed;
    public GameObject flowerSeed;
    public GameObject vineSeed;

    void Start()
    {
        Destroy(gameObject, 2);
    }
    private bool collided;
    private void OnCollisionEnter(Collision c)
    {

        if (c.gameObject.tag != "Bullet" && !collided && c.gameObject.tag != "Enemy")
        {
            collided = true;
            Destroy(gameObject, 0.5f);

            // SAPWNS ROSE
            if (plantType.ToString() == "Flower")
            {
                SpawnFlower(c);
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
        if (c.gameObject.tag == "Enemy" && !collided)
        {
            collided = true;
            Destroy(gameObject, 0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !collided)
        {
            collided = true;
            Destroy(gameObject, 0.1f);
        }
    }

    void SpawnFlower(Collision collision)
    {
        Instantiate(flowerSeed, collision.contacts[0].point, Quaternion.identity);
    }

    void SpawnPalm(Collision collision)
    {
        GameObject palmSeedUnit = Instantiate(palmSeed, collision.contacts[0].point, Quaternion.identity);
        palmSeedUnit.GetComponent<PalmSeed>().Plant();
    }

    void SpawnVine(Collision collision)
    {
        GameObject vineSeedUnit = Instantiate(vineSeed, collision.contacts[0].point, Quaternion.identity);
        vineSeedUnit.GetComponent<VineSeed>().SpawnVine(collision);
    }

    static Vector3 RandomVectorYPlane(float offset)
    {
        return new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset));
    }
}
