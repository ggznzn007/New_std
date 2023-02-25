using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ParticleCtrl : MonoBehaviourPunCallbacks
{
    public GameObject arrowSkilled;
    public GameObject arrowBomb;
    public Transform spawnPoint;
    private ParticleSystem _particleSystem;    
    private PhotonView PV;
    private GameObject curArrow;
    private float curTime;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        PV = GetComponent<PhotonView>();       
    }

    private void Update()
    {
        if(PN.IsMasterClient)
        {
            SpawnBomb();
        }
    }

    public void SpawnBomb()
    {
        if (curArrow == null)
        {
            if (curArrow != null) return;
            curTime += Time.deltaTime;
            if (curTime >= 3)
            {
                bool skilled = RandomArrow.RandArrowPer(50);
                if (skilled)
                {
                    curArrow = PN.InstantiateRoomObject(arrowSkilled.name, spawnPoint.position, spawnPoint.rotation, 0);
                    Debug.Log("胶懦 积己");
                    curTime = 0;
                }
                else
                {
                    curArrow = PN.InstantiateRoomObject(arrowBomb.name, spawnPoint.position, spawnPoint.rotation, 0);
                    Debug.Log("气藕 积己");
                    curTime = 0;
                }
            }
        }
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
        curArrow= null;
    }

}
