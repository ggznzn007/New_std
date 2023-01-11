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
using Unity.XR.PXR;

public class SpawnWeapon_RW : MonoBehaviourPun
{
    public static SpawnWeapon_RW RW;
    [SerializeField] GameObject gun;
    [SerializeField] Transform attachPoint;
    [SerializeField] int actorNumber;
    public InputDevice DeviceR;
    public bool weaponInIt = false;
    private GameObject myGun;    
   
    private void Awake()
    {
        RW = this;
    }
    private void Start()
    {
        List<InputDevice> devicesR = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics =
            InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devicesR);

        if (devicesR.Count > 0)
        {
            DeviceR = devicesR[0];
        }

       // DataManager.DM.grabGun = false;
       // DataManager.DM.grabBomb = false;
    }   
   
    public RevolverManager FindGun()
    {
        foreach (GameObject gun in GameObject.FindGameObjectsWithTag("Revolver"))
        {
            if (gun.GetPhotonView().IsMine) return gun.GetComponent<RevolverManager>();
            Debug.Log("¿Ã √—¿∫ ≥ª≤®");
        }
        return null;
    }
    private void OnTriggerStay(Collider coll)
    {       
        if (coll.CompareTag("ItemBox_R"))
        {
            if(DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R))
            {
                if (griped_R && !weaponInIt && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive && myGun == null)
                {
                    if (weaponInIt) { return; }
                   // if (myGun != null) { return; }
                    RevolverManager revolver = SpawnGun();
                    AudioManager.AM.PlaySE("GrabRevo");
                    myGun = revolver.gameObject;                                    
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

        if (coll.CompareTag("Bomb"))
        {
            if (DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R2))
            {
                if (griped_R2 && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive)
                {
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

        if (coll.CompareTag("Shield"))
        {
            if(DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R3))
            {
                if (griped_R3 && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive)
                {
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

    private RevolverManager SpawnGun()
    {
        myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);
        return myGun.GetComponent<RevolverManager>();
    }
}
