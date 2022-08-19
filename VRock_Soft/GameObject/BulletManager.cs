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


public class BulletManager : MonoBehaviourPunCallbacks//, IPunObservable //MonoBehaviourPun   //MonoBehaviour     Poolable                           // 총알 스크립트 
{
    public PhotonView PV;
    public float speed;     
    public Rigidbody rb;
    public ParticleSystem exploreEffect;    
    public Transform firePoint;  
    
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 1.2f);              
    }
   
    private void Update()
    {
        if (!PV.IsMine) return;
        //transform.Translate(firePoint.forward * speed * Time.deltaTime );
      // transform.GetComponent<Rigidbody>().AddRelativeForce(firePoint.forward * speed ,ForceMode.Force);
       transform.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision coll)
    {
        // 터지는 이펙트 보여지고
        if (coll.collider.CompareTag("Cube"))//&&col.collider.GetComponent<PhotonView>().IsMine)
        {
            // 충돌지점의 정보를 추출
            ContactPoint contact = coll.contacts[0];

            // 법선 벡타가 이루는 회전각도 추출
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);           

            // 충돌 지점에 이펙트 생성           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);

            PV.RPC("DestroyBullet", RpcTarget.AllBuffered);
            //Enqueue(); // 큐 방식 총알 풀링 => 사용한 총알을 다시 큐에 넣기
            //OP.PoolDestroy(this.gameObject);
            // BulletPool.BulletPooling.ReturnBullet(); // 기존 풀링
            // ShowEffect(collision); // 이펙트 메서드 
            //StartCoroutine(ExploreEffect(collision));  // 이펙트 코루틴         
            //StartCoroutine(DeactiveBullet()); // 총알 비활성화 코루틴
            //Destroy(collision.gameObject); // 총알 맞은 오브젝트가 사라짐 
            Debug.Log("목표물에 명중");
            Debug.Log("총알 파괴");
        }

      /*  if (!PV.IsMine && coll.collider.CompareTag("Player") && coll.collider.GetComponent<PhotonView>().IsMine)
        {

            // 충돌지점의 정보를 추출
            ContactPoint contact = coll.contacts[0];

            // 법선 벡타가 이루는 회전각도 추출
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

            // 충돌 지점에 이펙트 생성           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);

            coll.collider.GetComponent<PlayerNetworkSetup>().HitPlayer();
            PV.RPC("DestroyBullet", RpcTarget.AllBuffered);
            Destroy(effect, 0.5f);
            Debug.Log("적에게 명중");

        }*/


    }


    [PunRPC]
   public void BulletDir(float speed)//,int addSpeed)
    {
        this.speed = speed;
       // this.addSpeed = addSpeed;
    }

    [PunRPC]
    public void DestroyBullet() => Destroy(gameObject);

    /*[PunRPC]
    void DestoroyEffect() =>Destroy(exploreEffect);*/


    /*  public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
      {
          if (stream.IsWriting)
          {
              stream.SendNext(transform.position);
              stream.SendNext(transform.rotation);
              stream.SendNext(rb.position);
              stream.SendNext(rb.rotation);
              // stream.SendNext(transform.localPosition);
              // stream.SendNext(transform.localRotation);
          }
          else
          {

              transform.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
              rb.position = (Vector3)stream.ReceiveNext();
              rb.rotation = (Quaternion)stream.ReceiveNext();
              //this.transform.localPosition = (Vector3)stream.ReceiveNext();
              // this.transform.localRotation = (Quaternion)stream.ReceiveNext();
          }
      }
  */
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
    /*[PunRPC]
    public void LocalDestruction() => PN.Destroy(gameObject.GetPhotonView());*/
    /*
        [PunRPC]
        void DestroyBullet()
        {
            PN.Destroy(photonView.gameObject);
        }*/

    /*public void ShowEffect(Collision collision)   // 이펙트 메서드 버전
    {
        // 충돌지점의 정보를 추출
        ContactPoint contact = collision.contacts[0];

        // 법선 벡타가 이루는 회전각도 추출
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        // 폭발 효과 생성
        Instantiate(exploreEffet, contact.point, rot);

     Collision collision = new Collision();  // 충돌지점 추출 메서드
            // 충돌 지점 추출
            var contact = collision.GetContact(0);

            // 충돌 지점에 이펙트 생성           
            var effect = Instantiate(exploreEffect, contact.point, Quaternion.LookRotation(-contact.normal));

            Destroy(effect, 0.5f);

    }*/
    /*[PunRPC]
    public void SetActiveRPC(bool bull)
    {
        gameObject.SetActive(bull);
    }*/
}

