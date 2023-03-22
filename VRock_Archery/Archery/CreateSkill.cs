using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class CreateSkill : MonoBehaviourPun                                                 // ��ó Ư�� ȭ�� ���� ����
{
    public GameObject arrowSkilled;                                                   // ��ųȭ�� ������
    public GameObject arrowBomb;                                                      // ��źȭ�� ������
    public Transform spawnPoint;                                                      // Ư��ȭ�� ���� ����Ʈ
    private readonly float limitTime = 3;                                             // Ư��ȭ�� ���� ���ѽð�
    private readonly float perCent = 50;                                              // Ư��ȭ�� ���� Ȯ��       ��ų 1����, ��ź 2����
    private ParticleSystem _particleSystem;                                           // Ư��ȭ�� ���� ���� ��ƼŬ
    private AudioSource _audioSource;                                                 // Ư��ȭ�� ���� �����
    private PhotonView PV;                                                            // �����
    private GameObject curArrow;                                                      // ���� ȭ��
    private float curTime;                                                            // ���� �ð�
    private int selectNum = 0;
    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (PN.IsMasterClient)                                                        // ������ �� ȭ�� ����(��������� �����ϸ� ����)
        {
            SpawnBomb(); // ����Ȯ�� ����
            //SpawnItem(); // �����ư��� ���� ����
        }
    }

    public void SpawnBomb()                                                            // ȭ�� ���� �޼���
    {
        if (curArrow == null)                                                          // ���� ������ ����� ���� ����
        {
            if (curArrow != null) return;                                              // �ƴϸ� ����
            curTime += Time.deltaTime;                                                 // �ð�
            if (curTime >= limitTime)                                                  // �ð��� ���ѽð����� ���ų� Ŭ �� ����
            {
                bool skilled = RandomArrow.RandArrowPer(perCent);                      // Ȯ�� ����ϴ� �޼��� (���� ���� Ŭ����)
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
                    Debug.Log("��ų ȭ�� ����");
                    curTime = 0;
                    selectNum = 1;
                }
                else
                {
                    curArrow = PN.InstantiateRoomObject(arrowBomb.name, spawnPoint.position, spawnPoint.rotation, 0);
                    Debug.Log("��ź ȭ�� ����");
                    curTime = 0;
                    selectNum = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Skilled"))                         // ȭ���±��߿� Skilled��� �±װ� �ݶ��̴�(��������)�� �±� ���� ��
        {
            if (PV.IsMine)
            {
                //if (!PV.IsMine) return;
                PV.RPC(nameof(FxPlay), RpcTarget.AllBuffered);  // ���� ��ƼŬ ��� , RPC�� ��� �÷��̾�� ����
            }

        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Skilled"))                        // ȭ���±��߿� Skilled��� �±װ� �ݶ��̴�(��������)�� �±� �� ���� ��
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
        curArrow = null;
    }
}
