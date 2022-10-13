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
    public GameObject effect;
    Rigidbody rb;
    public bool isBeingHeld = false;
    int bCount;
    private void Awake()
    {
       
        PV = GetComponent<PhotonView>();
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
            rb.isKinematic = true;
            gameObject.layer = 7;
            bCount++;
        }
        else
        {
            rb.isKinematic = false;
            gameObject.layer = 6;
            //StartCoroutine(ThrowBomb());
            
        }
        
    }

   public void ThrowBomb()                                            // ���� ������ �� �ڵ� �ı�
    {
        if (SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R) &&
            SpawnWeapon_L.leftWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))// && !SpawnWeapon_R.rightWeapon.weaponInIt)
        {
            if (!griped_R && !griped_L)
            {
                if (PV.IsMine&&bCount>=1)
                {
                    StartCoroutine(Explosion());                   
                    SpawnWeapon_L.leftWeapon.weaponInIt = false;
                    SpawnWeapon_R.rightWeapon.weaponInIt = false;
                }

            }
        }
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(2);
        var exPlo = Instantiate(effect, transform.position, transform.rotation);
        Destroy(exPlo, 1f);
       /* Collider[] collider = Physics.OverlapSphere(transform.position, 4f);
        //RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 15, Vector3.up, 0,LayerMask.GetMask("Player"));
        foreach (Collider hit in collider)
        {
            AvartarController controller = hit.GetComponent<AvartarController>();
            if(controller != null)
            {
                hit.GetComponent<AvartarController>().CriticalDamage();
            }            
        }*/
        PV.RPC("ExploBomb", RpcTarget.All);
    }


    private void TransferOwnership()
    {
        PV.RequestOwnership();
    }
    public void OnSelectedEntered()
    {
        Debug.Log("��Ҵ�");
        PV.RPC("StartNetworkGrabbing", RpcTarget.AllBuffered);
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
        PV.RPC("StopNetworkGrabbing", RpcTarget.AllBuffered);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if(targetView != PV)
        {
            return;
        }

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

    [PunRPC]
    public void ExploBomb()
    {        
        Destroy(gameObject);
    }
}
