using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Debug;

public class testResolution : MonoBehaviour
{    
    void Start()
    {
        // 3360*1199
        Screen.SetResolution(3360, 1199, true);
        // Screen.fullScreen = !Screen.fullScreen;        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif   
        }
    }

    private void FixedUpdate()
    {        
        MoveObj();
        Zoominout();
    }


    void MoveObj()
    {
        float keyH = Input.GetAxis("Horizontal");
        float keyV = Input.GetAxis("Vertical");
        keyH = keyH * Utils.move_Speed * Time.deltaTime;
        keyV = keyV * Utils.move_Speed * Time.deltaTime;
        transform.Translate(Vector3.right * keyH);
        transform.Translate(Vector3.up * keyV);

        if (transform.position.x <= Utils.minPosX)
        {
            transform.position = new Vector3(Utils.minPosX, transform.position.y, transform.position.z);
        }
        if (transform.position.x >= Utils.maxPosX)
        {
            transform.position = new Vector3(Utils.maxPosX, transform.position.y, transform.position.z);
        }

        if (transform.position.z <= Utils.minPosZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Utils.minPosZ);
        }
        if (transform.position.z >= Utils.maxPosZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Utils.maxPosZ);
        }

        /*  float mouseX = Input.GetAxis("Mouse X");                        회전코드
          float mouseY = Input.GetAxis("Mouse Y");
          transform.Rotate(Vector3.up * speed_rota * mouseX);
          transform.Rotate(Vector3.left * speed_rota * mouseY);*/
    }

    void Zoominout()
    {
        float scroollWheel = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.fieldOfView += scroollWheel * Time.deltaTime * Utils.scroll_Speed;

        if (Camera.main.fieldOfView >= Utils.maxZoom)
        {
            Camera.main.fieldOfView = Utils.maxZoom;
        }
        if (Camera.main.fieldOfView <= Utils.minZoom)
        {
            Camera.main.fieldOfView = Utils.minZoom;
        }

        /* Vector3 cameraDirection = this.transform.localRotation * Vector3.back;           // 카메라 트랜스폼 자체이동코드

         this.transform.position += scrollSpeed * scroollWheel * Time.deltaTime * cameraDirection; */
    }
}
