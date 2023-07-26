using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;


public class HP_Nick : MonoBehaviourPunCallbacks
{    
    public Camera myCam;
    public GameObject hp_Bar;
    public GameObject nick;

    void Update()
    {
        // transform.LookAt(transform.position + Camera.main.transform.rotation * -Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        transform.SetPositionAndRotation(myCam.transform.position + new Vector3(0, 0.25f, 0), myCam.transform.rotation);
        transform.forward = -myCam.transform.forward;        
    }

    /*[PunRPC]
    public void HP_Nick_Lookat()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * -Vector3.forward, player.transform.rotation * Vector3.up);
    }*/
}
