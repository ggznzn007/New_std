using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class SpawnWeapon : MonoBehaviour
{
    public GameObject gunPrefab;
    public Transform attachPoint;
    private InputDevice targetDevice;
    private bool weaponInIt = false;
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
        if (other.gameObject.transform.CompareTag("ItemBox"))
        {
            Debug.Log("아이템박스 태그 시작");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.transform.CompareTag("ItemBox") && targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))
        {
            Debug.Log("아이템박스 태그 중");
            if (griped&&!weaponInIt)
            {
                Instantiate(gunPrefab, attachPoint.position, attachPoint.rotation);
                weaponInIt = true;
            }
            else
            {
                weaponInIt=false;
                return;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.CompareTag("ItemBox"))
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
