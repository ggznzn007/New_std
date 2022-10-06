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
    public PhotonView PV;
    private void OnCollisionEnter(Collision collision)                         // 총알 태그 시 메서드
    {
        if (collision.collider.CompareTag("Bullet") && AvartarController.ATC.isAlive && NetworkManager.NM.inGame )
        {
            AvartarController.ATC.NormalDamage();
        }
    }
    
}
