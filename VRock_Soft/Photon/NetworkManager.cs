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
using Unity.VisualScripting;
using static UnityEngine.UIElements.UxmlAttributeDescription;

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

    [Header("튜토리얼1 참가인원")]
    [SerializeField] TextMeshProUGUI countText_TT;

    /*[Header("TOY 참가인원")]
    [SerializeField] TextMeshProUGUI countText_T;*/

    [Header("튜토리얼2 참가인원")]
    [SerializeField] TextMeshProUGUI countText_TW;

    /*[Header("Western 참가인원")]
    [SerializeField] TextMeshProUGUI countText_W;*/

    [Header("로컬플레이어")]
    [SerializeField] GameObject localPlayer;

    [Header("페이드인 스크린")]
    [SerializeField] GameObject fadeScreen;

    [Header("인게임 판단")]
    public bool inGame;

    /*[Header("게임시간여부 판단")]
    public bool gamehasTime;*/

    private readonly string gameVersion = "1.0";
    //private readonly string masterAddress = "125.134.36.239";
    // private readonly string appID = "698049ca-edd8-41f6-9c9b-b8561355930a";
    // private readonly int portNum = 5055;
    private readonly int n = 1;
    private readonly int maxCount = 100;
    public Hashtable team = new Hashtable();
    
    private void Awake()
    {
        if (NM != null && NM != this)
        {
            Destroy(this.gameObject);
        }
        NM = this;
        //DontDestroyOnLoad(this);
        PN.AutomaticallySyncScene = true;

    }
    private void Start()
    {
        DataManager.DM.startingNum++;
    }

    public void StartToServer()                                                     // 서버연결 메서드
    {
        PN.ConnectUsingSettings();                                                  // 디폴트 연결
        //PN.ConnectToMaster(masterAddress, portNum, appID);                        // 서버주소, 포트넘버, 앱아이디로 서버연결
        PN.GameVersion = gameVersion;                                               // 게임 버전 *중요
        PN.AutomaticallySyncScene = true;                                           // 자동으로 씬 동기화
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // 겹치지 않는 난수 생성

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "번 플레이어";
        }
    }


    public void InitRed()
    {
        DataManager.DM.currentTeam = Team.RED;
        DataManager.DM.isSelected = true;
        PN.JoinLobby();
    }
    public void InitBlue()
    {
        DataManager.DM.currentTeam = Team.BLUE;
        DataManager.DM.isSelected = true;
        PN.JoinLobby();
    }

    public void InitTutoT(int defaultRoomIndex)
    {
        DataManager.DM.currentMap = Map.TUTORIAL_T;
        //PN.KeepAliveInBackground = 3;
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        Hashtable options = new Hashtable { { "Time", 300 } };

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

    }
    public void InitToy(int defaultRoomIndex)
    {
        DataManager.DM.currentMap = Map.TOY;
       
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        Hashtable options = new Hashtable { { "Time", 120 } };

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

    }

    public void InitTutoW(int defaultRoomIndex)
    {
        DataManager.DM.currentMap = Map.TUTORIAL_W;

        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        Hashtable options = new Hashtable { { "Time", 300 } };

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

    }

    public void InitWestern(int defaultRoomIndex)
    {
        DataManager.DM.currentMap = Map.WESTERN;
        
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        Hashtable options = new Hashtable { { "Time", 120 } };

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

    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public override void OnConnectedToMaster()                                       // 포톤 서버에 접속되면 호출되는 메서드
    {
        if (DataManager.DM.startingNum == 1)
        {
            connectUI.SetActive(false);
            teamSelectUI.SetActive(true);
        }
        else if (DataManager.DM.startingNum > 1)
        {
            connectUI.SetActive(false);
            teamSelectUI.SetActive(false);
            PN.JoinLobby();
        }

        Debug.Log($"{PN.LocalPlayer.NickName} 서버에 접속하였습니다.");
    }

    public override void OnJoinedLobby()                                             // 로비에 들어갔을 때 호출되는 메서드
    {
        if (DataManager.DM.isSelected && PN.InLobby)
        {
            teamSelectUI.SetActive(false);
            mapSelectUI.SetActive(true);
        }


        Debug.Log($"{PN.LocalPlayer.NickName}님이 로비에 입장하였습니다.");
    }

    public override void OnJoinedRoom()
    {
        mapSelectUI.SetActive(false);

        switch (DataManager.DM.currentMap)
        {
            case Map.TUTORIAL_T:
                PN.LoadLevel(1); // 튜토리얼T
                break;
            case Map.TOY:
                PN.LoadLevel(2); // 토이
                break;
            case Map.TUTORIAL_W:
                PN.LoadLevel(3); // 튜토리얼W
                break;
            case Map.WESTERN:
                PN.LoadLevel(4); // 웨스턴
                break;
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList.Count == 0)                                 // 방에 아무도 없을 때
        {
            countText_TT.text = 0 + " 명";
            //countText_T.text = 0 + " 명";
            countText_TW.text = 0 + " 명";
           // countText_W.text = 0 + " 명";
        }

        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
            // DefaultRoom roomName = new DefaultRoom();            
            string roomString = room.Name.ToString();
            switch (roomString)
            {
                case "Tutorial_T":
                    Debug.Log("토이튜토 인원 : " + room.PlayerCount);
                    countText_TT.text = room.PlayerCount + " 명";
                    break;
                case "Tutorial_W":
                    Debug.Log("웨스턴튜토 인원 : " + room.PlayerCount);
                    countText_TW.text = room.PlayerCount + " 명";
                    break;
               /* case "Toy":
                    Debug.Log("토이방 인원 : " + room.PlayerCount);
                    countText_T.text = room.PlayerCount + " 명";
                    break;
                case "Western":
                    Debug.Log("웨스턴방 인원 : " + room.PlayerCount);
                    countText_W.text = room.PlayerCount + " 명";
                    break;*/
            }
        }
    }

    // 방 참가인원 업데이트 if문
    /*if (room.Name.Contains(roomName.Name = "Tutorial"))
            {
               Debug.Log("튜토리얼방 인원 : " + room.PlayerCount);
                    countText_TU.text = room.PlayerCount + " 명";
            }
            else if(room.Name.Contains(roomName.Name = "Toy"))
            {
                Debug.Log("토이방 인원 : " + room.PlayerCount);
                    countText_T.text = room.PlayerCount + " 명";
            }
            else if (room.Name.Contains(roomName.Name = "Western"))
            {
                Debug.Log("웨스턴방 인원 : " + room.PlayerCount);
                    countText_W.text = room.PlayerCount + " 명";
            }*/

    // 로딩 맵 if문
    /* if (DataManager.DM.currentMap == Map.TUTORIAL1)
       {
           PN.LoadLevel(1); // 튜토리얼1
       }
       else if (DataManager.DM.currentMap == Map.TOY)
       {
           PN.LoadLevel(2); // 토이
       }
       else if (DataManager.DM.currentMap == Map.WESTERN)
       {
           PN.LoadLevel(3); // 웨스턴
       }*/
}
