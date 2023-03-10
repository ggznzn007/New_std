using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class CreateSkill : MonoBehaviourPun                                                 // ��ó Ư�� ȭ�� ���� ����
{
    public GameObject arrowSkilled;
    public GameObject arrowBomb;
    public Transform spawnPoint;
    private readonly float limitTime = 3;
    private readonly int perCent = 45;         // ��ų 1, ��ź 2
    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;
    private PhotonView PV;
    private GameObject curArrow;
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
        if (curArrow == null)
        {
            if (curArrow != null) return;
            curTime += Time.deltaTime;
            if (curTime >= limitTime)
            {
                bool skilled = RandomArrow.RandArrowPer(perCent);
                if (skilled)
                {
                    curArrow = PN.InstantiateRoomObject(arrowSkilled.name, spawnPoint.position, spawnPoint.rotation, 0);
                    Debug.Log("��ų ȭ�� ����");
                    curTime = 0;
                }
                else
                {
                    curArrow = PN.InstantiateRoomObject(arrowBomb.name, spawnPoint.position, spawnPoint.rotation, 0);
                    Debug.Log("��ź ȭ�� ����");
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
