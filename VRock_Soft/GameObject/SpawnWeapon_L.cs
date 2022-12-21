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
    [SerializeField] GameObject gun;
    [SerializeField] Transform attachPoint;
    [SerializeField] int actorNumber;
    public InputDevice targetDevice;
    public bool weaponInIt;    
    private GameObject myGun;
    
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
        DataManager.DM.grabBomb = false;
        weaponInIt= false;
        
    }

    public GunManager FindGun()
    {
        foreach (GameObject gun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            if (gun.GetPhotonView().IsMine) return gun.GetComponent<GunManager>();
            Debug.Log("¿Ã √—¿∫ ≥ª≤®");
        }
        return null;
    }
    private void OnTriggerStay(Collider coll)
    {        
        if (coll.CompareTag("ItemBox") &&
            targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))
        {
            if (griped_L && !weaponInIt && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive && !DataManager.DM.grabGun)
            {
                if (weaponInIt && DataManager.DM.grabGun) { return; }               
                GunManager gun = SpawnGun(attachPoint);
                myGun = gun.gameObject;
                weaponInIt = true;
                AudioManager.AM.PlaySE("GrabGun");                
                return;
            }

            else
            {
                weaponInIt = false;                
                return;
            }
        }

        if (coll.CompareTag("Bomb") &&
            targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L2))
        {
            if (griped_L2 && photonView.IsMine && photonView.AmOwner && AvartarController.ATC.isAlive)
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

    private GunManager SpawnGun(Transform attachPoint)
    {
        myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);
        return myGun.GetComponent<GunManager>();
    }
}



