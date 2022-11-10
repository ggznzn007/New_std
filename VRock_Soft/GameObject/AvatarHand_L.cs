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

public class AvatarHand_L : MonoBehaviourPunCallbacks, IPunObservable
{
    public InputDevice targetDevice;
    public Renderer avatarLeftHand;
    //public PhotonView PV;

    void Start()
    {
        //PV = GetComponent<PhotonView>();
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics leftControllerCharacteristics =
            InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devices);
        avatarLeftHand = GetComponentInChildren<Renderer>();

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        if (photonView.IsMine)
        {
            if (targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))
            {
                if (griped)
                {
                    photonView.RPC("RPC_IfGriped_HideHand_L", RpcTarget.All, true);
                }
                else
                {
                    photonView.RPC("RPC_IfGriped_HideHand_L", RpcTarget.All, false);
                }
            }
        }
    }

    [PunRPC]
    public void RPC_IfGriped_HideHand_L(bool isGrip)
    {
        if (isGrip)
        {
            avatarLeftHand.forceRenderingOff = true;            
        }
        else
        {
            avatarLeftHand.forceRenderingOff = false; 
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
