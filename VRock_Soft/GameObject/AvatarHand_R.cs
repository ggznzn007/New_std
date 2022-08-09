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
public class AvatarHand_R : MonoBehaviourPun//, IPunObservable
{
    public InputDevice targetDevice;
    public SkinnedMeshRenderer[] avatarRightHand;

    void Start()
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

    private void FixedUpdate()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))
        {
            if (griped)
            {
                avatarRightHand[0].forceRenderingOff = true;
                avatarRightHand[1].forceRenderingOff = true;
            }
            else
            {
                avatarRightHand[0].forceRenderingOff = false;
                avatarRightHand[1].forceRenderingOff = false;
            }
        }
      /*  if (photonView.IsMine)
        {
            photonView.RPC("HideHandGriped", RpcTarget.All);
        }*/
    }

  /*  public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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
    }*/

    /* [PunRPC]

     void HideHandGriped(InputDevice targetDevice, SkinnedMeshRenderer[] avatarRightHand)
     {
         if (targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))
         {
             if (griped)
             {
                 avatarRightHand[0].forceRenderingOff = true;
                 avatarRightHand[1].forceRenderingOff = true;
             }
             else
             {
                 avatarRightHand[0].forceRenderingOff = false;
                 avatarRightHand[1].forceRenderingOff = false;
             }
         }
     }*/
}
