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
using UnityEditor;

public class SnowBlock : XRGrabInteractable
{
    public static SnowBlock SB;
    public GameObject myMesh;
    public GameObject snowEX;    
    public PhotonView PV;
    Rigidbody rb;
    public bool isBeingHeld = false;

    void Start()
    {
        SB=this;
        PV = GetComponent<PhotonView>();
        myMesh.SetActive(true);
        snowEX.SetActive(false);
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!DataManager.DM.inBuild)
        {
            interactionLayers = 0;
        }       
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);        
        PV.RequestOwnership();
        //Debug.Log("잡았다");
        PV.RPC(nameof(StartGrabbing), RpcTarget.AllBuffered);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);        
        //Debug.Log("놓았다");
        PV.RPC(nameof(StopGrabbing), RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void StartGrabbing()
    {
        isBeingHeld = true;
    }

    [PunRPC]
    public void StopGrabbing()
    {
        isBeingHeld = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Stoneball"))
        {
            PV.RPC(nameof(Disappear), RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void Disappear()
    {        
        StartCoroutine(EXOnOff());
    }

    public IEnumerator EXOnOff()
    {
        myMesh.SetActive(false);
        snowEX.SetActive(true);
        //PN.Instantiate(DestroyEX.name, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    } 
}
