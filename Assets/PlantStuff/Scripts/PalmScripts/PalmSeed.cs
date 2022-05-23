using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmSeed : MonoBehaviour
{

    public GameObject part;
    public float maxAngle = 12.5f;
    public int leafMaxAngle = 30;
    public int maxPartCount = 14;
    public float growRate = 0.1f;
    public int waterRequired = 10;

    float startTime;
    int partCount = 1;

    int waterLevel = 0;

    GameObject nextPart;

    LeafSpawner leaf;
    Transform leaves;

    CoconutSpawner coconut;
    Transform coconuts;

    Transform parts;

    public void Plant()
    {

        Quaternion newRot = transform.rotation * Quaternion.Euler(RandomVectorYPlane(maxAngle));

        parts = gameObject.transform.Find("Parts");
        nextPart = Instantiate(part, gameObject.transform.position, newRot, parts);
        nextPart.GetComponent<PartSpawner_v2>().Initiate(0);

        leaves = gameObject.transform.Find("Leaves");
        leaf = leaves.GetComponent<LeafSpawner>();
        leaf.SpawnLeaf(gameObject.transform);
        leaves.transform.localScale = Vector3.one * (partCount + 10) / (maxPartCount + 10);

        coconuts = gameObject.transform.Find("Coconuts");
        coconut = coconuts.GetComponent<CoconutSpawner>();
    }

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {

        if (partCount >= maxPartCount)
        {
            coconut.SpawnCoconuts(nextPart.transform);

            Destroy(this);
        }
        else if (waterLevel > waterRequired)
        {
            nextPart = nextPart.GetComponent<PartSpawner_v2>().SpawnNextPart();

            Vector3 leavesOffset = nextPart.transform.up * -(1 - nextPart.transform.localScale.x / 100) * 2.5f;

            leaves.position = nextPart.transform.position + leavesOffset;
            leaves.rotation = nextPart.transform.rotation;
            leaves.localScale = Vector3.one * (partCount + 10) / (maxPartCount + 10);

            partCount++;
            waterLevel = 0;
        }
    }

    static Vector3 RandomVectorYPlane(float offset)
    {
        return new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset));
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Water")
        {
            waterLevel += 2;
        }
    }
}
