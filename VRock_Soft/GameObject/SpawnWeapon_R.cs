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

public class SpawnWeapon_R : MonoBehaviourPunCallbacks
{
    public static SpawnWeapon_R rightWeapon;
    public GameObject gunPrefab;
    public Transform attachPoint;
    private InputDevice targetDevice;
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
            targetDevice = devices[0];
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ItemBox"))
        {
            Debug.Log("�����۹ڽ� �±� ����");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("ItemBox") && targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))
        {
            Debug.Log("�����۹ڽ� �±� ��");
            if (griped && !weaponInIt)
            {
                PN.Instantiate(gunPrefab.name, attachPoint.position, attachPoint.rotation);  // ���� ��Ƽ�÷��� �� �� ����
               // Instantiate(gunPrefab, attachPoint.position, attachPoint.rotation);        // �̱��÷��� �� �� ����
                weaponInIt=true;
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
        if (other.gameObject.CompareTag("ItemBox"))
        {
            Debug.Log("�����۹ڽ� �±� ����");
        }
    }

    private void Update()
    {

    }


    IEnumerator DestroyGun()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gunPrefab);
    }
}
