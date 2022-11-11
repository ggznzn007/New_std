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
using Random = UnityEngine.Random;

public class GunManager : MonoBehaviourPun, IPunObservable  // 총을 관리하는 스크립트
{
    public static GunManager gunManager;
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
    private readonly float delayfireTime = 0.15f;    // 총알 딜레이 제한시간
    private readonly float fireDistance = 1000f;     // 총알 비거리    

    private void Awake()
    {
        gunManager = this;
    }
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();
        muzzleFlash = firePoint.GetComponentInChildren<ParticleSystem>();  // 하위 컴포넌트 추출 
        actorNumber = PV.OwnerActorNr;
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine) return;
        GetTarget();
        Reload();
       // OwnerTransferGun();
       /* switch (DataManager.DM.currentMap)
        {
            case Map.TUTORIAL_T:
            case Map.TOY:
                DroptheGunT();
                break;
            case Map.TUTORIAL_W:
            case Map.WESTERN:
                DropGunW();
                break;
        }*/

        if (!AvartarController.ATC.isAlive && PV.IsMine)
        {
            PV.RPC("DestroyGun", RpcTarget.All);
        }

    }

    public void OwnerTransferGun()  // 총을 잡고있으면 빼앗지 못하게
    {
        if (SpawnWeapon_L.leftWeapon.weaponInIt || SpawnWeapon_R.rightWeapon.weaponInIt)
        {
            PV.OwnershipTransfer = OwnershipOption.Fixed;
        }
        else
        {
            PV.OwnershipTransfer = OwnershipOption.Request;
        }
    }


    /*public void DroptheGunT()                                            // 총을 놓으면 총 자동 파괴
    {
        if (SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R) &&
            SpawnWeapon_L.leftWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))
        {
            if (!griped_R)
            {
                if (PV.IsMine)
                {
                    PV.RPC("DestroyGun_Delay", RpcTarget.All);
                }
            }
            else if (!griped_L)
            {
                if (PV.IsMine)
                {
                    PV.RPC("DestroyGun_Delay", RpcTarget.All);
                }
            }
        }


    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if(DataManager.DM.currentTeam!=Team.ADMIN)
        {
            if (collision.collider.CompareTag("Cube") && (!SpawnWeapon_R.rightWeapon.weaponInIt || !SpawnWeapon_L.leftWeapon.weaponInIt
            || !SpawnWeapon_RW.RW.weaponInIt || !SpawnWeapon_LW.LW.weaponInIt))
            {
                PV.RPC("DestroyGun_Delay", RpcTarget.All);
            }
        }
        
    }
    /*public void DropGunW()
    {
        if (SpawnWeapon_RW.RW.DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R) &&
            SpawnWeapon_LW.LW.DeviceL.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))
        {
            if (!griped_R && !griped_L)
            {
                if (PV.IsMine)
                {
                    PV.RPC("DestroyGun_Delay", RpcTarget.All);
                }
            }
            else if (!griped_L && griped_R)
            {
                if (PV.IsMine)
                {
                    PV.RPC("DestroyGun_Delay", RpcTarget.All);
                }
            }
            else if (!griped_R && griped_L)
            {
                if (PV.IsMine)
                {
                    PV.RPC("DestroyGun_Delay", RpcTarget.All);
                }
            }
        }
    }*/

    public void GetTarget()
    {
        ray = new Ray(firePoint.position, firePoint.forward);
        ray.origin = firePoint.position;
        ray.direction = firePoint.forward;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(ray.origin, ray.direction * fireDistance, Color.red);
        // ray.origin    시작위치
        // ray.direction 방향
        // fireDistance  길이
        // Color.red     레이광선 색
        // 10f           유지시간
        //Debug.Log(" 명중지점 : " + hit.point + "\n 거리 : "
        //    + hit.distance + "\n 이름 : " + hit.collider.name + "\n 태그 : " + hit.transform.tag);
    }
    public void FireBullet()                                              // 컨트롤러 트리거를 이용한 총알 발사로직
    {
        if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit) && AvartarController.ATC.isAlive)
        {
            if (fireTime < delayfireTime) { return; }
            audioSource.Play();
            muzzleFlash.Play();
            myBull = PN.Instantiate(bullet.name, ray.origin, Quaternion.identity);
            myBull.GetComponent<Rigidbody>().AddRelativeForce(ray.direction * speed, ForceMode.Force);// 질량적용 연속적인 힘을 가함
            myBull.GetComponent<PhotonView>().RPC("BulletDir", RpcTarget.Others, speed, PV.Owner.ActorNumber);
            PV.RPC("PunFire", RpcTarget.All);
            fireTime = 0;
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
            transform.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
        }
    }

    void Reload()                                   // 총알 재장전 시간
    {
        fireTime += Time.deltaTime;
    }
    public GunManager FindGun()
    {
        foreach (GameObject gun in GameObject.FindGameObjectsWithTag("Gun_Pun"))
        {
            if (gun.GetPhotonView().IsMine) return gun.GetComponent<GunManager>();
            //Debug.Log("이 총은 내꺼");
        }
        return null;
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

    /// <summary>
    /// 총알 딜레이 로직
    /// fireTime을 0이 될 때만 총알을 발사할 수 있게 텀을 두는 것 
    /// fireTime이 delayfireTime을 넘어가면 발사 불가
    /// 총알을 발사하고 나면 시간을 다시 0으로 초기화
    /// </summary>

    /*public void FireBullet_Red()
    {
        //if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit,fireDistance) && AvartarController.ATC.isAlive)
        if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit) && AvartarController.ATC.isAlive)
        {
            Debug.Log(" 명중지점 : " + hit.point + "\n 거리 : "
                + hit.distance + "\n 이름 : " + hit.collider.name + "\n 태그 : " + hit.transform.tag);
            audioSource.Play();
            muzzleFlash.Play();

            GameObject bullet = PN.Instantiate("Bullet_Red", ray.origin, Quaternion.identity);
            bullet.GetComponent<PhotonView>().RPC("BulletDir", RpcTarget.All, speed);
            bullet.GetComponent<Rigidbody>().AddRelativeForce(ray.direction * speed, ForceMode.Force);// 질량적용 연속적인 힘을 가함
            bullet.GetPhotonView().OwnerActorNr = actorNumber;
            PV.RPC("PunFire", RpcTarget.All);


            //GameObject _bullet = OP.PoolInstantiate("Bullet");
            //GameObject _bullet = PN.Instantiate(bullet.name, firePoint.position, firePoint.rotation);
            //GameObject _bullet = PoolManager.PoolingManager.pool.Dequeue();
            //GameObject _bullet = PN.Instantiate("Bullet", firePoint.transform.position, firePoint.transform.rotation);
            //_bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);   // 총알 위치
            //_bullet.GetPhotonView().ViewID = photonView.ViewID++;         
            //_bullet.GetPhotonView().OwnerActorNr = actorNumber;            
            //_bullet.GetPhotonView().ControllerActorNr = actorNumber;    
            //_bullet.transform.GetComponent<Rigidbody>().AddForce(firePoint.forward * speed);
            //_bullet.SetActive(true);

            //Debug.Log("총알 발사");
        }
    }
    public void FireBullet_Blue()
    {
        //if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit,fireDistance) && AvartarController.ATC.isAlive)
        if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit) && AvartarController.ATC.isAlive)
        {
            Debug.Log(" 명중지점 : " + hit.point + "\n 거리 : "
                + hit.distance + "\n 이름 : " + hit.collider.name + "\n 태그 : " + hit.transform.tag);
            audioSource.Play();
            muzzleFlash.Play();

            GameObject bullet = PN.Instantiate("Bullet_Blue", ray.origin, Quaternion.identity);
            bullet.GetComponent<PhotonView>().RPC("BulletDir", RpcTarget.All, speed);
            bullet.GetComponent<Rigidbody>().AddRelativeForce(ray.direction * speed, ForceMode.Force);// 질량적용 연속적인 힘을 가함
            bullet.GetPhotonView().OwnerActorNr = actorNumber;
            PV.RPC("PunFire", RpcTarget.All);


            //GameObject _bullet = OP.PoolInstantiate("Bullet");
            //GameObject _bullet = PN.Instantiate(bullet.name, firePoint.position, firePoint.rotation);
            //GameObject _bullet = PoolManager.PoolingManager.pool.Dequeue();
            //GameObject _bullet = PN.Instantiate("Bullet", firePoint.transform.position, firePoint.transform.rotation);
            //_bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);   // 총알 위치
            //_bullet.GetPhotonView().ViewID = photonView.ViewID++;         
            //_bullet.GetPhotonView().OwnerActorNr = actorNumber;            
            //_bullet.GetPhotonView().ControllerActorNr = actorNumber;    
            //_bullet.transform.GetComponent<Rigidbody>().AddForce(firePoint.forward * speed);
            //_bullet.SetActive(true);

            //Debug.Log("총알 발사");
        }
    }*/


    /* [PunRPC]
     public void TakeDamage(float damage,PhotonMessageInfo info)
     {
         AvartarController.ATC.curHP -= damage;
         AvartarController.ATC.HP.fillAmount = AvartarController.ATC.curHP / AvartarController.ATC.inItHP;

         if(AvartarController.ATC.curHP<=0.0f)
         {
             Debug.Log(info.Sender.NickName + "죽음" + info.photonView.Owner.NickName);
         }
     }*/

    /* [PunRPC]
     public void FireBull(int actorNumber)
     {
         audioSource.Play();
         muzzleFlash.Play();
         GameObject bullet = Instantiate(bulletPrefab, ray.origin, Quaternion.identity);
         bullet.GetComponent<Rigidbody>().AddRelativeForce(ray.direction * speed, ForceMode.Force);// 질량적용 연속적인 힘을 가함
        // bullet.GetComponent<PhotonView>().RPC("BulletDir", RpcTarget.Others, speed, PV.Owner.ActorNumber);
         bullet.GetComponent<BulletManager>().actNumber = actorNumber;
     }*/

}

