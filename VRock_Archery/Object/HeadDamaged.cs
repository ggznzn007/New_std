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
public class HeadDamaged : MonoBehaviourPun       // 아처 플레이어 머리 콜라이더 스크립트 - 헤드샷 대미지
{
    public AvartarController AT;

    private void Start()
    {
        AT = GetComponentInParent<AvartarController>();
    }

    private void OnCollisionEnter(Collision collision)     
    {
        if (collision.collider.CompareTag("Arrow") && AT.isAlive && DataManager.DM.inGame)  // 기본화살에 맞았을 때 대미지
        {
            if (!AT.isDamaged)
            {
                AT.HeadShotDamage();                
            }
        }
    }

    /*private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Effect") && AT.isAlive && DataManager.DM.inGame)
        {
            if (!AT.isDamaged)
            {
                AudioManager.AM.PlaySE("Damage");
                AT.SkillDamage();
                Debug.Log("폭탄데미지!");
            }
        }
    }*/            
}
