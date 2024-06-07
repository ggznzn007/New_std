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

public class SpawnWeapon_LW : MonoBehaviourPun
{
    public static SpawnWeapon_LW LW;
    [SerializeField] GameObject gun;
    [SerializeField] Transform attachPoint;
    [SerializeField] int actorNumber;
    public InputDevice DeviceL;
    public bool weaponInIt = false;
    private GameObject myGun;
    
    private void Awake()
    {
        LW = this;
    }

    private void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics leftControllerCharacteristics =
        InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            DeviceL = devices[0];
        }     
    }    
 
    private void OnTriggerStay(Collider coll)
    {        
        if (coll.CompareTag("ItemBox_L"))
        {
            if(DeviceL.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))
            {
                if (griped_L && !weaponInIt && photonView.IsMine && photonView.AmOwner
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
            if (DeviceL.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L2))
            {
                if (griped_L2 && photonView.IsMine && photonView.AmOwner
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
            if(DeviceL.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L3))
            {
                if (griped_L3 && photonView.IsMine && photonView.AmOwner
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
        myGun= PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);
        return myGun.GetComponent<RevolverManager>();
    }
  /*  private RevolverManager SpawnGun(Transform attachPoint)
    {
        myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);
        return myGun.GetComponent<RevolverManager>();
    }*/
}
