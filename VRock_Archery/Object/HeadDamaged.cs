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
public class HeadDamaged : MonoBehaviourPun       // ��ó �÷��̾� �Ӹ� �ݶ��̴� ��ũ��Ʈ - ��弦 �����
{
    public AvartarController AT;

    private void Start()
    {
        AT = GetComponentInParent<AvartarController>();
    }

    private void OnCollisionEnter(Collision collision)     
    {
        if (collision.collider.CompareTag("Arrow") && AT.isAlive && DataManager.DM.inGame)  // �⺻ȭ�쿡 �¾��� �� �����
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
                Debug.Log("��ź������!");
            }
        }
    }*/            
}
