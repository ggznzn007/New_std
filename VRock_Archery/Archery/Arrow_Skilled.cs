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

public class Arrow_Skilled : Arrow
{
    public GameObject effects;
    private bool isRotate;
    public SphereCollider tagColl;

    protected override void Awake()
    {
        base.Awake();
        isRotate = true;
        tagColl.tag = "Skilled";
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        DataManager.DM.grabArrow = true;       
        PV.RequestOwnership();
        //rotSpeed = 0;
        isRotate = false;
        DataManager.DM.inArrowBox = false;
        tagColl.enabled = false;
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        tagColl.tag = "Effect";
        DataManager.DM.grabArrow = false;
        if (args.interactorObject is Notch notch)
        {
            tagColl.tag = "Effect";
            if (notch.CanRelease)
            {
                DataManager.DM.arrowNum = 1;
                LaunchArrow(notch);
                if (effects != null)
                {
                    if (!PV.IsMine) return;
                    rigidbody.useGravity = false;
                    PV.RPC(nameof(DelayEX), RpcTarget.AllBuffered);
                    tail.gameObject.SetActive(false);
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
        if (isRotate)
        {
            transform.Rotate(rotSpeed * Time.deltaTime * new Vector3(0, 0, 1));
        }
        else
        {
            rotSpeed= 0;
        }
    }

    [PunRPC]
    public void DelayEX()
    {
        StartCoroutine(nameof(DelayOnEffect));
    }

    public IEnumerator DelayOnEffect()
    {
        yield return new WaitForSecondsRealtime(0.04f);
        effects.SetActive(true);
        //AudioManager.AM.PlaySE("aSkillShot");
    }

}
