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
    public GameObject gunPrefab;
    public Transform attachPoint;
    public InputDevice targetDevice;
    public int actorNumber;
    public bool weaponInIt = false;
    
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
    }

    public GunManager FindGun()
    {
        foreach (GameObject gun in GameObject.FindGameObjectsWithTag("Gun_Pun"))
        {
            if (gun.GetPhotonView().IsMine) return gun.GetComponent<GunManager>();
            //Debug.Log("이 총은 내꺼");
        }
        return null;
    }
    private void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("ItemBox") && targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R)
            && SpawnWeapon_L.leftWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))
        {            
            if (griped_R && !weaponInIt && photonView.IsMine && photonView.AmOwner 
                && AvartarController.ATC.isAlive &&!griped_L)
               // && GunShootingManager.gunShootingManager.isRed)// && photonView.AmOwner)//
            {
                GameObject gunPrefab = PN.Instantiate("Gun_Pun", attachPoint.position, attachPoint.rotation);  // 포톤서버 오브젝트 생성                    
                gunPrefab.GetPhotonView().OwnerActorNr = actorNumber;
                //FindGun();
               // Debug.Log("총 생성");
               
                weaponInIt = true;
                return;
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
            }
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

            else
            {
               
                weaponInIt = false;
                return;
            }
        }
    }

    /* [PunRPC]
     public void SpawnGun(int actNumber)
     {

         //GameObject myGun = Instantiate(gunPrefab);  // 포톤 멀티플레이 할 때 생성
         GameObject myGun = Instantiate(gunPrefab);//, attachPoint.position, attachPoint.rotation);  // 포톤
         myGun.GetComponent<GunManager>().actorNumber = actNumber;
         myGun.transform.SetPositionAndRotation(attachPoint.position, attachPoint.rotation); // 서버 내의 위치 보정        
         weaponInIt = true;                                                                                           //myGun.transform.parent = this.transform;

         // myGun.transform.SetPositionAndRotation(attachPoint.position, attachPoint.rotation);
         Debug.Log("포톤서버 건 생성");

     }*/

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
}

