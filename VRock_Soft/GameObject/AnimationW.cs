using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.XR;
using Unity.XR.PXR;

public class AnimationW : MonoBehaviourPunCallbacks
{
    /*[Header("ÆøÅº ÇÁ¸®ÆÕ")]
    public GameObject bomB;
    public Transform bSpawnSpot1;
    public Transform bSpawnSpot2;
    private PhotonView PV;*/
    void SpawnDynamite()
    {
        if (PN.IsMasterClient)
        {
            WesternManager.WM.SpawnDynamite();
        }
    }
    private void Start()
    {
       // PV=GetComponent<PhotonView>();
       /* bSpawnSpot1 = GameObject.Find("BombSpawnPoint").GetComponent<Transform>();
        bSpawnSpot2 = GameObject.Find("BombSpawnPoint2").GetComponent<Transform>();*/
    }


 
    


}
