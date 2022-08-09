using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author:Christopher Cruz and Luke Sapir
public class weaponPickup : Pickup
{
    public int weaponID;
    [HideInInspector]
    public SwitchWeapon mySwitchWeapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override bool PickupActivate(GameObject myPlayer)
    {
        //print("Picked up Gun");
        mySwitchWeapon = myPlayer.GetComponentInChildren<SwitchWeapon>();
        return mySwitchWeapon.AddWeapon(weaponID, gameObject);
    }

}
