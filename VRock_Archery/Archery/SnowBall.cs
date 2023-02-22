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
    public Collider gripColl;   
    //public GameObject head;   
    public Transform shootPoint;
    public GameObject effect;
    public ParticleSystem arrowEX;
    public GameObject wording_Cr;
    public GameObject wording_Hit;
    public int actNumber;

    private ArrowCaster caster;
    private RaycastHit hit;
    public bool launched = false;
    public bool isGrip;
    public string aImpact;
    public string sImpact;
    public string headShot;
    public string hitPlayer;
    public string bombBeep;
    public string bombExplo;
    public float MinForceHit = 0.02f;
    public float zVel = 0;
    private readonly float delTime = 0.4f;
    private readonly float arrowTime = 0.8f;
    private readonly float bombTime = 1f;
    private readonly float beepTime = 2.34f;

    protected override void Awake()
    {
        base.Awake();
        PV = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody>();
        isGrip = true;
        myColl = GetComponent<Collider>();
        gripColl= GetComponent<Collider>();
        myColl.enabled = false;
        gripColl.enabled = true;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        DataManager.DM.grabArrow = true;
        //PV.RequestOwnership();        
        isGrip = true;
        rigidbody.isKinematic = false;
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        // PV.RequestOwnership();
        rigidbody.isKinematic = true;
        DataManager.DM.grabArrow = false;
        if (args.interactorObject is Notch notch)
        {
            if (notch.CanRelease)
            {
                DataManager.DM.arrowNum = 0;
                LaunchArrow(notch);

                if (PV.IsMine)
                {
                    if (!PV.IsMine) return;
                    //PV.RPC(nameof(LaunchArrow), RpcTarget.AllBuffered,notch);
                   PV.RPC(nameof(ActiveColl), RpcTarget.AllBuffered);
                }
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
    public void LaunchArrow(Notch notch)
    {
        isGrip = false;
        launched = true;
        flightTime = 0f;
        transform.parent = null;
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        //rigidbody.constraints = RigidbodyConstraints.None;        
        ApplyForce(notch.PullMeasurer);
        //StartCoroutine(ReEnableCollider());
        // StartCoroutine(LaunchRoutine());
        DataManager.DM.grabArrow = false;
    }

    public void ApplyForce(PullMeasurer pullMeasurer)
    {
        //rigidbody.AddForce(transform.forward * (pullMeasurer.PullAmount * speed), ForceMode.Impulse);
        rigidbody.AddForce(shootPoint.forward * (pullMeasurer.PullAmount * speed), ForceMode.VelocityChange);
        /*if (rigidbody && MinForceHit != 0)
        {
            float zVel = System.Math.Abs(transform.InverseTransformDirection(rigidbody.velocity).z);

            // Minimum Force not achieved
            if (zVel < MinForceHit)
            {
                return;
            }
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ignore parent collisions
        if (transform.parent != null && collision.transform == transform.parent)
        {
            return;
        }

        if (isGrip) return;

        string colNameLower = collision.transform.name.ToLower();

        if (flightTime < 0.01f && colNameLower.Contains("bow"))//|| colNameLower.Contains("arrow")))
        //if (colNameLower.Contains("bow"))
        {
            Physics.IgnoreCollision(collision.collider, myColl, true);
            return;
        }
        //if (flightTime < 0.01f && (colNameLower.Contains("player") || colNameLower.Contains("lefthand") || colNameLower.Contains("righthand")))
        if (colNameLower.Contains("lefthand") || colNameLower.Contains("righthand"))
        {
            Physics.IgnoreCollision(collision.collider, myColl, true);
            return;
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
                    if (AvartarController.ATC.isAlive && DataManager.DM.inGame)
                    {
                        if (!AvartarController.ATC.isDamaged)
                        {
                            AvartarController.ATC.HeadShotDamage();
                        }
                    }
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                    var effect = Instantiate(wording_Cr, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    Destroy(effect, delTime);
                    PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered); // 스킬 화살
                  
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
                    if (AvartarController.ATC.isAlive && DataManager.DM.inGame)
                    {
                        if (!AvartarController.ATC.isDamaged)
                        {
                            AvartarController.ATC.NormalDamage();
                        }
                    }
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                    var effect = Instantiate(wording_Hit, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    Destroy(effect, delTime);
                    PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered); // 스킬 화살
                   
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
                    AudioManager.AM.PlaySE(aImpact);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                    var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성
                    //transform.position = contact.point + new Vector3(-contact.normal.x, -contact.normal.y, -contact.normal.z);
                    //transform.position = contact.point;
                    TrySticky(collision);
                    Destroy(effect, delTime);
                    PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);  // 기본 화살
                
                }
            }
        }

        if (collision.collider.CompareTag("Shield") || collision.collider.CompareTag("Bow"))
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
                    var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);  // 기본 화살
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


    IEnumerator ActiveDamColl()
    {
        yield return new WaitForSeconds(0.5f);
        myColl.enabled= true;
        gripColl.enabled= false;
    }
    [PunRPC]
    public void ActiveColl()
    {
        StartCoroutine(ActiveDamColl());
    }

    [PunRPC]
    public void DestroyArrow()
    {
        Destroy(gameObject);
        Debug.Log("화살이 즉시파괴되었습니다.");
    }

    [PunRPC]
    public void DelayArrow()
    {
        Destroy(gameObject, arrowTime);
        Debug.Log("화살이 딜레이파괴되었습니다.");
    }

    [PunRPC]
    public void ImpactS()
    {
        AudioManager.AM.PlaySE(sImpact);
    }
}
