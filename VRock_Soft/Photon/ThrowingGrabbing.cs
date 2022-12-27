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
    private AudioSource audioSource;                 // �Ѿ� �߻� �Ҹ�
    public string bombBeep;
    public string emp_Explo;
    SelectionOutline outline = null;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        isExplo = false;                    // ó�� ��ź�� �����Ǿ��� �� ������ ����  
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
        Debug.Log("��Ҵ�");
        PV.RPC(nameof(Grab_EMP), RpcTarget.AllBuffered);
        outline.RemoveHighlight();
        if (PV.Owner == PN.LocalPlayer)
        {
            Debug.Log("�̹� �������� ������ �ֽ��ϴ�.");
        }
        else
        {
            TransferOwnership();
        }
    }

    public void OnSelectedExited()
    {
        Debug.Log("���Ҵ�");
        PV.RPC(nameof(Put_EMP), RpcTarget.AllBuffered);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != PV) { return; }
        Debug.Log("������ ��û : " + targetView.name + "from " + requestingPlayer.NickName);
        PV.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("��������� �÷��̾�: " + targetView.name + "from " + previousOwner.NickName);
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
