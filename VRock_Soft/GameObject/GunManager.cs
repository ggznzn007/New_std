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
public class GunManager : MonoBehaviourPun, IPunObservable  // ÃÑÀ» °ü¸®ÇÏ´Â ½ºÅ©¸³Æ®
{
    public static GunManager gunManager;
    public GameObject bullet;
    public float speed;
    public float fireDistance = 20f;
    RaycastHit hit;
    Ray ray;
    public Transform firePoint;  // ÃÑ±¸        
    private ParticleSystem muzzleFlash;    // ÃÑ±¸ ÀÌÆåÆ®
                                           // private PhotonView PV;
    AudioSource audioSource;             // ÃÑ¾Ë ¹ß»ç ¼Ò¸®
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
        muzzleFlash = firePoint.GetComponentInChildren<ParticleSystem>();  // ÇÏÀ§ ÄÄÆ÷³ÍÆ® ÃßÃâ                                                                          

        actorNumber = PV.OwnerActorNr;
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        GetTarget();
        FindGun();
        DroptheGun();
        if (!AvartarController.ATC.isAlive&&PV.IsMine)
        {
            PV.RPC("DestroyGun", RpcTarget.AllBuffered);
        }
    }

    public void DroptheGun()
    {
        if (SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))// && !SpawnWeapon_R.rightWeapon.weaponInIt)
        {
            if (!griped)
            {
                if (PV.IsMine)
                {
                    PV.RPC("DestroyGun_Delay", RpcTarget.AllBuffered);
                    SpawnWeapon_L.leftWeapon.weaponInIt = false;
                    SpawnWeapon_R.rightWeapon.weaponInIt = false;
                }

            }
        }
    }

    public void GetTarget()
    {
        ray = new Ray(firePoint.position, firePoint.forward);
        ray.origin = firePoint.position;
        ray.direction = firePoint.forward;
    }
    public GunManager FindGun()
    {
        foreach (GameObject gun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            if (gun.GetPhotonView().IsMine) return gun.GetComponent<GunManager>();
            //Debug.Log("ÀÌ ÃÑÀº ³»²¨");
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(ray.origin, ray.direction * fireDistance, Color.red);
    }
    public void FireBullet()
    {
        if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit, fireDistance) && AvartarController.ATC.isAlive)
        {
            audioSource.Play();
            muzzleFlash.Play();
            GameObject bullet = PN.Instantiate("Bullet", ray.origin, Quaternion.identity);
            bullet.GetComponent<PhotonView>().RPC("BulletDir", RpcTarget.All, speed);
            bullet.GetComponent<Rigidbody>().AddRelativeForce(ray.direction * speed, ForceMode.Force);
            bullet.GetPhotonView().OwnerActorNr = actorNumber;
            PV.RPC("PunFire", RpcTarget.All);
            Debug.Log(hit.transform.tag);
            ray.origin = hit.point;
            //GameObject _bullet = OP.PoolInstantiate("Bullet");
            //GameObject _bullet = PN.Instantiate(bullet.name, firePoint.position, firePoint.rotation);
            //GameObject _bullet = PoolManager.PoolingManager.pool.Dequeue();
            //GameObject _bullet = PN.Instantiate("Bullet", firePoint.transform.position, firePoint.transform.rotation);
            //_bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);   // ÃÑ¾Ë À§Ä¡
            //_bullet.GetPhotonView().ViewID = photonView.ViewID++;         
            //_bullet.GetPhotonView().OwnerActorNr = actorNumber;            
            //_bullet.GetPhotonView().ControllerActorNr = actorNumber;    
            //_bullet.transform.GetComponent<Rigidbody>().AddForce(firePoint.forward * speed);
            //_bullet.SetActive(true);

            Debug.Log("ÃÑ¾Ë ¹ß»ç");
        }

        /*if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
        {
            hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 10f);
        }*/
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
             Debug.Log(info.Sender.NickName + "Á×À½" + info.photonView.Owner.NickName);
         }
     }*/

    [PunRPC]
    public void PunFire()
    {
        audioSource.Play();
        muzzleFlash.Play();
    }

    [PunRPC]
    public void DestroyGun_Delay()                  // ÃÑ ÆÄ±« ½Ã°£ µô·¹ÀÌ ¸Þ¼­µå
    {
        StartCoroutine(DestoryPN_Gun());
    }

    [PunRPC]
    public void DestroyGun()
    {
        Destroy(gameObject);
        Debug.Log("ÃÑ Áï½Ã ÆÄ±«");
    }
    public IEnumerator DestoryPN_Gun()
    {
        yield return new WaitForSeconds(1.4f);
        Destroy(gameObject);
        Debug.Log("ÃÑ µô·¹ÀÌ ÆÄ±«");
    }





}

