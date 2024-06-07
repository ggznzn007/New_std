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
using UnityEditor;

public class SpawnWeapon_L : MonoBehaviourPun
{
    public static SpawnWeapon_L leftWeapon;
    [SerializeField] GameObject gun;
    [SerializeField] Transform attachPoint;
    [SerializeField] int actorNumber;
    public InputDevice targetDevice;
    public bool weaponInIt=false;           
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

       // HandL = GetComponentInChildren<MeshRenderer>();

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
        //DataManager.DM.grabBomb = false;       
    }   
   
     private void OnTriggerStay(Collider coll)
    {        
        if (coll.CompareTag("ItemBox_L"))
        {
            if(targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))
            {
                if (griped_L && !weaponInIt && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive && myGun == null)
                {
                    if (weaponInIt) { return; }
                   // if (myGun != null) { return; }
                    GunManager gun = SpawnGun();
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
        }      

        if (coll.CompareTag("Bomb"))
        {
            if (targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L2))
            {
                if(griped_L2 && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive)
                {
                    weaponInIt=true;
                    return;
                }
                else
                {
                    weaponInIt=false;
                    return;
                }
            }
        }

        if(coll.CompareTag("Shield"))
        {
            if (targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L3))
            {
                if(griped_L3 && photonView.IsMine && photonView.AmOwner
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
    
    private GunManager SpawnGun()
    {
        myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);
        return myGun.GetComponent<GunManager>();
    }
}



