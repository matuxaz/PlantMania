using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Seeder : MonoBehaviour
{
    public Camera cam;
    public GameObject seed;
    public Transform firePoint;

    public float speed = 50;
    public float fireRate = 15;

    [SerializeField] public Text ammoCounter;
    public float ammo = 60;

    private float timeToFire;
    private Vector3 destination;
    public GameObject muzzleVfx;

    private void Update()
    {

        ammoCounter.text = ammo.ToString();
        if (Input.GetButton("Fire1") && Time.time >= timeToFire && ammo > 0 && !PauseMenuScript.gameIsPaused)
        {
            timeToFire = Time.time + 1 / fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) //find the crosshair position
        {
            destination = hit.point;
        }
        else
        {
            destination = ray.GetPoint(1000);
        }
        InstantiateSeed(firePoint);
        ammo -= 1;
        GameObject mVfx = Instantiate(muzzleVfx, firePoint.position, firePoint.rotation); //muzzle effect
        Destroy(mVfx, 1);
    }

    private void InstantiateSeed(Transform firePoint) //create a projectile at firepoint
    {
        var projectileObj = Instantiate(seed, firePoint.position, Quaternion.identity) as GameObject;
        projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * speed;
    }
}
