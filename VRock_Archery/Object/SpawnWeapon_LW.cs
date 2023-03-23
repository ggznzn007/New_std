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
    public bool weaponInIt;
    //public bool arrowInIt = false;
    public string spawnBow;
    private GameObject myBow;
    //private GameObject myArrow;



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
        weaponInIt = false;
    }

    private void Update()
    {
        if (DeviceL.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))
        {
            if (!griped_L)
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
                if (DeviceL.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))
                {
                    if (griped_L && !weaponInIt && photonView.IsMine && photonView.AmOwner
                    && AvartarController.ATC.isAlive && !DataManager.DM.grabString && myBow==null)
                    {
                        if (myBow != null) { return; }
                        AudioManager.AM.PlaySE(spawnBow);
                        Debug.Log("활이 정상적으로 생성됨.");
                        Bow bow = CreateBow();
                        myBow = bow.gameObject;
                        weaponInIt = true;
                        return;
                    }
                }
            }

            if (coll.CompareTag("Skilled"))
            {
                if (DeviceL.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L3))
                {
                    if (griped_L3 && !weaponInIt && photonView.IsMine && photonView.AmOwner
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
        if (coll.CompareTag("Skilled") || coll.CompareTag("String") || coll.CompareTag("Bow"))
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
}
