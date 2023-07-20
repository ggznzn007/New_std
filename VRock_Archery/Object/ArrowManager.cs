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

public class ArrowManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static ArrowManager ArrowM;
    public PhotonView PV;                           // Æ÷Åæºä
    private Vector3 remotePos;
    private Quaternion remoteRot;

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

    private void Awake()
    {
        ArrowM = this;
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }


    void Update()
    {
        if (!PV.IsMine)
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, 30 * Time.deltaTime)
                , Quaternion.Lerp(transform.rotation, remoteRot, 30 * Time.deltaTime));
        }
    }

   /* private void OnCollisionEnter(Collision collision)
    {
        if (!AvartarController.ATC.isAlive)
        {
            return;
        }
       
        if (collision.collider.CompareTag("Cube")&& PV.IsMine) // ÀÏ¹ÝÅÂ±×
        {
            PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);
        }

        if (collision.collider.CompareTag("BlueTeam") || collision.collider.CompareTag("RedTeam") && PV.IsMine)
        {
            PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);
        }

        if (collision.collider.CompareTag("Head") && PV.IsMine)
        {
            PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);
        }

        if (collision.collider.CompareTag("Body") && PV.IsMine)
        {
            PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);
        }

        else
        {
            Destroy(gameObject, 1);
        }
    }*/

    [PunRPC]
    public void DestroyArrow() => Destroy(gameObject,2);
}
