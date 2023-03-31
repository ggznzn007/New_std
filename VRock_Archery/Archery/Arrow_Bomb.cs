using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;
using PN = Photon.Pun.PN;

public class Arrow_Bomb : Arrow
{
    /// <summary>
    ///  폭탄화살 전체 메커니즘
    ///  1. FireMissileTiny, Sparks == 생성시 활성화
    ///  2. ExploArea == 생성시 비활성화 -> 폭탄화살이 발사되어 목표물에 명중시 활성화되고 폭발예비음 재생, 폭탄 화살 메쉬 비활성화
    ///  3. 폭발 효과 활성화되면 폭발예비음 정지, ExploArea 비활성화, areaColl 짧은 순간 활성화되었다가 대미지 입고 다시 비활성화
    ///  4. 폭탄화살 파괴
    /// </summary>
    public Collider tagColl;                                                       // 폭탄화살 태그 콜라이더
    public Collider areaColl;                                                      // 폭발 대미지 범위 콜라이더    
    public GameObject myMesh;                                                      // 폭탄화살 메쉬
    public GameObject[] myEX;                                                      // 폭탄화살 효과들(FireMissileTiny, Sparks, ExploArea)
    private bool isRotate;                                                         // 회전 여부

    private readonly float delTime = 0.4f;                                         // 딜레이 시간
    private readonly float bombTime = 1f;                                          // 폭발시간

    protected override void Awake()
    {
        base.Awake();
        isRotate = true;                                                           // 회전 on
        rigidbody.useGravity = false;                                              // 중력 off        
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        DataManager.DM.grabArrow = true;                                           // 화살 그립 on
        isRotate = false;                                                          // 회전 off
        isGrip = true;                                                             // 그립 on
        PV.RPC(nameof(DelayTagged), RpcTarget.AllBuffered);
        PV.RPC(nameof(OnColl), RpcTarget.AllBuffered);
        // rigidbody.useGravity = true;
        //damageColl.gameObject.SetActive(false);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        isGrip = false;                                                            // 그립 off
        //damageColl.gameObject.SetActive(true); 
        DataManager.DM.grabArrow = false;                                          // 화살 그립 off
                                               
        if (args.interactorObject is Notch notch)                                  // 활 시위에 장작했을 때
        {
            //damageColl.gameObject.SetActive(false);
            if (notch.CanRelease)
            {
                DataManager.DM.arrowNum = 3;
                LaunchArrow(notch);                                                // 화살 발사 메서드                
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

        //if (isGrip) return;

        string colNameLower = collision.transform.name.ToLower();
                
       /* if (flightTime < 0.05f && (colNameLower.Contains("head") || colNameLower.Contains("body")))
        {
            Physics.IgnoreCollision(collision.collider, myColl, true);
            return;
        }*/
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
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡터가 이루는 회전각도 추출                            
                    var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;                    
                    PV.RPC(nameof(BombA), RpcTarget.AllBuffered);
                } // 충돌EX 파괴메서드는 충돌EX에 붙어있음

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
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡터가 이루는 회전각도 추출                            
                    var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;                    
                    PV.RPC(nameof(BombA), RpcTarget.AllBuffered);
                }// 충돌EX 파괴메서드는 충돌EX에 붙어있음
            }
        }

        if (collision.collider.CompareTag("FloorBox") || collision.collider.CompareTag("Cube") || collision.collider.CompareTag("Obtacle"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)                                 // 화살을 쏘았을때
                {
                    AudioManager.AM.PlaySE(aImpact);
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡터가 이루는 회전각도 추출                            
                    var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성      
                    PV.RPC(nameof(BombA), RpcTarget.AllBuffered);
                }  // 충돌EX 파괴메서드는 충돌EX에 붙어있음
                else if(!isGrip&& !launched)                            // 화살을 쏘지않고 손에서 놓았을 경우
                {
                    PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);
                }
            }
        }

      
        if (collision.collider.CompareTag("NPC") || collision.collider.CompareTag("Effect"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    AudioManager.AM.PlaySE(aImpact);
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡터가 이루는 회전각도 추출                            
                    var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성
                    transform.SetParent(collision.transform);                    
                    PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);
                }// 충돌EX 파괴메서드는 충돌EX에 붙어있음
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
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡터가 이루는 회전각도 추출                            
                    var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;                    
                    PV.RPC(nameof(BombA), RpcTarget.AllBuffered);
                }// 충돌EX 파괴메서드는 충돌EX에 붙어있음

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
            transform.Rotate(rotSpeed * Time.deltaTime * Vector3.forward);
        }
        else
        {
            rotSpeed = 0;
        }
    }

    [PunRPC]
    public void BombA()                                                       // RPC 폭탄 화살이 박히면 실행
    {
        StartCoroutine(Explode());
    }
    public IEnumerator Explode()                                              // 폭탄 화살 코루틴
    {
        AudioManager.AM.PlaySX(bombBeep);                                                      // 폭발 예비음 오디오 재생 
        myEX[2].SetActive(true);                                              // 폭발대미지 범위 표시 ON
        yield return new WaitForSeconds(2);
        StartCoroutine(ExOnOff());
        // PV.RPC(nameof(BombArrow), RpcTarget.AllBuffered);                     // RPC 폭발 파티클 재생
    }

    [PunRPC]
    public void BombArrow()
    {
        StartCoroutine(ExOnOff());
    }

    public IEnumerator ExOnOff()
    {
        AudioManager.AM.StopSX(bombBeep);
        myMesh.SetActive(false);                                              // 폭탄화살 렌더링 OFF
        myEX[0].SetActive(false);                                             // 폭탄화살 효과 OFF
        myEX[1].SetActive(false);                                             // 폭탄화살 효과2 OFF
        myEX[2].SetActive(false);                                             // 폭발대미지 범위 표시 OFF
        effect.SetActive(true);                                               // 폭발파티클 ON
        StartCoroutine(CollCtrl());
        yield return new WaitForSeconds(1.5f);
        effect.SetActive(false);                                              // 폭발파티클 OFF
        Destroy(PV.gameObject);                                               // 폭탄화살 파괴
    }

    IEnumerator CollCtrl()
    {
        areaColl.enabled = true;
        yield return new WaitForSeconds(0.03f);
        areaColl.enabled = false;
    }  

    [PunRPC]
    public void DestroyBomb()
    {
        Destroy(gameObject, bombTime);
    }

    [PunRPC]
    public void OnColl()
    {
        tagColl.gameObject.SetActive(true);
    }
}
