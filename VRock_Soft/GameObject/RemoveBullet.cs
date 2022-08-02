using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject exploreEffet;

    private void OnCollisionEnter(Collision collision)
    {
        // ������ ����Ʈ ��������
        if (collision.collider.CompareTag("Bullet"))
        {
            //var exEffect = ObjectPooler.SpawnFromPool<ExploreEffect>("ExplorelEffect");
            ShowEffect(collision);     
           
            //Destroy(collision.gameObject); // �Ѿ� ���� ������Ʈ�� ����� 
            Debug.Log("�Ѿ��±׵�");
        }
    }
    private void ShowEffect(Collision coll)
    {
        // �浹������ ������ ����
        ContactPoint contact = coll.contacts[0];

        // ���� ��Ÿ�� �̷�� ȸ������ ����
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        // ���� ȿ�� ����
        Instantiate(exploreEffet, contact.point, rot);


    }
}
