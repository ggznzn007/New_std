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
public class AvatarHand_R : MonoBehaviourPunCallbacks, IPunObservable // 아바타 손 관리하는 스크립트
{
    public InputDevice targetDevice;
    public Renderer avatarRightHand;
    //public PhotonView PV;

    void Start()
    {
        //PV = GetComponent<PhotonView>();
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics =
            InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);
        avatarRightHand = GetComponentInChildren<Renderer>();

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
                    photonView.RPC("RPC_IfGriped_HideHand_R", RpcTarget.All, true);
                }
                else
                {
                    photonView.RPC("RPC_IfGriped_HideHand_R", RpcTarget.All, false);
                }
            }         
        }
    }

    [PunRPC]
    public void RPC_IfGriped_HideHand_R(bool isGrip)
    {
        if (isGrip)
        {
            avatarRightHand.forceRenderingOff = true;            
        }
        else
        {
            avatarRightHand.forceRenderingOff = false;            
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
           transform.position=(Vector3)stream.ReceiveNext();
           transform.rotation=(Quaternion)stream.ReceiveNext();
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
