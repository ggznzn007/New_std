using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol_01 : MonoBehaviour
{  
    [SerializeField] private float speed = 1.0f;// 이동속도
    [SerializeField] private float damping = 1.0f;// 회전속도
    [SerializeField] private Transform[] patrolPoints;// 모든 웨이포인트를 저장할 배열    
    private Transform tr;// 컴포넌트를 저장할 변수
    private int nextIdx = 1;// 다음에 이동해야 할 위치의 인덱스 변수    

    void Start()
    {
        tr = GetComponent<Transform>();// 트랜스폼 컴포넌트 추출 후 변수에 저장        
        GameObject patrolPointGroup = GameObject.Find("PatrolPointGroup");// 게임오브젝트를 검색해 변수에 저장
        if (patrolPointGroup != null)
        {   // 웨이포인트 자식으로 있는 모든 게임오브젝트의 트랜스폼 컴포넌트 추출
            patrolPoints = patrolPointGroup.GetComponentsInChildren<Transform>();
        }
    }
    void Update()
    {
        MovePatrolPoint();
    }

    void MovePatrolPoint()
    {
        // 현재 위치에서 다음 웨이포인트로 향하는 벡터를 계산
        Vector3 direction = patrolPoints[nextIdx].position - tr.position;

        // 산출된 벡터의 회전 각도를 쿼터니언 타입으로 산출
        Quaternion rot = Quaternion.LookRotation(direction.normalized);

        // 현재 각도에서 회전해야 할 각도까지 부드럽게 회전 처리
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, damping * Time.deltaTime);

        // 전진 방향으로 이동 처리        
        tr.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    
     void OnCollisionEnter(Collision collision)
     {
        nextIdx = Random.Range(0, 24); // 임의의 포인트로 이동       
                                       // 맨 마지막 웨이포인트에 도달했을 때 처음 인덱스로 변경
        nextIdx = (++nextIdx >= patrolPoints.Length) ? 0 : nextIdx;

         if (collision.collider.CompareTag("Fishes")||(collision.collider.CompareTag("Player")))
         {
            // 콜라이더가 물고기 또는 플레이어와 태그되면 물리반응
            this.transform.Translate(Vector3.back * damping * Time.deltaTime);
         }
     }   
}
