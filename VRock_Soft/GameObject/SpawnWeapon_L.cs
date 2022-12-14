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
            Debug.Log("¿Ã √—¿∫ ≥ª≤®");
        }
        return null;
    }
    private void OnTriggerStay(Collider coll)
    {        
        if (coll.CompareTag("ItemBox") &&
            targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L) &&
            SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R))
        {
            if (griped_L && !griped_R && !weaponInIt && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive)
            {
                if (weaponInIt) { return; }//if (myGun != null) { return; }
                GunManager gun = SpawnGun(attachPoint);
                AudioManager.AM.PlaySE("GrabGun");
                myGun = gun.gameObject;
                //myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);                
                weaponInIt = true;
                return;
            }

            if (griped_L && !griped_R && !weaponInIt && photonView.IsMine && photonView.AmOwner
                 && AvartarController.ATC.isAlive && DataManager.DM.grabBomb)
            {
                if (weaponInIt) { return; }//if (myGun != null) { return; }
                GunManager gun = SpawnGun(attachPoint);
                AudioManager.AM.PlaySE("GrabGun");
                myGun = gun.gameObject;
                //myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);                
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

    private GunManager SpawnGun(Transform attachPoint)
    {
        myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);
        return myGun.GetComponent<GunManager>();
    }
}



