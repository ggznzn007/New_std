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
public class BodyDamaged : MonoBehaviourPun
{
    public AvartarController AT;

    private void Start()
    {
        AT = GetComponentInParent<AvartarController>();
    }

    private void OnCollisionEnter(Collision collision)                         // 기본 화살이 몸에 맞았을 때 노멀 대미지 
    {
        if (collision.collider.CompareTag("Arrow") && AT.isAlive && DataManager.DM.inGame)
        {
            if (!AT.isDamaged)
            {
                AT.NormalDamage();
                Debug.Log("바디샷!");
            }
        }
       
    }

    private void OnTriggerEnter(Collider coll)                             
    {
        if (coll.CompareTag("Bomb") && AT.isAlive && DataManager.DM.inGame)             // 폭탄 화살 대미지
        {
            if (!AT.isDamaged)
            {
                AT.BombDamage();
                Debug.Log("폭탄대미지!");
            }
        }
        if (coll.CompareTag("SFX") && AT.isAlive && DataManager.DM.inGame)          // 스킬 화살 대미지
        {
            if (!AT.isDamaged)
            {
                AT.SkillDamage();
                Debug.Log("스킬대미지!");
            }
        }

        if (coll.CompareTag("Effect") && AT.isAlive && DataManager.DM.inGame)           // NPC 도트 대미지
        {
            if (!AT.isDamaged)
            {
                AT.DotDamage();
                Debug.Log("도트대미지!");
            }
        }
      
    }

   

    /* private void OnParticleCollision(GameObject explosion)
     {
         if (explosion.CompareTag("Bomb") && AT.isAlive && NetworkManager.NM.inGame)
         {
             if (!AT.isDamaged)
             {
                 AT.GrenadeDamage();
                 Debug.Log("바디샷!");
             }
         }
     }*/

}
