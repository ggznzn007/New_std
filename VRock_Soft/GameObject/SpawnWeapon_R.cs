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
    /*private Vector3 remotePos;
    private Quaternion remoteRot;
    private float intervalSpeed = 20;
*/
    
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
        /* remotePos = attachPoint.position;
         remoteRot = attachPoint.rotation;*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ItemBox"))
        {
            Debug.Log("아이템박스 태그 시작");
        }
    }

    private void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("ItemBox") && targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))
        {
            Debug.Log("아이템박스 태그 중");
            if (griped && !weaponInIt)
            {


                if (photonView.IsMine) // 자기 자신의 것일 때만
                {

                    GameObject myGun = PN.Instantiate("Gun_Pun", attachPoint.position,attachPoint.rotation);  // 포톤서버 오브젝트 생성
                     myGun.GetComponent<GunManager>().actorNumber = actorNumber;
                    
                    /*GameObject myGun = Instantiate(gunPrefab, attachPoint.position, attachPoint.rotation);  // 생성
                    myGun.transform.SetPositionAndRotation(attachPoint.position, attachPoint.rotation); // 서버 내의 위치 보정     */


                    //myGun.transform.parent = this.transform;

                    // photonView.RPC("SpawnGun", RpcTarget.AllBuffered,"Gun_Pun", myGun.transform.position, myGun.transform.rotation);

                    //GameObject myGun = Instantiate(gunPrefab);
                    //myGun.GetPhotonView().transform.SetPositionAndRotation(attachPoint.position, attachPoint.rotation);
                    //myGun.GetPhotonView().OwnerActorNr = this.photonView.OwnerActorNr;

                    // SpawnGun(photonView.Owner.ActorNumber);

                    weaponInIt = true;
                    return;

                }

            }
            else
            {
                weaponInIt = false;
                return;
            }




        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ItemBox"))
        {
            Debug.Log("아이템박스 태그 종료");
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

