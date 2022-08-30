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
public class SpawnWeapon_L : MonoBehaviourPun
{
    public static SpawnWeapon_L leftWeapon;
    public GameObject gunPrefab;
    public Transform attachPoint;
    private InputDevice targetDevice;
    public bool weaponInIt = false;
    //private PhotonView PV;
    private void Awake()
    {
        leftWeapon = this;
        //PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics leftControllerCharacteristics =
            InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }

    }
   

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ItemBox") && targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))
        {
           // Debug.Log("아이템박스 태그 중");
            if (griped && !weaponInIt)
            {
                //SpawnGun();
                weaponInIt = true;
            }
            else
            {
                weaponInIt = false;
                return;
            }
        }
    }

 

    public void SpawnGun()
    {
        GameObject myGun = Instantiate(gunPrefab);  // 포톤 멀티플레이 할 때 생성
        myGun.transform.SetPositionAndRotation(attachPoint.position, attachPoint.rotation);
        //myGun.transform.position = attachPoint.position;
        // myGun.transform.rotation = attachPoint.rotation;
    }



}
