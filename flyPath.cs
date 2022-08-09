using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Christopher Cruz
// reads an array of waypoints for the gameobject to follow and as well face when moving
// Also turns off UI in main menu when player clicks begin.
public class flyPath : MonoBehaviour
{
    public GameObject[] wayPoints;
    public float speed;
    public int currentPoint = 0;
    float rotSpeed;
    public float WPradius = 1;
    public int RotationSpeed;
    public bool distantionReached;
    MainMenu menu;
    public GameObject menuObject;

    void Start()
    {
        menu = menuObject.GetComponent<MainMenu>();
    }

    void Update()
    {
        if(menu.playShip)
        {
            menu.enabled = false;
            if(!distantionReached)
            {
                Vector3 relativePos = wayPoints[currentPoint].transform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
            
                if(Vector3.Distance(wayPoints[currentPoint].transform.position, transform.position) < WPradius)
                {
                    currentPoint++;
                }
                if(currentPoint >= wayPoints.Length)
                {
                    distantionReached = true;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, wayPoints[currentPoint].transform.position, Time.deltaTime * speed);
                }
            }
        }
    }
}
