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

public class SnowBall : XRGrabInteractable
{
    public float speed;                                        // 눈덩이 속도
    public float flightTime = 0f;                              // 눈덩이 비행시간
    public PhotonView PV;                                      // 포톤뷰
    public new Rigidbody rigidbody;                            // 리지드바디
    public Collider myColl;                                    // 인터렉션위한 콜라이더   
    public Collider damageColl;                                // 충돌감지 콜라이더
    public Transform shootPoint;                               // 발사 방향을 위한 트랜스폼 - 발사 위치
    public GameObject effect;                                  // 효과          
    public ParticleSystem ballEX;                              // 눈덩이 충돌효과EX
    public GameObject wording_Cr;                              // 크리티컬 대미지 문구 EX
    public GameObject wording_Hit;                             // 일반 대미지 문구 EX
    public int actNumber;                                      // 액터넘버
    protected RaycastHit hit;                                  // 충돌지점
    public bool launched = false;                              // 눈덩이 발사 여부
    public bool isGrip;                                        // 눈덩이 그립 여부
    public string snowImpact;                                  // 눈덩이 충돌효과EX 오디오재생을 위한 문자열
    public string tImpact;                                     // 방패 충돌 시 오디오재생을 위한 문자열 
    public string headShot;                                    // 머리 충돌 시 오디오재생을 위한 문자열
    public string hitPlayer;                                   // 몸 충돌 시 오디오재생을 위한 문자열       
    public float MinForceHit = 0.02f;                          // 충돌 시 최소시간
    public float zVel = 0;                                     // 눈덩이 z축 회전값   
    protected readonly float arrowTime = 0.8f;                 // 눈덩이 파괴 제한시간    

