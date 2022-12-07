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
public class BowManager : MonoBehaviourPun, IPunObservable
{
    public static BowManager BowM;
    public PhotonView PV;                           // 포톤뷰
    private Vector3 remotePos;
    private Quaternion remoteRot;
    public List<Collider> bowColls;
    public bool isBeingHeld = false;

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
        BowM = this;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cube"))
        {
            try
            {
                if (!isBeingHeld)
                {
                    if (PV.IsMine)
                    {
                        PV.RPC(nameof(DestroyBow), RpcTarget.AllBuffered);
                    }
                }
            }
            finally
            {
                if (PV.IsMine)
                {
                    PV.RPC(nameof(DestroyBow), RpcTarget.AllBuffered);
                }
            }
        }
    }

    [PunRPC]
    public void DestroyBow()
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
        Debug.Log("잡았다\n레이어 = Inhand");
        PV.RPC(nameof(StartGrabbing), RpcTarget.AllBuffered);
        /* if (PV.Owner == PN.LocalPlayer)
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
        Debug.Log("놓았다\n레이어 = Interactable");
        PV.RPC(nameof(StopGrabbing), RpcTarget.AllBuffered);
    }
}
