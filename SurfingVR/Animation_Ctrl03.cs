using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Ctrl03 : Animation_Ctrl02
{
    enum States
    {
        idle = 0,
        spin = 1,
        hold = 2,
        getup = 3,
        fall = 4,
        recall = 5
    }

    protected float moveSpeed = 15f;
    Vector3 movement = new Vector3();
    Animator animator;
    string aniState = "AnyState";
   
    protected

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateState();
    }

    new protected void FixedUpdate()
    {
        MoveCharacter();
    }

    new protected void MoveCharacter()
    {
        // 키보드 입력
        movement.x = Input.GetAxis("Horizontal");
        movement.z = Input.GetAxis("Vertical");
        // 이동 거리 보정
        movement.Normalize();
        // 기본 움직임
        transform.position += movement * moveSpeed * Time.deltaTime;
        // 캐릭터 기본 회전        
        transform.LookAt(transform.position + movement);

    }

    new protected void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetInteger(aniState, (int)States.spin);

        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetInteger(aniState, (int)States.hold);

        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetInteger(aniState, (int)States.getup);

        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetInteger(aniState, (int)States.fall);

        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            animator.SetInteger(aniState, (int)States.recall);

        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetInteger(aniState, (int)States.idle);

        }
        else if (Input.GetButtonDown("Fire1")) { moveSpeed += moveSpeed; }
        else if (Input.GetButtonDown("Fire2")) { moveSpeed -= moveSpeed * (float)0.5; }
    }
}
