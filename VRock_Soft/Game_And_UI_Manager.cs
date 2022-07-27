using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using Photon.Pun.Demo.PunBasics;
using UnityEngine.SceneManagement;

public class Game_And_UI_Manager : MonoBehaviourPunCallbacks
{
    public GameObject teamSelectPanel;
    public GameObject gameStartPanel;
    public GameObject lobbyPlayer;
    private GameObject player;
    public GameObject[] bgObjects;
    

    #region 유니티 함수
    void Start()
    {

    }
    void Update()
    {

    }
    #endregion
}
