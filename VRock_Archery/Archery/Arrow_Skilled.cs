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


public class Arrow_Skilled : Arrow
{
    public GameObject effects;
    private bool isRotate;
    public SphereCollider tagColl;
    public ParticleSystem[] parentsMesh;

    protected override void Awake()
    {
        base.Awake();
        isRotate = true;
        tagColl.tag = "Skilled";
        parentsMesh[0].gameObject.SetActive(true);
        parentsMesh[1].gameObject.SetActive(true);
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
                    PV.RPC(nameof(DelayEX), RpcTarget.AllBuffered);                    
                }
                else
                {
                    effects = null;
                }

            }
        }

    }

    private void OnTriggerEnter(Collider coll)
    {
        if (launched)
        {
            if (!launched) return;
            if (coll.CompareTag("Body"))
            {
                AudioManager.AM.PlaySE(hitPlayer);
                var effect = Instantiate(wording_Hit, coll.transform.position + new Vector3(0, 0, -0.25f), coll.transform.rotation);// 충돌 지점에 이펙트 생성
                Destroy(effect, 0.5f);
            }
        }
    }

    private void Update()
    {
        RotArrow();
    }

    public void RotArrow()
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
        StartCoroutine(DelayOnEffect());
        //StartCoroutine(DelayTime());
    }

    public IEnumerator DelayOnEffect()
    {
        //tail.gameObject.SetActive(false);
        head.SetActive(false);
        yield return new WaitForSecondsRealtime(0.04f);
        rigidbody.useGravity = false;
        effects.SetActive(true);
        parentsMesh[0].gameObject.SetActive(false);
        parentsMesh[1].gameObject.SetActive(false);
        yield return StartCoroutine(DelayTime());
    }

    public IEnumerator DelayTime()
    {
        yield return new WaitForSeconds(2.2f);
        Destroy(PV.gameObject);
    }

}
