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
public class Arrow_MultiShot : Arrow
{
    public static Arrow_MultiShot ArrowMul;
    public GameObject effects;
    public GameObject[] arrowMesh;
    public TrailRenderer[] tails;

    private void Start()
    {
        ArrowMul = this;
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        arrowM.OnSelectedEntered();
        DataManager.DM.grabArrow = true;
        PV.RequestOwnership();        
        DataManager.DM.arrowNum = 2;
        rotSpeed = 0;
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if (PV.IsMine) { if (!PV.IsMine) return; PV.RPC(nameof(Multipack), RpcTarget.AllBuffered); }       
        
        if (args.interactorObject is Notch notch)
        {
            //GetTarget();    
            if (notch.CanRelease)
            {
                LaunchArrow(notch);                
                arrowM.OnSelectedExited();
                
                if(PV.IsMine)
                {
                    if (!PV.IsMine) return;
                    PV.RPC(nameof(Tailpack), RpcTarget.AllBuffered);
                }
                
                if (effects != null)
                {
                    if (!PV.IsMine) return;                    
                    PV.RPC(nameof(DelayEX), RpcTarget.AllBuffered);                    
                }
                else
                {
                    effects = null;
                }

            }
        }

    }

    private void Update()
    {
        transform.Rotate(rotSpeed * Time.deltaTime * new Vector3(0, 0, 1));
    }

    [PunRPC]
    public void DelayEX()
    {
        StartCoroutine(nameof(DelayEffect));
    }

    [PunRPC]
    public void Multipack()
    {
        arrowMesh[0].SetActive(true);
        arrowMesh[1].SetActive(true);
        arrowMesh[0].transform.rotation= Quaternion.identity;
        arrowMesh[1].transform.rotation= Quaternion.identity;
    }

    [PunRPC]
    public void Tailpack()
    {
        tail.gameObject.SetActive(true);
        tails[0].gameObject.SetActive(true);
        tails[1].gameObject.SetActive(true);       
    }

    public IEnumerator DelayEffect()
    {
        yield return new WaitForSecondsRealtime(0.04f);
        effects.SetActive(true);                  
    }
}
