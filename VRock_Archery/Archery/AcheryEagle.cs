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
    public static AcheryEagle AE;
    public GameObject eagleBomb;
    public GameObject eagleDamEX;
    public Transform spawnPoint;
    public Transform[] wayPos;
    public float speed;
    public string eagleDam;
    int wayNum = 0;
    public GameObject myBomb = null;
    private GameObject myEagle;
    private Animator animator;
    private PhotonView PV;
    private Vector3 remotePos;
    private Quaternion remoteRot;
    private float curTime;

    public void Start()
    {
        AE = this;
        PV = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        transform.position = wayPos[wayNum].transform.position;
        /*if (PN.IsMasterClient)
        {
            //InvokeRepeating(nameof(SpawnEB), 1, 2);
        }*/
        switch (DataManager.DM.currentMap)
        {
            case Map.TUTORIAL_T:
                myEagle = TutorialManager.TM.eagleNPC;
                break;
            case Map.TOY:
                myEagle = GunShootManager.GSM.eagleNPC;
                break;
        }
        /* switch (DataManager.DM.currentMap)
         {
             case Map.TUTORIAL_T:
                 myBomb = TutorialManager.TM.myBomb;
                 break;
             case Map.TOY:
                 myBomb = GunShootManager.GSM.myBomb;
                 break;
         }*/
        /* wayPos = GameObject.Find("WayPoint").GetComponentsInChildren<Transform>();
         switch (DataManager.DM.currentMap)
         {
             case Map.TUTORIAL_T:
                 wayPos = TutorialManager.TM.wayPos;
                 break;
             case Map.TOY:
                 wayPos = GunShootManager.GSM.wayPos;
                 break;
         }*/
    }


    private void Update()
    {
        if (PN.IsMasterClient)
        {
            SpawnEB();
        }
        if (!PV.IsMine)
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, 30 * Time.deltaTime)
                , Quaternion.Lerp(transform.rotation, remoteRot, 30 * Time.deltaTime));
            return;
        }
        //myEagle = TutorialManager.TM.eagleNPC;
        //myEagle = GameObject.Find("Achery_Eagle");
        // BombIn();
        MovetoWay();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Arrow"))
        {
            PV.RPC(nameof(EDam), RpcTarget.AllBuffered, true);                         // 독수리가 대미지입고 폭탄투하            
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Effect"))
        {
            PV.RPC(nameof(BomNull), RpcTarget.AllBuffered);
        }
    }

    public void SpawnEB()
    {
        if (myBomb == null)
        {
            if (myBomb != null) return;
            curTime += Time.deltaTime;
            if (curTime >= 3)
            {
                PV.RPC(nameof(BombInit), RpcTarget.AllBuffered, true, false);                   // 독수리의 자식으로 폭탄생성
            }

        }
    }

    [PunRPC]
    public void BombInit(bool parent, bool child)
    {
        myBomb = PN.InstantiateRoomObject(eagleBomb.name, myEagle.transform.position + new Vector3(0, 0.9f, 0), myEagle.transform.rotation, 0);
        myBomb.transform.SetParent(myEagle.transform, parent);
        myBomb.GetComponentInChildren<Rigidbody>().useGravity = child;
        myBomb.GetComponentInChildren<Collider>().enabled = child;
        curTime = 0;
        Debug.Log("NPC폭탄 생성");
    }


    [PunRPC]
    public void EDam(bool child)
    {
        if (myBomb != null)
        {
            myBomb.transform.parent = null;
            myBomb.GetComponentInChildren<Rigidbody>().useGravity = child;
            myBomb.GetComponentInChildren<Collider>().enabled = child;
            StartCoroutine(EagleDamage());
        }
        else
        {
            return;
        }
    }

    [PunRPC]
    public void BomNull()
    {
        StartCoroutine(BombTime());
    }

    public IEnumerator EagleDamage()
    {
        animator.SetBool("Damaged", true);
        PN.Instantiate(eagleDamEX.name, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        animator.SetBool("Damaged", false);
        //StartCoroutine(BombTime());
    }

    public IEnumerator BombTime()
    {
        yield return new WaitForSeconds(0.001f);
        myBomb = null;
        Debug.Log("폭탄이 비었습니다.");
    }

    public void MovetoWay()
    {
        transform.position = Vector3.MoveTowards(transform.position, wayPos[wayNum].position, speed * Time.deltaTime);
        transform.LookAt(wayPos[wayNum].position);

        if (transform.position == wayPos[wayNum].transform.position)
        {
            wayNum++;
        }

        if (wayNum == wayPos.Length) { wayNum = 0; }
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
