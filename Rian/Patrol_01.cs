using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol_01 : MonoBehaviour
{  
    [SerializeField] private float speed = 1.0f;// �̵��ӵ�
    [SerializeField] private float damping = 1.0f;// ȸ���ӵ�
    [SerializeField] private Transform[] patrolPoints;// ��� ��������Ʈ�� ������ �迭    
    private Transform tr;// ������Ʈ�� ������ ����
    private int nextIdx = 1;// ������ �̵��ؾ� �� ��ġ�� �ε��� ����    

    void Start()
    {
        tr = GetComponent<Transform>();// Ʈ������ ������Ʈ ���� �� ������ ����        
        GameObject patrolPointGroup = GameObject.Find("PatrolPointGroup");// ���ӿ�����Ʈ�� �˻��� ������ ����
        if (patrolPointGroup != null)
        {   // ��������Ʈ �ڽ����� �ִ� ��� ���ӿ�����Ʈ�� Ʈ������ ������Ʈ ����
            patrolPoints = patrolPointGroup.GetComponentsInChildren<Transform>();
        }
    }
    void Update()
    {
        MovePatrolPoint();
    }

    void MovePatrolPoint()
    {
        // ���� ��ġ���� ���� ��������Ʈ�� ���ϴ� ���͸� ���
        Vector3 direction = patrolPoints[nextIdx].position - tr.position;

        // ����� ������ ȸ�� ������ ���ʹϾ� Ÿ������ ����
        Quaternion rot = Quaternion.LookRotation(direction.normalized);

        // ���� �������� ȸ���ؾ� �� �������� �ε巴�� ȸ�� ó��
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, damping * Time.deltaTime);

        // ���� �������� �̵� ó��        
        tr.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    
     void OnCollisionEnter(Collision collision)
     {
        nextIdx = Random.Range(0, 24); // ������ ����Ʈ�� �̵�       
                                       // �� ������ ��������Ʈ�� �������� �� ó�� �ε����� ����
        nextIdx = (++nextIdx >= patrolPoints.Length) ? 0 : nextIdx;

         if (collision.collider.CompareTag("Fishes")||(collision.collider.CompareTag("Player")))
         {
            // �ݶ��̴��� ����� �Ǵ� �÷��̾�� �±׵Ǹ� ��������
            this.transform.Translate(Vector3.back * damping * Time.deltaTime);
         }
     }   
}
