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
   // public GameObject effectEx;

    private void Start()
    {
        AT = GetComponentInParent<AvartarController>();
    }
    // public PhotonView PV;
    private void OnCollisionEnter(Collision collision)                         // ÃÑ¾Ë ÅÂ±× ½Ã ¸Þ¼­µå
    {
        if (collision.collider.CompareTag("Bullet") && AT.isAlive && NetworkManager.NM.inGame )
        {
            if(!AT.isDamaged)
            {
                AT.NormalDamage();
                Debug.Log("¹Ùµð¼¦!");
            }
            
        }
    }

    private void OnParticleCollision(GameObject explosion)
    {
        if(explosion.CompareTag("Bullet") && AT.isAlive && NetworkManager.NM.inGame)
        {
            if (!AT.isDamaged)
            {
                AT.NormalDamage();
                Debug.Log("¹Ùµð¼¦!");
            }
        }
    }

}
