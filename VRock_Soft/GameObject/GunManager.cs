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
using TMPro;

using static ObjectPooler;
public class GunManager : MonoBehaviourPun, IPunObservable  // 총을 관리하는 스크립트
{
    public static GunManager gunManager;
    public GameObject bullet;
    public float speed;
    public float fireDistance = 1000f;
    RaycastHit hit;
    Ray ray;
    public Transform firePoint;  // 총구        
    private ParticleSystem muzzleFlash;    // 총구 이펙트
                                           // private PhotonView PV;
    AudioSource audioSource;             // 총알 발사 소리
    public bool isBulletMine;

    /*private Vector3 remotePos;
    private Quaternion remoteRot;
    private float intervalSpeed = 20;*/

    private PhotonView PV;
    public int actorNumber;
    private void Awake()
    {
        gunManager = this;
    }
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();
        muzzleFlash = firePoint.GetComponentInChildren<ParticleSystem>();  // 하위 컴포넌트 추출                                                                     
        //GetTarget();
        actorNumber = PV.OwnerActorNr;
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        GetTarget();
        //FindGun();
        DroptheGun();
        if (!AvartarController.ATC.isAlive && PV.IsMine)
        {
            PV.RPC("DestroyGun", RpcTarget.AllBuffered);
        }
    }

    public void DroptheGun()
    {
        if (SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R)&&
            SpawnWeapon_L.leftWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))// && !SpawnWeapon_R.rightWeapon.weaponInIt)
        {
            if (!griped_R&&!griped_L)
            {
                if (PV.IsMine)
                {
                    PV.RPC("DestroyGun_Delay", RpcTarget.AllBuffered);
                    SpawnWeapon_L.leftWeapon.weaponInIt = false;
                    SpawnWeapon_R.rightWeapon.weaponInIt = false;
                }

            }
        }
        /*if (SpawnWeapon_L.leftWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))
        {
            if (!griped_L)
            {
                if (PV.IsMine)
                {
                    PV.RPC("DestroyGun_Delay", RpcTarget.AllBuffered);
                    SpawnWeapon_L.leftWeapon.weaponInIt = false;
                    SpawnWeapon_R.rightWeapon.weaponInIt = false;
                }

            }
        }*/

    }

    public void GetTarget()
    {
        ray = new Ray(firePoint.position, firePoint.forward);
        ray.origin = firePoint.position;
        ray.direction = firePoint.forward;
    }
    /*public GunManager FindGun()
    {
        foreach (GameObject gun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            if (gun.GetPhotonView().IsMine) return gun.GetComponent<GunManager>();
            //Debug.Log("이 총은 내꺼");
        }
        return null;
    }*/

    private void OnDrawGizmos()
    {
        Debug.DrawRay(ray.origin, ray.direction * fireDistance, Color.red);
        // ray.origin    시작위치
        // ray.direction 방향
        // fireDistance  길이
        // Color.red     레이광선 색
        // 10f           유지시간
    }
    public void FireBullet()
    {
        //if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit,fireDistance) && AvartarController.ATC.isAlive)
        if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit) && AvartarController.ATC.isAlive)
        {
            Debug.Log(" 명중지점 : " + hit.point + "\n 거리 : "
                + hit.distance + "\n 이름 : " + hit.collider.name + "\n 태그 : " + hit.transform.tag);
            audioSource.Play();
            muzzleFlash.Play();

            GameObject bullet = PN.Instantiate("Bullet", ray.origin, Quaternion.identity);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cube"))
        {
            //DroptheGun();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(firePoint.position);
            stream.SendNext(firePoint.rotation);
        }
        else
        {
            transform.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            firePoint.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
        }
    }

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
        Debug.Log("총 즉시 파괴");
    }
    public IEnumerator DestoryPN_Gun()
    {
        yield return new WaitForSeconds(1.4f);
        Destroy(gameObject);
        Debug.Log("총 딜레이 파괴");
    }





}

