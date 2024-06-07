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

public class SpawnWeapon_R_GS : MonoBehaviourPun
{
    public static SpawnWeapon_R_GS SGSR;
    public GameObject gunPrefab;
    //private PhotonView PV;

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
        SGSR = this;

        // PV = GetComponent<PhotonView>();
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

    private void FixedUpdate()
    {
        /*if (!AvartarController.ATC.isAlive && photonView.IsMine)
        {
            photonView.RPC("DestroyGun", RpcTarget.AllBuffered);
        }*/
    }

    private void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("ItemBox") && targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))
        {
            // Debug.Log("아이템박스 태그 중");
            if (griped && !weaponInIt && photonView.IsMine && photonView.AmOwner && GunAvartarController.GAC.isAlive)// && photonView.AmOwner)//
            {
                PN.Instantiate("Gun_Pun", attachPoint.position, attachPoint.rotation);  // 포톤서버 오브젝트 생성                    
                //myGun.GetPhotonView().OwnerActorNr = actorNumber;
                // GunManager.gunManager.FindGun();
                Debug.Log("총 생성");
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
            else
            {
                weaponInIt = false;
                return;
            }
        }
    }
}
