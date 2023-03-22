using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreateSkill2 : MonoBehaviourPun                                               // 스노우 특수 눈덩이 생성 슬롯
{
    public GameObject snowSkilled;                                                        // 고드름 프리팹
    public GameObject snowStone;                                                          // 돌덩이 프리팹
    public Transform spawnPoint;                                                          // 특수 눈덩이 생성 포인트
    private readonly float limitTime = 2;                                                // 특수 눈덩이 생성 제한시간
    private readonly float perCent = 0;                                                    // 특수눈덩이 확률 돌덩이 1순위, 고드름 2순위  // 수치가 높으면 돌덩이 확률이 높다
    private ParticleSystem _particleSystem;                                               // 생성 슬롯 파티클
    private AudioSource _audioSource;                                                     // 생성 슬롯 오디오
    private PhotonView PV;                                                                // 포톤뷰
    private GameObject curBall;                                                           // 현재 눈덩이
    private float curTime;                                                                // 현재 시간

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (PN.IsMasterClient)                                                         // 마스터 만 화살 생성(여러사람이 생성하면 에러)
        {
            SpawnBomb();
        }
    }

    public void SpawnBomb()                                                           // 눈덩이 생성 메서드
    {
        if (curBall == null)                                                          // 현재 슬롯이 비었을 때만 생성
        {
            if (curBall != null) return;                                              // 아니면 리턴
            curTime += Time.deltaTime;
            if (curTime >= limitTime)                                                 // 시간이 제한시간보다 같거나 클 때 생성
            {
                bool skilled = RandomArrow.RandArrowPer(perCent);                     // 확률 계산하는 메서드 (따로 만든 클래스)
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
        if (coll.CompareTag("Stoneball")|| coll.CompareTag("Icicle"))  // 특수눈덩이 태그 중 돌, 고드름이 슬롯에 태그 시작 시 
        {
            if (PV.IsMine)
            {
                //if (!PV.IsMine) return;
                PV.RPC(nameof(FxPlay), RpcTarget.AllBuffered); // 슬롯 파티클 재생 , RPC로 모든 플레이어에게 공유
            }

        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Stoneball") || coll.CompareTag("Icicle")) // 특수눈덩이 태그 중 돌, 고드름이 슬롯에 태그 중 나갈 때 
        {
            if (PV.IsMine)
            {
                //if (!PV.IsMine) return;
                PV.RPC(nameof(FxStop), RpcTarget.AllBuffered); // 슬롯 파티클 정지 , RPC로 모든 플레이어에게 공유
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
