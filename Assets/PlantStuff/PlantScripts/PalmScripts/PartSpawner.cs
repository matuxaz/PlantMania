using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSpawner : MonoBehaviour
{
    public GameObject part;
    public float maxAngle = 12.5f;
    public int maxPartCount = 14;
    public float subsequentScaleRatio = 0.95f;
    public float upwardsBias = 0.925f;
    public float chaosFactor = 175f;

    private void Start()
    {

    }

    public void SpawnNextPart(int gen)
    {

        if (gen < maxPartCount)
        {

            Vector3 newScale = transform.lossyScale * subsequentScaleRatio;
            Vector3 newPos = transform.position + transform.rotation * Vector3.up * newScale.x * 0.013f;
            Quaternion newRot = CalculateRotation(gen+1);

            GameObject newPart = Instantiate(part, newPos, newRot);

            newPart.transform.localScale = newScale;

            newPart.GetComponent<PartSpawner>().SpawnNextPart(++gen);

        }
        else
        {

            gameObject.GetComponent<OtherSpawner>().StartSpawnOther(gameObject.transform);

        }

    }

    Quaternion CalculateRotation(int gen)
    {

        float currUpwardsBias = Mathf.Pow(upwardsBias, gen) * chaosFactor;

        Quaternion rotation = transform.rotation * Quaternion.Euler(RandomVector(maxAngle));
        rotation = Quaternion.Euler(new Vector3(rotation.x * currUpwardsBias, rotation.y * currUpwardsBias, rotation.z * currUpwardsBias));

        return rotation;
    }

    static Vector3 RandomVector(float offset)
    {
        return new Vector3(Random.Range(-offset, offset), Random.Range(-offset, offset), Random.Range(-offset, offset));

    }

}
