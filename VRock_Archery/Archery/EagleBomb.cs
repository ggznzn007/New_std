using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class EagleBomb : MonoBehaviourPunCallbacks, IPunObservable//, IPunOwnershipCallbacks
{
    public PhotonView PV;
    public Rigidbody rb;
    public SphereCollider bombColl;
    public Collider exColl;
    public GameObject myEX;
    public GameObject myMesh;
    public Transform exploPoint;
    public string fireCircle;
    private Vector3 remotePos;
    private Quaternion remoteRot;
    

    void Awake()
    {
        PV = GetComponent<PhotonView>();        
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        bombColl = GetComponent<SphereCollider>();
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine)
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, 30 * Time.deltaTime)
                , Quaternion.Lerp(transform.rotation, remoteRot, 30 * Time.deltaTime));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("FloorBox"))
        {
            PV.RPC(nameof(FireCircle), RpcTarget.AllBuffered);           
        }

        if(collision.collider.CompareTag("Cube"))
        {
            PV.RPC(nameof(FireDelete), RpcTarget.AllBuffered);
        }
    }

    public IEnumerator Explode()
    {
        Destroy(gameObject);
        PN.InstantiateRoomObject(myEX.name, exploPoint.position, exploPoint.rotation);
        yield return new WaitForSeconds(0.1f);
       // AcheryEagle.AE.myBomb = null;
    }

/*    public IEnumerator CollOnOff()
    {
        while (true)
        {
            exColl.enabled = true;
            yield return new WaitForSeconds(0.03f);
            exColl.enabled = false;
            yield return new WaitForSeconds(0.4f);
        }
    }*/

    [PunRPC]
    public void FireCircle()
    {        
        //Destroy(gameObject);
        StartCoroutine(Explode());                
        //PN.Instantiate(myEX.name, transform.position+new Vector3(0,-0.3f,0), myEX.transform.rotation);
    }

    [PunRPC]
    public void FireDelete()
    {
        Destroy(gameObject);
    }
   
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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
    }

    /* public void OnSelectedEntered()
    {          
        
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
*/
}
