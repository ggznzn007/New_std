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


public class Shield : XRGrabInteractable//MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    public static Shield SD;
    public PhotonView PV;
    Rigidbody rb;
    public bool isBeingHeld = false;   
   // SelectionOutline outline = null;    

    private void Start()
    {
        SD= this;
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();          
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        PV.RequestOwnership();
        Debug.Log("잡았다");
        PV.RPC(nameof(Grab_EMP), RpcTarget.AllBuffered);        
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);        
        Debug.Log("놓았다");        
        PV.RPC(nameof(Put_EMP), RpcTarget.AllBuffered);       
    }

    public void OnCollisionStay(Collision collision)
    {
        if(collision.collider.CompareTag("FloorBox"))
        {
            transform.position = new Vector3(0,1.2f,0);
        }
    }

    private void Update()
    {
        if (isBeingHeld)
        {
            rb.isKinematic = true;
            //gameObject.layer = 7;
        }
        else
        {
            rb.isKinematic = false;
            //gameObject.layer = 12;
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

  
  /*  public void OnSelectedEntered()
    {
        Debug.Log("잡았다");
        PV.RPC(nameof(Grab_EMP), RpcTarget.AllBuffered);        
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

    public void TransferOwnership()
    {
        PV.RequestOwnership();
    }*/

   /* public void OnHoverEntered()
    {
        outline.Highlight();
    }

    public void OnHoverExited()
    {
        outline.RemoveHighlight();
    }*/
}
