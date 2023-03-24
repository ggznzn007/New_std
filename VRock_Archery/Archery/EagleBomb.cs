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

public class EagleBomb : MonoBehaviourPunCallbacks, IPunObservable// 아처 독수리 아래 생성되는 폭탄을 관리하는 스크립트
{
    public PhotonView PV;            // 포톤뷰
    public Rigidbody rb;             // 리지드바디
    public SphereCollider bombColl;  // 폭탄 콜라이더 = 바닥에 떨어졌을 때 충돌 감지    
    public GameObject myEX;          // 폭탄 폭발시 생성되는 파티클 프리팹
    public GameObject myMesh;        // 폭탄 메쉬
    public Transform exploPoint;     // 폭탄 폭발 위치
    public string fireCircle;        // 폭탄 폭발 시 재생되는 오디오 문자열
    private Vector3 remotePos;       // 포톤을 통해 공유되는 리모트 위치
    private Quaternion remoteRot;    // 포톤을 통해 공유되는 리모트 회전    

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
        if (!PV.IsMine)  // 자신(폭탄) 이외의 다른 플레이어에게 자신의 위치 및 회전값을 부드럽게 전달
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, 30 * Time.deltaTime)
                , Quaternion.Lerp(transform.rotation, remoteRot, 30 * Time.deltaTime));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("FloorBox"))  // 폭탄이 바닥에 떨어져서 태그되면 폭발하는 RPC 호출
        {
            PV.RPC(nameof(FireCircle), RpcTarget.AllBuffered);           
        }

        if(collision.collider.CompareTag("Cube"))       // 폭탄이 바닥 이외에 떨어져서 태그되면 사라지는 RPC 호출
        {
            PV.RPC(nameof(FireDelete), RpcTarget.AllBuffered);
        }
    }

    public IEnumerator Explode()
    {
        Destroy(gameObject);
        PN.InstantiateRoomObject(myEX.name, exploPoint.position, exploPoint.rotation);
        yield return new WaitForSeconds(0.1f);       
    }

    [PunRPC]
    public void FireCircle()
    {                
        StartCoroutine(Explode());          
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
}
