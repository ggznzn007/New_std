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

public class ArrowManager : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{   
    public PhotonView PV;    
    public bool isBeingHeld;
    private bool isGrip;

    private void Awake()
    {
        isBeingHeld = false;
        isGrip= false;
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();                   
    }

    void FixedUpdate()
    {     
        if (isBeingHeld)
        {
            isGrip = true;   
        }
        else
        {
            isGrip = false;
        }
    }
     
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

    public void OnSelectedEntered()
    {
        Debug.Log("화살을 잡았습니다.");
        PV.RPC(nameof(StartGrabbing), RpcTarget.AllBuffered);
        if (PV.Owner == PN.LocalPlayer)
        {
            Debug.Log("이미 소유권이 나에게 있습니다.");
        }
        else
        {
            TransferOwnership();
        }
    }

    public void OnSelectedExited()
    {
        Debug.Log("화살을 놓았습니다.");
        PV.RPC(nameof(StopGrabbing), RpcTarget.AllBuffered);       
    }

    private void TransferOwnership()
    {
        PV.RequestOwnership();
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
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

    }
}
