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

public class ThrowingGrabbing_D : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    PhotonView PV;
    Rigidbody rb;
    public GameObject effect;
    public int bCount;
    public bool isBeingHeld = false;
    public bool isExplo;
    public string bombBeep;
    public string dm_Explo;

    SelectionOutline outline = null;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        isExplo = false;
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
        
            if (SpawnWeapon_RW.RW.DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R) &&
               SpawnWeapon_LW.LW.DeviceL.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))
            {

                if (!griped_R && !griped_L && isExplo && bCount >= 1) // 양손 모두 놓았을 때
                {
                    StartCoroutine(Explosion());
                    SpawnWeapon_LW.LW.weaponInIt = false;
                    SpawnWeapon_RW.RW.weaponInIt = false;
                }
                if (!griped_R && griped_L && isExplo && bCount >= 1)              // 오른손만 놨을때
                {
                    StartCoroutine(Explosion());
                    SpawnWeapon_RW.RW.weaponInIt = false;
                }
                if (griped_R && !griped_L && isExplo && bCount >= 1)             // 왼손만 놨을때
                {
                    StartCoroutine(Explosion());
                    SpawnWeapon_LW.LW.weaponInIt = false;
                }
            }
    }

    public IEnumerator Explosion()
    {
        AudioManager.AM.PlaySX("BombBeep");        
        yield return new WaitForSeconds(2.35f);
        var exPlo = PN.Instantiate(effect.name, transform.position, Quaternion.identity);       
        Destroy(exPlo, 0.5f);
        yield return new WaitForSeconds(0.1f);
        PV.RPC("ExploBomb", RpcTarget.All);
    }


    [PunRPC]
    public void ExploBomb()
    {
        Destroy(gameObject);
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

    public void OnSelectedEntered()
    {
        Debug.Log("잡았다");
        PV.RPC("StartGrabbing", RpcTarget.AllBuffered);
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
        
        PV.RPC("StopGrabbing", RpcTarget.AllBuffered);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != PV)
        {
            return;
        }

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

}
