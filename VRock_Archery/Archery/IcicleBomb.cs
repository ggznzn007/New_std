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

public class IcicleBomb : MonoBehaviourPunCallbacks, IPunObservable
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
        if (collision.collider.CompareTag("FloorBox")|| collision.collider.CompareTag("Cube")
            ||collision.collider.CompareTag("Snowblock")|| collision.collider.CompareTag("Obtacle"))
        {
            PV.RPC(nameof(FireCircle), RpcTarget.AllBuffered);
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

    /*[PunRPC]
    public void FireDelete()
    {
        Destroy(gameObject);
    }*/

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
