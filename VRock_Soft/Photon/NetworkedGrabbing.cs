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
public class NetworkedGrabbing : MonoBehaviourPunCallbacks//, IPunOwnershipCallbacks
{
    PhotonView PV;
    Rigidbody rb;
    public bool isBeingHeld = false;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (isBeingHeld)
        {
            rb.isKinematic = true;
            gameObject.layer = 7;
        }
        else
        {
            rb.isKinematic = false;
            gameObject.layer = 6;
        }
    }



    public void OnSelectedEntered()
    {
        Debug.Log("잡았다");
        PV.RPC(nameof(StartGrabbing), RpcTarget.AllBuffered);
        /*if (PV.Owner == PN.LocalPlayer)
        {
            Debug.Log("이미 소유권이 나에게 있습니다.");
        }
        else
        {
            TransferOwnership();
        }*/
    }

    public void OnSelectedExited()
    {
        Debug.Log("놓았다");
        PV.RPC(nameof(StopGrabbing), RpcTarget.AllBuffered);
    }

    private void TransferOwnership()
    {
        PV.RequestOwnership();
    }

    /*public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != PV)
        {
            return;
        }
        Debug.Log("소유권 요청 : " + targetView.name + "from " + requestingPlayer.NickName);
        PV.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("현재소유한 플레이어: " + targetView.name + "from " + previousOwner.NickName);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {

    }*/

    [PunRPC]
    public void StartGrabbing()
    {
        isBeingHeld = true;
        
    }

    [PunRPC]
    public void StopGrabbing()
    {
        isBeingHeld = false;
        
    }


}
