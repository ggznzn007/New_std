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
    public GameObject[] effects;
    public GameObject[] arrowMesh;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        arrowM.OnSelectedEntered();
        DataManager.DM.grabArrow = true;
        PV.RequestOwnership();
        rotSpeed = 0;
        arrowMesh[0].SetActive(true);
        arrowMesh[1].SetActive(true);
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        DataManager.DM.arrowNum = 0;
        if (args.interactorObject is Notch notch)
        {
            //GetTarget();
            if (notch.CanRelease)
            {
                LaunchArrow(notch);
                DataManager.DM.arrowNum = 2;
                arrowM.OnSelectedExited();
                tail.gameObject.SetActive(true);
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

    public IEnumerator DelayEffect()
    {
        yield return new WaitForSecondsRealtime(0.04f);
        effects[0].SetActive(true);       
        effects[1].SetActive(true);
        effects[2].SetActive(true);       
        effects[3].SetActive(true);       
    }
}
