using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class CreateSkill : MonoBehaviourPun                                                 // 아처 특수 화살 생성 슬롯
{
    public GameObject arrowSkilled;                                                   // 스킬화살 프리팹
    public GameObject arrowBomb;                                                      // 폭탄화살 프리팹
    public Transform spawnPoint;                                                      // 특수화살 생성 포인트
    private readonly float limitTime = 3;                                             // 특수화살 생성 제한시간
    private readonly float perCent = 50;                                              // 특수화살 생성 확률       스킬 1순위, 폭탄 2순위
    private ParticleSystem _particleSystem;                                           // 특수화살 생성 슬롯 파티클
    private AudioSource _audioSource;                                                 // 특수화살 생성 오디오
    private PhotonView PV;                                                            // 포톤뷰
    private GameObject curArrow;                                                      // 현재 화살
    private float curTime;                                                            // 현재 시간
    private int selectNum = 0;
    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (PN.IsMasterClient)                                                        // 마스터 만 화살 생성(여러사람이 생성하면 에러)
        {
            SpawnBomb(); // 랜덤확률 생성
            //SpawnItem(); // 번갈아가며 교차 생성
        }
    }

    public void SpawnBomb()                                                            // 화살 생성 메서드
    {
        if (curArrow == null)                                                          // 현재 슬롯이 비었을 때만 생성
        {
            if (curArrow != null) return;                                              // 아니면 리턴
            curTime += Time.deltaTime;                                                 // 시간
            if (curTime >= limitTime)                                                  // 시간이 제한시간보다 같거나 클 때 생성
            {
                bool skilled = RandomArrow.RandArrowPer(perCent);                      // 확률 계산하는 메서드 (따로 만든 클래스)
                if (skilled)
                {
                    curArrow = PN.InstantiateRoomObject(arrowSkilled.name, spawnPoint.position, spawnPoint.rotation, 0);
                    Debug.Log("스킬 화살 생성");
                    curTime = 0;
                }
                else
                {
                    curArrow = PN.InstantiateRoomObject(arrowBomb.name, spawnPoint.position, spawnPoint.rotation, 0);
                    Debug.Log("폭탄 화살 생성");
                    curTime = 0;
                }
            }
        }
    }

    public void SpawnItem()
    {
        if (curArrow == null)
        {
            if (curArrow != null) { return; }
            curTime += Time.deltaTime;
            if(curTime>=limitTime)
            {
                int thisSelect = selectNum;
                if(thisSelect==0)
                {
                    curArrow = PN.InstantiateRoomObject(arrowSkilled.name, spawnPoint.position, spawnPoint.rotation, 0);
                    Debug.Log("스킬 화살 생성");
                    curTime = 0;
                    selectNum = 1;
                }
                else
                {
                    curArrow = PN.InstantiateRoomObject(arrowBomb.name, spawnPoint.position, spawnPoint.rotation, 0);
                    Debug.Log("폭탄 화살 생성");
                    curTime = 0;
                    selectNum = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Skilled"))                         // 화살태그중에 Skilled라는 태그가 콜라이더(생성슬롯)에 태그 시작 시
        {
            if (PV.IsMine)
            {
                //if (!PV.IsMine) return;
                PV.RPC(nameof(FxPlay), RpcTarget.AllBuffered);  // 슬롯 파티클 재생 , RPC로 모든 플레이어에게 공유
            }

        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Skilled"))                        // 화살태그중에 Skilled라는 태그가 콜라이더(생성슬롯)에 태그 중 나갈 때
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
        curArrow = null;
    }
}
