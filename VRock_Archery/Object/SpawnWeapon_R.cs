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
    public static SpawnWeapon_R SR;
    [SerializeField] GameObject slingShot;
    [SerializeField] GameObject snowBall;
    [SerializeField] Transform attachPoint;
    [SerializeField] int actorNumber;
    public InputDevice DeviceR;
    public bool weaponInIt = false;
    public string spawnSling;
    private GameObject mySling;

    private void Awake()
    {
        SR = this;
    }

    private void Start()
    {
        List<InputDevice> devicesR = new List<InputDevice>();
        InputDeviceCharacteristics leftControllerCharacteristics =
            InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devicesR);

        if (devicesR.Count > 0)
        {
            DeviceR = devicesR[0];
        }

        weaponInIt = false;
    }

    private void Update()
    {       
      if(DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R))
        {
            if(!griped_R)
            {
                weaponInIt = false; return;
            }           
        }
    }

    private void OnTriggerStay(Collider coll)
    {
        if (!weaponInIt)                      // 오른손이 빈손일 때
        {
            if (weaponInIt) { return; }
            if (coll.CompareTag("ItemBox"))  // 아이템 박스 오른손으로 태그하고 있을 때 컨트롤러 그립버튼을 누르면 새총생성 동시에 잡힘
            {
                if (DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R))
                {
                    if (griped_R && !weaponInIt && photonView.IsMine && photonView.AmOwner
                    && AvartarController.ATC.isAlive  && !DataManager.DM.grabString && mySling == null)
                    {
                        if(mySling!= null) { return; }
                        AudioManager.AM.PlaySE(spawnSling);
                        Debug.Log("새총이 정상적으로 생성됨.");
                        SlingShot sling = CreateSling();
                        mySling = sling.gameObject;
                        weaponInIt = true;
                        return;
                    }
                    else { return; }
                }
            }

            // 오른손 콜라이더가 빈손 일 때 아래 태그들의 콜라이더에 태그 중일 때 손에 쥐고 있는 것으로 만들어줌
            if (coll.CompareTag("Stoneball") || coll.CompareTag("Icicle")      
             || coll.CompareTag("Snowblock") || coll.CompareTag("Iceblock"))
            {
                if (DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R3))
                {
                    if (griped_R3 && !weaponInIt && photonView.IsMine && photonView.AmOwner
                    && AvartarController.ATC.isAlive)
                    {
                        weaponInIt = true;
                        return;
                    }
                }
            }
          
        }
    }

    private void OnTriggerExit(Collider coll)    // 오른손 콜라이더가 아래태그들의 콜라이더에서 나갈 때 빈손으로 만들어줌
    {
        if (coll.CompareTag("Stoneball") || coll.CompareTag("Icicle") || coll.CompareTag("SlingShot")
              || coll.CompareTag("Snowblock") || coll.CompareTag("Iceblock") || coll.CompareTag("String"))
        {
            weaponInIt = false;
            return;
        }
    }

    private SlingShot CreateSling() // 새총 생성 메서드
    {
        mySling = PN.Instantiate(slingShot.name, attachPoint.position, attachPoint.rotation);
        return mySling.GetComponent<SlingShot>();
    }


}

