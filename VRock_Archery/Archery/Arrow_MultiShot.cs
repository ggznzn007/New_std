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
    // public static Arrow_MultiShot ArrowMul;
    public SphereCollider tagColl;
    public ParticleSystem typing;
    public ParticleSystem[] effects;
    public GameObject[] arrowMesh;
    public TrailRenderer[] tails;
    private bool isRotate;

    protected override void Awake()
    {
        base.Awake();
        isRotate = true;
        typing.gameObject.SetActive(true);
        tagColl.tag = "Skilled";
    }
    /* private void Start()
     {
         //ArrowMul = this;

         isRotate = true;

     }*/
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        arrowM.OnSelectedEntered();
        //PV.RequestOwnership();
        DataManager.DM.grabArrow = true;
        DataManager.DM.arrowNum = 2;
        isRotate = false;
        //rotSpeed = 0;

        if (PV.IsMine)
        {
            //if (!PV.IsMine) return;
            if (DataManager.DM.grabArrow)
            {
                PV.RPC(nameof(HideType), RpcTarget.AllBuffered);
                PV.RPC(nameof(Multipack), RpcTarget.AllBuffered);
            }
        }
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        tagColl.tag = "Arrow";
        if (args.interactorObject is Notch notch)
        {
            //GetTarget();
            tagColl.tag = "Arrow";
            if (notch.CanRelease)
            {
                LaunchArrow(notch);
                arrowM.OnSelectedExited();
                if (PV.IsMine)
                {
                    // if (!PV.IsMine) return;
                    PV.RPC(nameof(Tailpack), RpcTarget.AllBuffered);
                    PV.RPC(nameof(DelayEX), RpcTarget.AllBuffered);
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
    public void DelayEX()
    {
        StartCoroutine(DelayEffect());
    }

    [PunRPC]
    public void Multipack()
    {
        arrowMesh[0].SetActive(true);
        arrowMesh[1].SetActive(true);
    }

    [PunRPC]
    public void Tailpack()
    {
        StartCoroutine(TailCtrl());
    }

    [PunRPC]
    public void HideType()
    {
        typing.gameObject.SetActive(false);
    }

    public IEnumerator DelayEffect()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        effects[0].gameObject.SetActive(true);
        effects[1].gameObject.SetActive(true);
        effects[2].gameObject.SetActive(true);
    }

    public IEnumerator TailCtrl()
    {
        tails[0].gameObject.SetActive(true);
        tails[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        tails[0].gameObject.SetActive(false);
        tails[1].gameObject.SetActive(false);
    }
}
