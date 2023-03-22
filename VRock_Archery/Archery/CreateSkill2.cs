using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreateSkill2 : MonoBehaviourPun                                               // ����� Ư�� ������ ���� ����
{
    public GameObject snowSkilled;                                                        // ��帧 ������
    public GameObject snowStone;                                                          // ������ ������
    public Transform spawnPoint;                                                          // Ư�� ������ ���� ����Ʈ
    private readonly float limitTime = 2;                                                // Ư�� ������ ���� ���ѽð�
    private readonly float perCent = 0;                                                    // Ư�������� Ȯ�� ������ 1����, ��帧 2����  // ��ġ�� ������ ������ Ȯ���� ����
    private ParticleSystem _particleSystem;                                               // ���� ���� ��ƼŬ
    private AudioSource _audioSource;                                                     // ���� ���� �����
    private PhotonView PV;                                                                // �����
    private GameObject curBall;                                                           // ���� ������
    private float curTime;                                                                // ���� �ð�

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (PN.IsMasterClient)                                                         // ������ �� ȭ�� ����(��������� �����ϸ� ����)
        {
            SpawnBomb();
        }
    }

    public void SpawnBomb()                                                           // ������ ���� �޼���
    {
        if (curBall == null)                                                          // ���� ������ ����� ���� ����
        {
            if (curBall != null) return;                                              // �ƴϸ� ����
            curTime += Time.deltaTime;
            if (curTime >= limitTime)                                                 // �ð��� ���ѽð����� ���ų� Ŭ �� ����
            {
                bool skilled = RandomArrow.RandArrowPer(perCent);                     // Ȯ�� ����ϴ� �޼��� (���� ���� Ŭ����)
                if (skilled)
                {
                    curBall = PN.InstantiateRoomObject(snowStone.name, spawnPoint.position, spawnPoint.rotation, 0);
                    Debug.Log("������ ����");
                    curTime = 0;                   
                }
                else
                {
                    curBall = PN.InstantiateRoomObject(snowSkilled.name, spawnPoint.position, spawnPoint.rotation, 0);
                    Debug.Log("��帧 ����");
                    curTime = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Stoneball")|| coll.CompareTag("Icicle"))  // Ư�������� �±� �� ��, ��帧�� ���Կ� �±� ���� �� 
        {
            if (PV.IsMine)
            {
                //if (!PV.IsMine) return;
                PV.RPC(nameof(FxPlay), RpcTarget.AllBuffered); // ���� ��ƼŬ ��� , RPC�� ��� �÷��̾�� ����
            }

        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Stoneball") || coll.CompareTag("Icicle")) // Ư�������� �±� �� ��, ��帧�� ���Կ� �±� �� ���� �� 
        {
            if (PV.IsMine)
            {
                //if (!PV.IsMine) return;
                PV.RPC(nameof(FxStop), RpcTarget.AllBuffered); // ���� ��ƼŬ ���� , RPC�� ��� �÷��̾�� ����
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
