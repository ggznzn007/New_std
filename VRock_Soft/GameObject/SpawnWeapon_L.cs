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
    public InputDevice targetDevice;
    public int actorNumber;
    public bool weaponInIt = false;

    private void Awake()
    {
        leftWeapon = this;
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

    private void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("ItemBox") && targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))
        {
            if (griped && !weaponInIt && photonView.IsMine && photonView.AmOwner && AvartarController.ATC.isAlive)// && photonView.AmOwner)//
            {
                GameObject myGun = PN.Instantiate("Gun_Pun", attachPoint.position, attachPoint.rotation);  // 포톤서버 오브젝트 생성                    
                myGun.GetPhotonView().OwnerActorNr = actorNumber;                
                Debug.Log("총 생성");
                weaponInIt = true;
                return;
            }

            else
            {
                weaponInIt = false;
                return;
            }
        }
    }
}



