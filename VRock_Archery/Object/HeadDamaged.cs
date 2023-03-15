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
public class HeadDamaged : MonoBehaviourPun
{
    public AvartarController AT;

    private void Start()
    {
        AT = GetComponentInParent<AvartarController>();
    }

    private void OnCollisionEnter(Collision collision)                         // �Ѿ� �±� �� �޼���
    {
        if (collision.collider.CompareTag("Arrow") && AT.isAlive && DataManager.DM.inGame)
        {
            if (!AT.isDamaged)
            {
                AT.HeadShotDamage();
                Debug.Log("��弦!!!");
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



    /*private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Bomb") && AT.isAlive && NetworkManager.NM.inGame)
        {
            if (!AT.isDamaged)
            {
                AT.GrenadeDamage();
                Debug.Log("��ź������!");
            }

        }
    }*/

    /*  private void OnParticleCollision(GameObject explosion)
      {
          if (explosion.CompareTag("Bomb") && AT.isAlive && NetworkManager.NM.inGame)
          {
              if (!AT.isDamaged)
              {
                  AT.GrenadeDamage();
                  Debug.Log("��弦!");
              }
          }
      }
  */

}
