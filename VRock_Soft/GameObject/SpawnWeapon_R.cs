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
public class SpawnWeapon_R : MonoBehaviourPun//, IPunObservable  // 손에서 총을 생성하는 스크립트
{
    public static SpawnWeapon_R rightWeapon;
    [SerializeField] GameObject gun;
    [SerializeField] Transform attachPoint;
    [SerializeField] int actorNumber;
    public InputDevice targetDevice;
    public bool weaponInIt;
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
        weaponInIt= false;
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
        if (coll.CompareTag("ItemBox") &&
            targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R))
        {
            if (griped_R && !weaponInIt && photonView.IsMine && photonView.AmOwner
                && AvartarController.ATC.isAlive && !DataManager.DM.grabGun)
            {
                if (weaponInIt && DataManager.DM.grabGun) { return; }
                GunManager gun = SpawnGun(attachPoint);
                myGun = gun.gameObject;
                weaponInIt = true;
                AudioManager.AM.PlaySE("GrabGun");
                 
                return;
            }
            else
            {
                weaponInIt = false;                
                return;
            }
        }

        if (coll.CompareTag("Bomb") &&
            targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R2))
        {
            if (griped_R2 && photonView.IsMine && photonView.AmOwner && AvartarController.ATC.isAlive)
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

    private GunManager SpawnGun(Transform attachPoint)
    {
        myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);
        return myGun.GetComponent<GunManager>();
    }

    /*  public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
      {
          if (stream.IsWriting)
          {
              stream.SendNext(transform.position);
              stream.SendNext(transform.rotation);
          }
          else
          {
              remotePos = (Vector3)stream.ReceiveNext();
              remoteRot = (Quaternion)stream.ReceiveNext();
          }
      }*/

    /* myGun = PN.Instantiate(gun.name, attachPoint.position, attachPoint.rotation);  // 포톤서버 오브젝트 생성                    
                        weaponInIt = true;
                        return;*/

    //myGun.GetPhotonView().OwnerActorNr = actorNumber;
    //FindGun();
    // Debug.Log("총 생성");

    // GameObject myGun = PN.Instantiate("Gun_Pun", attachPoint.position,attachPoint.rotation);  // 포톤서버 오브젝트 생성
    // myGun.GetComponent<GunManager>().actorNumber = actorNumber;

    /*GameObject myGun = Instantiate(gunPrefab, attachPoint.position, attachPoint.rotation);  // 생성
    myGun.transform.SetPositionAndRotation(attachPoint.position, attachPoint.rotation); // 서버 내의 위치 보정     */


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
                         GameObject myGun = PN.Instantiate("Gun_Blue", attachPoint.position, attachPoint.rotation);  // 포톤서버 오브젝트 생성                    
                         myGun.GetPhotonView().OwnerActorNr = actorNumber;
                         // GunManager.gunManager.FindGun();
                         Debug.Log("총 생성");

                         weaponInIt = true;
                         return;
                     }*/
}

