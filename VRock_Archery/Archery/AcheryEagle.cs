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
    public static AcheryEagle AE;                                                             // 싱글턴
    public GameObject eagleBomb;                                                              // 독수리 밑에 생성되는 폭탄 프리팹
    public GameObject eagleDamEX;                                                             // 독수리 대미지 이펙트
    public float speed;                                                                       // 독수리 속도
    public float limitTime;                                                                   // 폭탄 생성 제한시간
    public string eagleDam;                                                                   // 독수리 대미지 오디오 생성 문자열
    public Transform spawnPoint;                                                              // 폭탄 생성 포인트
    public Transform[] wayPos;                                                                // 독수리 이동 경로
    private int wayNum = 0;                                                                   // 독수리 이동 경로 번호
    private GameObject myBomb = null;                                                         // 현재 폭탄
    private GameObject myEagle;                                                               // 현재 독수리
    private Animator animator;                                                                // 독수리 애니메이터
    private PhotonView PV;                                                                    // 포톤뷰
    private Vector3 remotePos;                                                                // 리모트 위치
    private Quaternion remoteRot;                                                             // 리모트 회전
    private float curTime;                                                                    // 현재 시간(생성을 위해 필요)
    private bool eagleisDam;

    public void Start()
    {
        AE = this;
        eagleisDam = false;
        PV = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        transform.position = wayPos[wayNum].transform.position;

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
            if (myBomb == null)
            {
                if (myBomb != null) { return; }
                SpawnEB();
            }
        }

        if (!PV.IsMine)
        {
            //float t = Mathf.Clamp(Time.deltaTime * 10, 0f, 0.99f);
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, Time.deltaTime * 30)
                , Quaternion.Lerp(transform.rotation, remoteRot, Time.deltaTime * 30));
            return;
        }
        //myEagle = TutorialManager.TM.eagleNPC;
        //myEagle = GameObject.Find("Achery_Eagle");
        // BombIn();
        if (!eagleisDam)
        {
            if (eagleisDam) { return; }
            MovetoWay();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Arrow"))                                    // 기본화살태그에만 반응
        {
            PV.RPC(nameof(EDam), RpcTarget.AllBuffered, true);                         // 독수리가 대미지입고 폭탄투하     
        }
    }

    /*private void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Effect"))
        {
            PV.RPC(nameof(BomNull), RpcTarget.AllBuffered);
        }
    }*/

    public void SpawnEB()
    {
        curTime += Time.deltaTime;
        if (curTime >= limitTime)
        {
            PV.RPC(nameof(BombInit), RpcTarget.AllBuffered, true, false);                   // 독수리의 자식으로 폭탄생성
        }
        /*if (myBomb == null)
        {
            if (myBomb != null) return;
           

        }*/
    }

    [PunRPC]
    public void BombInit(bool onSwitch, bool offSwitch)
    {
        myBomb = PN.InstantiateRoomObject(eagleBomb.name, myEagle.transform.position + new Vector3(0, -0.1f, 0), myEagle.transform.rotation, 0);
        myBomb.transform.SetParent(spawnPoint.transform, onSwitch);
        myBomb.GetComponentInChildren<Rigidbody>().useGravity = offSwitch;
        myBomb.GetComponentInChildren<Collider>().enabled = offSwitch;
        curTime = 0;
        Debug.Log("NPC폭탄 생성");
    }


    [PunRPC]
    public void EDam(bool onSwitch)
    {
        if (myBomb != null)
        {
            if (myBomb == null) { return; }
            myBomb.transform.parent = null;
            myBomb.GetComponentInChildren<Rigidbody>().useGravity = onSwitch;
            myBomb.GetComponentInChildren<Collider>().enabled = onSwitch;
            myBomb = null;
            StartCoroutine(EagleDamage());
        }
    }

    /* [PunRPC]
     public void BomNull()
     {
         StartCoroutine(BombTime());
     }
 */
    public IEnumerator EagleDamage()
    {
        animator.SetBool("Damaged", true);
        eagleisDam = true;
        PN.Instantiate(eagleDamEX.name, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        animator.SetBool("Damaged", false);
        yield return new WaitForSeconds(0.5f);
        eagleisDam = false;
        //StartCoroutine(BombTime());
    }

    /*public IEnumerator BombTime()
    {
        yield return new WaitForSeconds(0.001f);
        myBomb = null;
        Debug.Log("폭탄이 비었습니다.");
    }*/

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
