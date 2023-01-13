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

public class ArrowManager : MonoBehaviourPunCallbacks//, IPunObservable
{
    public static ArrowManager ArrowM;
    public PhotonView PV;                           // 포톤뷰
    //private Vector3 remotePos;
    //private Quaternion remoteRot;
    public bool isBeingHeld = false;
    //Rigidbody rb;
    public bool isGrip;
    public string headShot;
    public string hit;
    public int actNumber;

    /*  public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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
        ArrowM = this;
        isGrip = true;
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
        //rb = GetComponent<Rigidbody>();
        isGrip = true;
        
    }


    void Update()
    {
      /*  if (!PV.IsMine)
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, 10 * Time.deltaTime)
                , Quaternion.Lerp(transform.rotation, remoteRot, 10 * Time.deltaTime));
        }*/

        if (isBeingHeld)
        {
            isGrip = true;
           // rb.isKinematic = true;
        }
        else
        {
            isGrip = false;
            // rb.isKinematic = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!AvartarController.ATC.isAlive)
        {
            return;
        }

        if (collision.collider.CompareTag("Cube"))
        {
            if (!isBeingHeld && !isGrip)
            {
                if (PV.IsMine)
                {
                    AudioManager.AM.PlaySE(hit);
                    PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);
                }
            }         
        }

        if (collision.collider.CompareTag("Finish"))
        {
            if (!isBeingHeld && !isGrip)
            {
                if (PV.IsMine)
                {
                    AudioManager.AM.PlaySE(hit);
                    PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);
                }
            }
        }

        if (collision.collider.CompareTag("Head"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;               
                AudioManager.AM.PlaySE(headShot);                
            }
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
        Debug.Log("화살을 잡았습니다.");
        PV.RPC(nameof(StartGrabbing), RpcTarget.AllBuffered);        
    }

    public void OnSelectedExited()
    {
        Debug.Log("화살을 놓았습니다.");
        PV.RPC(nameof(StopGrabbing), RpcTarget.AllBuffered);       
    }

    /* private void OnCollisionEnter(Collision collision)
     {
         if (!AvartarController.ATC.isAlive)
         {
             return;
         }

         if (collision.collider.CompareTag("Cube") && PV.IsMine) // 일반태그
         {
             PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);
         }

         if (collision.collider.CompareTag("BlueTeam") || collision.collider.CompareTag("RedTeam") && !PV.IsMine)
         {
             PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);
         }

         if (collision.collider.CompareTag("Head") && !PV.IsMine)
         {
             PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);
         }

         if (collision.collider.CompareTag("Body") && !PV.IsMine)
         {
             PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);
         }

     }*/

    [PunRPC]
    public void DestroyArrow()
    {
        Destroy(gameObject, 0.15f);
        Debug.Log("화살이 딜레이파괴되었습니다.");
    }  
}
