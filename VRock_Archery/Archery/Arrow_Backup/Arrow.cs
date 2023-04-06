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

public class Arrow : XRGrabInteractable      // 화살 클래스 XR상속
{
    public static Arrow Ar;                  // 싱글턴
        
    public float speed;                      // 화살 속도
    public float flightTime = 0f;            // 화살 비행 시간
    public PhotonView PV;                    // 포톤뷰
    public new Rigidbody rigidbody;          // 화살 리지드바디 => 물리
    public Collider myColl;                  // 화살 인터렉션(잡고,놓고) 관련 콜라이더 없으면 안됨
    public Collider damageColl;              // 화살이 다른 오브젝트에 충돌했을 때 충돌 감지, 대미지   
    public GameObject head;                  // 화살 머리
    public float rotSpeed;                   // 화살 생성 시 회전 속도
    public Transform shootPoint;             // 화살 발사 방향 포인트
    public GameObject effect;                // 화살 효과
    public ParticleSystem arrowEX;           // 화살 파티클
    public GameObject wording_Cr;            // 화살이 플레이어 머리 명중 시 출력하는 텍스트 오브젝트
    public GameObject wording_Hit;           // 화살이 플레이어 몸 명중 시 출력하는 텍스트 오브젝트
    public int actNumber;                    // 포톤 액터넘버

    private ArrowCaster caster;              // 화살 캐스터
    private RaycastHit hit;                  // 화살 hit 감지 레이캐스트
    public bool launched = false;            // 화살 발사 여부
    public bool isGrip;                      // 화살 그립 여부
    public string aImpact;                   // 화살이 일반 오브젝트 명중 시 출력되는 오디오 호출 문자열
    public string sImpact;                   // 화살이 방패 명중 시 출력되는 오디오 호출 문자열
    public string headShot;                  // 화살이 머리 명중 시 출력되는 오디오 호출 문자열
    public string hitPlayer;                 // 화살이 몸  명중 시 출력되는 오디오 호출 문자열
    public string bombBeep;                  // 폭탄 화살이 오브젝트 명중 시 출력되는 폭탄 예비음 오디오 호출 문자열
    public string bombExplo;                 // 폭탄 화살이 오브젝트 명중하고 폭발할 때 출력되는 오디오 호출 문자열
    public float MinForceHit = 0.02f;        // 화살이 목표물에 도달할 때 까지 시간
    public float zVel = 0;                   // 화살 z축 회전값
    private readonly float delTime = 0.4f;   // 임팩트 EX 사라지는 시간
    private readonly float arrowTime = 0.7f; // 화살 사라지는 시간
  
