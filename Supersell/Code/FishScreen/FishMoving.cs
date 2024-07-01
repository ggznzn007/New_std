using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static UnityEngine.Debug;
using TMPro;

public class FishMoving : MonoBehaviour
{
    [SerializeField] private float speed = 4.0f;                                         // 이동속도    
    [SerializeField] private float damping = 1.0f;                                       // 회전속도
    [SerializeField] private Transform[] wayPoints;                                      // 모든 웨이포인트를 저장할 배열    
    private Transform tr;                                                                // 컴포넌트를 저장할 변수
    private int nextIdx = 1;                                                             // 다음에 이동해야 할 위치의 인덱스 변수
    private Animator anim;
    private bool OnWaypoint;

    void Start()
    {
        OnWaypoint = true;
        tr = GetComponent<Transform>();                                                  // 트랜스폼 컴포넌트 추출 후 변수에 저장
        anim = GetComponent<Animator>();
        GameObject wayPointGroup = GameObject.Find("WayPointGroup");                     // 게임오브젝트를 검색해 변수에 저장
        
        if (wayPointGroup != null)
        {                                                                   // 웨이포인트 자식으로 있는 모든 게임오브젝트의 트랜스폼 컴포넌트 추출
            wayPoints = wayPointGroup.GetComponentsInChildren<Transform>();
        }

        nextIdx = Random.Range(1, wayPoints.Length);
        MovetoWayPoint();
    }

    void Update()
    {
        if (OnWaypoint)
        {
            if (!OnWaypoint) { return; }
            MovetoWayPoint();
        }
    }

    void MovetoWayPoint()
    {
        // 현재 위치에서 다음 웨이포인트로 향하는 벡터를 계산
        Vector3 direction = wayPoints[nextIdx].position - tr.position; direction.y = 0f;

        // 산출된 벡터의 회전 각도를 쿼터니언 타입으로 산출
        Quaternion rot = Quaternion.LookRotation(direction.normalized);

        // 현재 각도에서 회전해야 할 각도까지 부드럽게 회전 처리
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, damping * Time.deltaTime);

        // 방향으로 이동 처리        
        tr.Translate(speed * Time.deltaTime * Vector3.left);
        //tr.position = Vector3.MoveTowards(tr.position, wayPoints[nextIdx].position, speed*Time.deltaTime);
        //tr.LookAt(wayPoints[nextIdx].position);        
        anim.Play("swim");
        anim.Update(0);
        anim.updateMode = AnimatorUpdateMode.AnimatePhysics;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("DeadLine"))
        {
            StartCoroutine(BacktoWay());
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Hidden"))
        {
            StartCoroutine(TouchTheFish());
            Log("손 태그");
        }

        if (coll.CompareTag("Waypoint"))
        {
            nextIdx = Random.Range(1, wayPoints.Length); // 임의의 포인트로 이동       
                                                         // 맨 마지막 웨이포인트에 도달했을 때 처음 인덱스로 변경
            nextIdx = (++nextIdx >= wayPoints.Length) ? 0 : nextIdx;
            Log("웨이포인트 태그");
        }
    }

    IEnumerator TouchTheFish()
    {
        speed = 8f;
        anim = this.GetComponent<Animator>();
        anim.Play("fastswim");
        anim.Update(0);
        anim.updateMode = AnimatorUpdateMode.AnimatePhysics;
        yield return new WaitForSeconds(5f);
        speed = 4f;
        anim.Play("swim");
        anim.Update(0);
        anim.updateMode = AnimatorUpdateMode.AnimatePhysics;
    }

    IEnumerator BacktoWay()
    {
        OnWaypoint = false;
        yield return null;        
        OnWaypoint = true;
    }
}
