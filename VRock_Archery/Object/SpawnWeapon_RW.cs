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
    public bool weaponInIt;
    public string spawnBow;
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
        weaponInIt = false;
    }

    private void Update()
    {
        if (DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R))
        {
            if (!griped_R)
            {
                weaponInIt = false; return;
            }           
        }
    }

    private void OnTriggerStay(Collider coll)
    {
        if (!weaponInIt)
        {
            if (weaponInIt) { return; }
            if (coll.CompareTag("ItemBox"))
            {
                if (DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R))
                {
                    if (griped_R && !weaponInIt && photonView.IsMine && photonView.AmOwner
                    && AvartarController.ATC.isAlive && !DataManager.DM.grabString && myBow == null)
                    {
                        if(myBow!= null) { return; }
                        AudioManager.AM.PlaySE(spawnBow);
                        //Debug.Log("활이 정상적으로 생성됨.");
                        Bow bow = CreateBow();
                        myBow = bow.gameObject;
                        weaponInIt = true;
                        return;
                    }
                }
            }


            if (coll.CompareTag("Skilled"))
            {
                if (DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R3))
                {
                    if (griped_R3 && !weaponInIt && photonView.IsMine && photonView.AmOwner
                    && AvartarController.ATC.isAlive)
                    {
                        weaponInIt = true;
                        return;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Skilled") || coll.CompareTag("String")|| coll.CompareTag("Bow"))
        {
            weaponInIt = false;
            return;
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