    protected override void Awake()           // 화살 초기화
    {
        base.Awake();
        Ar = this;
        caster = GetComponent<ArrowCaster>();       
        PV = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody>();
        //isGrip = true;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)     // 화살을 손에 쥐었을 때
    {
        base.OnSelectEntered(args);
        DataManager.DM.grabArrow = true;                                   // 데이터에 화살을 그립여부 저장      
        rotSpeed = 0;                                                      // 기본화살은 회전속도 필요없으므로 0
        isGrip = true;                                                     // 기본화살 내부 그립여부 저장
    }
    protected override void OnSelectExited(SelectExitEventArgs args)       // 화살을 손에 놓았을 때
    {
        base.OnSelectExited(args);       
        rigidbody.useGravity = true;                                       // 화살을 손에서 놓았을 때 중력 적용
        DataManager.DM.grabArrow = false;                                  // 데이터에 화살 그립여부 해제
        
        if (args.interactorObject is Notch notch)                          // 화살이 활 시위에 붙었을 때
        {           
            if (notch.CanRelease)                                          // 화살아 활 시위에서 발사 될 때
            { 
                DataManager.DM.arrowNum = 0;                               // 화살번호 저장
                LaunchArrow(notch);                                        // 화살 발사 메서드 호출

                if (PV.IsMine)
                {
                    if (!PV.IsMine) return;                    
                    PV.RPC(nameof(Trailer), RpcTarget.AllBuffered);        // 화살 발사시 트레일러 호출
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

        if (launched)                                                          // 발사여부 판정으로 비행시간시작
        {
            flightTime += Time.fixedDeltaTime;
        }
    }


    public void LaunchArrow(Notch notch)                                                     // 화살 발사 메서드
    {
        isGrip = false;                                                                      // 손에서 놓았다
        launched = true;                                                                     // 발사되었다
        flightTime = 0f;                                                                     // 비행시간 0으로 초기화
        transform.parent = null;                                                             // 화살의 부모를 해제하고 다른 오브젝트에 붙을 수 있도록 함
        
        StartCoroutine(OnDamColl());
        //rigidbody.isKinematic = false;
       // rigidbody.useGravity = true;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;        // 화살의 물리감지모드를 연속적으로 만듬
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;                        // 화살의 회전값을 고정
        //rigidbody.constraints = RigidbodyConstraints.None;        
        ApplyForce(notch.PullMeasurer);                                                      // 화살에 힘을 가하는 메서드 호출
        //StartCoroutine(ReEnableCollider());
        // StartCoroutine(LaunchRoutine());
        DataManager.DM.grabArrow = false;                                                    // 활 시위에서 화살 그립여부 해제
    }

    public void ApplyForce(PullMeasurer pullMeasurer)                                        // 화살에 힘을 가하는 메서드
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

    public IEnumerator OnDamColl()                   // 화살이 발사된 후에 콜라이더 조작하는 메서드
    {
        //myColl.gameObject.SetActive(false);          // 화살 그립콜라이더를 off
        yield return new WaitForSeconds(0.04f);
        myColl.isTrigger = false;                      // 화살 콜라이더의 트리거를 false로 해서 물리충돌 발생
        //damageColl.gameObject.SetActive(true);       // 화살 대미지판정콜라이더 on
    }  

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("FloorBox") || collision.collider.CompareTag("Cube")  // 활에 붙지 않고 손에서 놓아서 떨어뜨렸을 때
            || collision.collider.CompareTag("Obtacle"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;                
                PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);  // 기본 화살     
            }

        }
        // Ignore parent collisions
        if (transform.parent != null && collision.transform == transform.parent)
        {
            return;
        }
      
        if (isGrip) return;

        string colNameLower = collision.transform.name.ToLower();

       /* if (flightTime < 0.07f && (colNameLower.Contains("head") || colNameLower.Contains("body")))//||colNameLower.Contains("bow")))//|| colNameLower.Contains("default") || colNameLower.Contains("localavatarbody"))
        //if (colNameLower.Contains("bow"))
        {
            Physics.IgnoreCollision(collision.collider, myColl, true);
           // Physics.IgnoreCollision(collision.collider, damageColl, true);
            
            return;
        }*/
        //if (flightTime < 0.01f && (colNameLower.Contains("player") || colNameLower.Contains("lefthand") || colNameLower.Contains("righthand")))
        if (colNameLower.Contains("lefthand") || colNameLower.Contains("righthand"))
        {
            Physics.IgnoreCollision(collision.collider, myColl, true);
           // Physics.IgnoreCollision(collision.collider, damageColl, true);
            return;
        }

        
        /*if (PV.IsMine)
        {
            if (!isGrip && launched)
            {
                TrySticky(collision);
            }
        }*/

        if (collision.collider.CompareTag("Head"))             // 플레이어 머리에 꽂혔을 때
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
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡터가 이루는 회전각도 추출                            
                    var effect = Instantiate(wording_Cr, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;                    
                    PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);
                }// 충돌EX 파괴메서드는 충돌EX에 붙어있음

            }
        }

        if (collision.collider.CompareTag("Body"))            // 플레이어 몸에 꽂혔을 때
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
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡터가 이루는 회전각도 추출                            
                    var effect = Instantiate(wording_Hit, contact.point, rot);// 충돌 지점에 이펙트 생성        
                    transform.position = contact.point;                    
                    PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);
                }// 충돌EX 파괴메서드는 충돌EX에 붙어있음
            }
        }

        if (collision.collider.CompareTag("FloorBox")|| collision.collider.CompareTag("Cube") || collision.collider.CompareTag("Obtacle"))
        {     // 바닥, 기둥, 건물, 장애물 등에 꽂혔을 때
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    AudioManager.AM.PlaySE(aImpact);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡터가 이루는 회전각도 추출                            
                    var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성                    
                    TrySticky(collision);                    
                    PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);
                }// 충돌EX 파괴메서드는 충돌EX에 붙어있음
            }
        }

        if (collision.collider.CompareTag("NPC") || collision.collider.CompareTag("Effect"))            // 독수리 또는 이펙트
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
                    transform.position = contact.point;
                    PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);
                }// 충돌EX 파괴메서드는 충돌EX에 붙어있음
            }
        }

        if (collision.collider.CompareTag("Bow"))                                           // 활이나 방패에 꽂혔을 때
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    PV.RPC(nameof(ImpactS), RpcTarget.AllBuffered);
                    myColl.enabled = false;
                    //TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// 충돌지점의 정보를 추출                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// 법선 벡터가 이루는 회전각도 추출                            
                    var effect = Instantiate(arrowEX, contact.point, rot);// 충돌 지점에 이펙트 생성        
                   //transform.position = contact.point;
                    PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);                     
                }// 충돌EX 파괴메서드는 충돌EX에 붙어있음

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
    public void Trailer()
    {
        StartCoroutine(TrailerCtrl());
    }

    public IEnumerator TrailerCtrl()
    {        
        head.SetActive(true);
        yield return new WaitForSeconds(1);
        head.SetActive(false);        
    }

    [PunRPC]
    public void DestroyArrow()
    {
        Destroy(gameObject);        
    }

    [PunRPC]
    public void DelayArrow()
    {
        Destroy(gameObject, arrowTime);       
    }

    [PunRPC]
    public void ImpactS()
    {
        AudioManager.AM.PlaySE(sImpact);
    }

    [PunRPC]
    public void DelayTagged()
    {
        StartCoroutine(DelayTag());
    }
    public IEnumerator DelayTag()                            // 아이템생성 슬롯 이중생성방지위한 메서드
    {
        yield return new WaitForSeconds(0.1f);
        myColl.tag = "Untagged";
    }

    

    /*  IEnumerator ReEnableCollider()
    {
        // Wait a few frames before re-enabling collider on bow shaft
        // This prevents the arrow from shooting ourselves, the bow, etc.
        // If you want the arrow to still have physics while attached,
        // parent a collider to the arrow near the tip
        int waitFrames = 3;
        for (int x = 0; x < waitFrames; x++)
        {
            yield return new WaitForFixedUpdate();
        }
        myColl.enabled = true;
    }

    public IEnumerator LaunchRoutine()
    {
        // Set direction while flying
        while (!caster.CheckForCollision(out hit))
        {
            // SetDirection();
            yield return null;
        }

        // Once the arrow has stopped flying
        // DisablePhysics();
        // ChildArrow(hit);
        //  CheckForHittable(hit);        
    }

    public void SetDirection()
    {
        if (rigidbody.velocity.z > 0.5f)
            transform.forward = rigidbody.velocity;
    }

    public void DisablePhysics()
    {
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
    }

    public void ChildArrow(RaycastHit hit)
    {
        transform.SetParent(hit.transform);       
    }

    public void CheckForHittable(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent(out IArrowHittable hittable))
            hittable.Hit(this);
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        return base.IsSelectableBy(interactor) && !launched;
    }*/                      // 이전코드

    /*    [SerializeField] private float speed = 2000.0f;

        private new Rigidbody rigidbody;
        private ArrowCaster caster;

        private bool launched = false;

        private RaycastHit hit;

        protected override void Awake()
        {
            base.Awake();
            rigidbody = GetComponent<Rigidbody>();
            caster = GetComponent<ArrowCaster>();
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            if (args.interactorObject is Notch notch)
            {
                if (notch.CanRelease)
                    LaunchArrow(notch);
            }
        }

        private void LaunchArrow(Notch notch)
        {
            launched = true;
            ApplyForce(notch.PullMeasurer);
            StartCoroutine(LaunchRoutine());
        }

        private void ApplyForce(PullMeasurer pullMeasurer)
        {
            rigidbody.AddForce(transform.forward * (pullMeasurer.PullAmount * speed));
        }

        private IEnumerator LaunchRoutine()
        {
            // Set direction while flying
            while (!caster.CheckForCollision(out hit))
            {
                SetDirection();
                yield return null;
            }

            // Once the arrow has stopped flying
            DisablePhysics();
            ChildArrow(hit);
            CheckForHittable(hit);
        }

        private void SetDirection()
        {
            if (rigidbody.velocity.z > 0.5f)
                transform.forward = rigidbody.velocity;
        }

        private void DisablePhysics()
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }

        private void ChildArrow(RaycastHit hit)
        {
            transform.SetParent(hit.transform);
        }

        private void CheckForHittable(RaycastHit hit)
        {
            if (hit.transform.TryGetComponent(out IArrowHittable hittable))
                hittable.Hit(this);
        }

        public override bool IsSelectableBy(IXRSelectInteractor interactor)
        {
            return base.IsSelectableBy(interactor) && !launched;
        }*/     // 이전코드
}
