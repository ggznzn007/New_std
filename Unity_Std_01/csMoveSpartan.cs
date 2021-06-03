using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csMoveSpartan : MonoBehaviour
{
    public float walkSpeed = 20f;
    public float gravity = 20f;
    public float jumpSpeed = 16f;
    private Vector3 velocity;

    CharacterController controller;
    Animation ani;
    bool isJump = false;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        ani = GetComponent<Animation>();
    }
    void Update()
    {
        if (controller.isGrounded)
        {
            velocity = 
                new Vector3(Input.GetAxis("Horizontal"),
                            0,
                            Input.GetAxis("Vertical"));
            velocity *= walkSpeed;

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = jumpSpeed;
                if(isJump==false)
                    StartCoroutine(doJump());
            }
            /*방향키를 충분히 눌러서 일정량 이상의 변화값이 생기면*/
            else if(velocity.magnitude > 0.5f)
            {
                ani.CrossFade("walk", 0.1f);
                transform.LookAt(transform.position + velocity);
            }
            else
            {
                if(isJump==false)
                    ani.CrossFade("idle", 0.1f);
            }
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator doJump()
    {
        isJump = true;
        ani.CrossFade("victory", 0.2f);
        yield return new WaitForSeconds(2.7f);
        isJump = false;
    }
}
