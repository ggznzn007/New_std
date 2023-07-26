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
public class Control : MonoBehaviourPunCallbacks
{
    public XRRayInteractor interactor;
    public PhotonView PV;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        interactor = GetComponent<XRRayInteractor>();
    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
        {
            if (NetworkedGrabbing.NG.isBeingHeld)
            {
                interactor.enabled = false;
            }
            else
            {
                interactor.enabled = true;
            }
        }
    }
}
