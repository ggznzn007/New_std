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

public class Bullet_Blue : MonoBehaviourPunCallbacks
{
    public static Bullet_Blue BB;
    [SerializeField] PhotonView PV;
    [SerializeField] Rigidbody rb;
    [SerializeField] ParticleSystem exploreEffect;
    [SerializeField] Transform firePoint;
    [SerializeField] int actNumber;
    [SerializeField] float speed;
    
    private void Start()
    {
        BB = this;
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 1f);      
    }


    private void OnCollisionEnter(Collision collsion)
    {
        if (!AvartarController.ATC.isAlive)
        {
            return;
        }
        // 터지는 이펙트 보여지고
        if (collsion.collider.CompareTag("Cube") && PV.IsMine) // 일반태그
        {
            // 충돌지점의 정보를 추출
            ContactPoint contact = collsion.contacts[0];

            // 법선 벡타가 이루는 회전각도 추출
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

            // 충돌 지점에 이펙트 생성           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);
            Destroy(gameObject);
            transform.position = contact.point;
            AudioManager.AM.EffectPlay(AudioManager.Effect.BulletImpact);
            PV.RPC("DestroyBulletB", RpcTarget.All);
        }

        if (collsion.collider.CompareTag("RedTeam")|| collsion.collider.CompareTag("BlueTeam"))
        {
            // 충돌지점의 정보를 추출
            ContactPoint contact = collsion.contacts[0];

            // 법선 벡타가 이루는 회전각도 추출
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

            // 충돌 지점에 이펙트 생성           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);
            Destroy(gameObject);
            PV.RPC("DestroyBulletB", RpcTarget.All);
            //Debug.Log("플레이어 명중");
        }
    }

    [PunRPC]
    public void BulletDirB(float speed, int actorNumber)//,int addSpeed)
    {
        this.speed = speed;
        actNumber = actorNumber;        
    }

    [PunRPC]
    public void DestroyBulletB()
    {
        Destroy(gameObject);       
    }
}
