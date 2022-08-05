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

public class GunManager : MonoBehaviourPunCallbacks
{
    public static GunManager gunManager;
    public Transform firePoint;  // ÃÑ±¸        
    private ParticleSystem muzzleFlash;    // ÃÑ±¸ ÀÌÆåÆ®
    private PhotonView PV;
    AudioSource audioSource;             // ÃÑ¾Ë ¹ß»ç ¼Ò¸®

    private void Awake()
    {
        gunManager = this;
        PV = GetComponent<PhotonView>();
    }


    private void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
        muzzleFlash = firePoint.GetComponentInChildren<ParticleSystem>();  // ÇÏÀ§ ÄÄÆ÷³ÍÆ® ÃßÃâ
    }



    public void FireBullet()
    {
        /*if (PV.IsMine)
        {
            PunFire();
            PV.RPC("PunFire", RpcTarget.AllBuffered);
            Debug.Log("RPC_¹ß»ç ¼º°ø");
        }*/


        audioSource.Play();// ÃÑ¾Ë ¹ß»ç ¼Ò¸® Àç»ý
        muzzleFlash.Play();

        var _bullet = PoolManager.PoolingManager.pool.Dequeue();
        if (_bullet != null)
        {
            _bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            _bullet.SetActive(true);
        }



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
            if (SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped) && !SpawnWeapon_R.rightWeapon.weaponInIt)
            {
                if (!griped)
                {
                    //Destroy(this.gameObject, 1.1f);
                    SpawnWeapon_L.leftWeapon.weaponInIt = false;
                    SpawnWeapon_R.rightWeapon.weaponInIt = false;
                    Debug.Log("ÃÑ ÆÄ±«µÊ");
                }
            }

            Debug.Log("¹Ù´Ú¿¡ ÅÂ±×µÊ");

        }
    }

    /*[PunRPC]
    void PunFire()
    {
        audioSource.Play();// ÃÑ¾Ë ¹ß»ç ¼Ò¸® Àç»ý
        muzzleFlash.Play();

        GameObject _bullet = PoolManager.PoolingManager.pool.Dequeue();
        if (_bullet != null)
        {
            _bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            _bullet.SetActive(true);
        }
    }
*/
}

