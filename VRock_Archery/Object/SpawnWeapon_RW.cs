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
//using UnityEngine.InputSystem.XR;

public class SpawnWeapon_RW : MonoBehaviourPun
{
    public static SpawnWeapon_RW RW;
    [SerializeField] GameObject bow;
    [SerializeField] Transform attachPoint;
    [SerializeField] int actorNumber;
    public InputDevice DeviceR;
    public bool weaponInIt = false;
    private GameObject myBow;    
    

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
        

        DataManager.DM.grabGun = false;
        DataManager.DM.grabBomb = false;
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
        if (coll.CompareTag("ItemBox")
            && DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R)
            && SpawnWeapon_LW.LW.DeviceL.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L2))
        {
            if (griped_R && !weaponInIt && photonView.IsMine && photonView.AmOwner      
                && AvartarController.ATC.isAlive)
            {
                if (myBow != null) { return; }
                myBow = PN.Instantiate(bow.name, attachPoint.position, attachPoint.rotation);
                //FindGun();
                weaponInIt = true;
                return;
            }

            else
            {
                weaponInIt = false;
                return;
            }
        }

        if (coll.CompareTag("Bomb") &&
            DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R3))
        {
            if (griped_R3 && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive && !DataManager.DM.grabBomb)
            {
                weaponInIt = true;
                DataManager.DM.grabBomb = true;
            }

            else
            {
                weaponInIt = false;
                DataManager.DM.grabBomb = false;
            }
        }
    }

    
}
