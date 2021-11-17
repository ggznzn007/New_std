using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move3D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 7.0f;
    [SerializeField]
    private float jumpForce = 3.0f;
    private float gravity = -9.8f;
    private Vector3 moveDir;

    [SerializeField]
   // private Transform cameraTransform;
    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if (characterController.isGrounded==false)
        {
            moveDir.y += gravity * Time.deltaTime;
        }
       characterController.Move(moveDir * moveSpeed * Time.deltaTime);
    }

    public void MoveTo(Vector3 dir)
    {
        moveDir = new Vector3(dir.x, moveDir.y, dir.z);
      //  Vector3 movedis = cameraTransform.rotation * dir;
      //  moveDir = new Vector3(movedis.x, moveDir.y, movedis.z); 
    }

    public void JumpTo()
    {
        if(characterController.isGrounded==true)
        {
            moveDir.y = jumpForce;
        }
    }

}
