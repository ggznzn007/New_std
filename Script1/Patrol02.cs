using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol02 : MonoBehaviour
{
    protected enum MoveType
    {
        WAY_POINT,
        LOOK_AT,
        DAYDREAM
    }

    [SerializeField] protected MoveType moveType = MoveType.WAY_POINT;//�̵����
    [SerializeField] private float speed = 14.0f;//�̵��ӵ�
    [SerializeField] private float damping = 2.0f;//ȸ���ӵ�
    [SerializeField] private Transform[] points;//��� ��������Ʈ�� ������ �迭    
    private Transform tr;//������Ʈ�� ������ ����
    private int nextIdx = 1;//������ �̵��ؾ� �� ��ġ�� �ε��� ����    

    void Start()
    {

        tr = GetComponent<Transform>();//Ʈ������ ������Ʈ ���� �� ������ ����        
        GameObject patrolPointGroup = GameObject.Find("PatrolPointGroup");//���ӿ�����Ʈ�� �˻��� ������ ����
        if (patrolPointGroup != null)
        {   //��������Ʈ �ڽ����� �ִ� ��� ���ӿ�����Ʈ�� Ʈ������ ������Ʈ ����
            points = patrolPointGroup.GetComponentsInChildren<Transform>();
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
    }

    protected void MoveWayPoint()
    {

        //���� ��ġ���� ���� ��������Ʈ�� ���ϴ� ���͸� ���
        Vector3 direction = points[nextIdx].position - tr.position;
        //direction.y = 0f;
        //����� ������ ȸ�� ������ ���ʹϾ� Ÿ������ ����
        Quaternion rot = Quaternion.LookRotation(direction.normalized);
        //���� �������� ȸ���ؾ� �� �������� �ε巴�� ȸ�� ó��
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * damping);
        //���� �������� �̵� ó��        
        tr.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    void OnTriggerEnter(Collider collision)
    {
        //nextIdx = (++nextIdx >= points.Length) ? 1 : nextIdx;
        //�� ������ ��������Ʈ�� �������� �� ó�� �ε����� ����

        if (collision.CompareTag("PATROL"))
        {
            nextIdx = Random.Range(0, 24); //������ ����Ʈ�� �̵�       
            nextIdx = (++nextIdx >= points.Length) ? 1 : nextIdx;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Fishes")
        {
            Debug.Log("����� �浹");
            this.transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
    }
}
