using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csCCMove : MonoBehaviour
{
    public float movSpeed = 50f;
    public float rotSpeed = 120f;
    public float jumpSpeed = 10f;
    public float gravity = 20f;
    CharacterController controller;
    Vector3 moveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (controller.isGrounded)
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            h = h * rotSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up * h);

            moveDirection = new Vector3(0, 0, v * movSpeed);
            // local -> world(global)방향으로 변환
            // CharacterController가 world를 사용함
            moveDirection = transform.TransformDirection(moveDirection);

            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
