﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveCtrl : MonoBehaviour
{
    public enum MoveType
    {
        WAY_POINT,
        LOOK_AT,
        DAYDREAM
    }

    public MoveType moveType = MoveType.WAY_POINT;//이동방식
    public float speed = 0.5f;//이동속도
    public float damping = 1.0f;//회전속도
    public Transform[] points;//모든 웨이포인트를 저장할 배열
    private Transform tr;//컴포넌트를 저장할 변수
    private int nextIdx = 1;//다음에 이동해야 할 위치의 인덱스 변수
    
    void Start()
    {    
        tr = GetComponent<Transform>();//트랜스폼 컴포넌트 추출 후 변수에 저장
        GameObject wayPointGroup = GameObject.Find("WayPointGroup");//게임오브젝트를 검색해 변수에 저장
        if (wayPointGroup != null)
        {
            //웨이포인트 자식으로 있는 모든 게임오브젝트의 트랜스폼 컴포넌트 추출
            points = wayPointGroup.GetComponentsInChildren<Transform>();            
        }
    }
    
    void Update()
    {
        switch (moveType)
        {
            case MoveType.WAY_POINT:
            MoveWayPoint();
            break;

            case MoveType.LOOK_AT:
            break;

            case MoveType.DAYDREAM:
            break;
        }

        /*if(Input.GetMouseButtonDown(0)||Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("Main");
        }*/
    }

    void MoveWayPoint()
    {        
        //현재 위치에서 다음 웨이포인트로 향하는 벡터를 계산
        Vector3 direction = points[nextIdx].position - tr.position;
        //산출된 벡터의 회전 각도를 쿼터니언 타입으로 산출
        Quaternion rot = Quaternion.LookRotation(direction);
        //현재 각도에서 회전해야 할 각도까지 부드럽게 회전 처리
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * damping);
        //전진 방향으로 이동 처리
        tr.Translate(Vector3.forward * Time.deltaTime * speed);
        
        /*if(nextIdx == 5)
        {
            SceneManager.LoadScene("Main");
        }*/
    }

    void OnTriggerEnter(Collider coll)
    {
        //웨이포인트에 충돌 여부 판단
        if (coll.CompareTag("WAY_POINT"))
        {
            //맨 마지막 웨이포인트에 도달했을 때 처음 인덱스로 변경
            nextIdx = Random.Range(0, 12); //임의의 포인트로 이동
            nextIdx = (++nextIdx >= points.Length) ? 1 : nextIdx;
            
        }
    }
}
