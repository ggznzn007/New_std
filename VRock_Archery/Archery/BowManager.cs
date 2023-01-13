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
public class BowManager : MonoBehaviourPun//, IPunObservable
{
    public static BowManager BowM;
    public PhotonView PV;                           // 포톤뷰
    //private Vector3 remotePos;
    //private Quaternion remoteRot;
    public List<Collider> bowColls;
    public bool isBeingHeld = false;
    public bool isGrip;
    Rigidbody rb;
    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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
    }*/

    private void Awake()
    {
        BowM = this;
        PV = GetComponent<PhotonView>();
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cube"))
        {
            try
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
            }
        }
    }

    [PunRPC]
    public void DestroyBow()=> Destroy(PV.gameObject);


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
        Debug.Log("활을 잡았습니다.");
        PV.RPC(nameof(StartGrabbing), RpcTarget.AllBuffered);       
    }

    public void OnSelectedExited()
    {
        Debug.Log("활을 놓았습니다.");
        PV.RPC(nameof(StopGrabbing), RpcTarget.AllBuffered);
    }
}
