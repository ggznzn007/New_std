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
public class IceBlock : XRGrabInteractable
{
    public static IceBlock IB;
    public PhotonView PV;
    public GameObject DestroyEX;
    Rigidbody rb;
    public bool isBeingHeld = false;
    void Start()
    {
        IB = this;
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (isBeingHeld)
        {
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        PV.RequestOwnership();
        Debug.Log("아이스블럭 잡았다");
        PV.RPC(nameof(Grab_), RpcTarget.AllBuffered);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        Debug.Log("아이스블럭 놓았다");
        PV.RPC(nameof(Put_), RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Grab_()
    {
        isBeingHeld = true;
    }

    [PunRPC]
    public void Put_()
    {
        isBeingHeld = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Icicle"))
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
        PN.InstantiateRoomObject(DestroyEX.name, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.01f);
        Destroy(gameObject);
    }
}
