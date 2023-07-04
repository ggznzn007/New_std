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
    private bool eagleisDam;

    public void Start()
    {
        SNOWE = this;
        eagleisDam = false;
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
        if (PN.IsMasterClient&&!DataManager.DM.gameOver)
        {
            if(DataManager.DM.gameOver) { return; }
            if (myBlock == null)
            {
                if (myBlock != null) { return; }
                SpawnEB();
            }
        }

        if (!PV.IsMine)
        {
            // float t = Mathf.Clamp(Time.deltaTime * 10, 0f, 0.99f);
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, Time.deltaTime * 10)
                , Quaternion.Lerp(transform.rotation, remoteRot, Time.deltaTime * 10));
            return;
        }
        if (!eagleisDam)
        {
            if (eagleisDam) { return; }
            MovetoWay();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Snowball"))
        {
            if(!DataManager.DM.gameOver)
            {
                if (DataManager.DM.gameOver) { return; }
                PV.RPC(nameof(SEDam), RpcTarget.AllBuffered);                                  // �������� ������԰� ������ RPC ���   
            }
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
            PV.RPC(nameof(BlockInit), RpcTarget.AllBuffered);                    // �������� �ڽ����� ������ RPC ���
        }
        /*  if (myBlock == null)
          {
              if (myBlock != null) return;


          }*/
    }

    [PunRPC]
    public void BlockInit()                                                 // �������� �ڽ����� ������ RPC ����
    {
        myBlock = PN.InstantiateRoomObject(eagleBlock.name, myEagle.transform.position, myEagle.transform.rotation, 0);
        myBlock.transform.SetParent(spawnPoint.transform, true);         // �θ� ����
        myBlock.GetComponentInChildren<Rigidbody>().useGravity = false;  // �߷� Off
        myBlock.GetComponentInChildren<Collider>().enabled = false;      // �ݶ��̴� Off
        curTime = 0;
        //Debug.Log("ICE�� ����");
    }

    [PunRPC]
    public void SEDam()
    {
        if (myBlock != null)
        {
            if (myBlock == null) { return; }
            myBlock.transform.parent = null;                                  // �θ� ����
            myBlock.GetComponentInChildren<Rigidbody>().useGravity = true;// �߷� On
            myBlock.GetComponentInChildren<Collider>().enabled = true;    // �ݶ��̴� On
            myBlock = null;
            StartCoroutine(EagleDamage());
        }
    }

    public IEnumerator EagleDamage()
    {
        animator.SetBool("Damaged", true);
        eagleisDam = true;
        PN.Instantiate(eagleDamEX.name, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        animator.SetBool("Damaged", false);
        yield return new WaitForSeconds(0.5f);
        eagleisDam = false;
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
