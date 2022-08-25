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
    public GameObject player;

    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * -Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    /*[PunRPC]
    public void HP_Nick_Lookat()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * -Vector3.forward, player.transform.rotation * Vector3.up);
    }*/

}
