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
    PhotonView PV;
    Rigidbody rb;
    public GameObject effect;
    public int bCount;   
    public bool isBeingHeld = false;
    public bool isExplo;
    private AudioSource audioSource;                 // 총알 발사 소리
                                                     // public string bombBeep;
                                                     //public string emp_Explo;
    private XRRayInteractor interactor;

    SelectionOutline outline = null;
    private void Awake()
    {
        PN.AddCallbackTarget(this);
        PV = GetComponent<PhotonView>();
        interactor = FindObjectOfType<XRRayInteractor>();
        isExplo = false;        
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        bCount = 0;
        outline = GetComponent<SelectionOutline>();

    }

    private void OnDestroy()
    {
        PN.RemoveCallbackTarget(this);
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
            Debug.Log("소유권 고정");
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

        if (SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R) &&
            SpawnWeapon_L.leftWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))
        {

            if (!griped_R && !griped_L && isExplo && bCount >= 1)
            {                
                StartCoroutine(Explosion());
                SpawnWeapon_L.leftWeapon.weaponInIt = false;
                SpawnWeapon_R.rightWeapon.weaponInIt = false;
            }
            if (!griped_R && griped_L && isExplo && bCount >= 1)
            {                
                StartCoroutine(Explosion());
                SpawnWeapon_R.rightWeapon.weaponInIt = false;
            }
            if (griped_R && !griped_L && isExplo && bCount >= 1)
            {               
                StartCoroutine(Explosion());
                SpawnWeapon_L.leftWeapon.weaponInIt = false;
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
        PV.RPC(nameof(StartGrabbing), RpcTarget.All);        
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
        PV.RPC(nameof(StopGrabbing), RpcTarget.All);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != PV)return;  // 오브젝트가 생성되었을때 누구나 소유권가져갈수 있고 누군가 소유권을 가져가면 소유권고정
        //Debug.Log("소유권 요청 : " + targetView.name + "from " + requestingPlayer.NickName);
       // PV.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if (targetView != PV) return;
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
