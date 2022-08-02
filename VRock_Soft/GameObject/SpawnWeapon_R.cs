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
public class SpawnWeapon_R : MonoBehaviourPun
{
    public static SpawnWeapon_R rightWeapon;    
    public GameObject gunPrefab;
    public Transform attachPoint;
    private InputDevice targetDevice;
    public bool weaponInIt = false;
    private PhotonView PV;
    private void Awake()
    {
        rightWeapon = this;
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics =
            InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }

    }

    private void Update()
    {
      /*  if (weaponInIt)
        {
            if (targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerOn))
            {
                if (triggerOn)
                {
                    ShootingGun();
                }
            }
        }*/

    }

   /* public void ShootingGun()
    {
        var bullet = ObjectPooler.SpawnFromPool<BulletManager>("Bullet", FireBullet.FireGun.firePoint.forward);
        bullet.transform.position = FireBullet.FireGun.firePoint.position;
        //bullet.GetComponentInChildren<Rigidbody>().velocity = FireBullet.FireGun.firePoint.forward * FireBullet.FireGun.speed;
        bullet.GetComponentInChildren<Rigidbody>().AddForce(FireBullet.FireGun.firePoint.forward * FireBullet.FireGun.speed);



        var muzzle = ObjectPooler.SpawnFromPool<MuzzleEffect>("MuzzleEffect", FireBullet.FireGun.firePoint.position);
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ItemBox"))
        {
            Debug.Log("아이템박스 태그 시작");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ItemBox") && targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))
        {
            Debug.Log("아이템박스 태그 중");
            if (griped && !weaponInIt)
            {
                Instantiate(gunPrefab, attachPoint.position, attachPoint.rotation);  // 포톤 멀티플레이 할 때 생성
                                                                                     //Instantiate(gunPrefab, attachPoint.position, attachPoint.rotation);    //  싱글플레이 할 때 생성
                weaponInIt = true;
               /* if (PV.IsMine)
                {
                    PV.RPC("SpawnGun", RpcTarget.AllBuffered);
                }*/
            }
            else
            {
                weaponInIt = false;
                return;
            }


        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ItemBox"))
        {
            Debug.Log("아이템박스 태그 종료");
        }
    }



   

    /*[PunRPC]
    public void SpawnGun()
    {
        Instantiate(gunPrefab, attachPoint.position, attachPoint.rotation);  // 포톤 멀티플레이 할 때 생성
                                                                             //Instantiate(gunPrefab, attachPoint.position, attachPoint.rotation);    //  싱글플레이 할 때 생성
        weaponInIt = true;
    }*/

}
