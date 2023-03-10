using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using static UnityEngine.ParticleSystem;

public class StoneBall : SnowBall
{
    public string stoneImpact;
    //private int gripCount = 0;
    protected override void Awake()
    {
        base.Awake();
        PV = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody>();
        isGrip = true;
        rigidbody.useGravity = false;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args) // 잡았을때
    {
        base.OnSelectEntered(args);
        DataManager.DM.grabBall = true;
        isGrip = true;
        damageColl.gameObject.SetActive(false);
    }
    protected override void OnSelectExited(SelectExitEventArgs args) // 놓았을때 
    {
        base.OnSelectExited(args);
        isGrip = false;
        rigidbody.useGravity = true;
        damageColl.gameObject.SetActive(true);
        DataManager.DM.grabBall = false;

        if (args.interactorObject is Notch_S notchs)                 // 새총 시위에 붙었을 때
        {
            damageColl.gameObject.SetActive(false);
            if (notchs.CanRelease)
            {
                DataManager.DM.arrowNum = 0;
                LaunchBall(notchs);

                /*if (PV.IsMine)
                {
                    if (!PV.IsMine) return;
                    //PV.RPC(nameof(LaunchArrow), RpcTarget.AllBuffered,notch);
                    PV.RPC(nameof(ActiveColl), RpcTarget.AllBuffered);
                }*/
            }
        }
        else
        {
            flightTime = 1;
        }

    }

    private void FixedUpdate()
    {
        if (!isGrip && rigidbody != null && rigidbody.velocity != Vector3.zero && launched && zVel > 0.02)
        {
            rigidbody.rotation = Quaternion.LookRotation(rigidbody.velocity);
        }

        zVel = transform.InverseTransformDirection(rigidbody.velocity).z;

        if (launched)
        {
            flightTime += Time.fixedDeltaTime;
        }

    }
    public new void LaunchBall(Notch_S notchs)
    {
        isGrip = false;
        launched = true;
        flightTime = 0f;
        StartCoroutine(OnDamColl());
        transform.parent = null;
        //rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        //rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        //rigidbody.constraints = RigidbodyConstraints.None;        
        ApplyForce(notchs.PullMeasurer_S);
        //StartCoroutine(ReEnableCollider());
        // StartCoroutine(LaunchRoutine());
        DataManager.DM.grabBall = false;
    }

    public new void ApplyForce(PullMeasurer_S pullMeasurers)
    {
        //rigidbody.AddForce(transform.forward * (pullMeasurer.PullAmount * speed), ForceMode.Impulse);
        rigidbody.AddForce(shootPoint.forward * (pullMeasurers.PullAmount * speed), ForceMode.VelocityChange);
        if (rigidbody && MinForceHit != 0)
        {
            float zVel = System.Math.Abs(transform.InverseTransformDirection(rigidbody.velocity).z);

            // Minimum Force not achieved
            if (zVel < MinForceHit)
            {
                return;
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {

        // Ignore parent collisions
        if (transform.parent != null && collision.transform == transform.parent)
        {
            return;
        }

        if (isGrip) return;

        string colNameLower = collision.transform.tag.ToLower();

        //if (flightTime < 0.5f && colNameLower.Contains("slingshot"))//|| colNameLower.Contains("arrow")))
        if (flightTime < 0.07f && (colNameLower.Contains("head") || colNameLower.Contains("body")))
        {
            Physics.IgnoreCollision(collision.collider, myColl, true);
            Physics.IgnoreCollision(collision.collider, damageColl, true);
            return;
        }

        //if (flightTime < 0.01f && (colNameLower.Contains("player") || colNameLower.Contains("lefthand") || colNameLower.Contains("righthand")))
        if (colNameLower.Contains("lefthand") || colNameLower.Contains("righthand"))
        {
            Physics.IgnoreCollision(collision.collider, myColl, true);
            Physics.IgnoreCollision(collision.collider, damageColl, true);
            return;
        }

        if (collision.collider.CompareTag("FloorBox") || collision.collider.CompareTag("Cube")
           || collision.collider.CompareTag("Snowblock") || collision.collider.CompareTag("Iceblock")
           || collision.collider.CompareTag("SlingShot") || collision.collider.CompareTag("Obtacle"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip)
                {
                    AudioManager.AM.PlaySE(snowImpact);
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                    var effect = Instantiate(ballEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered);  // 기본 화살
                    Destroy(effect, delTime);
                }
            }

        }


        /*if (PV.IsMine)
        {
            if (!isGrip && launched)
            {
                TrySticky(collision);
            }
        }*/

        if (collision.collider.CompareTag("Head"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    AudioManager.AM.PlaySE(headShot);
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                    var effect = Instantiate(wording_Cr, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    Destroy(effect, delTime);
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered); // 스킬 화살


                }

            }
        }

        if (collision.collider.CompareTag("Body"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    AudioManager.AM.PlaySE(hitPlayer);
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                    var effect = Instantiate(wording_Hit, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    Destroy(effect, delTime);
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered); // 스킬 화살

                }
            }
        }

        if (collision.collider.CompareTag("FloorBox") || collision.collider.CompareTag("Cube") || collision.collider.CompareTag("Obtacle"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    TrySticky(collision);
                    AudioManager.AM.PlaySE(stoneImpact);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                    var effect = Instantiate(ballEX, contact.point, rot);
                    Destroy(effect, delTime);
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered);  // 기본 화살

                }
            }
        }

        if (collision.collider.CompareTag("SlingShot"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    TrySticky(collision);
                    PV.RPC(nameof(ImpactS), RpcTarget.AllBuffered);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                    var effect = Instantiate(ballEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered);  // 기본 화살
                    Destroy(effect, delTime);

                }

            }
        }
        if (collision.collider.CompareTag("Snowblock") || collision.collider.CompareTag("Iceblock")
            || collision.collider.CompareTag("NPC"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    TrySticky(collision);
                    AudioManager.AM.PlaySE(stoneImpact);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                    var effect = Instantiate(ballEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered);
                    Destroy(effect, delTime);
                }

            }
        }
    }



}
