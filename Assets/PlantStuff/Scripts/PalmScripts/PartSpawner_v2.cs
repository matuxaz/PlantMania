using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSpawner_v2 : MonoBehaviour
{
    public GameObject part;
    public float maxAngle = 12.5f;
    public float subsequentScaleRatio = 0.95f;
    public float upwardsBias = 0.925f;
    public float chaosFactor = 175f;
    public float partPopInDuration = 0.25f;
    public float size = 100f;

    [SerializeField]
    private AnimationCurve growthCurve;

    float startTime;
    [HideInInspector]
    public int gen;
    Vector3 scaleTarget;

    private void Start()
    {
        startTime = Time.time;
        gameObject.transform.localScale = Vector3.zero;
    }

    public void Initiate(int gen)
    {
        this.gen = gen;
        scaleTarget = Mathf.Pow(subsequentScaleRatio, gen) * size * Vector3.one;
    }

    public GameObject SpawnNextPart()
    {

        Vector3 newPos = transform.position + transform.rotation * Vector3.up * scaleTarget.x * subsequentScaleRatio * 0.013f;
        Quaternion newRot = CalculateRotation(gen + 1);

        GameObject nextPart = Instantiate(part, newPos, newRot, gameObject.transform.parent);
        nextPart.GetComponent<PartSpawner_v2>().Initiate(++gen);

        Destroy(this, 1.0f);

        return nextPart;

    }

    private void Update()
    {
        Grow();
    }

    void Grow()
    {
        float stage = (Time.time - startTime) / partPopInDuration;

        gameObject.transform.localScale = scaleTarget * growthCurve.Evaluate(stage);
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
