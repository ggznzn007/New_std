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
public class SnowEagle : MonoBehaviourPunCallbacks, IPunObservable
{
    public static SnowEagle SNOWE;
    public GameObject eagleBlock;
    public GameObject eagleDamEX;
    public float speed;
    public float limitTime;
    public string eagleDam;
    public Transform spawnPoint;
    public Transform[] wayPos;
    private int wayNum = 0;
    private GameObject myBlock = null;
    private GameObject myEagle;
    private Animator animator;
    private PhotonView PV;
    private Vector3 remotePos;
    private Quaternion remoteRot;
    private float curTime;


    public void Start()
    {
        SNOWE = this;
        PV = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        transform.position = wayPos[wayNum].transform.position;

        switch (DataManager.DM.currentMap)
        {
            case Map.TUTORIAL_W:
                myEagle = TutorialManager2.TM2.eagleNPC;
                break;
            case Map.WESTERN:
                myEagle = WesternManager.WM.eagleNPC;
                break;
        }

    }

    private void Update()
    {
        if (PN.IsMasterClient)
        {
            if (myBlock == null)
            {
                if (myBlock != null && !PN.IsMasterClient) { return; }
                SpawnEB();
            }
        }

        if (!PV.IsMine)
        {
            //float t = Mathf.Clamp(Time.deltaTime * 10, 0f, 0.99f);
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, Time.deltaTime * 20)
                , Quaternion.Lerp(transform.rotation, remoteRot, Time.deltaTime * 20));
            return;
        }
        MovetoWay();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Snowball") || collision.collider.CompareTag("Stoneball")
            || collision.collider.CompareTag("Icicle"))
        {
            PV.RPC(nameof(EDam), RpcTarget.AllBuffered, true);                                  // 독수리가 대미지입고 블럭투하 RPC 사용            
        }
    }

    /*private void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Iceblock"))
        {
            PV.RPC(nameof(MyBlockNull), RpcTarget.AllBuffered);
        }
    }*/

    public void SpawnEB()
    {
        curTime += Time.deltaTime;
        if (curTime >= limitTime)
        {
            PV.RPC(nameof(BlockInit), RpcTarget.AllBuffered, true, false);                    // 독수리의 자식으로 블럭생성 RPC 사용
        }
      /*  if (myBlock == null)
        {
            if (myBlock != null) return;
           

        }*/
    }

    [PunRPC]
    public void BlockInit(bool onSwitch, bool offSwitch)                                                 // 독수리의 자식으로 블럭생성 RPC 구현
    {
        myBlock = PN.InstantiateRoomObject(eagleBlock.name, spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
        myBlock.transform.SetParent(this.transform, onSwitch);         // 부모 지정
        myBlock.GetComponent<Rigidbody>().useGravity = offSwitch;  // 중력 Off
        myBlock.GetComponent<Collider>().enabled = offSwitch;      // 콜라이더 Off
        curTime = 0;
        Debug.Log("ICE블럭 생성");
    }


    [PunRPC]
    public void EDam(bool onSwitch)
    {
        if (myBlock != null)
        {
            if (myBlock == null) return;
            myBlock.transform.parent = null;                                  // 부모 해제
            myBlock.GetComponent<Rigidbody>().useGravity = onSwitch;// 중력 On
            myBlock.GetComponent<Collider>().enabled = onSwitch;    // 콜라이더 On
            StartCoroutine(EagleDamage());
            StartCoroutine(BlockNull());
        }
    }

    public IEnumerator EagleDamage()
    {
        animator.SetBool("Damaged", true);
        PN.Instantiate(eagleDamEX.name, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        animator.SetBool("Damaged", false);
    }


    /*[PunRPC]
    public void MyBlockNull()
    {
        StartCoroutine(BlockNull());
    }*/
    public IEnumerator BlockNull()
    {
        yield return new WaitForSeconds(0.1f);
        myBlock = null;
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

}
