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
using static ObjectPooler;
using static UnityEngine.Rendering.DebugUI.Table;


public class BulletManager : MonoBehaviourPun//Poolable//, IPunObservable //MonoBehaviourPun   //MonoBehaviour                                // �Ѿ� ��ũ��Ʈ 
{
    public static BulletManager BM;
    public PhotonView PV;
    [SerializeField] Rigidbody rb;
    [SerializeField] ParticleSystem exploreEffect;
    [SerializeField] Transform firePoint;
    [SerializeField] float speed;
    public int actNumber;
    public string bulletImpact;
    public string hitPlayer;
    public string headShot;
    public string shieldHit;

    private void Start()
    {
        BM = this;
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        if (this != null)
        {
            if (this == null) return;
            Destroy(gameObject, 0.5f);
        }       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!AvartarController.ATC.isAlive)
        {
            return;
        }
        // ������ ����Ʈ ��������
        if ((collision.collider.CompareTag("Cube") || collision.collider.CompareTag("Bullet")
             || collision.collider.CompareTag("Effect") || collision.collider.CompareTag("Gun")
             || collision.collider.CompareTag("Bomb"))|| collision.collider.CompareTag("FloorBox")) // �Ϲ��±�
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                // �浹������ ������ ����
                ContactPoint contact = collision.contacts[0];

                // ���� ��Ÿ�� �̷�� ȸ������ ����
                Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

                // �浹 ������ ����Ʈ ����           
                var effect = Instantiate(exploreEffect, contact.point, rot);

                transform.position = contact.point;
                Destroy(effect, 0.3f);
                AudioManager.AM.PlaySE(bulletImpact);
                PV.RPC(nameof(DestroyBullet), RpcTarget.AllBuffered);

            }
            //Enqueue(); // ť ��� �Ѿ� Ǯ�� => ����� �Ѿ��� �ٽ� ť�� �ֱ�
            //OP.PoolDestroy(this.gameObject);
            // BulletPool.BulletPooling.ReturnBullet(); // ���� Ǯ��
            // ShowEffect(collision); // ����Ʈ �޼��� 
            //StartCoroutine(ExploreEffect(collision));  // ����Ʈ �ڷ�ƾ         
            //StartCoroutine(DeactiveBullet()); // �Ѿ� ��Ȱ��ȭ �ڷ�ƾ
            //Destroy(collision.gameObject); // �Ѿ� ���� ������Ʈ�� �����            
        }

        if (collision.collider.CompareTag("Shield"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                ContactPoint contact = collision.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
                var effect = Instantiate(exploreEffect, contact.point, rot);
                transform.position = contact.point;
                Destroy(effect, 0.3f);
                //AudioManager.AM.PlaySE(shieldHit);
                PV.RPC(nameof(HitShield), RpcTarget.AllBuffered);
                PV.RPC(nameof(DestroyBullet), RpcTarget.AllBuffered);
            }

        }

        if (collision.collider.CompareTag("BlueTeam") || collision.collider.CompareTag("RedTeam"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                // �浹������ ������ ����
                ContactPoint contact = collision.contacts[0];

                // ���� ��Ÿ�� �̷�� ȸ������ ����
                Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

                // �浹 ������ ����Ʈ ����           
                var effect = Instantiate(exploreEffect, contact.point, rot);

                Destroy(effect, 0.3f);
                AudioManager.AM.PlaySE(hitPlayer);
                PV.RPC(nameof(DestroyBullet), RpcTarget.AllBuffered);
            }
        }

        if (collision.collider.CompareTag("Head"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                // �浹������ ������ ����
                ContactPoint contact = collision.contacts[0];

                // ���� ��Ÿ�� �̷�� ȸ������ ����
                Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

                // �浹 ������ ����Ʈ ����           
                var effect = Instantiate(exploreEffect, contact.point, rot);

                Destroy(effect, 0.3f);
                AudioManager.AM.PlaySE(headShot);
                PV.RPC(nameof(DestroyBullet), RpcTarget.AllBuffered);
            }
        }

        if (collision.collider.CompareTag("Body"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                // �浹������ ������ ����
                ContactPoint contact = collision.contacts[0];

                // ���� ��Ÿ�� �̷�� ȸ������ ����
                Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

                // �浹 ������ ����Ʈ ����           
                var effect = Instantiate(exploreEffect, contact.point, rot);

                Destroy(effect, 0.3f);
                AudioManager.AM.PlaySE(hitPlayer);
                PV.RPC(nameof(DestroyBullet), RpcTarget.AllBuffered);
            }
        }
    }



    [PunRPC]
    public void BulletDir(float speed, int actorNumber)//,int addSpeed)
    {
        this.speed = speed;
        actNumber = actorNumber;
    }

    [PunRPC]
    public void DestroyBullet() => Destroy(gameObject);
       
    /*gameObject.SetActive(false);
    gameObject.transform.position = firePoint.position;*/

    /* [PunRPC]
     public void DestroyDelay() => Destroy(gameObject, 0.5f);*/

    [PunRPC]
    public void HitShield() => AudioManager.AM.PlaySE(shieldHit);
}

