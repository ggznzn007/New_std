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
    public PhotonView PV;    
    public bool isBeingHeld = false;
    public bool isGrip;    
  

   

    private void Awake()
    {
       
        isGrip = true;
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();       
        isGrip = true;        
    }


    void Update()
    {     
        if (isBeingHeld)
        {
            isGrip = true;   
        }
        else
        {
            isGrip = false;
        }
    }

   /* private void OnCollisionEnter(Collision collision)
    {
        if (!AvartarController.ATC.isAlive)
        {
            return;
        }

        if (collision.collider.CompareTag("Cube"))
        {
            if (PV.IsMine)
            {
                if (!isGrip)
                {
                    try
                    {
                        PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);
                    }
                    finally { PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered); }
                }
            }
            if (!isBeingHeld && !isGrip)
            {
                if (PV.IsMine)
                {
                    // AudioManager.AM.PlaySE(hit);
                    PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);
                }
            }
        }

        if (collision.collider.CompareTag("Finish"))
        {
            if (PV.IsMine)
            {
                if (!isGrip)
                {
                    try
                    {
                        PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);
                    }
                    finally { PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered); }
                }
            }
            if (!isBeingHeld && !isGrip)
            {
                if (PV.IsMine)
                {
                    //AudioManager.AM.PlaySE(hit);     
                    PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);
                }
            }
        }


    }*/


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
   
}
