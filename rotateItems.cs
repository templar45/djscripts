using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Christopher Cruz
public class rotateItems : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up* speed * Time.deltaTime);
    }
}
