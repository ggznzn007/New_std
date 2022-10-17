using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using Antilatency;
using Antilatency.TrackingAlignment;
using Antilatency.DeviceNetwork;
using Antilatency.Alt;
using Antilatency.SDK;
public class VisualControll : MonoBehaviourPun
{
    public Camera myCam;

    private void Start()
    {
        
    }
    void Update()
    {
        if(!photonView.IsMine)
        {
            myCam.cullingMask = SortingLayer.layers.Length;
        }

    }
}
