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
using TMPro;
using Unity.XR.PXR;
using Unity.VisualScripting;

public class SpawnBox : XRBaseInteractable
{
    [SerializeField] GameObject bow;
    [SerializeField] Transform attachPoint;
    private GameObject myBow;

    private void Start()
    {
        DataManager.DM.grabBow = false;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        //SpawnBow();
        DataManager.DM.grabBow = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        DataManager.DM.grabBow = false;
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
