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
        Debug.Log("ȭ���� ��ҽ��ϴ�.");
        PV.RPC(nameof(StartGrabbing), RpcTarget.AllBuffered);
        if (PV.Owner == PN.LocalPlayer)
        {
            Debug.Log("�̹� �������� ������ �ֽ��ϴ�.");
        }
        else
        {
            TransferOwnership();
        }
    }

    public void OnSelectedExited()
    {
        Debug.Log("ȭ���� ���ҽ��ϴ�.");
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
        Debug.Log("������ ��û : " + targetView.name + "from " + requestingPlayer.NickName);
        PV.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("��������� �÷��̾�: " + targetView.name + "from " + previousOwner.NickName);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {

    }
}
