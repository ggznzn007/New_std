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
using Photon.Pun.Demo.Procedural;
using UnityEngine.InputSystem.HID;
public class Arrow_MultiShot : Arrow
{
    public static Arrow_MultiShot AMS;
    public SphereCollider tagColl;
    public ParticleSystem[] effects;
    public GameObject[] arrowMesh;
    public TrailRenderer[] tails;
    private bool isRotate;
    public Rigidbody[] mulRid;
    public float zVel2 = 0;
    public float zVel3 = 0;
    private float plusSpeed = 1.8f;
    //public ParticleSystem typing;

    protected override void Awake()
    {
        base.Awake();
        AMS = this;
        isRotate = true;
        //typing.gameObject.SetActive(true);
        tagColl.tag = "Skilled";
    }
    /* private void Start()
     {
         //ArrowMul = this;

         isRotate = true;

     }*/
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        PV.RequestOwnership();
        DataManager.DM.grabArrow = true;
        isRotate = false;
        //rotSpeed = 0;

    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        DataManager.DM.grabArrow = false;
        if (PV.IsMine)
        {
            PV.RPC(nameof(Multipack), RpcTarget.AllBuffered);
        }

        tagColl.tag = "Arrow";
        if (args.interactorObject is Notch notch)
        {
            //GetTarget();
            tagColl.tag = "Arrow";
            if (notch.CanRelease)
            {
                DataManager.DM.arrowNum = 2;
                LaunchArrow(notch);
                if (PV.IsMine)
                {
                    // if (!PV.IsMine) return;
                    PV.RPC(nameof(Tailpack), RpcTarget.AllBuffered);
                    PV.RPC(nameof(DelayEX), RpcTarget.AllBuffered);
                }

            }
        }

    }

    public new void LaunchArrow(Notch notch)
    {

        isGrip = false;
        launched = true;
        flightTime = 0f;
        transform.parent = null;
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        mulRid[0].isKinematic = false;
        mulRid[0].useGravity = true;
        mulRid[0].collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        //rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        mulRid[0].constraints = RigidbodyConstraints.FreezeRotation;

        mulRid[1].isKinematic = false;
        mulRid[1].useGravity = true;
        mulRid[1].collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        //rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        mulRid[1].constraints = RigidbodyConstraints.FreezeRotation;
        ApplyForce(notch.PullMeasurer);
        ApplyForce2(notch.PullMeasurer);
        ApplyForce3(notch.PullMeasurer);
        //StartCoroutine(ReEnableCollider());
        // StartCoroutine(LaunchRoutine());
        DataManager.DM.grabArrow = false;
    }

    public new void ApplyForce(PullMeasurer pullMeasurer)
    {
        //rigidbody.AddForce(transform.forward * (pullMeasurer.PullAmount * speed), ForceMode.Impulse);
        rigidbody.AddForce(shootPoint.forward * (pullMeasurer.PullAmount * speed), ForceMode.VelocityChange);

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

    public void ApplyForce2(PullMeasurer pullMeasurer)
    {
        mulRid[0].AddForce(shootPoint.forward * (pullMeasurer.PullAmount * speed * plusSpeed), ForceMode.VelocityChange);
        if (mulRid[0] && MinForceHit != 0)
        {
            float zVel = System.Math.Abs(transform.InverseTransformDirection(mulRid[0].velocity).z);

            // Minimum Force not achieved
            if (zVel < MinForceHit)
            {
                return;
            }
        }
    }

    public void ApplyForce3(PullMeasurer pullMeasurer)
    {
        mulRid[1].AddForce(shootPoint.forward * (pullMeasurer.PullAmount * speed * plusSpeed), ForceMode.VelocityChange);
        if (mulRid[1] && MinForceHit != 0)
        {
            float zVel = System.Math.Abs(transform.InverseTransformDirection(mulRid[1].velocity).z);

            // Minimum Force not achieved
            if (zVel < MinForceHit)
            {
                return;
            }
        }
    }


    public new void TrySticky(Collision coll)                               // 화살이 목표물에 박혔을 때 메서드
    {
        Rigidbody colRid = coll.collider.GetComponent<Rigidbody>();

        transform.parent = null;

        if (coll.gameObject.isStatic)
        {
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.isKinematic = true;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;

            /* mulRid[0].collisionDetectionMode = CollisionDetectionMode.Discrete;
             mulRid[0].isKinematic = true;
             mulRid[0].constraints = RigidbodyConstraints.FreezeAll;

             mulRid[1].collisionDetectionMode = CollisionDetectionMode.Discrete;
             mulRid[1].isKinematic = true;
             mulRid[1].constraints = RigidbodyConstraints.FreezeAll;*/
        }

        else if (colRid != null && !colRid.isKinematic)
        {
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = colRid;
            joint.enableCollision = false;
            joint.breakForce = float.MaxValue;
            joint.breakTorque = float.MaxValue;

            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            /*mulRid[0].constraints = RigidbodyConstraints.FreezeAll;
            mulRid[1].constraints = RigidbodyConstraints.FreezeAll;*/
        }
        else if (colRid != null && colRid.isKinematic && coll.transform.localScale == Vector3.one)
        {
            transform.SetParent(coll.transform);
            rigidbody.useGravity = false;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.isKinematic = true;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            rigidbody.WakeUp();

            /*  mulRid[0].useGravity = false;
              mulRid[0].collisionDetectionMode = CollisionDetectionMode.Discrete;
              mulRid[0].isKinematic = true;
              mulRid[0].constraints = RigidbodyConstraints.FreezeAll;
              mulRid[0].WakeUp();

              mulRid[1].useGravity = false;
              mulRid[1].collisionDetectionMode = CollisionDetectionMode.Discrete;
              mulRid[1].isKinematic = true;
              mulRid[1].constraints = RigidbodyConstraints.FreezeAll;
              mulRid[1].WakeUp();*/
        }
        else
        {
            if (coll.transform.localScale == Vector3.one)
            {
                transform.SetParent(coll.transform);
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                /* mulRid[0].constraints = RigidbodyConstraints.FreezeAll;
                 mulRid[1].constraints = RigidbodyConstraints.FreezeAll;*/
            }
            else
            {
                rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
                /*
                                mulRid[0].collisionDetectionMode = CollisionDetectionMode.Discrete;
                                mulRid[0].useGravity = false;
                                mulRid[0].isKinematic = true;

                                mulRid[1].collisionDetectionMode = CollisionDetectionMode.Discrete;
                                mulRid[1].useGravity = false;
                                mulRid[1].isKinematic = true;*/
            }
        }

        /* switch (DataManager.DM.arrowNum)
         {
             case 0:
                 PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);  // 기본 화살
                 break;
             case 1:
                 PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered); // 스킬 화살
                 break;
             case 2:
                 PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);  // 멀티샷
                 break;
             case 3:
                 PV.RPC(nameof(BombArrow), RpcTarget.AllBuffered);  // 폭탄 화살
                 break;
         }*/
    }


    public void TrySticky2(Collision coll)                               // 화살이 목표물에 박혔을 때 메서드
    {
        Rigidbody colRid = coll.collider.GetComponent<Rigidbody>();

        transform.parent = null;

        if (coll.gameObject.isStatic)
        {
            mulRid[0].collisionDetectionMode = CollisionDetectionMode.Discrete;
            mulRid[0].isKinematic = true;
            mulRid[0].constraints = RigidbodyConstraints.FreezeAll;
        }

        else if (colRid != null && !colRid.isKinematic)
        {
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = colRid;
            joint.enableCollision = false;
            joint.breakForce = float.MaxValue;
            joint.breakTorque = float.MaxValue;

            mulRid[0].constraints = RigidbodyConstraints.FreezeAll;
        }

        else if (colRid != null && colRid.isKinematic && coll.transform.localScale == Vector3.one)
        {
            transform.SetParent(coll.transform);

            mulRid[0].useGravity = false;
            mulRid[0].collisionDetectionMode = CollisionDetectionMode.Discrete;
            mulRid[0].isKinematic = true;
            mulRid[0].constraints = RigidbodyConstraints.FreezeAll;
            mulRid[0].WakeUp();
        }

        else
        {
            if (coll.transform.localScale == Vector3.one)
            {
                transform.SetParent(coll.transform);
                mulRid[0].constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                mulRid[0].collisionDetectionMode = CollisionDetectionMode.Discrete;
                mulRid[0].useGravity = false;
                mulRid[0].isKinematic = true;
            }
        }

        // PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);  // 멀티샷



    }


    public void TrySticky3(Collision coll)                               // 화살이 목표물에 박혔을 때 메서드
    {
        Rigidbody colRid = coll.collider.GetComponent<Rigidbody>();

        transform.parent = null;

        if (coll.gameObject.isStatic)
        {
            mulRid[1].collisionDetectionMode = CollisionDetectionMode.Discrete;
            mulRid[1].isKinematic = true;
            mulRid[1].constraints = RigidbodyConstraints.FreezeAll;
        }

        else if (colRid != null && !colRid.isKinematic)
        {
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = colRid;
            joint.enableCollision = false;
            joint.breakForce = float.MaxValue;
            joint.breakTorque = float.MaxValue;

            mulRid[1].constraints = RigidbodyConstraints.FreezeAll;
        }

        else if (colRid != null && colRid.isKinematic && coll.transform.localScale == Vector3.one)
        {
            transform.SetParent(coll.transform);

            mulRid[1].useGravity = false;
            mulRid[1].collisionDetectionMode = CollisionDetectionMode.Discrete;
            mulRid[1].isKinematic = true;
            mulRid[1].constraints = RigidbodyConstraints.FreezeAll;
            mulRid[1].WakeUp();
        }

        else
        {
            if (coll.transform.localScale == Vector3.one)
            {
                transform.SetParent(coll.transform);
                mulRid[1].constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                mulRid[1].collisionDetectionMode = CollisionDetectionMode.Discrete;
                mulRid[1].useGravity = false;
                mulRid[1].isKinematic = true;
            }
        }

        /*switch (DataManager.DM.arrowNum)
        {
            case 0:
                PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);  // 기본 화살
                break;
            case 1:
                PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered); // 스킬 화살
                break;
            case 2:
                PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);  // 멀티샷
                break;
            case 3:
                PV.RPC(nameof(BombArrow), RpcTarget.AllBuffered);  // 폭탄 화살
                break;
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (transform.parent != null && collision.transform == transform.parent)
        {
            return;
        }
        if (PV.IsMine)
        {
            if (!isGrip && launched && !rigidbody.isKinematic)
            {
                TrySticky(collision);
            }
            if (!isGrip && launched && !mulRid[0].isKinematic)
            {
                TrySticky2(collision);
            }
            if (!isGrip && launched && !mulRid[1].isKinematic)
            {
                TrySticky3(collision);
            }
        }

        if (collision.collider.CompareTag("Head"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip)
                {
                    try
                    {
                        AudioManager.AM.PlaySE(headShot);
                        if (AvartarController.ATC.isAlive && DataManager.DM.inGame)
                        {
                            if (!AvartarController.ATC.isDamaged)
                            {
                                AvartarController.ATC.HeadShotDamage();
                            }
                        }
                        //StartCoroutine(CollOff());
                        ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                        var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                        transform.position = contact.point;
                        Destroy(effect, 0.5f);
                        switch (DataManager.DM.arrowNum)
                        {
                            case 0:
                            case 1:
                            case 2:
                                PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered); // 스킬 화살
                                break;
                            case 3:
                                PV.RPC(nameof(BombArrow), RpcTarget.AllBuffered);  // 폭탄 화살
                                break;
                        }
                    }
                    finally
                    {
                        AudioManager.AM.PlaySE(headShot);
                        if (AvartarController.ATC.isAlive && DataManager.DM.inGame)
                        {
                            if (!AvartarController.ATC.isDamaged)
                            {
                                AvartarController.ATC.HeadShotDamage();
                            }
                        }
                        //StartCoroutine(CollOff());
                        ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                        var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                        transform.position = contact.point;
                        Destroy(effect, 0.5f);
                        switch (DataManager.DM.arrowNum)
                        {
                            case 0:
                            case 1:
                            case 2:
                                PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered); // 스킬 화살
                                break;
                            case 3:
                                PV.RPC(nameof(BombArrow), RpcTarget.AllBuffered);  // 폭탄 화살
                                break;
                        }
                    }
                }

            }
        }

        if (collision.collider.CompareTag("Body"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip)
                {
                    try
                    {
                        AudioManager.AM.PlaySE(hitPlayer);
                        if (AvartarController.ATC.isAlive && DataManager.DM.inGame)
                        {
                            if (!AvartarController.ATC.isDamaged)
                            {
                                AvartarController.ATC.NormalDamage();
                            }
                        }
                        // StartCoroutine(CollOff());
                        ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                        var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                        transform.position = contact.point;
                        Destroy(effect, 0.5f);
                        switch (DataManager.DM.arrowNum)
                        {
                            case 0:
                            case 1:
                            case 2:
                                PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered); // 스킬 화살
                                break;
                            case 3:
                                PV.RPC(nameof(BombArrow), RpcTarget.AllBuffered);  // 폭탄 화살
                                break;
                        }
                    }
                    finally
                    {
                        AudioManager.AM.PlaySE(hitPlayer);// StartCoroutine(CollOff());
                        if (AvartarController.ATC.isAlive && DataManager.DM.inGame)
                        {
                            if (!AvartarController.ATC.isDamaged)
                            {
                                AvartarController.ATC.NormalDamage();
                            }
                        }
                        ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                        var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                        transform.position = contact.point;
                        Destroy(effect, 0.5f);
                        switch (DataManager.DM.arrowNum)
                        {
                            case 0:
                            case 1:
                            case 2:
                                PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered); // 스킬 화살
                                break;
                            case 3:
                                PV.RPC(nameof(BombArrow), RpcTarget.AllBuffered);  // 폭탄 화살
                                break;
                        }
                    }

                }

            }
        }

        if (collision.collider.CompareTag("Cube") || collision.collider.CompareTag("Obtacle"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip)
                {
                    try
                    {
                        AudioManager.AM.PlaySE(aImpact);
                        ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                        var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                        transform.position = contact.point;
                        Destroy(effect, 0.5f);
                        switch (DataManager.DM.arrowNum)
                        {
                            case 0:
                                PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);  // 기본 화살
                                break;
                            case 1:
                                PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered); // 스킬 화살
                                break;
                            case 2:
                                PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);  // 멀티샷
                                break;
                            case 3:
                                PV.RPC(nameof(BombArrow), RpcTarget.AllBuffered);  // 폭탄 화살
                                break;
                        }
                    }
                    finally
                    {
                        AudioManager.AM.PlaySE(aImpact);
                        ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                        var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                        transform.position = contact.point;
                        Destroy(effect, 0.5f);
                        switch (DataManager.DM.arrowNum)
                        {
                            case 0:
                                PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);  // 기본 화살
                                break;
                            case 1:
                                PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered); // 스킬 화살
                                break;
                            case 2:
                                PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);  // 멀티샷
                                break;
                            case 3:
                                PV.RPC(nameof(BombArrow), RpcTarget.AllBuffered);  // 폭탄 화살
                                break;
                        }
                    }
                }

            }
        }

        if (collision.collider.CompareTag("Shield"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip)
                {
                    try
                    {
                        AudioManager.AM.PlaySE(sImpact);
                        ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                        var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                        transform.position = contact.point;
                        Destroy(effect, 0.5f);
                        switch (DataManager.DM.arrowNum)
                        {
                            case 0:
                                PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);  // 기본 화살
                                break;
                            case 1:
                                PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered); // 스킬 화살
                                break;
                            case 2:
                                PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);  // 멀티샷
                                break;
                            case 3:
                                PV.RPC(nameof(BombArrow), RpcTarget.AllBuffered);  // 폭탄 화살
                                break;
                        }
                    }
                    finally
                    {
                        AudioManager.AM.PlaySE(sImpact);
                        ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                        var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                        transform.position = contact.point;
                        Destroy(effect, 0.5f);
                        switch (DataManager.DM.arrowNum)
                        {
                            case 0:
                                PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);  // 기본 화살
                                break;
                            case 1:
                                PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered); // 스킬 화살
                                break;
                            case 2:
                                PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);  // 멀티샷
                                break;
                            case 3:
                                PV.RPC(nameof(BombArrow), RpcTarget.AllBuffered);  // 폭탄 화살
                                break;
                        }
                    }
                }

            }
        }
    }
    private void FixedUpdate()
    {
        if (!isGrip && mulRid[0] != null && rigidbody.velocity != Vector3.zero && launched && zVel > 0.02)
        {
            rigidbody.rotation = Quaternion.LookRotation(rigidbody.velocity);
        }
        zVel = transform.InverseTransformDirection(rigidbody.velocity).z;

        if (!isGrip && mulRid[0] != null && mulRid[0].velocity != Vector3.zero && launched && zVel2 > 0.02)
        {
            mulRid[0].rotation = Quaternion.LookRotation(mulRid[0].velocity);
        }
        zVel2 = transform.InverseTransformDirection(mulRid[0].velocity).z;

        if (!isGrip && mulRid[1] != null && mulRid[1].velocity != Vector3.zero && launched && zVel3 > 0.02)
        {
            mulRid[1].rotation = Quaternion.LookRotation(mulRid[1].velocity);
        }
        zVel3 = transform.InverseTransformDirection(mulRid[1].velocity).z;


        if (isRotate)
        {
            transform.Rotate(rotSpeed * Time.deltaTime * new Vector3(0, 0, 1));
        }
        else
        {
            rotSpeed = 0;
        }
    }

    [PunRPC]
    public void DelayEX()
    {
        StartCoroutine(DelayEffect());
    }

    [PunRPC]
    public void Multipack()
    {
        arrowMesh[0].SetActive(true);
        arrowMesh[1].SetActive(true);
        arrowMesh[2].SetActive(true);
        arrowMesh[3].SetActive(false);
    }

    [PunRPC]
    public void Tailpack()
    {
        StartCoroutine(TailCtrl());
    }

    /*[PunRPC]
    public void HideType()
    {
        typing.gameObject.SetActive(false);
    }*/

    public IEnumerator DelayEffect()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        effects[0].gameObject.SetActive(true);
        effects[1].gameObject.SetActive(true);
        effects[2].gameObject.SetActive(true);
    }

    public IEnumerator TailCtrl()
    {
        tails[0].gameObject.SetActive(true);
        tails[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        tails[0].gameObject.SetActive(false);
        tails[1].gameObject.SetActive(false);
    }
}
