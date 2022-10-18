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
    [SerializeField] RectTransform connectUI;

    [Header("유저 닉네임")]
    [SerializeField] TMP_InputField nick;

    [Header("로컬플레이어")]
    [SerializeField] GameObject localPlayer;

    [Header("맵선택 창")]
    [SerializeField] RectTransform mapSelectUI;     

    [Header("토이 팀선택 창")]
    [SerializeField] RectTransform teamSelectUI_T;

    [Header("웨스턴 팀선택 창")]
    [SerializeField] RectTransform teamSelectUI_W;

    [Header("튜토리얼 & 토이 참가인원")]
    [SerializeField] TextMeshProUGUI countText_TT;   

    [Header("튜토리얼 & 웨스턴 참가인원")]
    [SerializeField] TextMeshProUGUI countText_TW;    

    [Header("페이드인 스크린")]
    [SerializeField] Canvas fadeScreen;

    
    /*[Header("인게임 판단")]
    public bool inGame;  */ 

    private readonly string gameVersion = "1.0";
    //private readonly string masterAddress = "125.134.36.239";
    // private readonly string appID = "698049ca-edd8-41f6-9c9b-b8561355930a";
    // private readonly int portNum = 5055;
    //private readonly int n = 1;
    //private readonly int maxCount = 100;
    public Hashtable team = new Hashtable();

    [Header("관리자옵션")]
    readonly private string adminName = "관리자";
    [SerializeField] GameObject adminPlayer;
    [SerializeField] RectTransform ad_ConnectUI;
    [SerializeField] RectTransform ad_MapUI;
    [SerializeField] RectTransform ad_ToyUI;
    [SerializeField] RectTransform ad_WesternUI;

    private void Awake()
    {
        if (NM != null && NM != this)
        {
            Destroy(this.gameObject);
        }
        NM = this;
        //DontDestroyOnLoad(this);
        PN.AutomaticallySyncScene = true;
        localPlayer.SetActive(true);
/*#if UNITY_EDITOR_WIN
        adminPlayer.SetActive(true);
        localPlayer.SetActive(false);
#endif*/
    }
    private void Start()
    {        
        // DataManager.DM.startingNum++;
    }

    private void Update()
    {
#if UNITY_EDITOR_WIN        
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { StartToServer_Admin(); }             // 관리자        접속
        else if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }           // 종료
        else if (Input.GetKeyDown(KeyCode.T)) { InitTutoT(); }                       // 토이
        else if (Input.GetKeyDown(KeyCode.W)) { InitTutoW(); }                       // 웨스턴
        else if (Input.GetKeyDown(KeyCode.Keypad0)) { InitRed(0); }                  //  토이 레드
        else if (Input.GetKeyDown(KeyCode.Keypad1)) { InitBlue(0); }                 //  토이 블루
        else if (Input.GetKeyDown(KeyCode.Keypad2)) { InitRed(2); }                  //  웨스턴 레드
        else if (Input.GetKeyDown(KeyCode.Keypad3)) { InitBlue(2); }                 //  웨스턴 블루
        else if (Input.GetKeyDown(KeyCode.A)) { InitAdmin(0); }                      // 토이 관리자    입장
        else if (Input.GetKeyDown(KeyCode.S)) { InitAdmin(2); }                      // 웨스턴 관리자   입장
