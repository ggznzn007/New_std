using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController01 : MonoBehaviour
{
    [SerializeField]
    private KeyCode    jumpKeyCode = KeyCode.Space;
    [SerializeField]
   // private CameraController cameraController;
    private Move3D move3D;

    private void Awake()
    {
        move3D = GetComponent<Move3D>();
     }
  private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        move3D.MoveTo(new Vector3(x, 0, z));

        if(Input.GetKeyDown(jumpKeyCode))
        {
            move3D.JumpTo();            
        }

        //float mouseX = Input.GetAxis("Mouse X"); // 마우스 좌우 움직임
        //float mouseY = Input.GetAxis("Mouse Y"); // 마우스 위아래 움직임

        //cameraController.RotateTo(mouseX, mouseY);
    }
}
