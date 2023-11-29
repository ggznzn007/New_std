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
        startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);  // ó�� ������ ��ġ��
        startRot = transform.rotation;                                                             // ó�� ������ ȸ����
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.1f);        
        rb = GetComponent<Rigidbody>();
        rb.Sleep();        
    }


    void Update()
    {
        if (target != null)                                                   // ������ �������� �޼���
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
        if(coll.CompareTag("Finish"))               // Finish��� ���� �±� �Ǿ��� ��
        {
            StartCoroutine(DelayPos());             // �����ð� �Ŀ� ���� ��ġ�� �ǵ��ư��� �޼��� ȣ��
        }
    }

    private void UpdateTarget()                     // �������� ������Ʈ ���� �޼��� 
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

    IEnumerator DelayRidDisable()                                  // ������ٵ� �޼���
    {
        yield return new WaitForSeconds(1);
        isMoving = false;
        rb.Sleep();       
    }

    IEnumerator DelayPos()                                                     // �����ð� �� ������ ������� �ǵ����� �޼���
    {
        yield return new WaitForSecondsRealtime(10);                           // (10) 10.5�ʸ� ���ϸ� (10.5f) ���� �� ����Ʈ 10�� 
        transform.position = startPos;                                         // ������ ��ġ���� ó����ġ������ �ʱ�ȭ
        transform.rotation = startRot;                                         // ������ ȸ������ ó��ȸ�������� �ʱ�ȭ
        transform.localScale = new Vector3(0, 0, 0);                           // ������ ������(ũ��)�� 0,0,0���� ����
        yield return new WaitForSecondsRealtime(1);                            // 1���� �����̸� ����
        transform.LeanScale(new Vector3(1.5f,1.5f, 1.5f), 2).setEaseLinear();  // ������ �ʱ�ũ�Ⱑ 1.5f�� 1.5f�� �����ϰ� �ڿ� 2�� Ŀ���� �ð�
                                                                               // ex) transform.LeanScale(new Vector3(1,1,1), 1.5f).setEaseLinear();
                                                                               // ���� ���ô� ũ�Ⱑ 1 Ŀ���� �ð��� 1.5�� 
        transform.position = startPos;                                         // �ٽ� ������ ��ġ�� �ʱ�ȭ
        transform.rotation = startRot;                                         // �ٽ� ������ ȸ���� �ʱ�ȭ
    }
}
