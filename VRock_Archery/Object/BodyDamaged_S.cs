using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
public class BodyDamaged_S : MonoBehaviourPun
{
    public AvartarController AT;

    private void OnCollisionEnter(Collision collision)                         // �⺻ ȭ���� ���� �¾��� �� ��� ����� 
    {
        if (collision.collider.CompareTag("Snowball") && AT.isAlive && DataManager.DM.inGame)
        {
            if (!AT.isDamaged)
            {
                AT.NormalDamage();
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
