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
    public float speed;
    public float flightTime = 0f;
    public PhotonView PV;
    public new Rigidbody rigidbody;
    public Collider myColl;   
    public Collider damageColl;
    //public Collider bodyColl;
   // public Collider headColl;   
   
    public Transform shootPoint;
    public GameObject effect;
    public ParticleSystem ballEX;
    public GameObject wording_Cr;
    public GameObject wording_Hit;
    public int actNumber;

    protected RaycastHit hit;
    public bool launched = false;
    public bool isGrip;    
    public string snowImpact;
    public string tImpact;
    public string headShot;
    public string hitPlayer;
    public string bombBeep;
    public string bombExplo;
    public float MinForceHit = 0.02f;
    public float zVel = 0;
    protected readonly float delTime = 0.4f;
    protected readonly float arrowTime = 0.8f;
    protected readonly float bombTime = 1f;
    protected readonly float beepTime = 2.34f;

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
                
                /*if (PV.IsMine)
                {
                    if (!PV.IsMine) return;
                    //PV.RPC(nameof(LaunchArrow), RpcTarget.AllBuffered,notch);
                    PV.RPC(nameof(ActiveColl), RpcTarget.AllBuffered);
                }*/
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
        //rigidbody.isKinematic = false;
        //rigidbody.useGravity = true;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;       
        //rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        //rigidbody.constraints = RigidbodyConstraints.None;        
        ApplyForce(notchs.PullMeasurer_S);
        //StartCoroutine(ReEnableCollider());
        // StartCoroutine(LaunchRoutine());
        DataManager.DM.grabBall = false;
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

    public IEnumerator OnDamColl()
    {
        myColl.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.05f);
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
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                    var effect = Instantiate(ballEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered);  // 기본 화살
                    Destroy(effect, delTime);
                }
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
                    /*if (AvartarController.ATC.isAlive && DataManager.DM.inGame)
                    {
                        if (!AvartarController.ATC.isDamaged)
                        {
                            AvartarController.ATC.NormalDamage();
                        }
                    }*/
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
                    AudioManager.AM.PlaySE(snowImpact);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                    var effect = Instantiate(ballEX, contact.point, rot);// 충돌 지점에 이펙트 생성
                    //transform.position = contact.point + new Vector3(-contact.normal.x, -contact.normal.y, -contact.normal.z);
                    //transform.position = contact.point;
                    TrySticky(collision);
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
                    PV.RPC(nameof(ImpactS), RpcTarget.AllBuffered);
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
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                    var effect = Instantiate(ballEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered);  // 기본 화살
                    Destroy(effect, delTime);

                }

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
        Debug.Log("눈덩이가 즉시파괴되었습니다.");
    }

    [PunRPC]
    public void DelayBall()
    {
        Destroy(gameObject, arrowTime);
        Debug.Log("눈덩이가 딜레이파괴되었습니다.");
    }

    [PunRPC]
    public void ImpactS()
    {
        AudioManager.AM.PlaySE(tImpact);
    }
}
