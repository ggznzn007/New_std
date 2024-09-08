using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMove2 : MonoBehaviour
{
    [Header("이동 속도")]
    [SerializeField] protected float moveSpeed;
    protected float xAxis;
    protected float zAxis;
    Vector3 moveVec;
    Animator ani;
    
    void Start()
    {
        ani = GetComponent<Animator>(); // 애니메이터 생성
    }
    
    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal"); // 키보드 수평 움직임 
        zAxis = Input.GetAxisRaw("Vertical"); // 키보드 수직 움직임

        moveVec = new Vector3(xAxis, 0, zAxis).normalized;// 방향값 보정

        transform.position += moveVec * moveSpeed * Time.deltaTime; // 기본 움직임
        transform.LookAt(transform.position + moveVec);// 캐릭터 기본 회전구현

        if (Input.GetButtonDown("Jump")) { moveSpeed += moveSpeed; }
        else if (Input.GetButtonDown("Fire3")) { moveSpeed -= moveSpeed / 2; } 
    }
}
