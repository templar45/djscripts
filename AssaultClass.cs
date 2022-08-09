using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Christopher Cruz
//Weapon Script that can be used on most of the weapons since the only thing that would be
//different is rate of fire, reloadtime, and ammo data which are all controled on WeaponStats[]
public class AssaultClass : RayCastWeapon
{
    public Camera arCamera;
    public float radius = 0.1f;
    public float range =  200f;

    public new void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire && magazineAmmo > 0 && !isReloading)
        {
            nextFire = Time.time + rateOfFire;
            Shoot();
            isShooting = false;
        }
         if(Input.GetButton("Fire2") && !isReloading && !isShooting)
        {
            isADS = true;
            AimDownSight();
            
        }
        
        else if(!Input.GetButton("Fire2"))
        {
            idleGun.Invoke();
            isADS = false;
        }
        //Edit by Chris: Added condition to prevent continueous reload on max 
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && !isShooting && magazineAmmo != maxAmmo && ammoStorage > 0)
        {
            StartCoroutine(ReloadGun());
        }
    }
    //overrides shoot funciton in weaponStats that Luke taught me
    /*  public override void Shoot()
      {
          isShooting = true;
          Vector3 direction = arCamera.transform.forward;
          RaycastHit Hit;

          //if loop that checks players ammo count when shoot shooting and automatically starts reloading when gun is empty
          if (isReloading)
          {
              Debug.Log("No, Still Reloading");
              return;
          }
          if(magazineAmmo <= 0)
          {
              Debug.Log("no ammo dummy");
              //StartCoroutine(ReloadGun());
              return;
          } 

          //deducts ammo here
          magazineAmmo--;

          //Cast Ray from center camera
         if(Physics.SphereCast(arCamera.transform.position,radius,direction,out Hit, range))
          {
              print ("Objected: " + Hit.transform.gameObject.name);
              nextFire = Time.time + rateOfFire;
              // Hit = Blue Line
              Debug.DrawLine(arCamera.transform.position, Hit.point, Color.blue, 0.5f);
          }
       */
}

