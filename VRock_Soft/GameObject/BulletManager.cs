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


public class BulletManager : MonoBehaviourPunCallbacks//Poolable//, IPunObservable //MonoBehaviourPun   //MonoBehaviour                                // �Ѿ� ��ũ��Ʈ 
{
    public static BulletManager BM;
    [SerializeField] PhotonView PV;
    [SerializeField] Rigidbody rb;
    [SerializeField] ParticleSystem exploreEffect;
    [SerializeField] Transform firePoint;
    [SerializeField] int actNumber;
    [SerializeField] float speed;
    //Transform tr;
    private void Start()
    {
        BM = this;
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
         Destroy(this.gameObject, 1f);
        //OP.PoolDestroy(gameObject);
       // StartCoroutine(DestroyDelay());
    }
   
    private void Update()
    {        
        //GunManager.gunManager.FindGun();

        //transform.Translate(Vector3.forward * speed * Time.deltaTime );
        //transform.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed ,ForceMode.Force);
        //GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed, ForceMode.Force);
        //transform.Translate(Vector3.forward * speed * Time.smoothDeltaTime);
        // GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed, ForceMode.Force);
    }

    /*private void OnTriggerEnter(Collider collider)
    {
        //if (!PV.IsMine) return;
        if (!PV.IsMine && collider.tag == "Player" && collider.GetComponent<PhotonView>().IsMine)
        {
            Debug.Log("�÷��̾� ����");           

            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, transform.position,transform.rotation);

            Destroy(effect, 0.5f);

            collider.GetComponent<PlayerNetworkSetup>().HitPlayer();

            PV.RPC("DestroyBullet", RpcTarget.AllBuffered);
            //Destroy(effect, 0.5f);


        }
        // ������ ����Ʈ ��������
        if (collider.tag == "Cube")
        {
            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, transform.position, transform.rotation);

            Destroy(effect, 0.5f);

            PV.RPC("DestroyBullet", RpcTarget.AllBuffered);
            //Enqueue(); // ť ��� �Ѿ� Ǯ�� => ����� �Ѿ��� �ٽ� ť�� �ֱ�
            //OP.PoolDestroy(this.gameObject);
            // BulletPool.BulletPooling.ReturnBullet(); // ���� Ǯ��
            // ShowEffect(collision); // ����Ʈ �޼��� 
            //StartCoroutine(ExploreEffect(collision));  // ����Ʈ �ڷ�ƾ         
            //StartCoroutine(DeactiveBullet()); // �Ѿ� ��Ȱ��ȭ �ڷ�ƾ
            //Destroy(collision.gameObject); // �Ѿ� ���� ������Ʈ�� ����� 

            Debug.Log("��ǥ���� ����");

        }
    }*/
    private void OnCollisionEnter(Collision collsion)
    {              
        if(!AvartarController.ATC.isAlive)
        {
            return;
        }
       
        // ������ ����Ʈ ��������
        if (collsion.collider.CompareTag("Cube")&& PV.IsMine)
        {
            // �浹������ ������ ����
            ContactPoint contact = collsion.contacts[0];

            // ���� ��Ÿ�� �̷�� ȸ������ ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);


            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);
            //Destroy(this.gameObject);
            transform.position = contact.point;
            AudioManager.AM.EffectPlay(AudioManager.Effect.BulletImpact);
            PV.RPC("DestroyBullet", RpcTarget.All);
            //Enqueue(); // ť ��� �Ѿ� Ǯ�� => ����� �Ѿ��� �ٽ� ť�� �ֱ�
            //OP.PoolDestroy(this.gameObject);
            // BulletPool.BulletPooling.ReturnBullet(); // ���� Ǯ��
            // ShowEffect(collision); // ����Ʈ �޼��� 
            //StartCoroutine(ExploreEffect(collision));  // ����Ʈ �ڷ�ƾ         
            //StartCoroutine(DeactiveBullet()); // �Ѿ� ��Ȱ��ȭ �ڷ�ƾ
            //Destroy(collision.gameObject); // �Ѿ� ���� ������Ʈ�� ����� 

           // Debug.Log("��ǥ���� ����");

        }

