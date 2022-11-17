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

public class ThrowingGrabbing_D : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks//, IPunObservable
{
    PhotonView PV;
    Rigidbody rb;
    public GameObject effect;
    public int bCount;
    public bool isBeingHeld = false;
    public bool isExplo;
    public string bombBeep;
    public string dm_Explo;
    //private Vector3 remotePos;
    //private Quaternion remoteRot;

    SelectionOutline outline = null;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        isExplo = false;
        gameObject.layer = 6;
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
        //ObjPunLocation_Dm();

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

   /* public void ObjPunLocation_Dm()
    {
        if (!PV.IsMine)
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, 20 * Time.deltaTime)
                , Quaternion.Lerp(transform.rotation, remoteRot, 20 * Time.deltaTime));
        }
    }*/

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
        AudioManager.AM.PlaySX(bombBeep);
        yield return new WaitForSeconds(2.35f);
        var exPlo = Instantiate(effect, transform.position, Quaternion.identity);
        AudioManager.AM.PlaySE(dm_Explo);
        Destroy(exPlo, 0.5f);
        yield return new WaitForSeconds(0.1f);
        PV.RPC(nameof(ExploBomb), RpcTarget.All);
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
        Debug.Log("놓았다");

        PV.RPC(nameof(Put_DM), RpcTarget.AllBuffered);
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

    [PunRPC]
    public void Grab_DM()
    {
        isBeingHeld = true;       
    }

    [PunRPC]
    public void Put_DM()
    {
        isBeingHeld = false;       
    }

 /*   public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            remotePos = (Vector3)stream.ReceiveNext();
            remoteRot = (Quaternion)stream.ReceiveNext();
        }
    }*/
}
