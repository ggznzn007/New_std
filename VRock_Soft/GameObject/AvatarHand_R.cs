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
public class AvatarHand_R : MonoBehaviourPun, IPunObservable // 아바타 손 관리하는 스크립트
{
    public InputDevice targetDevice;
    public MeshRenderer avatarRightHand;
    public PhotonView PV;
    private Vector3 remotePos;
    private Quaternion remoteRot;    

    void Start()
    {
        PV = GetComponent<PhotonView>();
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics =
            InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);
        avatarRightHand = GetComponentInChildren<MeshRenderer>();

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

 /*   private void Update()
    {
        //if (!photonView.IsMine) return;
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
                    PV.RPC(nameof(HandHide_R), RpcTarget.AllBuffered, griped);
                }
                else
                {
                    PV.RPC(nameof(HandHide_R), RpcTarget.AllBuffered, griped);
                }
            }

        }

    }

    [PunRPC]
    public void HandHide_R(bool grip)
    {
        if (grip)
        {
            avatarRightHand.forceRenderingOff = grip;
        }
        else
        {
            avatarRightHand.forceRenderingOff = grip;
        }
    }
*/
    /*[PunRPC]
    public void HandBye(bool grip)
    {
        avatarRightHand.forceRenderingOff = grip;
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

    /* public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
     {
         if (stream.IsWriting)
         {
             stream.SendNext(transform.position);
             stream.SendNext(transform.rotation);

             // stream.SendNext(avatarRightHand[0].forceRenderingOff);
             //stream.SendNext(avatarRightHand[1].forceRenderingOff);

         }
         else if (stream.IsReading)
         {
             stream.ReceiveNext();
             stream.ReceiveNext();
         }
         else if (stream.IsReading)
         {
             this.transform.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());

             //  this.avatarRightHand[0].forceRenderingOff = (bool)stream.ReceiveNext();
             //  this.avatarRightHand[1].forceRenderingOff = (bool)stream.ReceiveNext();
         }
     }
 */


    /*  public void IfGriped_HideHand()
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
