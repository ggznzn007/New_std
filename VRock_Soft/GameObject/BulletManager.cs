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
   
   
    private void OnCollisionEnter(Collision collision)
    {              
        if(!AvartarController.ATC.isAlive)
        {
            return;
        }      
        // ������ ����Ʈ ��������
        if (collision.collider.CompareTag("Cube")&& PV.IsMine) // �Ϲ��±�
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
            //Enqueue(); // ť ��� �Ѿ� Ǯ�� => ����� �Ѿ��� �ٽ� ť�� �ֱ�
            //OP.PoolDestroy(this.gameObject);
            // BulletPool.BulletPooling.ReturnBullet(); // ���� Ǯ��
            // ShowEffect(collision); // ����Ʈ �޼��� 
            //StartCoroutine(ExploreEffect(collision));  // ����Ʈ �ڷ�ƾ         
            //StartCoroutine(DeactiveBullet()); // �Ѿ� ��Ȱ��ȭ �ڷ�ƾ
            //Destroy(collision.gameObject); // �Ѿ� ���� ������Ʈ�� ����� 

           // Debug.Log("��ǥ���� ����");

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

    /*[PunRPC]
    public void StartBtn2()
    {
       GunShootManager.GSM.StartBtnT();
       GunShootManager.GSM.startBtn.SetActive(false);
    }*/

   /* [PunRPC]
    public void StartBtn3()
    {
        WesternManager.WM.StartBtnW();
        WesternManager.WM.startBtn.SetActive(false);
    }*/

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

    /*if (collsion.collider.CompareTag("Toy"))   // ���� Ʃ�丮�� ���� �±�
       {           
           // �浹������ ������ ����
           ContactPoint contact = collsion.contacts[0];

           // ���� ��Ÿ�� �̷�� ȸ������ ����
           Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);


           // �浹 ������ ����Ʈ ����           
           var effect = Instantiate(exploreEffect, contact.point, rot);
           AudioManager.AM.EffectPlay(AudioManager.Effect.BulletImpact);
           Destroy(effect, 0.5f);
           Destroy(collsion.collider.gameObject);
           PV.RPC("DestroyBullet", RpcTarget.All);

           if (PN.IsMasterClient)
           {
               PN.LoadLevel(2);
           }

       }      

       if (collsion.collider.CompareTag("West"))  // ����Ʈ Ʃ�丮�� ���� �±�
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
           Destroy(collsion.collider.gameObject);
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
           Destroy(collsion.collider);
           PV.RPC("DestroyBullet", RpcTarget.All);
           PN.LeaveRoom();

       }
*/
}

