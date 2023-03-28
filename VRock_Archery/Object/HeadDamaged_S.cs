using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class HeadDamaged_S : MonoBehaviourPun // 스노우 플레이어 머리 콜라이더 스크립트 - 헤드샷 대미지
{
    public AvartarController AT;

    private void Start()
    {
        AT = GetComponentInParent<AvartarController>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Snowball") && AT.isAlive && DataManager.DM.inGame)  // 기본눈덩이 머리에 맞을 때 대미지
        {
            if (!AT.isDamaged)
            {
                AT.HeadShotDamage();
            }
        }

        if ((collision.collider.CompareTag("Stoneball") || collision.collider.CompareTag("Icicle"))
            && AT.isAlive && DataManager.DM.inGame)                                                  // 특수 눈덩이 머리에 맞을 때 대미지
        {
            if (!AT.isDamaged)
            {
                AT.HeadShotDamage();
            }
        }

    }
}
