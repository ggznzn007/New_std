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
public class Arrow_Bomb : Arrow
{   
    public Collider tagColl;
    public ParticleSystem fireEX;
    private bool isRotate;    

    protected override void Awake()
    {
        base.Awake();        
        isRotate = true;
        tagColl.tag = "Skilled";
        //fireEX.gameObject.SetActive(false);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        arrowM.OnSelectedEntered();
        //PV.RequestOwnership();
        DataManager.DM.grabArrow = true;       
        isRotate = false;    
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        tagColl.tag = "Arrow";
        //DataManager.DM.arrowNum = 3;
        if (args.interactorObject is Notch notch)
        {    
            if (notch.CanRelease)
            {
                DataManager.DM.arrowNum = 3;
                tagColl.tag = "Arrow";
                LaunchArrow(notch);
                arrowM.OnSelectedExited();
                if (PV.IsMine)
                {
                    if (!PV.IsMine) return;
                    PV.RPC(nameof(Active_EX), RpcTarget.AllBuffered);
                }

            }
        }

    }

    private void Update()
    {
        if (isRotate)
        {
            transform.Rotate(rotSpeed * Time.deltaTime * new Vector3(0, 0, 1));
        }
        else
        {
            rotSpeed = 0;
        }
    }
    

    [PunRPC]
    public void Active_EX()
    {
       StartCoroutine(ActiveCtrl());
    }

    public IEnumerator ActiveCtrl()
    {
        fireEX.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        fireEX.gameObject.SetActive(false);
    }
}
