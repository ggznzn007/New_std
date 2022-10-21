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

    private RaycastHit hit;                          // 레이캐스트광선 히트
    private Ray ray;                                 // 레이캐스트 광선
    private ParticleSystem muzzleFlash;              // 총구 이펙트                                           
    private AudioSource audioSource;                 // 총알 발사 소리
    private PhotonView PV;                           // 포톤뷰
    private GameObject myBull;                       // 자기 총알    
    private float fireTime = 0;                      // 총알 딜레이 타임 
    private readonly float delayfireTime = 0.5f;    // 총알 딜레이 제한시간
    private readonly float fireDistance = 1000f;     // 총알 비거리
    private XRController xt;
    private bool triggerBtnR;
    private bool triggerBtnL;

    private void Awake()
    {
        RM = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();
        muzzleFlash = firePoint.GetComponentInChildren<ParticleSystem>();  // 하위 컴포넌트 추출 
        actorNumber = PV.OwnerActorNr;
        xt = (XRController)GameObject.FindObjectOfType(typeof(XRController));

    }

    private void FixedUpdate()
    {
        if (!PV.IsMine) return;
        GetTarget();
        Reload();
        if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out triggerBtnR) && triggerBtnR)
        {
            PXR_Input.SetControllerVibration(0.5f, 8, PXR_Input.Controller.RightController);
        }
        if (InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.triggerButton, out triggerBtnL) && triggerBtnL)
        {
            PXR_Input.SetControllerVibration(0.5f, 8, PXR_Input.Controller.LeftController);
        }

        if (!AvartarController.ATC.isAlive && PV.IsMine)
        {
            PV.RPC("DestroyGun", RpcTarget.All);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cube") && (!SpawnWeapon_R.rightWeapon.weaponInIt || !SpawnWeapon_L.leftWeapon.weaponInIt
            || !SpawnWeapon_RW.RW.weaponInIt || !SpawnWeapon_LW.LW.weaponInIt))
        {
            PV.RPC("DestroyGun_Delay", RpcTarget.All);
        }
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

    public void FireBullet()                                              // 컨트롤러 트리거를 이용한 총알 발사로직
    {
        if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit) && AvartarController.ATC.isAlive)
        {
            if (fireTime < delayfireTime) { return; }
            ActivateHaptic();
            audioSource.Play();
            muzzleFlash.Play();
            myBull = PN.Instantiate(bullet.name, ray.origin, Quaternion.identity);
            myBull.GetComponent<Rigidbody>().AddRelativeForce(ray.direction * speed, ForceMode.Force);// 질량적용 연속적인 힘을 가함
            myBull.GetComponent<PhotonView>().RPC("BulletDir", RpcTarget.Others, speed, PV.Owner.ActorNumber);
            PV.RPC("PunFire", RpcTarget.All);
            fireTime = 0;
        }
    }

    void ActivateHaptic()
    {    // 앞에 수는 진동의 진폭
         // 뒤에 수는 진동의 강도
        xt.SendHapticImpulse(0.2f, 0.3f);
        
       

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
            transform.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
        }
    }

    void Reload()                                   // 총알 재장전 시간
    {
        fireTime += Time.deltaTime;
    }

    [PunRPC]
    public void PunFire()
    {
        audioSource.Play();
        muzzleFlash.Play();
    }

    [PunRPC]
    public void DestroyGun_Delay()                  // 총 파괴 시간 딜레이 메서드
    {
        StartCoroutine(DestoryPN_Gun());
    }

    [PunRPC]
    public void DestroyGun()
    {
        Destroy(gameObject);
    }
    public IEnumerator DestoryPN_Gun()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
