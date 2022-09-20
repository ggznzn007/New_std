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
public class PhotonManager : MonoBehaviourPunCallbacks
{
    //public static PhotonManager PUN2;
   // private readonly string version = "1.0"; // 게임 버전 입력 == 같은 버전의 유저끼리 접속허용
    [SerializeField] GameObject lobbyPlayer;
    [SerializeField] GameObject selectUI;

    #region 유니티 함수
    private void Awake()
    {
       /* PN.AutomaticallySyncScene = true;
        PN.GameVersion = version;
        Debug.Log($"서버와 통신횟수 초당 : {PN.SendRate}");                            // 포톤 서버와 통신 횟수 설정. 초당 30회     
        PN.ConnectUsingSettings();*/
       
    }

    public void SelectRed()
    {
        PN.LoadLevel("LobbyScene");
    }
    private void Start()
    {
        
    }

    void Update()
    {

       
    }
       
    #endregion

    #region 포톤 콜백 함수

    public override void OnConnected()
    {
        Debug.Log("OnConnect 호출완료. 서버 사용가능");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버에 연결 성공");
        Debug.Log($"{PN.InLobby}");
        PN.JoinLobby();                                           // 로비 입장     
    }

    public override void OnJoinedLobby()
    {
        selectUI.SetActive(true);
    }

    

    #endregion




}
