using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiveScript : MonoBehaviour
{
    public GameObject bee;
    public Transform spawnPoint;
    public float spawnTime = 3000;

    [SerializeField] private Image loader;

    public float i = 1;

    void Update()
    {
        loader.fillAmount = i / spawnTime;
        Debug.Log(i / spawnTime * 100);

        if (!PauseMenuScript.gameIsPaused)
        {
            if (i >= spawnTime)
            {
                InstantiateBee(spawnPoint);
                i = 0;
            }
            i++;
        }
        
    }

    private void InstantiateBee(Transform spawnPoint) //create bee at spawnpoint
    {
        var projectileObj = Instantiate(bee, spawnPoint.position, Quaternion.identity) as GameObject;
    }
}
