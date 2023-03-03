using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreateSkill2 : MonoBehaviourPun
{
    public GameObject snowSkilled;
    public GameObject snowStone;
    public Transform spawnPoint;
    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;
    private PhotonView PV;
    private GameObject curBall;
    private float curTime;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (PN.IsMasterClient)
        {
            SpawnBomb();
        }
    }

    public void SpawnBomb()
    {
        if (curBall == null)
        {
            if (curBall != null) return;
            curTime += Time.deltaTime;
            if (curTime >= 2)
            {
                bool skilled = RandomArrow.RandArrowPer(10);
                if (skilled)
                {
                    curBall = PN.InstantiateRoomObject(snowStone.name, spawnPoint.position, spawnPoint.rotation, 0);
                    Debug.Log("돌덩이 생성");
                    curTime = 0;
                   
                }
                else
                {
                    curBall = PN.InstantiateRoomObject(snowSkilled.name, spawnPoint.position, spawnPoint.rotation, 0);
                    Debug.Log("고드름 생성");
                    curTime = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Stoneball")|| coll.CompareTag("Icicle"))
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
        if (coll.CompareTag("Stoneball") || coll.CompareTag("Icicle"))
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
        _audioSource.Play();
    }

    [PunRPC]
    public void FxStop()
    {
        _particleSystem.Stop();
        _audioSource.Stop();
        curBall = null;
    }
}
