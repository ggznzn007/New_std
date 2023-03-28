using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class HeadDamaged_S : MonoBehaviourPun // ����� �÷��̾� �Ӹ� �ݶ��̴� ��ũ��Ʈ - ��弦 �����
{
    public AvartarController AT;

    private void Start()
    {
        AT = GetComponentInParent<AvartarController>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Snowball") && AT.isAlive && DataManager.DM.inGame)  // �⺻������ �Ӹ��� ���� �� �����
        {
            if (!AT.isDamaged)
            {
                AT.HeadShotDamage();
            }
        }

        if ((collision.collider.CompareTag("Stoneball") || collision.collider.CompareTag("Icicle"))
            && AT.isAlive && DataManager.DM.inGame)                                                  // Ư�� ������ �Ӹ��� ���� �� �����
        {
            if (!AT.isDamaged)
            {
                AT.HeadShotDamage();
            }
        }

    }
}
