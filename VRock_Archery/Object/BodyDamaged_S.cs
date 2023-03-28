using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class BodyDamaged_S : MonoBehaviourPun  // 스노우 플레이어 몸통 콜라이더 스크립트 - 바디 대미지
{
    public AvartarController AT;

    private void Start()
    {
        AT = GetComponentInParent<AvartarController>();
    }

    private void OnCollisionEnter(Collision collision)                         // 기본 눈덩이 맞았을 때 노멀 대미지 
    {
        if (collision.collider.CompareTag("Snowball") && AT.isAlive && DataManager.DM.inGame)
        {
            if (!AT.isDamaged)
            {
                AT.NormalDamage();
            }
        }

        if ((collision.collider.CompareTag("Stoneball") || collision.collider.CompareTag("Icicle"))
           && AT.isAlive && DataManager.DM.inGame)                                                  // 특수 눈덩이 맞을 때 대미지
        {
            if (!AT.isDamaged)
            {
                AT.HeadShotDamage(); // 특수 눈덩이는 헤드샷과 같은 대미지 적용
            }
        }

    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Bomb") && AT.isAlive && DataManager.DM.inGame)             // 아이스 폭탄 대미지
        {
            if (!AT.isDamaged)
            {
                AT.IceDamage();               
            }
        }

        if (coll.CompareTag("Effect") && AT.isAlive && DataManager.DM.inGame)           // 고드름 스킬 대미지
        {
            if (!AT.isDamaged)
            {
                AT.SkillDamage();                
            }
        }
    }

}
