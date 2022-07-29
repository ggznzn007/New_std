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
public class SpawnWeapon_L : MonoBehaviourPunCallbacks
{
    public static SpawnWeapon_L leftWeapon;
    public GameObject gunPrefab;
    public Transform attachPoint;
    public InputDevice targetDevice;
    public bool weaponInIt = false;
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
           
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ItemBox"))
        {
            Debug.Log("아이템박스 태그 시작");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("ItemBox") && targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))
        {
            Debug.Log("아이템박스 태그 중");
            if (griped && !weaponInIt)
            {
                PN.Instantiate(gunPrefab.name, attachPoint.position, attachPoint.rotation);  // 포톤 멀티플레이 할 때 생성
                // Instantiate(gunPrefab, attachPoint.position, attachPoint.rotation);        // 싱글플레이 할 때 생성
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
            Debug.Log("아이템박스 태그 종료");
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
