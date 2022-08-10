using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.XR;



public class BulletManager : Poolable, IPunObservable  //MonoBehaviour                                   // �Ѿ� ��ũ��Ʈ 
{
    public float speed;
    Transform tr;
    Rigidbody rb;
    public ParticleSystem exploreEffet;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        ReadySceneManager.readySceneManager.FindGun();

    }
    private void OnEnable()
    {
       
        rb.AddForce(transform.forward * speed);

    }



    private void OnBecameInvisible()
    {
        Enqueue();
    }
    private void OnDisable()
    {
        tr.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        rb.Sleep();
    }
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ������ ����Ʈ ��������
        if (collision.collider.CompareTag("Cube"))
        {
            // �浹 ���� ����
            var contact = collision.GetContact(0);

            // �浹 ������ ����Ʈ ����
            var effect = Instantiate(exploreEffet, contact.point, Quaternion.LookRotation(-contact.normal));

            Destroy(effect, 1f);

            Enqueue(); // ť ��� �Ѿ� Ǯ�� => ����� �Ѿ��� �ٽ� ť�� �ֱ�

            // BulletPool.BulletPooling.ReturnBullet(); // ���� Ǯ��
            // ShowEffect(collision); // ����Ʈ �޼��� 
            //StartCoroutine(ExploreEffect(collision));  // ����Ʈ �ڷ�ƾ         
            //StartCoroutine(DeactiveBullet()); // �Ѿ� ��Ȱ��ȭ �ڷ�ƾ
            //Destroy(collision.gameObject); // �Ѿ� ���� ������Ʈ�� ����� 
            Debug.Log("�ݶ��̴� �±׵�");
        }


    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);         
        }       
        else if (stream.IsReading)
        {
            this.transform.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());            
        }
    }



    /*public void ShowEffect(Collision collision)   // ����Ʈ �޼��� ����
    {
        // �浹������ ������ ����
        ContactPoint contact = collision.contacts[0];

        // ���� ��Ÿ�� �̷�� ȸ������ ����
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        // ���� ȿ�� ����
        Instantiate(exploreEffet, contact.point, rot);
    }*/

}

