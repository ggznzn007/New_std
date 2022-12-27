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

public class ThrowingGrabbing : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    public PhotonView PV;
    Rigidbody rb;
    public GameObject effect;
    public int bCount;
    public bool isBeingHeld = false;
    public bool isExplo;
    private AudioSource audioSource;                 // 총알 발사 소리
    public string bombBeep;
    public string emp_Explo;
    SelectionOutline outline = null;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        isExplo = false;                    // 처음 폭탄에 생성되었을 때 폭발을 막음  
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        bCount = 0;
        outline = GetComponent<SelectionOutline>();
    }

    private void Update()
    {
        if (DataManager.DM.currentTeam != Team.ADMIN)
        {
            ThrowBomb();
        }
        if (isBeingHeld)
        {
            bCount++;
            isExplo = false;
            rb.isKinematic = true;
            gameObject.layer = 7;
        }

        else
        {
            isExplo = true;
            rb.isKinematic = false;
            gameObject.layer = 6;
        }
    }

    public void ThrowBomb()
    {
        if (!isBeingHeld && isExplo && bCount >= 1)
        {
            StartCoroutine(ExploEMP());
        }        
    }

    public IEnumerator ExploEMP()
    {
        //yield return new WaitForSeconds(2.35f);
        yield return StartCoroutine(Beep());
        yield return StartCoroutine(EmpEX());
        //Destroy(PV.gameObject);
        PV.RPC(nameof(DestroyEMP), RpcTarget.AllBuffered);
    }

    public IEnumerator Beep()
    {
        AudioManager.AM.PlaySX(bombBeep);
        yield return new WaitForSeconds(2.35f);
    }

    public IEnumerator EmpEX()
    {
        AudioManager.AM.PlaySE(emp_Explo);
        PN.Instantiate(effect.name, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.001f);
    }

    /*[PunRPC]
    public void ExploBomb()
    {
        StartCoroutine(Explosion());        
    }*/
    [PunRPC]
    public void DestroyEMP()
    {
        Destroy(PV.gameObject);
    }

    [PunRPC]
    public void Grab_EMP()
    {
        isBeingHeld = true;
    }

    [PunRPC]
    public void Put_EMP()
    {
        isBeingHeld = false;
    }
    public void OnSelectedEntered()
    {
        Debug.Log("잡았다");
        PV.RPC(nameof(Grab_EMP), RpcTarget.AllBuffered);
        outline.RemoveHighlight();
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
        Debug.Log("놓았다");
        PV.RPC(nameof(Put_EMP), RpcTarget.AllBuffered);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != PV) { return; }
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
