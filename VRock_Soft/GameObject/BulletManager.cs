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


public class BulletManager : MonoBehaviourPunCallbacks//Poolable//, IPunObservable //MonoBehaviourPun   //MonoBehaviour                                // 총알 스크립트 
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
        // 터지는 이펙트 보여지고
        if (collision.collider.CompareTag("Cube")&& PV.IsMine) // 일반태그
        {
            // 충돌지점의 정보를 추출
            ContactPoint contact = collision.contacts[0];

            // 법선 벡타가 이루는 회전각도 추출
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

            // 충돌 지점에 이펙트 생성           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            transform.position = contact.point;
            Destroy(effect, 0.5f);
            AudioManager.AM.PlaySE("Bulletimpact");
            //Destroy(gameObject);
            PV.RPC("DestroyBullet", RpcTarget.All);
            //Enqueue(); // 큐 방식 총알 풀링 => 사용한 총알을 다시 큐에 넣기
            //OP.PoolDestroy(this.gameObject);
            // BulletPool.BulletPooling.ReturnBullet(); // 기존 풀링
            // ShowEffect(collision); // 이펙트 메서드 
            //StartCoroutine(ExploreEffect(collision));  // 이펙트 코루틴         
            //StartCoroutine(DeactiveBullet()); // 총알 비활성화 코루틴
            //Destroy(collision.gameObject); // 총알 맞은 오브젝트가 사라짐 

           // Debug.Log("목표물에 명중");

        }

        if (collision.collider.CompareTag("BlueTeam") || collision.collider.CompareTag("RedTeam"))
        {
            // 충돌지점의 정보를 추출
            ContactPoint contact = collision.contacts[0];

            // 법선 벡타가 이루는 회전각도 추출
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

            // 충돌 지점에 이펙트 생성           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);
            //Destroy(gameObject);
            AudioManager.AM.PlaySE("Hit");
            PV.RPC("DestroyBullet", RpcTarget.All);
            //Debug.Log("플레이어 명중");
           

        }


        if (collision.collider.CompareTag("Head"))
        {
            // 충돌지점의 정보를 추출
            ContactPoint contact = collision.contacts[0];

            // 법선 벡타가 이루는 회전각도 추출
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

            // 충돌 지점에 이펙트 생성           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);
            //Destroy(gameObject);
            AudioManager.AM.PlaySE("Hit");

            PV.RPC("DestroyBullet", RpcTarget.All);
        }
        if (collision.collider.CompareTag("Body"))
        {
            // 충돌지점의 정보를 추출
            ContactPoint contact = collision.contacts[0];

            // 법선 벡타가 이루는 회전각도 추출
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

            // 충돌 지점에 이펙트 생성           
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





    /* private void OnEnable()  //풀링할때 
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

    /*if (collsion.collider.CompareTag("Toy"))   // 토이 튜토리얼 갈릭 태그
       {           
           // 충돌지점의 정보를 추출
           ContactPoint contact = collsion.contacts[0];

           // 법선 벡타가 이루는 회전각도 추출
           Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);


           // 충돌 지점에 이펙트 생성           
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

       if (collsion.collider.CompareTag("West"))  // 웨스트 튜토리얼 갈릭 태그
       {
           // 충돌지점의 정보를 추출
           ContactPoint contact = collsion.contacts[0];
           // 법선 벡타가 이루는 회전각도 추출
           Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
           // 충돌 지점에 이펙트 생성           
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
           // 충돌지점의 정보를 추출
           ContactPoint contact = collsion.contacts[0];
           // 법선 벡타가 이루는 회전각도 추출
           Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
           // 충돌 지점에 이펙트 생성           
           var effect = Instantiate(exploreEffect, contact.point, rot);
           AudioManager.AM.EffectPlay(AudioManager.Effect.BulletImpact);
           Destroy(effect, 0.5f);
           Destroy(collsion.collider);
           PV.RPC("DestroyBullet", RpcTarget.All);
           PN.LeaveRoom();

       }
*/
}

