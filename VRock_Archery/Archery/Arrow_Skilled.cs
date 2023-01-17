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

    protected override void Awake()
    {
        base.Awake();
        
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);       
        DataManager.DM.grabArrow = true;
        PV.RequestOwnership();
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (args.interactorObject is Notch notch)
        {
            if (notch.CanRelease)
            {
                LaunchArrow(notch);
                DataManager.DM.arrowNum = 1;
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
    
    [PunRPC]
    public void DelayEX()
    {
        StartCoroutine(nameof(DelayEffect));
    }

    public IEnumerator DelayEffect()
    {
        yield return new WaitForSecondsRealtime(0.04f);
        effects.SetActive(true);
        //AudioManager.AM.PlaySE("aSkillShot");
    }

}
