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
using UnityEditor;

public class SnowBlock : XRGrabInteractable
{
    public static SnowBlock SB;
    public PhotonView PV;
    public GameObject DestroyEX;
    
    void Start()
    {
        SB=this;
        PV = GetComponent<PhotonView>();     
    }
   
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Stoneball"))
        {
            PV.RPC(nameof(Disappear), RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void Disappear()
    {        
        StartCoroutine(EXOnOff());
    }

    public IEnumerator EXOnOff()
    {
        PN.InstantiateRoomObject(DestroyEX.name, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.01f);
        Destroy(gameObject);
    } 
}
