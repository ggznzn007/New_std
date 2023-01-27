using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.PXR;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;

public class RevolverManager : MonoBehaviourPun, IPunObservable
{
    public static RevolverManager RM;
    [Header("총알 프리팹")][SerializeField] GameObject bullet;
    [Header("총구 위치")][SerializeField] Transform firePoint;
    [Header("총알 속도")][SerializeField] float speed;
    [Header("총알 소유권")][SerializeField] bool isBulletMine;
    [Header("액터넘버")][SerializeField] int actorNumber;
    public PhotonView PV;                           // 포톤뷰
    public bool isBeingHeld = false;
    public bool isGrip;

    private RaycastHit hit;                          // 레이캐스트광선 히트
    private Ray ray;                                 // 레이캐스트 광선
    private ParticleSystem muzzleFlash;              // 총구 이펙트                                           
    private AudioSource audioSource;                 // 총알 발사 소리
    private GameObject myBull;                       // 자기 총알    
    private float fireTime = 0;                      // 총알 딜레이 타임 
    private readonly float delayfireTime = 0.3f;    // 총알 딜레이 제한시간
    private readonly float fireDistance = 1000f;     // 총알 비거리   
    private bool triggerBtnR;
    private bool triggerBtnL;
    private Vector3 remotePos;
    private Quaternion remoteRot;
    private Rigidbody rb;

    private void Awake()
    {
        RM = this;        
    }
    
    void Start()
    {
        PV = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        muzzleFlash = firePoint.GetComponentInChildren<ParticleSystem>();  // 하위 컴포넌트 추출 
        actorNumber = PV.OwnerActorNr;
        isGrip = true;
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine)
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, 30 * Time.deltaTime)
                , Quaternion.Lerp(transform.rotation, remoteRot, 30 * Time.deltaTime));
        }       
        GetTarget();       // 표적에 레이캐스트를 쏴서 타겟팅하는 메서드
        Reload();          // 총을 재장전하는 시간 메서드       
        //WhenDead();       // 플레이어가 죽었을때 총이 사라지는 메서드
        ActivateHaptic();

        if (isBeingHeld)               // 총의 입장에서 손에 잡혀있음
        {
            isGrip = true;
            rb.isKinematic = true;
        }
        else
        {
            isGrip = false;
            rb.isKinematic = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Cube") || collision.collider.CompareTag("FloorBox") || collision.collider.CompareTag("Shield"))
        {
            if (PV.IsMine)
            {
                if (!isGrip)
                {
                    try
                    {
                        PV.RPC(nameof(DestroyGun), RpcTarget.AllBuffered);
                    }
                    finally
                    {
                        PV.RPC(nameof(DestroyGun), RpcTarget.AllBuffered);
                    }                    
                }
            }
            /*try
            {
                if (PV.IsMine)
                {
                    if (!isBeingHeld && !isGrip)
                    {
                        PV.RPC(nameof(DestroyGun), RpcTarget.AllBuffered);
                    }
                }
            }
            finally
            {
                if (PV.IsMine)
                {
                    PV.RPC(nameof(DestroyGun), RpcTarget.AllBuffered);
                }
            }*/
        }        
    }

    public void FireBullet()                                              // 컨트롤러 트리거를 이용한 총알 발사로직
    {
        if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit) && AvartarController.ATC.isAlive)
        {
            if (fireTime < delayfireTime) { return; }
            PV.RPC(nameof(Fire_EX), RpcTarget.All);   
            myBull = PN.Instantiate(bullet.name, ray.origin, Quaternion.identity);
            myBull.GetComponent<Rigidbody>().AddRelativeForce(ray.direction * speed, ForceMode.Force);// 질량적용 연속적인 힘을 가함
            myBull.GetComponent<PhotonView>().RPC("BulletDir", RpcTarget.Others, speed, PV.Owner.ActorNumber);
            myBull.GetComponent<Bullet_rManager>().actNumber = actorNumber;
            fireTime = 0;
        }
    }
    
    void ActivateHaptic()
    {    // 앞에 수는 진동의 진폭
         // 뒤에 수는 진동의 강도
         // xt.SendHapticImpulse(0.2f, 0.3f); //오큘러스

        if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out triggerBtnR) && triggerBtnR)
        {
            PXR_Input.SetControllerVibration(0.25f, 2, PXR_Input.Controller.RightController);
        }
        if (InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.triggerButton, out triggerBtnL) && triggerBtnL)
        {
            PXR_Input.SetControllerVibration(0.25f, 2, PXR_Input.Controller.LeftController);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            remotePos = (Vector3)stream.ReceiveNext();
            remoteRot = (Quaternion)stream.ReceiveNext();
        }
    }
    void Reload()                                   // 총알 재장전 시간
    {
        fireTime += Time.deltaTime;
    }

    public void GetTarget()
    {
        ray = new Ray(firePoint.position, firePoint.forward);
        ray.origin = firePoint.position;
        ray.direction = firePoint.forward;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(ray.origin, ray.direction * fireDistance, Color.red);
    } 

    [PunRPC]
    public void DestroyGun()
    {
        Destroy(gameObject);
        //Destroy(PV.gameObject);
    }

    [PunRPC]
    public void StartGrabbing()
    {
        isBeingHeld = true;
    }

    [PunRPC]
    public void StopGrabbing()
    {
        isBeingHeld = false;
    }

    public void OnSelectedEntered()
    {
        Debug.Log("잡았다");
        PV.RPC(nameof(StartGrabbing), RpcTarget.AllBuffered);
    }

    public void OnSelectedExited()
    {
        Debug.Log("놓았다");
        PV.RPC(nameof(StopGrabbing), RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Fire_EX()
    {
        audioSource.Play();
        muzzleFlash.Play();
    }
   
    
}
