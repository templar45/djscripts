using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class flyAway : MonoBehaviour
{
    public GameObject[] wayPoints;
    public float speed;
    public int currentPoint = 0;
    float rotSpeed;
    public float WPradius = 1;
    public int RotationSpeed;
    public bool distantionReached;
    public string sceneName;
    public bool flyShip = false;
    public float timer;
    void Update()
    {
        if(timer > 0f)
        {
            timer -= 1f * Time.deltaTime;
        }
        else
        flyShip = true;
        if(flyShip)
        {
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
                    SceneManager.LoadScene(sceneName);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, wayPoints[currentPoint].transform.position, Time.deltaTime * speed);
                }
            }
        }
    }
}
