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

        // 터지는 이펙트 보여지고
        if (collsion.collider.CompareTag("Cube") && PV.IsMine)
        {
            // 충돌지점의 정보를 추출
            ContactPoint contact = collsion.contacts[0];

            // 법선 벡타가 이루는 회전각도 추출
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);


            // 충돌 지점에 이펙트 생성           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);

            transform.position = contact.point;

            PV.RPC("DestroyBullet", RpcTarget.AllBuffered);
            //Enqueue(); // 큐 방식 총알 풀링 => 사용한 총알을 다시 큐에 넣기
            //OP.PoolDestroy(this.gameObject);
            // BulletPool.BulletPooling.ReturnBullet(); // 기존 풀링
            // ShowEffect(collision); // 이펙트 메서드 
            //StartCoroutine(ExploreEffect(collision));  // 이펙트 코루틴         
            //StartCoroutine(DeactiveBullet()); // 총알 비활성화 코루틴
            //Destroy(collision.gameObject); // 총알 맞은 오브젝트가 사라짐 

            // Debug.Log("목표물에 명중");

        }

        if (collsion.collider.CompareTag("BlueTeam"))
        {
            // 충돌지점의 정보를 추출
            ContactPoint contact = collsion.contacts[0];

            // 법선 벡타가 이루는 회전각도 추출
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);


            // 충돌 지점에 이펙트 생성           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);
            PV.RPC("DestroyBullet", RpcTarget.AllBuffered);
            //Debug.Log("플레이어 명중");
        }


        if (collsion.collider.CompareTag("Next"))
        {
            // 충돌지점의 정보를 추출
            ContactPoint contact = collsion.contacts[0];

            // 법선 벡타가 이루는 회전각도 추출
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);


            // 충돌 지점에 이펙트 생성           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);
            PV.RPC("DestroyBullet", RpcTarget.AllBuffered);

            PN.Disconnect();
            //Debug.Log("플레이어 명중");
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
        Debug.Log("총알 파괴");
    }
}
