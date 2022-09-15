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
    public GameObject gunPrefab;
    public Transform attachPoint;
    public InputDevice targetDevice;
    public int actorNumber;
    public bool weaponInIt = false;    

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
    }

    public GunManager FindGun()
    {
        foreach (GameObject gun in GameObject.FindGameObjectsWithTag("Gun_Pun"))
        {
            if (gun.GetPhotonView().IsMine) return gun.GetComponent<GunManager>();
            //Debug.Log("ÀÌ ÃÑÀº ³»²¨");
        }
        return null;
    }
    private void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("ItemBox") && targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L) 
            && SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R))
        {
            if (griped_L && !weaponInIt && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive&&!griped_R)
               // && GunShootingManager.gunShootingManager.isRed)// && photonView.AmOwner)//
            {
                GameObject gunPrefab = PN.Instantiate("Gun_Pun", attachPoint.position, attachPoint.rotation);  // Æ÷Åæ¼­¹ö ¿ÀºêÁ§Æ® »ý¼º                    
                gunPrefab.GetPhotonView().OwnerActorNr = actorNumber;
                //FindGun();
                Debug.Log("ÃÑ »ý¼º");
                weaponInIt = true;
                return;
            }
          /*  else if(griped_L && !weaponInIt && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive && !griped_R
                && !GunShootingManager.gunShootingManager.isRed)
            {
                GameObject myGun = PN.Instantiate("Gun_Blue", attachPoint.position, attachPoint.rotation);  // Æ÷Åæ¼­¹ö ¿ÀºêÁ§Æ® »ý¼º                    
                myGun.GetPhotonView().OwnerActorNr = actorNumber;
                Debug.Log("ÃÑ »ý¼º");
                weaponInIt = true;
                return;
            }*/

            else
            {
                weaponInIt = false;
                return;
            }
        }

        
    }
}



