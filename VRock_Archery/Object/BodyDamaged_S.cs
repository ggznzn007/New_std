using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class BodyDamaged_S : MonoBehaviourPun  // ����� �÷��̾� ���� �ݶ��̴� ��ũ��Ʈ - �ٵ� �����
{
    public AvartarController AT;

    private void Start()
    {
        AT = GetComponentInParent<AvartarController>();
    }

    private void OnCollisionEnter(Collision collision)                         // �⺻ ������ �¾��� �� ��� ����� 
    {
        if (collision.collider.CompareTag("Snowball") && AT.isAlive && DataManager.DM.inGame)
        {
            if (!AT.isDamaged)
            {
                AT.NormalDamage();
            }
        }

        if ((collision.collider.CompareTag("Stoneball") || collision.collider.CompareTag("Icicle"))
           && AT.isAlive && DataManager.DM.inGame)                                                  // Ư�� ������ ���� �� �����
        {
            if (!AT.isDamaged)
            {
                AT.HeadShotDamage(); // Ư�� �����̴� ��弦�� ���� ����� ����
            }
        }

    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Bomb") && AT.isAlive && DataManager.DM.inGame)             // ���̽� ��ź �����
        {
            if (!AT.isDamaged)
            {
                AT.IceDamage();               
            }
        }

        if (coll.CompareTag("Effect") && AT.isAlive && DataManager.DM.inGame)           // ��帧 ��ų �����
        {
            if (!AT.isDamaged)
            {
                AT.SkillDamage();                
            }
        }
    }

}
