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
    private Vector3 remotePos;
    private Quaternion remoteRot;

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
       // if (!photonView.IsMine) return;
        if (!photonView.IsMine)
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, 20 * Time.deltaTime)
                , Quaternion.Lerp(transform.rotation, remoteRot, 20 * Time.deltaTime));
        }
        if (photonView.IsMine)
        {
            if (targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))
            {
                if (griped)
                {
                    photonView.RPC(nameof(HideHand_L), RpcTarget.All, true);
                }
                else
                {
                    photonView.RPC(nameof(HideHand_L), RpcTarget.All, false);
                }
            }
        }
    }

    [PunRPC]
    public void HideHand_L(bool isGrip)
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
            remotePos = (Vector3)stream.ReceiveNext();
            remoteRot = (Quaternion)stream.ReceiveNext();
            // transform.position = (Vector3)stream.ReceiveNext();
            //transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
