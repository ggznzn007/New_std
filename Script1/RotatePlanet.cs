using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlanet : MonoBehaviour
{
    public Transform targetTr;
    public float rotSpeed = 15f;
    Transform tr;
    void Start()
    {
        tr = GetComponent<Transform>();
    }
    void Update()
    {
        tr.RotateAround(targetTr.position, Vector3.up,
                        Time.deltaTime * rotSpeed);
    }
}