        if(collsion.collider.CompareTag("BlueTeam")|| collsion.collider.CompareTag("RedTeam"))
        {
            // �浹������ ������ ����
            ContactPoint contact = collsion.contacts[0];

            // ���� ��Ÿ�� �̷�� ȸ������ ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);


            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, contact.point, rot);
            AudioManager.AM.EffectPlay(AudioManager.Effect.BulletImpact);
            Destroy(effect, 0.5f);
            //Destroy(this.gameObject);
            PV.RPC("DestroyBullet", RpcTarget.All);
            //Debug.Log("�÷��̾� ����");
        }
        
        
        if (collsion.collider.CompareTag("Toy"))
        {           
            // �浹������ ������ ����
            ContactPoint contact = collsion.contacts[0];

            // ���� ��Ÿ�� �̷�� ȸ������ ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);


            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, contact.point, rot);
            AudioManager.AM.EffectPlay(AudioManager.Effect.BulletImpact);
            Destroy(effect, 0.5f);
            PV.RPC("DestroyBullet", RpcTarget.All);

            //PN.LeaveRoom();
            if (PN.IsMasterClient)
            {
                PN.LoadLevel(2);
            }
        }

        if (collsion.collider.CompareTag("Tuto_W"))
        {
            // �浹������ ������ ����
            ContactPoint contact = collsion.contacts[0];
            // ���� ��Ÿ�� �̷�� ȸ������ ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, contact.point, rot);
            AudioManager.AM.EffectPlay(AudioManager.Effect.BulletImpact);
            Destroy(effect, 0.5f);
            PV.RPC("DestroyBullet", RpcTarget.All);
             PN.LeaveRoom();
            /*if (PN.IsMasterClient)
            {
                PN.LoadLevel(3);
            }*/
        }

        if (collsion.collider.CompareTag("West"))
        {
            // �浹������ ������ ����
            ContactPoint contact = collsion.contacts[0];
            // ���� ��Ÿ�� �̷�� ȸ������ ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, contact.point, rot);
            AudioManager.AM.EffectPlay(AudioManager.Effect.BulletImpact);
            Destroy(effect, 0.5f);
            PV.RPC("DestroyBullet", RpcTarget.All);
            // PN.LeaveRoom();
            if (PN.IsMasterClient)
            {
                PN.LoadLevel(4);
            }
        }

        if (collsion.collider.CompareTag("Next"))
        {
            // �浹������ ������ ����
            ContactPoint contact = collsion.contacts[0];
            // ���� ��Ÿ�� �̷�� ȸ������ ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, contact.point, rot);
            AudioManager.AM.EffectPlay(AudioManager.Effect.BulletImpact);
            Destroy(effect, 0.5f);
            PV.RPC("DestroyBullet", RpcTarget.All);
            PN.LeaveRoom();
           
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
       // PoolManager.PoolingManager.pool.Enqueue(this.gameObject);
       // OP.PoolDestroy(gameObject);
        // Debug.Log("�Ѿ� �ı�");
    }

   

    
    /* private void OnEnable()  //Ǯ���Ҷ� 
      {
          // rb.AddRelativeForce(GunManager.gunManager.firePoint.forward * speed);
          rb.AddForce(transform.forward * speed);

      }

      private void OnBecameInvisible()
      {
          //Enqueue();
          // OP.PoolDestroy(this.gameObject);

          //Destroy(gameObject);
          //PN.Destroy(photonView);
          if (GetComponent<PhotonView>().IsMine)
          {
              PN.Destroy(gameObject);
          }
          else
          {
              Destroy(this.gameObject);
          }
      }

      private void OnDisable()
      {
          //tr.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
          rb.Sleep();
      }*/

}

