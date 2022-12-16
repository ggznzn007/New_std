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

public class AvatarHand_L : MonoBehaviourPun, IPunObservable
{
    public InputDevice targetDevice;
    public MeshRenderer avatarLeftHand;
    public PhotonView PV;
    private Vector3 remotePos;
    private Quaternion remoteRot;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics leftControllerCharacteristics =
            InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devices);
        avatarLeftHand = GetComponentInChildren<MeshRenderer>();

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    private void Update()
    {
        if (!PV.IsMine)
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, 10 * Time.deltaTime)
                , Quaternion.Lerp(transform.rotation, remoteRot, 10 * Time.deltaTime));
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))
        {
            if (PV.IsMine)
            {
                if (griped)
                {
                    PV.RPC(nameof(HandHide_L), RpcTarget.AllBuffered, griped);
                }
                else
                {
                    PV.RPC(nameof(HandHide_L), RpcTarget.AllBuffered, griped);
                }
            }
        }
    }

    [PunRPC]
    public void HandHide_L(bool grip)
    {
        if(grip)
        {
            avatarLeftHand.forceRenderingOff = grip;
        }
        else
        {
            avatarLeftHand.forceRenderingOff = grip;
        }        
    }

   /* [PunRPC]
    public void HandBye(bool grip)
    {
        avatarLeftHand.forceRenderingOff = grip;
    }*/

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
        }
    }
    /*[PunRPC]
    public void HideHand_L(bool isGrip)
    {
        if (isGrip)
        {
            gameObject.layer = 10;
            //avatarLeftHand.forceRenderingOff = true;
        }
        else
        {
            gameObject.layer = 3;
            //avatarLeftHand.forceRenderingOff = false;
        }
    }*/

}
