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
public class BodyDamaged_S : MonoBehaviourPun
{
    public AvartarController AT;

    private void OnCollisionEnter(Collision collision)                         // 기본 화살이 몸에 맞았을 때 노멀 대미지 
    {
        if (collision.collider.CompareTag("Snowball") && AT.isAlive && DataManager.DM.inGame)
        {
            if (!AT.isDamaged)
            {
                AT.NormalDamage();
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
