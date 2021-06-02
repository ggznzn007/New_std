using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Animation_Ctrl02 : MonoBehaviour
{
    [Header("이동속도")]
    [SerializeField] float moveSpeed = 12f;
    [Header("캐릭터위치")]
    [SerializeField] Vector3 movement = new Vector3();
    Animator animator;
    void Start()
    { animator = GetComponent<Animator>(); }
    void Update()
    { UpdateState(); }
    protected void FixedUpdate()
    { MoveCharacter(); }
    protected void MoveCharacter()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
        Quaternion newRot = Quaternion.LookRotation(movement);
        transform.rotation = newRot;
        // 키보드 입력
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
        // 이동 거리 보정
        movement.Normalize();
        // 기본 움직임
        transform.position += movement * moveSpeed * Time.deltaTime;
        // 캐릭터 기본 회전
        transform.LookAt(transform.position);
        //transform.LookAt(transform.position + movement);
    }
    protected void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Z)) { animator.SetTrigger("Spin"); }
        else if (Input.GetKeyDown(KeyCode.X)) { animator.SetTrigger("Hold"); }
        else if (Input.GetKeyDown(KeyCode.C)) { animator.SetTrigger("Getup"); }
        else if (Input.GetKeyDown(KeyCode.V)) { animator.SetTrigger("Recall"); }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) { moveSpeed += moveSpeed + 1.5f; }
        else if (Input.GetKeyUp(KeyCode.UpArrow)) { moveSpeed -= moveSpeed * 0.5f; }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) { moveSpeed -= moveSpeed * 0.5f; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        { animator.SetTrigger("Fall"); }
    }

}
