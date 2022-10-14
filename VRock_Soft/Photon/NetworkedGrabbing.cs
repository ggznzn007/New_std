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
public class NetworkedGrabbing : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    PhotonView PV;
    Rigidbody rb;
    public GameObject effect;
    public int bCount;
    public bool isBeingHeld = false;
    public bool isExplo;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        isExplo = false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        bCount = 0;
    }
    private void Update()
    {
        ThrowBomb();
        if (isBeingHeld)
        {
            bCount++;
            isExplo = false;
            //rb.isKinematic = true;
            gameObject.layer = 7;
        }
        else
        {
            isExplo = true;
            //rb.isKinematic = false;
            gameObject.layer = 6;
            //StartCoroutine(ThrowBomb());          
        }

    }
    public void ThrowBomb()
    {
        if (SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R) &&
            SpawnWeapon_L.leftWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))// && !SpawnWeapon_R.rightWeapon.weaponInIt)
        {
            if (!griped_R && !griped_L)
            {
                if (PV.IsMine && isExplo && bCount >=1 )
                {
                    StartCoroutine(Explosion());
                    SpawnWeapon_L.leftWeapon.weaponInIt = false;
                    SpawnWeapon_R.rightWeapon.weaponInIt = false;
                }

            }
        }
    }

    public IEnumerator Explosion()
    {
        yield return new WaitForSeconds(2.5f);
        var exPlo = PN.Instantiate(effect.name, transform.position, transform.rotation);
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
    public void OnSelectedEntered()
    {
        Debug.Log("잡았다");
        PV.RPC("StartNetworkGrabbing", RpcTarget.AllBuffered);
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
        PV.RPC("StopNetworkGrabbing", RpcTarget.AllBuffered);
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
    public void StartNetworkGrabbing()
    {
        isBeingHeld = true;
    }

    [PunRPC]
    public void StopNetworkGrabbing()
    {
        isBeingHeld = false;
    }


}
