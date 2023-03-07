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
using Unity.XR.PXR;

public class SlingManager : MonoBehaviourPun, IPunObservable
{
    public static SlingManager slM;
    public PhotonView PV;
    public Transform slingString;
    public bool isBeingHeld = false;
    public bool isGrip;
    Rigidbody rb;
    //public GameObject shield_R;
    //public GameObject shield_L;
    public GameObject sling;
    public GameObject trashCan;
    public Notch_S notch;
    public Collider pullColl;
    public Collider crashColl;
    public bool isRight;

    private void Awake()
    {
        slM = this;
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pullColl.enabled = true;
        notch.enabled = true;
        isGrip = true;
        isRight = false;
        sling.SetActive(true);
        trashCan.SetActive(false);
    }

    void Update()
    {
        /* if (!PV.IsMine)
         {
             transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, 10 * Time.deltaTime)
                 , Quaternion.Lerp(transform.rotation, remoteRot, 10 * Time.deltaTime));
         }*/

        if (isBeingHeld)
        {
            isGrip = true;
            rb.isKinematic = true;
            crashColl.isTrigger = true;
        }
        else
        {
            isGrip = false;
            rb.isKinematic = false;
            crashColl.isTrigger = false;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //stream.SendNext(notch.position);
            //stream.SendNext(notch.rotation);
            //stream.SendNext(pull.position);
            //stream.SendNext(pull.rotation);
            stream.SendNext(slingString.position);
            stream.SendNext(slingString.rotation);
        }
        else
        {
            //notch.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            //pull.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            slingString.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("FloorBox") || collision.collider.CompareTag("Cube"))
        {
            if (PV.IsMine)
            {
                if (!isGrip)
                {
                    try
                    {
                        PV.RPC(nameof(DestroyBow), RpcTarget.AllBuffered);
                        Debug.Log("새총이 파괴되었습니다.");
                    }
                    finally
                    {
                        PV.RPC(nameof(DestroyBow), RpcTarget.AllBuffered);
                    }
                }
            }

        }

    }


    [PunRPC]
    public void DestroyBow() => Destroy(PV.gameObject);


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
        Debug.Log("새총을 잡았습니다.");
        PV.RPC(nameof(StartGrabbing), RpcTarget.AllBuffered);
    }

    public void OnSelectedExited()
    {
        Debug.Log("새총을 놓았습니다.");
        PV.RPC(nameof(StopGrabbing), RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void CanOn()
    {
        sling.SetActive(false);
        trashCan.SetActive(true);
        pullColl.enabled = false;
        notch.enabled = false;
    }

    [PunRPC]
    public void CanOff()
    {
        sling.SetActive(true);
        trashCan.SetActive(false);
        pullColl.enabled = true;
        notch.enabled = true;
    }

    public void OnActive()
    {
        if(PV.IsMine)
        PV.RPC(nameof(CanOn), RpcTarget.AllBuffered);
    }

    public void OnDeactive()
    {
        if(PV.IsMine)
        PV.RPC(nameof(CanOff), RpcTarget.AllBuffered);
    }
}
