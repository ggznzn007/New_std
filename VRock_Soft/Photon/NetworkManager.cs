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
using System.Security.Cryptography;

[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneNum;
    public int maxPLayer;    
}


public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager NM;

    [Header("방 목록")]
    [SerializeField] List<DefaultRoom> defaultRooms;

    [Header("서버 접속창")]
    [SerializeField] GameObject connectUI;

    [Header("팀선택 창")]
    [SerializeField] GameObject teamSelectUI;

    [Header("맵선택 창")]
    [SerializeField] GameObject mapSelectUI;

    [Header("맵선택 창")]
    [SerializeField] GameObject mapRedUI;

    [Header("맵선택 창")]
    [SerializeField] GameObject mapBlueUI;

    [Header("로컬플레이어")]
    [SerializeField] GameObject localPlayer;
    
    [Header("페이드인 스크린")]
    [SerializeField] GameObject fadeScreen;

    /*[Header("게임플레이 팀선택 판단")]
    public bool isRed;*/

    [Header("게임중 여부 판단")]
    public bool inGame;

    [Header("게임시간 유무 판단")]
    public bool isTime;    

    private readonly string gameVersion = "1.0";                                           // 게임버전
    //private readonly string masterAddress = "125.134.36.239";                            // 서버주소
    // private readonly string appID = "698049ca-edd8-41f6-9c9b-b8561355930a";             // 앱아이디
    // private readonly int portNum = 5055;                                                // 포트넘버
    private readonly int n = 1;                                                            // 난수 n개 생성
    private readonly int maxCount = 1000;                                                  // 난수 최대 범위

    #region ######################################### 유니티 & UI 메서드 시작 #####################################################################
    private void Awake()
    {
        if (NM != null && NM != this)
        {
            Destroy(this.gameObject);
        }        
        NM = this;
        PN.AutomaticallySyncScene = true;
    }
    private void Start()
    {       
        DataManager.DM.startingNum++;                                                // 씬이 로딩 될 때마다 증가       
    }

    public void StartToServer()                                                     // 서버연결 메서드
    {
        PN.ConnectUsingSettings();                                                  // 디폴트 연결 포톤클라우드
        //PN.ConnectToMaster(masterAddress, portNum, appID);                        // 포톤서버주소, 포트넘버, 앱아이디로 서버연결
        PN.GameVersion = gameVersion;                                               // 게임 버전 *중요
        PN.AutomaticallySyncScene = true;                                           // 자동으로 씬 동기화
        PN.SendRate = 60;                                                           // 서버전송 비율
        PN.SerializationRate = 30;                                                  // 동기화   비율
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // 겹치지 않는 난수 생성

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "번 플레이어";                  // 닉네임 n개 난수 번 플레이어
        }
    }
 
    public void InitRed()                                                           // 레드팀 선택
    {
        DataManager.DM.currentTeam = Team.RED;
        DataManager.DM.isSelected= true;           
        
        PN.JoinLobby();
    }
    public void InitBlue()                                                          // 블루팀 선택
    {
        DataManager.DM.currentTeam = Team.BLUE;
        DataManager.DM.isSelected = true;
        
        PN.JoinLobby();
    }

    public void InitRoomNormal(int defaultRoomIndex)                                      // 게임시간이 없는 방 생성 메서드
    {
        isTime = false;                                                                   // 시간 프로퍼티 유무
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];
       
        RoomOptions roomOptions = new RoomOptions
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)roomSettings.maxPLayer,
            PlayerTtl = 235,
            EmptyRoomTtl = 236
        };

        PN.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
        //localPlayer.SetActive(false);
    }
    public void InitRoomHasTime(int defaultRoomIndex)                                       // 게임시간이 있는 방 생성 메서드
    {
        isTime = true;                                                                      // 시간 프로퍼티 유무
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        
        Hashtable options = new Hashtable
        {
            { "Time", 40 }
        };
        RoomOptions roomOptions = new RoomOptions
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)roomSettings.maxPLayer,
            PlayerTtl = 235,
            EmptyRoomTtl = 236,
            CustomRoomProperties = options
        };

        PN.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
        // localPlayer.SetActive(false);
    }

    #endregion ######################################### 유니티 & UI 메서드 끝 ###################################################################

    #region @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 포톤 서버 콜백 메서드 시작 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public override void OnConnectedToMaster()                                       // 포톤 서버에 접속되면 호출되는 메서드
    {              
        if(DataManager.DM.startingNum==1)                                            // 게임 처음 시작시에만 호출
        {
            connectUI.SetActive(false);
            teamSelectUI.SetActive(true);            
        }
        else if (DataManager.DM.startingNum > 1)                                     // 두번째부터는 맵선택만 호출
        {
            connectUI.SetActive(false);
            teamSelectUI.SetActive(false);
            PN.JoinLobby();
        }

        Debug.Log($"{PN.LocalPlayer.NickName} 서버에 접속하였습니다.");
    }

    public override void OnJoinedLobby()                                             // 로비에 들어갔을 때 호출되는 메서드
    {        
        if (DataManager.DM.isSelected)                                               // 팀 선택을 했을 때 
        {
            teamSelectUI.SetActive(false);
            mapSelectUI.SetActive(true);
        }            

        Debug.Log($"{PN.LocalPlayer.NickName}님이 로비에 입장하였습니다.");
    }

    public override void OnJoinedRoom()
    {
        mapSelectUI.SetActive(false);
        if (!isTime)                                                                 // 게임씬에 시간 프로퍼티 적용 여부
        {
            PN.LoadLevel(1); // 튜토리얼씬
        }
        else
        {
            PN.LoadLevel(2); // 건슈팅씬
        }

    }
    

    #endregion @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 포톤 서버 콜백 메서드 끝 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


    private void OnApplicationQuit()
    {
        PN.Disconnect();
    }

}
