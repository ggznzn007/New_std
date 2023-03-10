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

public class BlockManager : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks, IPunObservable
{
    public PhotonView PV;
    public bool isBeingHeld;
    private bool isGrip;
    private Vector3 remotePos;
    private Quaternion remoteRot;

    private void Awake()
    {
        isBeingHeld = false;
        isGrip = false;
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
        {
            //float t = Mathf.Clamp(Time.deltaTime * 10, 0f, 0.99f);
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, Time.deltaTime * 30)
                , Quaternion.Lerp(transform.rotation, remoteRot, Time.deltaTime * 30));
            return;
        }
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
}
