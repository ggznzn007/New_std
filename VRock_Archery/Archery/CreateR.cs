using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreateR : MonoBehaviourPunCallbacks
{
    public GameObject arrowSkilled;
    public GameObject arrowBomb;
    public Transform spawnPoint;
    private ParticleSystem _particleSystem;
    private PhotonView PV;
    private GameObject curArrow;
    private float curTime;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        PV = GetComponent<PhotonView>();
    }
    
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Skilled"))
        {
            if (PV.IsMine)
            {
                //if (!PV.IsMine) return;
                PV.RPC(nameof(FxPlay), RpcTarget.AllBuffered);
            }

        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Skilled"))
        {
            if (PV.IsMine)
            {
                //if (!PV.IsMine) return;
                PV.RPC(nameof(FxStop), RpcTarget.AllBuffered);
            }

        }
    }

    [PunRPC]
    public void FxPlay()
    {
        _particleSystem.Play();
    }

    [PunRPC]
    public void FxStop()
    {
        _particleSystem.Stop();
    }
}
