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
public class BodyDamaged : MonoBehaviourPun   // 아처 플레이어 몸통 콜라이더 스크립트 - 바디 대미지
{
    public AvartarController AT;

    private void Start()
    {
        AT = GetComponentInParent<AvartarController>();
    }

    private void OnCollisionEnter(Collision collision)                         
    {
        if (collision.collider.CompareTag("Arrow") && AT.isAlive && DataManager.DM.inGame) // 기본 화살이 몸에 맞았을 때 노멀 대미지 
        {
            if (!AT.isDamaged)
            {
                AT.NormalDamage();               
            }
        }
       
    }

    private void OnTriggerEnter(Collider coll)                             
    {
        if (coll.CompareTag("Bomb") && AT.isAlive && DataManager.DM.inGame)         // 폭탄 화살 대미지
        {
            if (!AT.isDamaged)
            {
                AT.BombDamage();                
            }
        }
        if (coll.CompareTag("SFX") && AT.isAlive && DataManager.DM.inGame)          // 스킬 화살 대미지
        {
            if (!AT.isDamaged)
            {
                AT.SkillDamage();                
            }
        }

        if (coll.CompareTag("Effect") && AT.isAlive && DataManager.DM.inGame)      // 독수리 폭탄 도트 대미지
        {
            if (!AT.isDamaged)
            {
                AT.DotDamage();                
            }
        }
      
    }
}
