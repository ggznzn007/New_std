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

    private void OnCollisionEnter(Collision collision)                         // �⺻ ȭ���� ���� �¾��� �� ��� ����� 
    {
        if (collision.collider.CompareTag("Arrow") && AT.isAlive && DataManager.DM.inGame)
        {
            if (!AT.isDamaged)
            {
                AT.NormalDamage();
                Debug.Log("�ٵ�!");
            }
        }
       
    }

    private void OnTriggerEnter(Collider coll)                             
    {
        if (coll.CompareTag("Bomb") && AT.isAlive && DataManager.DM.inGame)             // ��ź ȭ�� �����
        {
            if (!AT.isDamaged)
            {
                AT.BombDamage();
                Debug.Log("��ź�����!");
            }
        }
        if (coll.CompareTag("SFX") && AT.isAlive && DataManager.DM.inGame)          // ��ų ȭ�� �����
        {
            if (!AT.isDamaged)
            {
                AT.SkillDamage();
                Debug.Log("��ų�����!");
            }
        }

        if (coll.CompareTag("Effect") && AT.isAlive && DataManager.DM.inGame)           // NPC ��Ʈ �����
        {
            if (!AT.isDamaged)
            {
                AT.DotDamage();
                Debug.Log("��Ʈ�����!");
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
                 Debug.Log("�ٵ�!");
             }
         }
     }*/

}
