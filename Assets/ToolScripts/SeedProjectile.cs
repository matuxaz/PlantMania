using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedProjectile : MonoBehaviour
{
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

            gameObject.GetComponent<FlowerGrow_v2>().spawnFlower(c.contacts[0].point);
        }
    }
}
