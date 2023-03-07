using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public GameObject parentObj;
    public float scrollSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float scroollWheel = Input.GetAxis("Mouse ScrollWheel");
        //float scroollWheel = Input.GetAxis("Vertical");

        Vector3 cameraDirection = this.transform.localRotation * Vector3.forward;
        
        this.transform.position += scrollSpeed * scroollWheel * Time.deltaTime * cameraDirection;
    }
}
