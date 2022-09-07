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

public class AvatarHand_L : MonoBehaviourPunCallbacks
{
    public InputDevice targetDevice;
    public Renderer[] avatarLeftHand;
    public PhotonView PV;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics leftControllerCharacteristics =
            InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devices);
        avatarLeftHand = GetComponentsInChildren<Renderer>();

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine) return;
        if (PV.IsMine)
        {
            if (targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))
            {
                if (griped)
                {
                    PV.RPC("RPC_IfGriped_HideHand_L", RpcTarget.AllBuffered, true);
                }
                else
                {
                    PV.RPC("RPC_IfGriped_HideHand_L", RpcTarget.AllBuffered, false);
                }
            }
        }
    }

    [PunRPC]
    public void RPC_IfGriped_HideHand_L(bool isGrip)
    {
        if (isGrip)
        {
            avatarLeftHand[0].forceRenderingOff = true;
            avatarLeftHand[1].forceRenderingOff = true;
        }
        else
        {
            avatarLeftHand[0].forceRenderingOff = false;
            avatarLeftHand[1].forceRenderingOff = false;
        }
    }
}
