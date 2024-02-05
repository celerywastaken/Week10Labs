using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastShoot : MonoBehaviour
{
    public int gunDamage = 1; //gunDamage will determine how much damage is applied to an object when it is shot.
                              //
    public float fireRate = .25f; //fireRate will control how often the player can fire their weapon.
    public float weaponRange = 50f; //determines how far our ray will be cast into the scene. 
    public float hitForce = 100f; // applies Force to Rigidbodys it hits.

    public Transform gunEnd; //will mark the position at which our laser line will begin
    private Camera fpsCam; // will use this to determine the position the player is aiming from.

    private WaitForSeconds shotDuration = new WaitForSeconds(.07f); //determine how long we want the laser to remain visible in the game view.
    private AudioSource gunAudio; // will use to play our shooting sound effect.
    private LineRenderer laserLine; //The  LineRenderer  takes an array of 2 or more points in 3D space and draws a straight line between them in the game view.

    private float nextFire; //will hold the time at which the player will be allowed to fire again after firing.

    // Start is called before the first frame update
    void Start()
    {
        laserLine = GetComponent<LineRenderer>(); //Get and store a reference to our LineRenderer component in our variable laser line.
        gunAudio = GetComponent<AudioSource>(); //Get and store a reference to our AudioSource in gunAudio.
        fpsCam = GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //check if the Fire1 button is currently held down and Time.time is greater than the value stored in our nextFire variable.
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire) 
        { 
            StartCoroutine(ShotEffect());
            nextFire = Time.time + fireRate; 
        }
        // if statement evaluates to true then we will reset our nextFire time by setting nextFire to equal Time.time plus fireRate.

        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));

        RaycastHit hit;
        laserLine.SetPosition(0,gunEnd.position);

        //using Physics.Raycast to cast our Ray.
        //Physics.Raycast returns a boolean, meaning that if it hits something it will evaluate to true.

        //rayOrigin. This will be the point in our world where our ray will begin.

        //the direction which we will cast our ray in will be the forward direction of our camera fpsCam.

        // We will store additional information about the object that we hit including its rigidbody,
        // collider and surface normal in our RaycastHit variable hit.

        //Finally we will pass in weaponRange which is the distance over which we want to cast our ray, in this case 50 units.
        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
        {
            laserLine.SetPosition(1, hit.point);
        }
        else
        {
            laserLine.SetPosition(1, fpsCam.transform.forward * weaponRange);
        }
    }

    private IEnumerator ShotEffect()
    {
        gunAudio.Play();
        laserLine.enabled = true;
        yield return shotDuration; //will cause our coroutine to wait for .07 seconds.
        laserLine.enabled = false; //Once it's done waiting, disable the laserLine.
    }



}
