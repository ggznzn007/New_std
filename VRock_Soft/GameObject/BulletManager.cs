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


public class BulletManager : Poolable, IPunObservable //MonoBehaviourPun   //MonoBehaviour     Poolable                           // 총알 스크립트 
{
    public float speed;
    Transform tr;
    Rigidbody rb;
    public ParticleSystem exploreEffet;
    // public int actorNumber;
    private PhotonView PV;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        //OP.PrePoolInstantiate();
        // GetComponent<Rigidbody>().AddRelativeForce(GunManager.gunManager.firePoint.forward * speed);        
      //GetComponent<Rigidbody>().AddRelativeForce(transform.forward * speed);        
    }

    private void OnEnable()  //풀링할때 
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
       /* if (GetComponent<PhotonView>().IsMine)
        {
            PN.Destroy(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }*/
    }

    private void OnDisable()
    {
        //tr.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        rb.Sleep();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 터지는 이펙트 보여지고
        if (collision.collider.CompareTag("Cube"))
        {
            // 충돌 지점 추출
            var contact = collision.GetContact(0);

            // 충돌 지점에 이펙트 생성
            var effect = Instantiate(exploreEffet, contact.point, Quaternion.LookRotation(-contact.normal));

            Destroy(effect, 0.8f);

           // PV.RPC("LocalDestruction", RpcTarget.All);

            //PN.Destroy(photonView);
            Enqueue(); // 큐 방식 총알 풀링 => 사용한 총알을 다시 큐에 넣기
            //OP.PoolDestroy(this.gameObject);
            //Destroy(gameObject);
           

            // BulletPool.BulletPooling.ReturnBullet(); // 기존 풀링
            // ShowEffect(collision); // 이펙트 메서드 
            //StartCoroutine(ExploreEffect(collision));  // 이펙트 코루틴         
            //StartCoroutine(DeactiveBullet()); // 총알 비활성화 코루틴
            //Destroy(collision.gameObject); // 총알 맞은 오브젝트가 사라짐 
            Debug.Log("목표물에 명중");
            Debug.Log("총알 파괴");

        }


    }
    private void Update()
    {
        if (!PV.IsMine) return;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            // stream.SendNext(transform.localPosition);
            // stream.SendNext(transform.localRotation);
        }
        else
        {

            transform.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            //this.transform.localPosition = (Vector3)stream.ReceiveNext();
            // this.transform.localRotation = (Quaternion)stream.ReceiveNext();
        }
    }

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
    }*/
    /*[PunRPC]
    public void SetActiveRPC(bool bull)
    {
        gameObject.SetActive(bull);
    }*/
}

