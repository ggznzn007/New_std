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
        if (!weaponInIt)                      // �������� ����� ��
        {
            if (weaponInIt) { return; }
            if (coll.CompareTag("ItemBox"))  // ������ �ڽ� ���������� �±��ϰ� ���� �� ��Ʈ�ѷ� �׸���ư�� ������ ���ѻ��� ���ÿ� ����
            {
                if (DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R))
                {
                    if (griped_R && !weaponInIt && photonView.IsMine && photonView.AmOwner
                    && AvartarController.ATC.isAlive  && !DataManager.DM.grabString && mySling == null)
                    {
                        if(mySling!= null) { return; }
                        AudioManager.AM.PlaySE(spawnSling);
                        Debug.Log("������ ���������� ������.");
                        SlingShot sling = CreateSling();
                        mySling = sling.gameObject;
                        weaponInIt = true;
                        return;
                    }
                    else { return; }
                }
            }

            // ������ �ݶ��̴��� ��� �� �� �Ʒ� �±׵��� �ݶ��̴��� �±� ���� �� �տ� ��� �ִ� ������ �������
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

    private void OnTriggerExit(Collider coll)    // ������ �ݶ��̴��� �Ʒ��±׵��� �ݶ��̴����� ���� �� ������� �������
    {
        if (coll.CompareTag("Stoneball") || coll.CompareTag("Icicle") || coll.CompareTag("SlingShot")
              || coll.CompareTag("Snowblock") || coll.CompareTag("Iceblock") || coll.CompareTag("String"))
        {
            weaponInIt = false;
            return;
        }
    }

    private SlingShot CreateSling() // ���� ���� �޼���
    {
        mySling = PN.Instantiate(slingShot.name, attachPoint.position, attachPoint.rotation);
        return mySling.GetComponent<SlingShot>();
    }


}

