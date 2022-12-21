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
    public Transform left_Grab;
    public Transform right_Grab;
    public XRGrabInteractable XRG;    
    public bool isGrab;
    //  SelectionOutline outline = null;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        XRG=GetComponent<XRGrabInteractable>();        
        // outline = GetComponent<SelectionOutline>();
        isGrab= false;
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

    private void OnTriggerStay(Collider coll)
    {
        if(coll.CompareTag("RightHand")&&!isGrab&&PV.IsMine)
        {
            XRG.attachTransform = right_Grab;            
        }

        if (coll.CompareTag("LeftHand")&&!isGrab && PV.IsMine)
        {
            XRG.attachTransform = left_Grab;            
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
        isGrab= true;
        //spColl.enabled = false;
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
        isGrab= false;
        //spColl.enabled = true;
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

    /* public void OnHoverEntered()
     {
         outline.Highlight();
     }

     public void OnHoverExited()
     {
         outline.RemoveHighlight();
     }*/
}
