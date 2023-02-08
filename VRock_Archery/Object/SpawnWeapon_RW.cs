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
using Unity.VisualScripting;
//using UnityEngine.InputSystem.XR;

public class SpawnWeapon_RW : MonoBehaviourPun
{
    public static SpawnWeapon_RW RW;
    [SerializeField] GameObject bow;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform attachPoint;
    [SerializeField] int actorNumber;
    public InputDevice DeviceR;
    public bool weaponInIt = false;
    public bool arrowInIt = false;
    private GameObject myBow;
    private GameObject myArrow;
    //public GameObject myShield;
   // private bool triggerBtnR;


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
        //DataManager.DM.grabBomb = false;
    }

    private GameObject GetMyBow()
    {
        return myBow;
    }
   
    private void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("ItemBox"))
        {
            if (DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R))
            {
                if (griped_R && !weaponInIt && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive && myBow == null && myArrow == null)
                {
                    if (weaponInIt || myBow != null || myArrow != null) { return; }//if (myBow != null) { return; }
                    Debug.Log("활이 정상적으로 생성됨.");
                    Bow bow = CreateBow();
                    myBow = bow.gameObject;
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


        if (coll.CompareTag("Skilled"))
        {
            if (DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R3))
            {
                if (griped_R3 && !weaponInIt && myBow == null && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive && !DataManager.DM.grabArrow)
                {
                    if (weaponInIt || DataManager.DM.grabArrow) { return; }
                    // myArrow = coll.gameObject;
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


    private Bow CreateBow()
    {
        myBow = PN.Instantiate(bow.name, attachPoint.position, attachPoint.rotation);
        return myBow.GetComponent<Bow>();
    }



    /* private Arrow CreateArrow(Transform orientation)
     {
         // Create arrow, and get arrow component
         myArrow = PN.Instantiate(arrow.name, orientation.position, orientation.rotation);
         return myArrow.GetComponent<Arrow>();
     }

     private Bow CreateBow(Transform attachPoint)
     {
         myBow = PN.Instantiate(bow.name, attachPoint.position, attachPoint.rotation);
         return myBow.GetComponent<Bow>();
     }*/
}
