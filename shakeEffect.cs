using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class shakeEffect : MonoBehaviour
{
    public GameObject[] dropWaypoints;
    public UnityEvent shakeElevator;
    public Collider trigger;
    public bool isTrigger = false;
    public Animator animator;
    public float timerDrop;
    public bool dropNow = false;
    public bool pauseElevator = false;
    public bool notTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isTrigger)
        {
            Invoke("stopElevator", 4.0f);
            if(pauseElevator)
            {
                if(timerDrop > 0f)
                {
                    timerDrop -= 1f * Time.deltaTime;
                    animator.enabled = true;
                    shakeElevator.Invoke();
                }
                else
                {
                    animator.enabled = false;
                    dropNow = true;
                }
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if(!notTriggered)
            {
                isTrigger = true;
                notTriggered = true;
            }
        }
    }
    void stopElevator()
    {
        pauseElevator = true;
    }

}
