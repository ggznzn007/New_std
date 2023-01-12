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
    public static NetworkedGrabbing NG;
    PhotonView PV;
    Rigidbody rb;
    public bool isBeingHeld = false;
    public bool isGrip;
    
    private void Awake()
    {
        NG = this;
        PV = GetComponent<PhotonView>(); 
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();        
        isGrip = true;
    }
   
    private void FixedUpdate()
    {       
        if (isBeingHeld)               // ���� ���忡�� �տ� ��������
        {
            isGrip = true;
            rb.isKinematic = true;
            this.gameObject.layer = 7;           
        }
        else
        {      
            isGrip = false;
            rb.isKinematic = false;
            this.gameObject.layer = 6;         
        }       
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cube")|| collision.collider.CompareTag("FloorBox"))
        {
            try
            {
                if (!isBeingHeld && !isGrip)
                {
                    if (PV.IsMine)
                    {
                        PV.RPC(nameof(DestroyGun), RpcTarget.AllBuffered);
                    }
                }
            }
            finally
            {
                if (PV.IsMine)
                {
                    PV.RPC(nameof(DestroyGun), RpcTarget.AllBuffered);
                }
            }
        }
    }

    [PunRPC]
    public void DestroyGun()
    {
        Destroy(PV.gameObject);
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
        Debug.Log("��Ҵ�\n���̾� = Inhand");              
        PV.RPC(nameof(StartGrabbing), RpcTarget.AllBuffered);
       /* if (PV.Owner == PN.LocalPlayer)
        {
            Debug.Log("�̹� �������� ������ �ֽ��ϴ�.");
        }
        else
        {
            TransferOwnership();
        }*/
    }

    public void OnSelectedExited()
    {
        Debug.Log("���Ҵ�\n���̾� = Interactable");                    
        PV.RPC(nameof(StopGrabbing), RpcTarget.AllBuffered);
    }

/*    private void TransferOwnership()
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
*/
    


}
