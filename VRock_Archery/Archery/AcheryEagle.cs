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
using Unity.VisualScripting;

public class AcheryEagle : MonoBehaviourPunCallbacks, IPunObservable//, IPunOwnershipCallbacks
{
    public GameObject eagleBomb;
    public GameObject eagleDamEX;
    public Transform spawnPoint;
    public Transform[] wayPos;
    public float speed;
    public string eagleDam;
    int wayNum = 1;
    private GameObject myBomb;
    private Animator animator;
    private PhotonView PV;
    private Vector3 remotePos;
    private Quaternion remoteRot;

    public void Start()
    {
        PV = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        wayPos = GameObject.Find("WayPoint").GetComponentsInChildren<Transform>();
        /*switch(DataManager.DM.currentMap)
        {
            case Map.TUTORIAL_T:
                wayPos = TutorialManager.TM.wayPos;
                break;
            case Map.TOY:
                wayPos = GunShootManager.GSM.wayPos;
                break;
        }*/
        //transform.position = wayPos[wayNum].transform.position;   
        if (PN.IsMasterClient)
        {
            InvokeRepeating(nameof(SpawnEB), 2, 5);
        }
    }

   
    private void Update()
    {
        if (!PV.IsMine)
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, 30 * Time.deltaTime)
                , Quaternion.Lerp(transform.rotation, remoteRot, 30 * Time.deltaTime));
            return;
        }

        /*switch (DataManager.DM.currentMap)
        {
            case Map.TUTORIAL_T:
                myBomb = TutorialManager.TM.myBomb;
                break;
            case Map.TOY:
                myBomb = GunShootManager.GSM.myBomb;
                break;
        }*/

        // myBomb = GameObject.Find("Barrel_Bomb");

        MovetoWay();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Arrow"))
        {
            PV.RPC(nameof(BombOut), RpcTarget.AllBuffered);
            PV.RPC(nameof(EDamage), RpcTarget.AllBuffered);
            //AudioManager.AM.PlaySE(eagleDam);
            /*if (PV.IsMine)
            {
                if (myBomb == null) return;
                if (myBomb != null)
                {
                   
                }
            }*/
        }
    }

    public void SpawnEB()
    {
        if (myBomb == null)
        {
            if (myBomb != null) return;
            PV.RPC(nameof(BombInit), RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void BombInit()
    {
        myBomb = PN.Instantiate(eagleBomb.name, transform.position + new Vector3(0, 0.9f, 0), transform.rotation, 0);
        myBomb.transform.SetParent(transform, true);
        myBomb.GetComponent<Rigidbody>().useGravity = false;
        myBomb.GetComponent<SphereCollider>().enabled = false;
    }

    [PunRPC]
    public void BombOut()
    {
        myBomb.transform.SetParent(transform, false);
        myBomb.GetComponent<Rigidbody>().useGravity = true;
        myBomb.GetComponent<SphereCollider>().enabled = true;
    }

    [PunRPC]
    public void EDamage()
    {        
        StartCoroutine(EagleDamage());
    }

    public IEnumerator EagleDamage()
    {
        animator.SetBool("Damaged", true);
        
        PN.Instantiate(eagleDamEX.name, transform.position + new Vector3(0, 1.2f, 0), transform.rotation, 0);
        yield return new WaitForSeconds(1f);
        animator.SetBool("Damaged", false);
    }

    public void MovetoWay()
    {
        transform.position = Vector3.MoveTowards(transform.position, wayPos[wayNum].position, speed * Time.deltaTime);
        transform.LookAt(wayPos[wayNum].position);

        if (transform.position == wayPos[wayNum].transform.position)
        {
            wayNum++;
        }

        if (wayNum == wayPos.Length) { wayNum = 1; }
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

    /*  public void OnSelectedEntered()
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
    }*/

}
