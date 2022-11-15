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
    public bool weaponInIt = false;
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
    }

    public GunManager FindGun()
    {
        foreach (GameObject gun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            if (gun.GetPhotonView().IsMine) return gun.GetComponent<GunManager>();
            Debug.Log("이 총은 내꺼");
        }
        return null;
    }
    private void OnTriggerStay(Collider coll)
    {
        if (coll == null) return;
        if (coll.CompareTag("ItemBox") &&
            targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L) &&
            SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R))
        {
            if (griped_L && !griped_R && !weaponInIt && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive)
            {
                myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);  
                //FindGun();
                weaponInIt = true;
                return;
            }

            else if (griped_L && !griped_R && !weaponInIt && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive && DataManager.DM.grabBomb)
            {
                myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);  
                //FindGun();
                weaponInIt = true;
                SpawnWeapon_R.rightWeapon.weaponInIt = false;
                return;
            }

            else
            {
                weaponInIt = false;
                return;
            }
        }

        if (coll.CompareTag("Bomb") &&
            targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L2) &&
            SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R2))
        {
            if (griped_L2 && griped_R2 && !weaponInIt && photonView.IsMine && photonView.AmOwner
               && AvartarController.ATC.isAlive && !DataManager.DM.grabBomb)
            {
                weaponInIt = true;
                DataManager.DM.grabBomb = true;
                return;
            }
            else
            {
                weaponInIt = false;
                DataManager.DM.grabBomb = false;
                return;
            }
        }
    }

    /*if (coll.CompareTag("ItemBox") && targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L)
           && SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R))
       {
           if (griped_L && griped_R && !weaponInIt && photonView.IsMine && photonView.AmOwner
               && AvartarController.ATC.isAlive)
           {
               switch (DataManager.DM.currentMap)
               {
                   case Map.TUTORIAL_T:
                       myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);  // 포톤서버 오브젝트 생성                    
                       weaponInIt = true;
                       break;
                   case Map.TOY:
                       myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);  // 포톤서버 오브젝트 생성                    
                       weaponInIt = true;
                       break;
                   case Map.TUTORIAL_W:
                       myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);  // 포톤서버 오브젝트 생성                    
                       weaponInIt = true;
                       break;
                   case Map.WESTERN:
                       myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);  // 포톤서버 오브젝트 생성                    
                       weaponInIt = true;
                       break;
               }
           }
           else
           {
               weaponInIt = false;
               return;
           }
           */
    /*  else if(griped_L && !weaponInIt && photonView.IsMine && photonView.AmOwner
                 && AvartarController.ATC.isAlive && !griped_R
                 && !GunShootingManager.gunShootingManager.isRed)
             {
                 GameObject myGun = PN.Instantiate("Gun_Blue", attachPoint.position, attachPoint.rotation);  // 포톤서버 오브젝트 생성                    
                 myGun.GetPhotonView().OwnerActorNr = actorNumber;
                 Debug.Log("총 생성");
                 weaponInIt = true;
                 return;
             }*//*


       }*/

}



