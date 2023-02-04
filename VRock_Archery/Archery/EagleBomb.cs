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

public class EagleBomb : MonoBehaviourPunCallbacks,IPunObservable
{
    public Rigidbody rb;
    public SphereCollider bombColl;
    public ParticleSystem effect;
    public string fireCircle;
    private PhotonView PV;
    private Vector3 remotePos;
    private Quaternion remoteRot;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        bombColl = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (!PV.IsMine)
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, 30 * Time.deltaTime)
                , Quaternion.Lerp(transform.rotation, remoteRot, 30 * Time.deltaTime));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cube"))
        {
            if (PV.IsMine)
            {
                PV.RPC(nameof(FireCircle), RpcTarget.AllBuffered);
            }
        }
    }


    [PunRPC]
    public void FireCircle()
    {
        Destroy(gameObject);
        AudioManager.AM.PlaySX(fireCircle);
        PN.Instantiate(effect.name, transform.position - new Vector3(0,0.35f,0), effect.transform.rotation,0);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(remotePos);
            stream.SendNext(remoteRot);
        }
        else
        {
            remotePos = (Vector3)stream.ReceiveNext();
            remoteRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
