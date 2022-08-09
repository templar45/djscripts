using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author:Christopher Cruz
public class rocketUse : MonoBehaviour
{
     public bool isAiming = false;
    public bool isShooting;
    public Camera mainCamera;
    public GameObject rocket;
    public Transform attackpoint;
    public float rocketForce;
    public float spread;

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            fireRocket();
        }
    }

    void fireRocket()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint;
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(500);
        }

        Vector3 directionWithoutSpread = targetPoint - attackpoint.position;

        if(!isAiming)
        {
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

            GameObject currentrocket = Instantiate(rocket, attackpoint.position, Quaternion.identity);
            currentrocket.transform.forward = directionWithSpread.normalized;

            currentrocket.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * rocketForce, ForceMode.Impulse);
        }
        else
        {
            GameObject currentrocket = Instantiate(rocket, attackpoint.position, Quaternion.identity);
            currentrocket.transform.forward = directionWithoutSpread.normalized;

            currentrocket.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * rocketForce, ForceMode.Impulse);
        }
    }
}
