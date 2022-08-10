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

public class GunManager : MonoBehaviourPun, IPunObservable
{
    public static GunManager gunManager;
    public Transform firePoint;  // ÃÑ±¸        
    private ParticleSystem muzzleFlash;    // ÃÑ±¸ ÀÌÆåÆ®
                                           // private PhotonView PV;
    AudioSource audioSource;             // ÃÑ¾Ë ¹ß»ç ¼Ò¸®
    private Vector3 remotePos;
    private Quaternion remoteRot;
    private float intervalSpeed = 20;

    private void Awake()
    {
        gunManager = this;
        ReadySceneManager.readySceneManager.FindGun();
        // PV = GetComponent<PhotonView>();
       
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        muzzleFlash = firePoint.GetComponentInChildren<ParticleSystem>();  // ÇÏÀ§ ÄÄÆ÷³ÍÆ® ÃßÃâ
       
    }

    /* GunManager FindGun()
     {
         foreach (GameObject gun in GameObject.FindGameObjectsWithTag("Gun"))
         {
             if (gun.GetPhotonView().IsMine) return gun.GetComponent<GunManager>();
         }
         return null;
     }*/

    public void FireBullet()
    {
        if (!photonView.IsMine) return;   
        if (photonView.IsMine)
        {           
            photonView.RPC("PunFire", RpcTarget.AllViaServer);            
        }
        


       /* audioSource.Play();// ÃÑ¾Ë ¹ß»ç ¼Ò¸® Àç»ý
        muzzleFlash.Play();

        var _bullet = PoolManager.PoolingManager.pool.Dequeue();
        if (_bullet != null)
        {
            _bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            _bullet.SetActive(true);
        }*/



        /* var _bullet = BulletPool.BulletPooling.GetBullet();  //1
         if (_bullet != null)
         {
             _bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
             _bullet.SetActive(true);
         }*/




        //var bullet = Instantiate(bulletPrefab);
        /* var bullet = ObjectPooler.SpawnFromPool<BulletManager>("Bullet");
             bullet.transform.position= firePoint.position;*/
        // bullet.transform.Translate(firePoint.forward * speed);
        // bullet.GetComponentInChildren<Rigidbody>().velocity = firePoint.transform.TransformDirection(new Vector3(0,0,speed));
        //bullet.GetComponentInChildren<Rigidbody>().velocity = firePoint.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cube"))
        {
            if (SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))// && !SpawnWeapon_R.rightWeapon.weaponInIt)
            {
                if (!griped)
                {
                    if(photonView.IsMine)
                    {
                        photonView.RPC("DestroyGun_Delay", RpcTarget.AllViaServer);
                        //DestroyGun_Delay();
                        //PN.Destroy(this.gameObject);
                        SpawnWeapon_L.leftWeapon.weaponInIt = false;
                        SpawnWeapon_R.rightWeapon.weaponInIt = false;
                        Debug.Log("ÃÑ ÆÄ±«µÊ");
                    }
                    
                }
            }

            Debug.Log("¹Ù´Ú¿¡ ÅÂ±×µÊ");

        }
    }

    [PunRPC]
    public void DestroyGun_Delay()
    {
        StartCoroutine(DestoryPN_Gun());
    }
    public IEnumerator DestoryPN_Gun()
    {
        yield return new WaitForSeconds(1.1f);
        PN.Destroy(gameObject);
       // Destroy(gameObject);
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
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void PunFire()
    {
        audioSource.Play();// ÃÑ¾Ë ¹ß»ç ¼Ò¸® Àç»ý
        muzzleFlash.Play();
        
        GameObject _bullet = PoolManager.PoolingManager.pool.Dequeue();
        if (_bullet != null)
        {
            _bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            //_bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            _bullet.SetActive(true);
            Debug.Log("RPC_Bullet ¹ß»ç");
        }
    }

    
   
}

