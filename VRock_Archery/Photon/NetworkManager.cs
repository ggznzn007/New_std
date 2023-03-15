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

    //[Header("유저 닉네임")]
    //public TMP_InputField nick;

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

    private readonly string gameVersion = "1.0";
    //private readonly string masterAddress = "125.134.36.239";
    // private readonly string appID = "698049ca-edd8-41f6-9c9b-b8561355930a";
    // private readonly int portNum = 5055;
    //private readonly int n = 1;
    //private readonly int maxCount = 10;
   
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

        PN.AutomaticallySyncScene = true;
        localPlayer.SetActive(true);
         // 윈도우 프로그램 빌드 시
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            adminPlayer.SetActive(true);
            localPlayer.SetActive(false);
        }
        // 유니티 에디터에서 재생 시
       /* if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            adminPlayer.SetActive(true);
            localPlayer.SetActive(false);
        }*/
    }

    private void Start()
    {
        DataManager.DM.startingNum++;
        if (DataManager.DM.startingNum >= 2)
        {
            connectUI.gameObject.SetActive(false);
              // 윈도우 프로그램 빌드 시
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                ad_ConnectUI.gameObject.SetActive(false);
            }

            // 유니티 에디터에서 재생 시
           /* if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                ad_ConnectUI.gameObject.SetActive(false);
            }*/
        }
    }
    private void Update()
    {
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
        // 윈도우 프로그램 빌드 시
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
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
        }
    }

    public void StartToServer()                                                     // 서버연결 메서드
    {
        PN.ConnectUsingSettings();                                                  // 디폴트 연결
        //PN.ConnectToMaster(masterAddress, portNum, appID);                        // 서버주소, 포트넘버, 앱아이디로 서버연결
        PN.GameVersion = gameVersion;                                               // 게임 버전 *중요
        PN.AutomaticallySyncScene = true;                                           // 자동으로 씬 동기화
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        /*int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // 겹치지 않는 난수 생성

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = "플레이어 "+ NickNumber[i];
            DataManager.DM.nickName= "플레이어 " + NickNumber[i];
        }*/
        /* string str = nick.text;
         PN.LocalPlayer.NickName = str.ToUpper();
         DataManager.DM.nickName = str;*/
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
        //DataManager.DM.nickName = adminName;
    }

    public void InitAdmin(int defaultRoomIndex)                                       // 레드팀 선택
    {
        DataManager.DM.currentTeam = Team.ADMIN;
        DataManager.DM.isSelected = true;

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
        DataManager.DM.teamInt = 1;
        DataManager.DM.isSelected = true;

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
        DataManager.DM.teamInt = 0;
        DataManager.DM.isSelected = true;

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


    /* public void InitToy(int defaultRoomIndex)
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

     }*/

    public override void OnConnectedToMaster()                                       // 포톤 서버에 접속되면 호출되는 메서드
    {
        //mapSelectUI.gameObject.SetActive(true);
        connectUI.gameObject.SetActive(false);

        switch (DataManager.DM.startingNum)
        {
            case 1:
            case 3:
                InitTutoT();
                // 유니티 에디터에서 재생 시
                /* if (Application.platform == RuntimePlatform.WindowsEditor)
                 {
                     ad_ConnectUI.gameObject.SetActive(false);
                     ad_MapUI.gameObject.SetActive(false);
                 }                */

                // 윈도우 프로그램 빌드 시
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    ad_ConnectUI.gameObject.SetActive(false);
                    ad_MapUI.gameObject.SetActive(false);
                }
                break;
            case 2:
            case 4:
                InitTutoW();
                // 유니티 에디터에서 재생 시
                /*    if (Application.platform == RuntimePlatform.WindowsEditor)
                    {
                        ad_ConnectUI.gameObject.SetActive(false);
                        ad_MapUI.gameObject.SetActive(false);
                    }               */
                // 윈도우 프로그램 빌드 시
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    ad_ConnectUI.gameObject.SetActive(false);
                    ad_MapUI.gameObject.SetActive(false);
                }
                break;
        }
        Debug.Log($"{PN.LocalPlayer.NickName}님이 서버에 접속하였습니다.");
    }

    public override void OnJoinedLobby()                                             // 로비에 들어갔을 때 호출되는 메서드
    {
        switch (DataManager.DM.startingNum)
        {
            case 1:
            case 3:
                teamSelectUI_T.gameObject.SetActive(true);
                //mapSelectUI.gameObject.SetActive(false);
                //ad_MapUI.gameObject.SetActive(false);
                // 유니티 에디터에서 재생 시     
                /* if (Application.platform == RuntimePlatform.WindowsEditor)
                 {
                     ad_ToyUI.gameObject.SetActive(true);
                 }*/

                // 윈도우 프로그램 빌드 시                
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    ad_ToyUI.gameObject.SetActive(true);
                }

                if (DataManager.DM.isSelected && DataManager.DM.currentTeam == Team.BLUE)
                {
                    InitBlue(0);
                    teamSelectUI_T.gameObject.SetActive(false);
                }
                else if (DataManager.DM.isSelected && DataManager.DM.currentTeam == Team.RED)
                {
                    InitRed(0);
                    teamSelectUI_T.gameObject.SetActive(false);
                }
                break;
            case 2:
            case 4:
                //mapSelectUI.gameObject.SetActive(false);
                teamSelectUI_W.gameObject.SetActive(true);
                // 유니티 에디터에서 재생 시
                /* if (Application.platform == RuntimePlatform.WindowsEditor)
                 {
                     ad_WesternUI.gameObject.SetActive(false);
                     InitAdmin(2);
                 }*/
                // 윈도우 프로그램 빌드 시
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    ad_WesternUI.gameObject.SetActive(false);
                    InitAdmin(2);

                }

                if (DataManager.DM.isSelected && DataManager.DM.currentTeam == Team.BLUE)
                {
                    InitBlue(2);
                    teamSelectUI_W.gameObject.SetActive(false);
                }
                else if (DataManager.DM.isSelected && DataManager.DM.currentTeam == Team.RED)
                {
                    InitRed(2);
                    teamSelectUI_W.gameObject.SetActive(false);
                }
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
                //teamSelectUI_T.gameObject.SetActive(false);
                // 유니티 에디터에서 재생 시
               /* if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    ad_ToyUI.gameObject.SetActive(false);
                }     */         
                // 윈도우 프로그램 빌드 시
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    ad_ToyUI.gameObject.SetActive(false);
                }
               
                PN.LoadLevel(1); // 튜토리얼T
                break;
            case Map.TOY:
               
                PN.LoadLevel(2); // 토이
                break;
            case Map.TUTORIAL_W:
                teamSelectUI_W.gameObject.SetActive(false);
                // 유니티 에디터에서 재생 시
               /* if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    ad_WesternUI.gameObject.SetActive(false);
                }*/
                 // 윈도우 프로그램 빌드 시
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    ad_WesternUI.gameObject.SetActive(false);
                }
               
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
            countText_TW.text = 0 + " 명";           
        }

        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room.Name);                   
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

}
