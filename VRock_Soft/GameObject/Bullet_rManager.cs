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
    public string revolverImpact;
    public string hitPlayer;
    void Start()
    {
        BrM = this;
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 1.5f);
      
    }
  
    private void OnCollisionEnter(Collision collision)
    {
        if (!AvartarController.ATC.isAlive)
        {
            return;
        }
        
        if ((collision.collider.CompareTag("Cube") || collision.collider.CompareTag("Bullet")
            || collision.collider.CompareTag("Effect") || collision.collider.CompareTag("Revolver")
            || collision.collider.CompareTag("Bomb")) && PV.IsMine) // 일반태그
        {
            // 충돌지점의 정보를 추출
            ContactPoint contact = collision.contacts[0];

            // 법선 벡타가 이루는 회전각도 추출
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

            // 충돌 지점에 이펙트 생성           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            transform.position = contact.point;
            Destroy(effect, 0.3f);
            AudioManager.AM.PlaySE(revolverImpact);            
            PV.RPC(nameof(DestroyBullet), RpcTarget.AllBuffered);
        }        

        if (collision.collider.CompareTag("BlueTeam") || collision.collider.CompareTag("RedTeam") && PV.IsMine)
        {
            // 충돌지점의 정보를 추출
            ContactPoint contact = collision.contacts[0];

            // 법선 벡타가 이루는 회전각도 추출
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

            // 충돌 지점에 이펙트 생성           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.3f);
            AudioManager.AM.PlaySE(hitPlayer);
            PV.RPC(nameof(DestroyBullet), RpcTarget.AllBuffered);            
        }


        if (collision.collider.CompareTag("Head") && PV.IsMine)
        {
            // 충돌지점의 정보를 추출
            ContactPoint contact = collision.contacts[0];

            // 법선 벡타가 이루는 회전각도 추출
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

            // 충돌 지점에 이펙트 생성           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.3f);           
            AudioManager.AM.PlaySE(hitPlayer);
            PV.RPC(nameof(DestroyBullet), RpcTarget.AllBuffered);
        }
        if (collision.collider.CompareTag("Body") && PV.IsMine)
        {
            // 충돌지점의 정보를 추출
            ContactPoint contact = collision.contacts[0];

            // 법선 벡타가 이루는 회전각도 추출
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

            // 충돌 지점에 이펙트 생성           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.3f);            
            AudioManager.AM.PlaySE(hitPlayer);
            PV.RPC(nameof(DestroyBullet), RpcTarget.AllBuffered);
        }   

        else
        {
            Destroy(gameObject, 1);
        }
    }


    [PunRPC]
    public void BulletDir(float speed, int actorNumber)//,int addSpeed)
    {
        this.speed = speed;
        actNumber = actorNumber;        
    }

    [PunRPC]
    public void DestroyBullet()=> Destroy(gameObject);   

}
