using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreateSkill2 : MonoBehaviourPun                                               // 스노우 특수 눈덩이 생성 슬롯
{
    public GameObject snowSkilled;
    public GameObject snowStone;
    public Transform spawnPoint;
    private readonly float limitTime = 3;
    private readonly int perCent = 55;         // 돌덩이 1, 얼음 2
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
            if (curTime >= limitTime)
            {
                bool skilled = RandomArrow.RandArrowPer(perCent);
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
