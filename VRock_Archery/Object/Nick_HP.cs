using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
public class Nick_HP : MonoBehaviourPunCallbacks //,IPunObservable
{
    public Camera myCam;
    public GameObject hp_Bar;
    public GameObject nick;

    void FixedUpdate()
    {
        hp_Bar.transform.SetPositionAndRotation(myCam.transform.position + new Vector3(0, 0.15f, 0), myCam.transform.rotation);
        nick.transform.SetPositionAndRotation(myCam.transform.position + new Vector3(0, 0.25f, 0), myCam.transform.rotation);    

        hp_Bar.transform.forward = -myCam.transform.forward;
        nick.transform.forward = -myCam.transform.forward;
    }  

    // transform.LookAt(transform.position + Camera.main.transform.rotation * -Vector3.forward, Camera.main.transform.rotation * Vector3.up);
}
