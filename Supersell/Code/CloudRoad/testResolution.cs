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
        Application.Quit(); // ���ø����̼� ����
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

        /*  float mouseX = Input.GetAxis("Mouse X");                        ȸ���ڵ�
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

        /* Vector3 cameraDirection = this.transform.localRotation * Vector3.back;           // ī�޶� Ʈ������ ��ü�̵��ڵ�

         this.transform.position += scrollSpeed * scroollWheel * Time.deltaTime * cameraDirection; */
    }
}
