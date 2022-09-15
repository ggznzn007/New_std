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
public class Bullet_Red : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public float speed;
    public Rigidbody rb;
    public ParticleSystem exploreEffect;
    public Transform firePoint;
    public int actNumber = -1;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 1.2f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collsion)
    {
        if (!AvartarController.ATC.isAlive)
        {
            return;
        }

        // ������ ����Ʈ ��������
        if (collsion.collider.CompareTag("Cube") && PV.IsMine)
        {
            // �浹������ ������ ����
            ContactPoint contact = collsion.contacts[0];

            // ���� ��Ÿ�� �̷�� ȸ������ ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);


            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);

            transform.position = contact.point;

            PV.RPC("DestroyBullet", RpcTarget.AllBuffered);
            //Enqueue(); // ť ��� �Ѿ� Ǯ�� => ����� �Ѿ��� �ٽ� ť�� �ֱ�
            //OP.PoolDestroy(this.gameObject);
            // BulletPool.BulletPooling.ReturnBullet(); // ���� Ǯ��
            // ShowEffect(collision); // ����Ʈ �޼��� 
            //StartCoroutine(ExploreEffect(collision));  // ����Ʈ �ڷ�ƾ         
            //StartCoroutine(DeactiveBullet()); // �Ѿ� ��Ȱ��ȭ �ڷ�ƾ
            //Destroy(collision.gameObject); // �Ѿ� ���� ������Ʈ�� ����� 

            // Debug.Log("��ǥ���� ����");

        }

        if (collsion.collider.CompareTag("BlueTeam"))
        {
            // �浹������ ������ ����
            ContactPoint contact = collsion.contacts[0];

            // ���� ��Ÿ�� �̷�� ȸ������ ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);


            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);
            PV.RPC("DestroyBullet", RpcTarget.AllBuffered);
            //Debug.Log("�÷��̾� ����");
        }


        if (collsion.collider.CompareTag("Next"))
        {
            // �浹������ ������ ����
            ContactPoint contact = collsion.contacts[0];

            // ���� ��Ÿ�� �̷�� ȸ������ ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);


            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);
            PV.RPC("DestroyBullet", RpcTarget.AllBuffered);

            PN.Disconnect();
            //Debug.Log("�÷��̾� ����");
        }




    }


    [PunRPC]
    public void BulletDir(float speed)//,int addSpeed)
    {
        this.speed = speed;
        // this.addSpeed = addSpeed;
    }

    [PunRPC]
    public void DestroyBullet()
    {
        Destroy(gameObject);
        Debug.Log("�Ѿ� �ı�");
    }
}
