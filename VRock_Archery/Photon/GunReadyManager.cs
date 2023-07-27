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
using System.Security.Cryptography;

public class GunReadyManager : MonoBehaviourPunCallbacks
{
    public static GunReadyManager GRM;
    [Header("페이드인 스크린")]
    [SerializeField] GameObject fadeScreen;

    [Header("로컬플레이어")]
    [SerializeField] GameObject localPlayer;

    [Header("팀선택 판단")]
    public bool isRed = false;

    [Header("인게임 판단")]
    public bool inGame;

    private readonly string gameVersion = "1.0";
    private readonly string masterAddress = "125.134.36.239";
    private readonly string appID = "698049ca-edd8-41f6-9c9b-b8561355930a";
    private readonly int portNum = 5055;
    private readonly int n = 1;
    private readonly int maxCount = 6;

    private void Awake()
    {
        if (GRM != null && GRM != this)
        {
            Destroy(this.gameObject);
        }
        GRM = this;
    }

    private void Start()
    {
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        StartToServer();
    }

    public void StartToServer()                                                     // 서버연결 메서드
    {
        //PN.ConnectUsingSettings();                                                // 디폴트 연결
        PN.ConnectToMaster(masterAddress, portNum, appID);                          // 서버주소, 포트넘버, 앱아이디로 서버연결
        PN.GameVersion = gameVersion;                                               // 게임 버전 *중요
        PN.AutomaticallySyncScene = true;                                           // 자동으로 씬 동기화
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // 겹치지 않는 난수 생성

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "번 플레이어";
        }
    }

    public void InitiliazeRedTeam()       // 레드팀 버튼                            // 로비 진입 후 팀선택 패널에서 레드팀선택 메서드
    {
        isRed = true;
        fadeScreen.SetActive(true);
        SceneManager.LoadScene(3);
        //mapSelectUI.SetActive(true);
        //localPlayer.SetActive(false);
        //PN.JoinRoom("LobbyRoom");
        /*RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 10, EmptyRoomTtl = 1000 }; // 방 옵션
        PN.JoinOrCreateRoom("LobbyRoom", options, TypedLobbyInfo.Default);*/
    }

    public void InitiliazeBlueTeam()      // 블루팀 버튼                            // 로비 진입 후 팀선택 패널에서 블루팀선택 메서드
    {
        isRed = false;
        fadeScreen.SetActive(true);
        SceneManager.LoadScene(3);
        //mapSelectUI.SetActive(true);
        //localPlayer.SetActive(false);
        //PN.JoinRoom("LobbyRoom");
        /* RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 10, EmptyRoomTtl = 1000 }; // 방 옵션
         PN.JoinOrCreateRoom("LobbyRoom", options, TypedLobbyInfo.Default);*/
    }

    #region 포톤 서버 콜백 메서드 시작//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override void OnConnectedToMaster()                                       // 포톤 서버에 접속되면 호출되는 메서드
    {
        Debug.Log($"{PN.LocalPlayer.NickName} 서버에 접속하였습니다.");
        PN.JoinLobby();
    }

    public override void OnJoinedLobby()                                             // 로비에 들어갔을 때 호출되는 메서드
    {
        Debug.Log($"{PN.LocalPlayer.NickName}님이 로비에 입장하였습니다.");
    }

    #endregion 포톤 서버 콜백 메서드 끝 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
