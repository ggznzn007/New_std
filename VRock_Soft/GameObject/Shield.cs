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


public class Shield : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    public PhotonView PV;
    Rigidbody rb;
    public bool isBeingHeld = false;   
    SelectionOutline outline = null;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        outline= GetComponent<SelectionOutline>();
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
   
    [PunRPC]
    public void Grab_EMP()
    {
        isBeingHeld = true;
    }

    [PunRPC]
    public void Put_EMP()
    {
        isBeingHeld = false;
    }
    public void OnSelectedEntered()
    {
        Debug.Log("잡았다");
        PV.RPC(nameof(Grab_EMP), RpcTarget.AllBuffered);
        outline.RemoveHighlight();
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
        Debug.Log("놓았다");
        PV.RPC(nameof(Put_EMP), RpcTarget.AllBuffered);
        outline.RemoveHighlight();
    }
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != PV) { return; }
        Debug.Log("소유권 요청 : " + targetView.name + "from " + requestingPlayer.NickName);
        PV.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("현재소유한 플레이어: " + targetView.name + "from " + previousOwner.NickName);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        throw new NotImplementedException();
    }

    private void TransferOwnership()
    {
        PV.RequestOwnership();
    }

    public void OnHoverEntered()
    {
        outline.Highlight();
    }

    public void OnHoverExited()
    {
        outline.RemoveHighlight();
    }
}
