using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class specialRotate : MonoBehaviour
{
    public float speed;
    void Update()
    {
        transform.Rotate(Vector3.left * speed * Time.deltaTime);
    }
}
