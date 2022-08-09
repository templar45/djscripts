using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author:Christopher Cruz
//Weapon class that fires multiple raycast to similute shotgun pellets and spread
public class ShotgunScript : RayCastWeapon
{
    public Camera sgCamera;
    public float radius = 0.1f;
    private int pelletsPerShot = 6;
    private float sgSpread = 3.5f;
    
    //overrides shoot funciton in weaponStats that Luke taught me[]
    public void Start()
    {
        playerStatsObject = GameObject.Find("Player");
        weaponRange = 50f;
        ignoreMask = ~(ignoreMask);
    }
    public override void Shoot()
    {
        isShooting = true;
        Vector3 direction = sgCamera.transform.forward;
        RaycastHit Hit;

        //if no ammo or is reloading they cannot fire
        if(isReloading)
        {
            //Debug.Log("No, Still Reloading");
            return;
        }
        if(magazineAmmo <= 0)
        {
            //Debug.Log("No SG ammo");
            return;
        }
        fireEvent.Invoke();
        //deduct ammo
        magazineAmmo--;
        //Debug.Log("Fired! Ammo left" +magazineAmmo);
        
        //array that creates a raycast depending on the number of pellets
        for(int i = 0; i < pelletsPerShot; i++ )
        {
            //the raycast calls a vector3 function to get the direction in the parameter
            HitScanLogic(getShotgunSpread());
            if(Physics.SphereCast(sgCamera.transform.position,radius,getShotgunSpread(),out Hit, weaponRange))
            {
                //print ("Objected: " + Hit.transform.gameObject.name);
                //Green gizmo for SG
                Debug.DrawLine(sgCamera.transform.position, Hit.point, Color.green, 0.5f);
            }
        }
    }

    //Vector3 function to get direction
    Vector3 getShotgunSpread()
    {
        Vector3 targetPos = sgCamera.transform.position + sgCamera.transform.forward * weaponRange;

        targetPos = new Vector3(targetPos.x + Random.Range(-sgSpread, sgSpread), targetPos.y + Random.Range(-sgSpread, sgSpread), targetPos.z + Random.Range(-sgSpread, sgSpread));

        Vector3 direction = targetPos - sgCamera.transform.position;
        return direction.normalized;
    }
}
