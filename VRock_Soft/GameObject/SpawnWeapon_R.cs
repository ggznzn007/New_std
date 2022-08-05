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
public class SpawnWeapon_R : MonoBehaviourPun
{
    public static SpawnWeapon_R rightWeapon;
    public GameObject gunPrefab;
    public Transform attachPoint;
    public InputDevice targetDevice;
    public bool weaponInIt = false;
    private PhotonView PV;
    private void Awake()
    {
        rightWeapon = this;
        PV = GetComponent<PhotonView>();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ItemBox"))
        {
            Debug.Log("�����۹ڽ� �±� ����");
        }
    }

    private void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("ItemBox") && targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))
        {
            Debug.Log("�����۹ڽ� �±� ��");
            if (griped && !weaponInIt)
            {
                
                SpawnGun();               
                //GameObject myGun = Instantiate(gunPrefab);  // ���� ��Ƽ�÷��� �� �� ����
                // myGun.transform.SetPositionAndRotation(attachPoint.position, attachPoint.rotation);
                //Instantiate(gunPrefab, attachPoint.position, attachPoint.rotation);    //  �̱��÷��� �� �� ����
                weaponInIt = true;
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
            Debug.Log("�����۹ڽ� �±� ����");
        }
    }


    private void FixedUpdate()
    {
        if (!PV.IsMine) { return; }
    }


    // [PunRPC]
    public void SpawnGun()
    {
        //GameObject myGun = Instantiate(gunPrefab, attachPoint.position, attachPoint.rotation);  // ���� ��Ƽ�÷��� �� �� ����
        GameObject myGun = PN.Instantiate("Gun_Pun", attachPoint.position, attachPoint.rotation);  // ���� 
        //myGun.transform.SetPositionAndRotation(attachPoint.position, attachPoint.rotation);
        Debug.Log("���漭�� �� ����");
        //myGun.transform.position = attachPoint.position;
        // myGun.transform.rotation = attachPoint.rotation;
    }

}
