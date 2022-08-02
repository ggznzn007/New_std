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

public class FireBullet : MonoBehaviourPun
{
    public static FireBullet FireGun;
    public Transform firePoint;  // �ѱ�    
    //public float speed = 75f;
    private ParticleSystem muzzleFlash;    // �ѱ� ����Ʈ
    private PhotonView PV;
    AudioSource audioSource;             // �Ѿ� �߻� �Ҹ�
    /* private Vector3 firePos;
     private Vector3 endPos;
 */
    private void Awake()
    {
        FireGun = this;
    }


    private void Start()
    {
        PV = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();
        muzzleFlash = firePoint.GetComponentInChildren<ParticleSystem>();  // ���� ������Ʈ ����
    }

    public void FireBull()
    {
        audioSource.Play();// �Ѿ� �߻� �Ҹ� ���
        muzzleFlash.Play();
        var bullet = BulletPoolManager.BulletPool.GetBullet();
        if (bullet != null)
        {
            
            bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            bullet.SetActive(true);
        }

       /* var muzzle = Instantiate(muzzlePrefab);
            muzzle.transform.position = firePoint.position;*/


        //var bullet = Instantiate(bulletPrefab);
        /* var bullet = ObjectPooler.SpawnFromPool<BulletManager>("Bullet");
             bullet.transform.position= firePoint.position;*/
        // bullet.transform.Translate(firePoint.forward * speed);
        // bullet.GetComponentInChildren<Rigidbody>().velocity = firePoint.transform.TransformDirection(new Vector3(0,0,speed));
        //bullet.GetComponentInChildren<Rigidbody>().velocity = firePoint.forward * speed;




        // var muzzle = ObjectPooler.SpawnFromPool<MuzzleEffect>("MuzzleEffect");

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cube"))
        {
            Debug.Log("�ٴڿ� �±׵�");
            SpawnWeapon_L.leftWeapon.weaponInIt = false;
            SpawnWeapon_R.rightWeapon.weaponInIt = false;

            Destroy(this.gameObject, 1.1f);
        }
    }




}

