using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;


public class HP_Bar : MonoBehaviourPunCallbacks
{
   // public PhotonView PV;

    private void Start()
    {
      //  PV = GetComponent<PhotonView>();
    }
    void FixedUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * -Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        /*if(PV.IsMine)
          {
              PV.RPC("HP_Nick", RpcTarget.All);
          }*/
    }

   /* [PunRPC]
    public void HP_Nick()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * -Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }*/
}
