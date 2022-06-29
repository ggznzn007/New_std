using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using TMPro;
public class NetworkedGrabbing : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    PhotonView PV;

    Rigidbody rb;
    bool isBeingHeld = false;
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
            gameObject.layer = 11;
        }
        else
        {
            rb.isKinematic = false;
            gameObject.layer = 9;
        }
    }
    private void TransferOwnership()
    {
        PV.RequestOwnership();
    }
    public void OnSelectedEntered()
    {
        print("��Ҵ�");
        PV.RPC("StartNetworkGrabbing", RpcTarget.AllBuffered);
        if (PV.Owner == PN.LocalPlayer)
        {
            print("�̹� �������� ������ �ֽ��ϴ�.");
        }
        else
        {
            TransferOwnership();
        }
    }

    public void OnSelectedEXisted()
    {
        print("���Ҵ�");
        PV.RPC("StopNetworkGrabbing", RpcTarget.AllBuffered);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if(targetView != PV)
        {
            return;
        }

        print("������ �ѱ��: " + targetView.name + "from " + requestingPlayer.NickName);
        PV.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        print("��������� �÷��̾�: " + targetView.name + "from " + previousOwner.NickName);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {

    }

    [PunRPC]

    public void StartNetworkGrabbing()
    {
        isBeingHeld = true;
    }

    [PunRPC]
    public void StopNetworkGrabbing()
    {
        isBeingHeld = false;
    }

}
