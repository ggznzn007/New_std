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
using Random = UnityEngine.Random;
using Unity.XR.PXR;
using Unity.VisualScripting;

public class ThrowingGrabbing_D : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    PhotonView PV;
    Rigidbody rb;
    public GameObject effect;
    public AudioSource dynaSound;
    public bool isBeingHeld = false;        
    public string bombBeep;
    public string dm_Explo;
    SelectionOutline outline = null;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();      
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();       
        outline = GetComponent<SelectionOutline>();        
    }
    private void FixedUpdate()
    {      
        if (isBeingHeld)
        {            
            rb.isKinematic = true;
            gameObject.layer = 7;
        }
        else
        {            
            rb.isKinematic = false;
            gameObject.layer = 6;
        }
    }

    public IEnumerator Explode_D()
    {        
        dynaSound.Stop();
        PV.RPC(nameof(BeepSound), RpcTarget.AllBuffered);
        yield return new WaitForSecondsRealtime(2.35f);
        PV.RPC(nameof(ExploSound), RpcTarget.AllBuffered);
        PN.Instantiate(effect.name, transform.position, Quaternion.identity);
        PV.RPC(nameof(DestroyEMP), RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void BeepSound()
    {        
        AudioManager.AM.PlaySB(bombBeep);
    }

    [PunRPC]
    public void ExploSound()
    {
        AudioManager.AM.PlaySX(dm_Explo);
    }
    [PunRPC]
    public void DestroyEMP()
    {
        Destroy(PV.gameObject);
    }

    [PunRPC]
    public void Grab_DM()
    {
        isBeingHeld = true;
        dynaSound.Play();
    }

    [PunRPC]
    public void Put_DM()
    {
        isBeingHeld = false;        
    }

    public void OnSelectedEntered()
    {
        Debug.Log("잡았다");
        PV.RPC(nameof(Grab_DM), RpcTarget.AllBuffered);        
        if (PV.Owner == PN.LocalPlayer)
        {
            Debug.Log("이미 소유권이 나에게 있습니다.");
        }
        else
        {
            TransferOwnership();
        }
    }

    public void OnSelectedExited()
    {        
        StartCoroutine(Explode_D());
        //Invoke(nameof(Explode_D), 2.35f);
        PV.RPC(nameof(Put_DM), RpcTarget.AllBuffered);
        Debug.Log("놓았다");
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != PV) return;
        Debug.Log("소유권 요청 : " + targetView.name + "from " + requestingPlayer.NickName);
        PV.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {

        Debug.Log("현재소유한 플레이어: " + targetView.name + "from " + previousOwner.NickName);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {

    }

    private void TransferOwnership()
    {
        PV.RequestOwnership();
    }

    public void OnHoverEntered()
    {
        outline.Highlight();
    }

    public void OnHoverExited()
    {
        outline.RemoveHighlight();
    }
}