    protected override void Awake()
    {
        base.Awake();
        PV = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody>();
        isGrip = true;               
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        DataManager.DM.grabBall = true;              
        isGrip = true;        
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        isGrip = false;       
        DataManager.DM.grabBall = false;

        if (args.interactorObject is Notch_S notchs)
        {
            if (notchs.CanRelease)
            {
                DataManager.DM.arrowNum = 0;
                LaunchBall(notchs);                
            }
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

    public void LaunchBall(Notch_S notchs)
    {
        isGrip = false;
        launched = true;
        flightTime = 0f;   
        StartCoroutine(OnDamColl());
        transform.parent = null;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;       
        ApplyForce(notchs.PullMeasurer_S);
        DataManager.DM.grabBall = false;
        //rigidbody.isKinematic = false;
        //rigidbody.useGravity = true;
        //rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        //rigidbody.constraints = RigidbodyConstraints.None;        
        //StartCoroutine(ReEnableCollider());
        // StartCoroutine(LaunchRoutine());
    }

    public void ApplyForce(PullMeasurer_S pullMeasurers)
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

    public IEnumerator OnDamColl()                      // 그립콜라이더와 대미지콜라이더를 바꿔서 끄고 켜주면서 충돌가능하게 해주는 메서드
    {
        myColl.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.03f);
        damageColl.gameObject.SetActive(true);
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
         if (flightTime < 0.07f&&(colNameLower.Contains("head") || colNameLower.Contains("body")||colNameLower.Contains("slingshot")))
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
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡터가 이루는 회전각도 추출                           
                    var effect = Instantiate(ballEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered); // 기본 눈덩이
                }// 눈덩이 충돌EX 파괴메서드는 눈덩이 충돌EX에 붙어있음
            }

        }

      /*  if (collision.collider.CompareTag("Body"))
        {
            if (PV.IsMine)
            {
                if (!isGrip)
                {                   
                    AudioManager.AM.PlaySE(hitPlayer);
                    if (AvartarController.ATC.isAlive && DataManager.DM.inGame)
                    {
                        if (!AvartarController.ATC.isDamaged)
                        {
                            AvartarController.ATC.NormalDamage();
                        }
                    }
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                    var effect = Instantiate(wording_Hit, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    Destroy(effect, delTime);
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered); // 스킬 화살
                }
            }
        }

        if (collision.collider.CompareTag("Head"))
        {
            if (PV.IsMine)
            {
                if (!isGrip)
                {                   
                    AudioManager.AM.PlaySE(hitPlayer);
                    if (AvartarController.ATC.isAlive && DataManager.DM.inGame)
                    {
                        if (!AvartarController.ATC.isDamaged)
                        {
                            AvartarController.ATC.HeadShotDamage();
                        }
                    }
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                    var effect = Instantiate(wording_Cr, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    Destroy(effect, delTime);
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered); // 스킬 화살
                }
            }
        }*/
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
                  /*  if (AvartarController.ATC.isAlive && DataManager.DM.inGame)
                    {
                        if (!AvartarController.ATC.isDamaged)
                        {
                            AvartarController.ATC.HeadShotDamage();
                        }
                    }*/
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡터가 이루는 회전각도 추출                            
                    var effect = Instantiate(wording_Cr, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered); // 기본 눈덩이                
                }// 눈덩이 충돌EX 파괴메서드는 눈덩이 충돌EX에 붙어있음

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
                    /*if (AvartarController.ATC.isAlive && DataManager.DM.inGame)
                    {
                        if (!AvartarController.ATC.isDamaged)
                        {
                            AvartarController.ATC.NormalDamage();
                        }
                    }*/
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡터가 이루는 회전각도 추출                            
                    var effect = Instantiate(wording_Hit, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered); // 기본 눈덩이                    
                }// 눈덩이 충돌EX 파괴메서드는 눈덩이 충돌EX에 붙어있음
            }
        }

        if (collision.collider.CompareTag("FloorBox") || collision.collider.CompareTag("Cube") || collision.collider.CompareTag("Obtacle"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    AudioManager.AM.PlaySE(snowImpact);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡터가 이루는 회전각도 추출                            
                    var effect = Instantiate(ballEX, contact.point, rot);// 충돌 지점에 이펙트 생성
                    //transform.position = contact.point + new Vector3(-contact.normal.x, -contact.normal.y, -contact.normal.z);
                    //transform.position = contact.point;
                    TrySticky(collision);                    
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered);  // 기본 눈덩이 
                }// 눈덩이 충돌EX 파괴메서드는 눈덩이 충돌EX에 붙어있음
            }
        }

       
        if (collision.collider.CompareTag("SlingShot"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    PV.RPC(nameof(ImpactS), RpcTarget.AllBuffered);
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡터가 이루는 회전각도 추출                            
                    var effect = Instantiate(ballEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    //transform.position = contact.point;
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered);  // 기본 눈덩이                                      
                }// 눈덩이 충돌EX 파괴메서드는 눈덩이 충돌EX에 붙어있음

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
                    AudioManager.AM.PlaySE(snowImpact);
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡터가 이루는 회전각도 추출                            
                    var effect = Instantiate(ballEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered);  // 기본 눈덩이 
                }// 눈덩이 충돌EX 파괴메서드는 눈덩이 충돌EX에 붙어있음
            }
        }
    }

    public void TrySticky(Collision coll)                               // 화살이 목표물에 박혔을 때 메서드
    {
        Rigidbody colRid = coll.collider.GetComponent<Rigidbody>();
        transform.parent = null;

        if (coll.gameObject.isStatic) // 정적오브젝트
        {
            // transform.SetParent(coll.transform);
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.isKinematic = true;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        else if (colRid != null && !colRid.isKinematic) // 리지드바디가 있는 동적오브젝트
        {
            transform.SetParent(coll.transform);
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = colRid;
            joint.enableCollision = false;
            joint.breakForce = float.MaxValue;
            joint.breakTorque = float.MaxValue;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else if (colRid != null && colRid.isKinematic && coll.transform.localScale == Vector3.one) // 리지드바디가 있고 정적이고 로컬스케일이 1
        {
            transform.SetParent(coll.transform);
            rigidbody.useGravity = false;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.isKinematic = true;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            rigidbody.WakeUp();
        }
        else
        {
            if (coll.transform.localScale == Vector3.one) // 로컬스케일이 1
            {
                transform.SetParent(coll.transform);
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
            }
        }
    
    }

   
    [PunRPC]
    public void DestroyBall()
    {
        Destroy(gameObject);
       // Debug.Log("눈덩이가 즉시파괴되었습니다.");
    }

    [PunRPC]
    public void DelayBall()
    {
        Destroy(gameObject, arrowTime);
       // Debug.Log("눈덩이가 딜레이파괴되었습니다.");
    }

    [PunRPC]
    public void ImpactS()
    {
        AudioManager.AM.PlaySE(tImpact);
    }    
}
