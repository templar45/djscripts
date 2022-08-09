using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponStats : MonoBehaviour
{
    //Author: Luke Sapir
    //Edited:Christopher Cruz
    //Generic Weapon class, good for specific weapons to inherit from.
    public int magazineAmmo;
    public int ammoStorage;
    public int ammoStorageMax;
    public int maxAmmo;
    public float rateOfFire;
    public float nextFire = 0f;
    public float reloadTime;
    public bool isReloading;
    public bool isShooting;
    public bool isADS;
    public UnityEvent fireEvent;
    public UnityEvent reloadEvent;
    public UnityEvent AmmoPickupEvent;
    public UnityEvent weaponDraw;
    public UnityEvent adsGun;
    public UnityEvent AdsFire;
    public UnityEvent idleGun;
    public bool unlocked;

    public void Update()
    {
        if(Input.GetButtonDown("Fire1") && Time.time > nextFire && magazineAmmo > 0 && !isReloading && Time.timeScale > 0)
        {
            nextFire = Time.time + rateOfFire;
            isShooting = true;
            if(isShooting)
            {
                //Debug.Log("shot");
                Shoot();
                isShooting = false;
            }
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
        

        //Editor note by Chris: Added conditon to disable players to reload the gun even when current clip is max
        if(Input.GetKeyDown(KeyCode.R) && !isReloading && !isShooting && magazineAmmo != maxAmmo && ammoStorage > 0 && Time.timeScale > 0)
        {
            StartCoroutine(ReloadGun());
        }
    }
    
    public virtual void Shoot()
    {
        //fireEvent.Invoke();

    }

    public void AimDownSight()
    {
        adsGun.Invoke();
    }
    public IEnumerator ReloadGun()
    {
        isReloading = true;
        reloadEvent.Invoke();
        //Debug.Log("Reloading");

       

        if(ammoStorage > 0 && magazineAmmo < maxAmmo)
        {
            if (ammoStorage < maxAmmo - magazineAmmo)
            {
                yield return new WaitForSeconds(reloadTime);
                magazineAmmo += ammoStorage;
                ammoStorage = 0;

            }
            else
            {
                yield return new WaitForSeconds(reloadTime);
                ammoStorage -= maxAmmo - magazineAmmo;
                magazineAmmo = maxAmmo;
            }
        }
      
        isReloading = false;
    }
    //This is to add ammo to the ammo storage, not reloading.
    public bool AddAmmo(int amount, GameObject collidedObject)
    {
        if(ammoStorage >=ammoStorageMax)
        {
          return false;
        }
        if (ammoStorage < ammoStorageMax)
        {
           
            AmmoPickupEvent.Invoke();
            if(ammoStorage + amount > ammoStorageMax)
            {
                ammoStorage = ammoStorageMax;
            }
            else
            {
                ammoStorage += amount;
            }
            return true;
        }
        return false;
    }
}
