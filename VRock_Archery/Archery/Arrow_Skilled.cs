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
    public Collider tagColl;
    public Collider gripColl;
    public ParticleSystem[] parentsMesh;
    private GameObject skillEX;
    private bool isRotate;

    protected override void Awake()
    {
        base.Awake();
        isRotate = true;
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
        //tagColl.enabled = false;
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        //tagColl.tag = "Effect";
        DataManager.DM.grabArrow = false;
        if (args.interactorObject is Notch notch)
        {
            //tagColl.tag = "Effect";
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
        if (coll.CompareTag("Body"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    StartCoroutine(DelayHit(coll));
                }
            }

        }

    }

    public IEnumerator DelayHit(Collider coll)
    {
        AudioManager.AM.PlaySE(hitPlayer);
        skillEX = Instantiate(wording_Hit, coll.transform.position + new Vector3(0, 1, 0), coll.transform.rotation);// 충돌 지점에 이펙트 생성
        Destroy(skillEX, 0.5f);
        yield return new WaitForSeconds(0.02f);
        tagColl.enabled = false;
    }

    public IEnumerator CollOnOff()
    {
        tagColl.enabled = true;
        //yield return new WaitForSeconds(0.005f);
        yield return new WaitForSeconds(0.03f);
        tagColl.enabled = false;
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
        gripColl.gameObject.SetActive(false);
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
