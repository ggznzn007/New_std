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

public class Bullet_rManager : MonoBehaviourPunCallbacks
{
    public static Bullet_rManager BrM;
    [SerializeField] PhotonView PV;
    [SerializeField] Rigidbody rb;
    [SerializeField] ParticleSystem exploreEffect;
    [SerializeField] Transform firePoint;
    [SerializeField] int actNumber;
    [SerializeField] float speed;
    void Start()
    {
        BrM=this;
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        Destroy(this.gameObject, 1f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!AvartarController.ATC.isAlive)
        {
            return;
        }
        // ������ ����Ʈ ��������
        if (collision.collider.CompareTag("Cube") && PV.IsMine) // �Ϲ��±�
        {
            // �浹������ ������ ����
            ContactPoint contact = collision.contacts[0];

            // ���� ��Ÿ�� �̷�� ȸ������ ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            transform.position = contact.point;
            Destroy(effect, 0.5f);
            AudioManager.AM.PlaySE("Bulletimpact");
            //Destroy(gameObject);
            PV.RPC("DestroyBullet", RpcTarget.All);
          
        }

        if (collision.collider.CompareTag("BlueTeam") || collision.collider.CompareTag("RedTeam"))
        {
            // �浹������ ������ ����
            ContactPoint contact = collision.contacts[0];

            // ���� ��Ÿ�� �̷�� ȸ������ ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);
            //Destroy(gameObject);
            AudioManager.AM.PlaySE("Hit");
            PV.RPC("DestroyBullet", RpcTarget.All);
            //Debug.Log("�÷��̾� ����");


        }


        if (collision.collider.CompareTag("Head"))
        {
            // �浹������ ������ ����
            ContactPoint contact = collision.contacts[0];

            // ���� ��Ÿ�� �̷�� ȸ������ ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);
            //Destroy(gameObject);
            AudioManager.AM.PlaySE("Hit");

            PV.RPC("DestroyBullet", RpcTarget.All);
        }
        if (collision.collider.CompareTag("Body"))
        {
            // �浹������ ������ ����
            ContactPoint contact = collision.contacts[0];

            // ���� ��Ÿ�� �̷�� ȸ������ ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);
            //Destroy(gameObject);
            AudioManager.AM.PlaySE("Hit");

            PV.RPC("DestroyBullet", RpcTarget.All);
        }
    }


    [PunRPC]
    public void BulletDir(float speed, int actorNumber)//,int addSpeed)
    {
        this.speed = speed;
        actNumber = actorNumber;
        // this.addSpeed = addSpeed;
    }

    [PunRPC]
    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

}
