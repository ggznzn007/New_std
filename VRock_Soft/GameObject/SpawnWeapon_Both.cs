using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class SpawnWeapon_Both : MonoBehaviour
{
    public GameObject gunPrefab;
    public Transform attachPoint;
    private InputDevice targetDevice_R;
    private InputDevice targetDevice_L;
    private bool weaponInIt = false;

    void Start()
    {
        List<InputDevice> devices_L = new List<InputDevice>();
        InputDeviceCharacteristics leftControllerCharacteristics =
        InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devices_L);

        List<InputDevice> devices_R = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics =
        InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices_R);

        if (devices_R.Count > 0)
        {
            targetDevice_R = devices_R[0];
        }

        if (devices_L.Count > 1)
        {
            targetDevice_L = devices_L[0];
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
        if (other.gameObject.CompareTag("ItemBox"))
        {
            Debug.Log("아이템박스 태그 중");
            if (targetDevice_R.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R))
            {
                if (griped_R && !weaponInIt)
                {
                    Debug.Log("오른손 그립");
                    Instantiate(gunPrefab, attachPoint.position, attachPoint.rotation);
                    Debug.Log("총 꺼내기완료");
                    weaponInIt = true;
                }
                else
                {
                    weaponInIt = false;
                    return;
                }
            }
            if (targetDevice_L.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))
            {
                if (griped_L && !weaponInIt)
                {
                    Debug.Log("왼손 그립");
                    Instantiate(gunPrefab, attachPoint.position, attachPoint.rotation);
                    Debug.Log("총 꺼내기완료");
                    weaponInIt = true;
                }
                else
                {
                    weaponInIt = false;
                    return;
                }
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
}
