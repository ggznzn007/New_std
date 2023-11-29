using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafMove : MonoBehaviour
{
    protected Transform target;
    protected float moveSpeed = 10f;
    protected Rigidbody rb;

    protected bool isMoving;
    protected Vector3 startPos;
    protected Quaternion startRot;

    void Start()
    {
        startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);  // 처음 나뭇잎 위치값
        startRot = transform.rotation;                                                             // 처음 나뭇잎 회전값
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.1f);        
        rb = GetComponent<Rigidbody>();
        rb.Sleep();        
    }


    void Update()
    {
        if (target != null)                                                   // 나뭇잎 물리적용 메서드
        {               
            rb.WakeUp();
            if (Vector3.Distance(transform.position, target.position) > 100f)
            {
                isMoving = true;
                Vector3 dir = target.position + transform.position;                
                transform.position = Vector3.Lerp(transform.position, dir.normalized, moveSpeed * Time.deltaTime);                            
                StartCoroutine(DelayRidDisable());
            }
        }       
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.CompareTag("Finish"))               // Finish라는 벽에 태그 되었을 때
        {
            StartCoroutine(DelayPos());             // 지연시간 후에 원래 위치로 되돌아가는 메서드 호출
        }
    }

    private void UpdateTarget()                     // 물리적용 오브젝트 감지 메서드 
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 10f);//, 1 << 8);

        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].CompareTag("Player"))
                {
                    target = cols[i].gameObject.transform;
                    Debug.Log("Physics Enemy : Target found");
                }
                else
                {
                    return;
                }
            }
        }
        else
        {
            Debug.Log("Physics Enemy : Target lost");
            target = null;
        }
    }

    IEnumerator DelayRidDisable()                                  // 리지드바디 메서드
    {
        yield return new WaitForSeconds(1);
        isMoving = false;
        rb.Sleep();       
    }

    IEnumerator DelayPos()                                                     // 설정시간 후 나뭇잎 원래대로 되돌리는 메서드
    {
        yield return new WaitForSecondsRealtime(10);                           // (10) 10.5초를 원하면 (10.5f) 넣을 것 디폴트 10초 
        transform.position = startPos;                                         // 나뭇잎 위치값을 처음위치값으로 초기화
        transform.rotation = startRot;                                         // 나뭇잎 회전값을 처음회전값으로 초기화
        transform.localScale = new Vector3(0, 0, 0);                           // 나뭇잎 스케일(크기)를 0,0,0으로 설정
        yield return new WaitForSecondsRealtime(1);                            // 1초의 딜레이를 설정
        transform.LeanScale(new Vector3(1.5f,1.5f, 1.5f), 2).setEaseLinear();  // 나뭇잎 초기크기가 1.5f라서 1.5f로 설정하고 뒤에 2는 커지는 시간
                                                                               // ex) transform.LeanScale(new Vector3(1,1,1), 1.5f).setEaseLinear();
                                                                               // 위의 예시는 크기가 1 커지는 시간이 1.5초 
        transform.position = startPos;                                         // 다시 나뭇잎 위치값 초기화
        transform.rotation = startRot;                                         // 다시 나뭇잎 회전값 초기화
    }
}
