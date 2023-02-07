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
using Photon.Pun.Demo.Procedural;
using UnityEngine.InputSystem.HID;

public class ItemBox : XRBaseInteractable
{
    [SerializeField] Transform attachPoint;
    [SerializeField] GameObject bow;
    private GameObject myBow;

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);        
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        SpawnBow();
    }

    public void SpawnBow()
    {
        if (!DataManager.DM.grabBow)
        {
            Bow bow = CreateBow();
            myBow = bow.gameObject;
        }
    }

    private Bow CreateBow()
    {
        myBow = PN.Instantiate(bow.name, attachPoint.position, attachPoint.rotation);
        return myBow.GetComponent<Bow>();
    }
}
