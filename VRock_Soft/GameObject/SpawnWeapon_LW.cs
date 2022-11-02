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
        DataManager.DM.grabGun = false;
        DataManager.DM.grabBomb = false;
    }

    public GunManager FindGun()
    {
        foreach (GameObject gun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            if (gun.GetPhotonView().IsMine) return gun.GetComponent<GunManager>();
            //Debug.Log("이 총은 내꺼");
        }
        return null;
    }
    private void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("ItemBox")
            && DeviceL.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L)
            && SpawnWeapon_RW.RW.DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R))
        {
            if (griped_L && !weaponInIt && photonView.IsMine && photonView.AmOwner       // 양손에 아무것도 없을때
                 && AvartarController.ATC.isAlive)
            {
                //PXR_Input.SetControllerVibration(1, 10, PXR_Input.Controller.LeftController);
                myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);  // 포톤서버 오브젝트 생성                    
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
            DeviceL.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L3))
        {
            if (griped_L3 && photonView.IsMine && photonView.AmOwner
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
