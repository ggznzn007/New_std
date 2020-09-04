using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Billboard : MonoBehaviour
{
    Transform camTr;
    Transform tr;
    void Start()
    {
        camTr = GameObject.Find("ARCamera").GetComponent<Transform>();
        tr = GetComponent<Transform>();
    }
    void Update()
    {
        tr.LookAt(camTr.position);
    }
}
