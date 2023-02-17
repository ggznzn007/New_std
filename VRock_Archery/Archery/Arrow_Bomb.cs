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
public class Arrow_Bomb : Arrow
{   
    public Collider tagColl;
    public ParticleSystem fireEX;
    public GameObject myMesh;
    public GameObject myEX;
    private bool isRotate;
    
    private readonly float delTime = 0.4f;
    private readonly float arrowTime = 0.8f;
    private readonly float bombTime = 1f;
    private readonly float beepTime = 2.34f;
    //public AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();        
        isRotate = true;     
                
    }
   
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        PV.RequestOwnership();
        DataManager.DM.grabArrow = true;       
        isRotate = false; 
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        DataManager.DM.grabArrow = false;
        if (args.interactorObject is Notch notch)
        {    
            if (notch.CanRelease)
            {
                DataManager.DM.arrowNum = 3;               
                  
                //audioSource.Stop();
                    LaunchArrow(notch);
               /* if (PV.IsMine)
                {
                    if (!PV.IsMine) return;
                    PV.RPC(nameof(Active_EX), RpcTarget.AllBuffered);
                }*/

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

        string colNameLower = collision.transform.name.ToLower();

        //if (flightTime < 0.01f && (colNameLower.Contains("bow")|| colNameLower.Contains("arrow")))
        if (colNameLower.Contains("bow"))
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
                    /*if (AvartarController.ATC.isAlive && DataManager.DM.inGame)
                    {
                        if (!AvartarController.ATC.isDamaged)
                        {
                            AvartarController.ATC.HeadShotDamage();
                        }
                    }*/
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                    var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    Destroy(effect, delTime);
                    PV.RPC(nameof(BombA), RpcTarget.AllBufferedViaServer);

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
                   /* if (AvartarController.ATC.isAlive && DataManager.DM.inGame)
                    {
                        if (!AvartarController.ATC.isDamaged)
                        {
                            AvartarController.ATC.NormalDamage();
                        }
                    }*/
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                    var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;
                    Destroy(effect, delTime);
                    PV.RPC(nameof(BombA), RpcTarget.AllBufferedViaServer);

                }
            }
        }

        if (collision.collider.CompareTag("Cube") || collision.collider.CompareTag("Obtacle"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    AudioManager.AM.PlaySE(aImpact);
                    TrySticky(collision);                   
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡타가 이루는 회전각도 추출                           
                    var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성
                    //transform.position = contact.point+new Vector3(-contact.normal.x, -contact.normal.y, -contact.normal.z);
                    //transform.position = contact.point;                                             
                    Destroy(effect, delTime);
                    PV.RPC(nameof(BombA), RpcTarget.AllBufferedViaServer);                  
                    
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
                    Destroy(effect, delTime);
                    PV.RPC(nameof(BombA), RpcTarget.AllBufferedViaServer);                   
                    
                }

            }
        }
    }

  

    public new void TrySticky(Collision coll)                               // 화살이 목표물에 박혔을 때 메서드
    {
        Rigidbody colRid = coll.collider.GetComponent<Rigidbody>();
        transform.parent = null;

        if (coll.gameObject.isStatic) // 정적오브젝트
        {
            //transform.SetParent(coll.transform);
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.isKinematic = true;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        else if (colRid != null && !colRid.isKinematic) // 리지드바디가 있고 동적오브젝트
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

    private void Update()
    {
        RotArrow();
    }

    public void RotArrow()
    {
        if (isRotate)
        {
            transform.Rotate(rotSpeed * Time.deltaTime * new Vector3(0, 0, 1));
        }
        else
        {
            rotSpeed = 0;
        }
    }
    

  /*  [PunRPC]
    public void Active_EX()
    {
       StartCoroutine(ActiveCtrl());
    }*/

    [PunRPC]
    public void BombA()
    {
        StartCoroutine(Explode());
    }

    [PunRPC]
    public void BombArrow()
    {
        //Instantiate(effect, pos, rot);
        //Destroy(bombEx, 1.5f);
        StartCoroutine(ExOnOff());
        
    }

    public IEnumerator ExOnOff()
    {
        effect.SetActive(true);        
        yield return new WaitForSeconds(1.5f);
        effect.SetActive(false);
        Destroy(PV.gameObject);
    }
   

    public IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.1f);
        AudioManager.AM.PlaySX(bombBeep);
        yield return new WaitForSeconds(2.35f);
        myMesh.SetActive(false);
        myEX.SetActive(false);
        PV.RPC(nameof(BombArrow), RpcTarget.AllBuffered);

        // AudioManager.AM.PlaySE(bombExplo);       
    }


    [PunRPC]
    public void DestroyBomb()
    {
        Destroy(gameObject, bombTime);
    }
   /* public IEnumerator ActiveCtrl()
    {
        fireEX.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        fireEX.gameObject.SetActive(false);
    }*/
}
