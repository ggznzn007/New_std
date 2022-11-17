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
public class SpawnWeapon_R : MonoBehaviourPun//, IPunObservable  // �տ��� ���� �����ϴ� ��ũ��Ʈ
{
    public static SpawnWeapon_R rightWeapon;
    [SerializeField] GameObject gun;
    [SerializeField] Transform attachPoint;
    [SerializeField] int actorNumber;
    public InputDevice targetDevice;
    public bool weaponInIt = false;
    private GameObject myGun;
    
    private void Awake()
    {
        rightWeapon = this;        
    }
    private void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics =
            InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);
        
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
            Debug.Log("�� ���� ����");
        }
        return null;
    }
    private void OnTriggerStay(Collider coll)
    {
        if(coll == null) return;
        if (coll.CompareTag("ItemBox") &&
            targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R) &&
            SpawnWeapon_L.leftWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))
        {
            if (griped_R && !griped_L && !weaponInIt && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive)
            {
                if (myGun != null) { return; }
                myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);                
                weaponInIt = true;
                return;
            }

           else if (griped_R && !griped_L && !weaponInIt && photonView.IsMine && photonView.AmOwner
                 && AvartarController.ATC.isAlive && DataManager.DM.grabBomb)
            {
                if (myGun != null) { return; }
                myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);                
                weaponInIt = true;
                SpawnWeapon_L.leftWeapon.weaponInIt = false;
                return;
            }

            else
            {
                weaponInIt = false;
                return;
            }
        }

        if (coll.CompareTag("Bomb") &&
            targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R2) &&
            SpawnWeapon_L.leftWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L2))
        {
            if (griped_R2 && griped_L2 && !weaponInIt && photonView.IsMine && photonView.AmOwner
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
    
    /* public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
     {
         if (stream.IsWriting)
         {
             stream.SendNext(transform.position);
             stream.SendNext(transform.rotation);
         }
         else
         {
             attachPoint.position = (Vector3)stream.ReceiveNext();
             attachPoint.rotation = (Quaternion)stream.ReceiveNext();
         }
     }*/

    /* myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);  // ���漭�� ������Ʈ ����                    
                        weaponInIt = true;
                        return;*/

    //myGun.GetPhotonView().OwnerActorNr = actorNumber;
    //FindGun();
    // Debug.Log("�� ����");

    // GameObject myGun = PN.Instantiate("Gun_Pun", attachPoint.position,attachPoint.rotation);  // ���漭�� ������Ʈ ����
    // myGun.GetComponent<GunManager>().actorNumber = actorNumber;

    /*GameObject myGun = Instantiate(gunPrefab, attachPoint.position, attachPoint.rotation);  // ����
    myGun.transform.SetPositionAndRotation(attachPoint.position, attachPoint.rotation); // ���� ���� ��ġ ����     */


    //myGun.transform.parent = this.transform;

    // photonView.RPC("SpawnGun", RpcTarget.AllBuffered,"Gun_Pun", myGun.transform.position, myGun.transform.rotation);

    //GameObject myGun = Instantiate(gunPrefab);
    //myGun.GetPhotonView().transform.SetPositionAndRotation(attachPoint.position, attachPoint.rotation);
    //myGun.GetPhotonView().OwnerActorNr = this.photonView.OwnerActorNr;

    // SpawnGun(photonView.Owner.ActorNumber);
    /* else if(griped_R && !weaponInIt && photonView.IsMine && photonView.AmOwner
                         && AvartarController.ATC.isAlive && !griped_L
                         && !GunShootingManager.gunShootingManager.isRed)
                     {
                         GameObject myGun = PN.Instantiate("Gun_Blue", attachPoint.position, attachPoint.rotation);  // ���漭�� ������Ʈ ����                    
                         myGun.GetPhotonView().OwnerActorNr = actorNumber;
                         // GunManager.gunManager.FindGun();
                         Debug.Log("�� ����");

                         weaponInIt = true;
                         return;
                     }*/
}

