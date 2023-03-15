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

public class BowManager : MonoBehaviourPun, IPunObservable
{
    public static BowManager BowM;
    public PhotonView PV;
    // public Transform pull;
    // public Transform notch;
    public Transform bowString;
    //private Vector3 remotePos;
    //private Quaternion remoteRot;
    // public List<Collider> bowColls;
    public bool isBeingHeld = false;
    public bool isGrip;
    Rigidbody rb;
    public GameObject shield;
    //public GameObject shield_L;
    public GameObject bow;
    public Notch notch;
    public Collider pullColl;
    //public bool isRight;

    private void Awake()
    {
        BowM = this;
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shield.SetActive(false);
        bow.SetActive(true);
        pullColl.enabled = true;
        notch.enabled = true;
        isGrip = true;
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
        }
        else
        {
            isGrip = false;
            rb.isKinematic = false;
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
            stream.SendNext(bowString.position);
            stream.SendNext(bowString.rotation);
        }
        else
        {
            //notch.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            //pull.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            bowString.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {// 앞에 수는 진동의 진폭
     // 뒤에 수는 진동의 강도
        if (collision.collider.CompareTag("Arrow"))
        {
            if (PV.IsMine && isRight)
            {
                if (!PV.IsMine) return;
                PXR_Input.SetControllerVibration(0.5f, 10, PXR_Input.Controller.RightController);
            }
            if (PV.IsMine && !isRight)
            {
                if (!PV.IsMine) return;
                PXR_Input.SetControllerVibration(0.5f, 10, PXR_Input.Controller.LeftController);
            }
        }
    }*/
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("FloorBox") || collision.collider.CompareTag("Cube"))
        {
            if (PV.IsMine)
            {
                if (!isGrip)
                {
                    PV.RPC(nameof(DestroyBow), RpcTarget.AllBuffered);
                    Debug.Log("활이 파괴되었습니다.");
                }
            }
            /* try
             {
                 if (!isBeingHeld && !isGrip)
                 {
                     if (PV.IsMine)
                     {
                         PV.RPC(nameof(DestroyBow), RpcTarget.AllBuffered);
                         Debug.Log("활이 파괴되었습니다.");
                     }
                 }
             }
             finally
             {
                 if (PV.IsMine)
                 {
                     PV.RPC(nameof(DestroyBow), RpcTarget.AllBuffered);
                 }
             }*/
        }

    }

    /*private void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("RightHand"))
        {
            //if (coll.CompareTag("LeftHand")) return;
            if (PV.IsMine)
            {
                isRight = true;
            }

        }

        if (coll.CompareTag("LeftHand"))
        {
            //if (coll.CompareTag("RightHand")) return;
            if (PV.IsMine)
            {
                isRight = false;
            }
        }
    }*/

    [PunRPC]
    public void DestroyBow() => Destroy(gameObject);


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

    [PunRPC]
    public void ShieldOn()
    {
        shield.SetActive(true);
        bow.SetActive(false);
        pullColl.enabled = false;
        notch.enabled = false;
    }

    [PunRPC]
    public void ShieldOff()
    {
        shield.SetActive(false);
        bow.SetActive(true);
        pullColl.enabled = true;
        notch.enabled = true;
    }

    public void OnSelectedEntered()
    {
        Debug.Log("활을 잡았습니다.");
        PV.RPC(nameof(StartGrabbing), RpcTarget.AllBuffered);
    }

    public void OnSelectedExited()
    {
        Debug.Log("활을 놓았습니다.");
        PV.RPC(nameof(StopGrabbing), RpcTarget.AllBuffered);
    }

    public void OnActive()
    {
        if (!DataManager.DM.grabString)
        {
            if (DataManager.DM.grabString) { return; }
            PV.RPC(nameof(ShieldOn), RpcTarget.AllBuffered);
        }
    }

    public void OnDeactive()
    {
        if (!DataManager.DM.grabString)
        {
            if(DataManager.DM.grabString) { return; }
            PV.RPC(nameof(ShieldOff), RpcTarget.AllBuffered);
        }
    }


}
