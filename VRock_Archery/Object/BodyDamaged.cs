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
public class BodyDamaged : MonoBehaviourPun   // ��ó �÷��̾� ���� �ݶ��̴� ��ũ��Ʈ - �ٵ� �����
{
    public AvartarController AT;

    private void Start()
    {
        AT = GetComponentInParent<AvartarController>();
    }

    private void OnCollisionEnter(Collision collision)                         
    {
        if (collision.collider.CompareTag("Arrow") && AT.isAlive && DataManager.DM.inGame) // �⺻ ȭ���� ���� �¾��� �� ��� ����� 
        {
            if (!AT.isDamaged)
            {
                AT.NormalDamage();               
            }
        }
       
    }

    private void OnTriggerEnter(Collider coll)                             
    {
        if (coll.CompareTag("Bomb") && AT.isAlive && DataManager.DM.inGame)         // ��ź ȭ�� �����
        {
            if (!AT.isDamaged)
            {
                AT.BombDamage();                
            }
        }
        if (coll.CompareTag("SFX") && AT.isAlive && DataManager.DM.inGame)          // ��ų ȭ�� �����
        {
            if (!AT.isDamaged)
            {
                AT.SkillDamage();                
            }
        }

        if (coll.CompareTag("Effect") && AT.isAlive && DataManager.DM.inGame)      // ������ ��ź ��Ʈ �����
        {
            if (!AT.isDamaged)
            {
                AT.DotDamage();                
            }
        }
      
    }
}
