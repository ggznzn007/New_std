using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csMove : MonoBehaviour
{
    public float movSpeed = 50f;
    public float rotSpeed = 360f;
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        h = h * rotSpeed * Time.deltaTime;
        v = v * movSpeed * Time.deltaTime;

        this.transform.Translate(Vector3.forward * v);
        this.transform.Rotate(Vector3.up * h);
    }
}
