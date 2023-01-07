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
    [SerializeField] GameObject bow;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform attachPoint;
    [SerializeField] int actorNumber;
    public InputDevice DeviceL;
    public bool weaponInIt = false;
    public bool arrowInIt = false;
    public GameObject myBow;
    public GameObject myArrow;
    // public XRController myController;

    private void Awake()
    {
        LW = this;
    }

    private void Start()
    {
        List<InputDevice> devicesL = new List<InputDevice>();
        InputDeviceCharacteristics leftControllerCharacteristics =
            InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devicesL);

        if (devicesL.Count > 0)
        {
            DeviceL = devicesL[0];
        }
        //DataManager.DM.grabGun = false;
        //DataManager.DM.grabBomb = false;
    }

   /* private void Update()
    {
        if (DeviceL.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))
        {
            if (griped_L)
            {
                weaponInIt = true;
            }
            else if (!griped_L)
            {
                weaponInIt = false;
            }
        }

    }*/
    private void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("ItemBox"))
        {
            if (DeviceL.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))
            {
                if (griped_L && !weaponInIt && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive && myBow == null&&myArrow==null)
                {
                    if (weaponInIt || myBow != null||myArrow!=null) { return; }//if (myBow != null) { return; }
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

        if (coll.CompareTag("ArrowBox"))
        {
            if (DeviceL.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L2))
            {
                if (griped_L2 && !weaponInIt && myBow == null && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive && !DataManager.DM.grabArrow)
                {
                    if (weaponInIt  || DataManager.DM.grabArrow) { return; }
                    Debug.Log("화살이 정상적으로 생성됨.");
                    Arrow arrow = CreateArrow();
                    myArrow = arrow.gameObject;
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


      /*  if (coll.CompareTag("String"))
        {
            if (DeviceL.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L3))
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
        }*/
    }
     

    private Arrow CreateArrow()
    {
        // Create arrow, and get arrow component
        myArrow = PN.Instantiate(arrow.name, attachPoint.position, attachPoint.rotation);
        return myArrow.GetComponent<Arrow>();
    }

    private Bow CreateBow()
    {
        myBow = PN.Instantiate(bow.name, attachPoint.position, attachPoint.rotation);
        return myBow.GetComponent<Bow>();
    }
}
