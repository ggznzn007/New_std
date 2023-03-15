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
public class LoadingSceneManager : MonoBehaviourPunCallbacks
{    
    void Awake()
    {
        
       // GameObject.Find("Photon Manager").GetComponent<PhotonManager_Ver_2>().SpawnPlayer(); 
    }

    
}
