using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static UnityEngine.Debug;
using TMPro;

public class FishMoving : MonoBehaviour
{
    [SerializeField] private float speed = 4.0f;                                         // �̵��ӵ�    
    [SerializeField] private float damping = 1.0f;                                       // ȸ���ӵ�
    [SerializeField] private Transform[] wayPoints;                                      // ��� ��������Ʈ�� ������ �迭    
    private Transform tr;                                                                // ������Ʈ�� ������ ����
    private int nextIdx = 1;                                                             // ������ �̵��ؾ� �� ��ġ�� �ε��� ����
    private Animator anim;
    private bool OnWaypoint;

    void Start()
    {
        OnWaypoint = true;
        tr = GetComponent<Transform>();                                                  // Ʈ������ ������Ʈ ���� �� ������ ����
        anim = GetComponent<Animator>();
        GameObject wayPointGroup = GameObject.Find("WayPointGroup");                     // ���ӿ�����Ʈ�� �˻��� ������ ����
        
        if (wayPointGroup != null)
        {                                                                   // ��������Ʈ �ڽ����� �ִ� ��� ���ӿ�����Ʈ�� Ʈ������ ������Ʈ ����
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
        // ���� ��ġ���� ���� ��������Ʈ�� ���ϴ� ���͸� ���
        Vector3 direction = wayPoints[nextIdx].position - tr.position; direction.y = 0f;

        // ����� ������ ȸ�� ������ ���ʹϾ� Ÿ������ ����
        Quaternion rot = Quaternion.LookRotation(direction.normalized);

        // ���� �������� ȸ���ؾ� �� �������� �ε巴�� ȸ�� ó��
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, damping * Time.deltaTime);

        // �������� �̵� ó��        
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
            Log("�� �±�");
        }

        if (coll.CompareTag("Waypoint"))
        {
            nextIdx = Random.Range(1, wayPoints.Length); // ������ ����Ʈ�� �̵�       
                                                         // �� ������ ��������Ʈ�� �������� �� ó�� �ε����� ����
            nextIdx = (++nextIdx >= wayPoints.Length) ? 0 : nextIdx;
            Log("��������Ʈ �±�");
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