#endif

    }

    public void StartToServer()                                                     // 서버연결 메서드
    {
        PN.ConnectUsingSettings();                                                  // 디폴트 연결
        //PN.ConnectToMaster(masterAddress, portNum, appID);                        // 서버주소, 포트넘버, 앱아이디로 서버연결
        PN.GameVersion = gameVersion;                                               // 게임 버전 *중요
        PN.AutomaticallySyncScene = true;                                           // 자동으로 씬 동기화
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        string str = nick.text;
        PN.LocalPlayer.NickName = str.ToUpper();
        // int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // 겹치지 않는 난수 생성
        /*for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "번 플레이어";
        }*/

    }


    public void StartToServer_Admin()                                                     // 서버연결 메서드
    {
        PN.ConnectUsingSettings();                                                  // 디폴트 연결
        //PN.ConnectToMaster(masterAddress, portNum, appID);                        // 서버주소, 포트넘버, 앱아이디로 서버연결
        PN.GameVersion = gameVersion;                                               // 게임 버전 *중요
        PN.AutomaticallySyncScene = true;                                           // 자동으로 씬 동기화
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        PN.LocalPlayer.NickName = adminName;
        // int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // 겹치지 않는 난수 생성
        /*for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "번 플레이어";
        }*/
        

    }

    public void InitAdmin(int defaultRoomIndex)                                       // 레드팀 선택
    {
        DataManager.DM.currentTeam = Team.ADMIN;

        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        Hashtable options = new Hashtable { { "Time", 240 } };

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

    public void InitTutoT()                                                          // 토이 선택
    {
        DataManager.DM.currentMap = Map.TUTORIAL_T;
        PN.JoinLobby();
    }

    public void InitTutoW()                                                         // 웨스턴 선택
    {
        DataManager.DM.currentMap = Map.TUTORIAL_W;
        PN.JoinLobby();
    }

   

    public void InitRed(int defaultRoomIndex)                                       // 레드팀 선택
    {
        DataManager.DM.currentTeam = Team.RED;

        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        Hashtable options = new Hashtable { { "Time", 240 } };

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
    public void InitBlue(int defaultRoomIndex)                                      // 블루팀 선택
    {
        DataManager.DM.currentTeam = Team.BLUE;

        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        Hashtable options = new Hashtable { { "Time", 240 } };

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
        connectUI.gameObject.SetActive(false);
        mapSelectUI.gameObject.SetActive(true);
/*#if UNITY_EDITOR_WIN
        ad_ConnectUI.gameObject.SetActive(false);
        ad_MapUI.gameObject.SetActive(true);
#endif*/
        Debug.Log($"{PN.LocalPlayer.NickName}님이 서버에 접속하였습니다.");
    }

    public override void OnJoinedLobby()                                             // 로비에 들어갔을 때 호출되는 메서드
    {
        switch (DataManager.DM.currentMap)
        {
            case Map.TUTORIAL_T:
                teamSelectUI_T.gameObject.SetActive(true);
                mapSelectUI.gameObject.SetActive(false);
/*#if UNITY_EDITOR_WIN                
                ad_MapUI.gameObject.SetActive(false);
                ad_ToyUI.gameObject.SetActive(true);
#endif*/
                break;
            case Map.TUTORIAL_W:
                mapSelectUI.gameObject.SetActive(false);
                teamSelectUI_W.gameObject.SetActive(true);
/*#if UNITY_EDITOR_WIN
                ad_WesternUI.gameObject.SetActive(true);
                ad_MapUI.gameObject.SetActive(false);
#endif*/
                break;
            default:
                return;
        }
        Debug.Log($"{PN.LocalPlayer.NickName}님이 로비에 입장하였습니다.");
    }

    public override void OnJoinedRoom()
    {
        switch (DataManager.DM.currentMap)
        {
            case Map.TUTORIAL_T:
            teamSelectUI_T.gameObject.SetActive(false);
/*#if UNITY_EDITOR_WIN
                ad_ToyUI.gameObject.SetActive(false);
#endif*/
                PN.LoadLevel(1); // 튜토리얼T
                break;
            case Map.TOY:
                PN.LoadLevel(2); // 토이
                break;
            case Map.TUTORIAL_W:
            teamSelectUI_W.gameObject.SetActive(false);
/*#if UNITY_EDITOR_WIN
                ad_WesternUI.gameObject.SetActive(false);
#endif*/
                PN.LoadLevel(3); // 튜토리얼W
                break;
            case Map.WESTERN:
                PN.LoadLevel(4); // 웨스턴
                break;
            default:
                return;
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
                default:
                    return;
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

    // 에디터 인풋 switch문
    /* switch(Input.inputString)
         {
             case "Return":
                 { StartToServer_Admin(); }
                 break;
             case "Escape":
                 { Application.Quit(); }
                 break;
             case "T":
                 { InitTutoT(); }
                 break;
             case "W":
                 { InitTutoW(); }
                 break;
             case "Keypad0":
                 { InitRed(0); }
                 break;
             case "Keypad1":
                 { InitBlue(0); }
                 break;
             case "Keypad2":
                 { InitRed(2); }
                 break;
             case "Keypad3":
                 { InitBlue(2); }
                 break;
             case "A":
                 { InitAdmin(0); }
                 break;
             case "S":
                 { InitAdmin(2); }
                 break;
         }*/
}
