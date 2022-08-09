using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class elevator : MonoBehaviour
{
    public GameObject[] wayPoints;
    public shakeEffect newWaypoints;
    public UnityEvent playElevator;
    public UnityEvent stopSound;
    public UnityEvent shakeSound;
    public float speed;
    public int currentPoint = 0;
    float rotSpeed;
    public float WPradius = 1;
    public int RotationSpeed;
    public bool distantionReached;
    public bool triggerActivate = false;
    public bool elevatorActivate = false;
   
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(triggerActivate || newWaypoints.dropNow)
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
                    if(!newWaypoints.dropNow)
                    {
                        ResetTrigger();
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, wayPoints[currentPoint].transform.position, Time.deltaTime * speed);
                }
            }
        }
    }

    void ResetTrigger()
    {
        newWaypoints = GameObject.Find("dropWaypoints").GetComponent<shakeEffect>();
        stopSound.Invoke();
        shakeSound.Invoke();

        wayPoints = newWaypoints.dropWaypoints;
        newWaypoints.trigger.enabled = true;

        triggerActivate = false;
        distantionReached = false;
        currentPoint = 0;
        speed = 20f;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Player")
        {
            if(!elevatorActivate)
            {
                playElevator.Invoke();
                triggerActivate = true;
                elevatorActivate = true;
            }
        }
    }
}
