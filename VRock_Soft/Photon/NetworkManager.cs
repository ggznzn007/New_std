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

    [Header("팀선택 판단")]
    public bool isRed;

    [Header("인게임 판단")]
    public bool inGame;

    [Header("게임시간여부 판단")]
    public bool isTime;

    [Header("리플레이 판단")]
    public bool isRePlay;

    private readonly string gameVersion = "1.0";
    //private readonly string masterAddress = "125.134.36.239";
   // private readonly string appID = "698049ca-edd8-41f6-9c9b-b8561355930a";
   // private readonly int portNum = 5055;
    private readonly int n = 1;
    private readonly int maxCount = 1000;

    private void Awake()
    {
        if (NM != null && NM != this)
        {
            Destroy(this.gameObject);
        }       
        NM = this;    
        
    }
    private void Start()
    {       
        if(PN.IsConnectedAndReady)
        {
           teamSelectUI.SetActive(false);
            if(isRed)
            {
                mapRedUI.SetActive(true);
            }
            else
            {
                mapBlueUI.SetActive(true);
            }
            
        }
    }

    /*public void StartToServer()                                                     // 서버연결 메서드
    {
        PN.ConnectUsingSettings();                                                // 디폴트 연결
        //PN.ConnectToMaster(masterAddress, portNum, appID);                          // 서버주소, 포트넘버, 앱아이디로 서버연결
        PN.GameVersion = gameVersion;                                               // 게임 버전 *중요
        PN.AutomaticallySyncScene = true;                                           // 자동으로 씬 동기화
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // 겹치지 않는 난수 생성

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "번 플레이어";
        }
    }*/

    public void InitRed()       
    {
        isRed = true;      
        
        PN.ConnectUsingSettings();                                                // 디폴트 연결
        //PN.ConnectToMaster(masterAddress, portNum, appID);                          // 서버주소, 포트넘버, 앱아이디로 서버연결
        PN.GameVersion = gameVersion;                                               // 게임 버전 *중요
        PN.AutomaticallySyncScene = true;                                           // 자동으로 씬 동기화
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // 겹치지 않는 난수 생성

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "번 플레이어";
        }
       // PN.JoinLobby();
    }
    public void InitBlue()
    {
        isRed = false;   
        
        PN.ConnectUsingSettings();                                                // 디폴트 연결
        //PN.ConnectToMaster(masterAddress, portNum, appID);                          // 서버주소, 포트넘버, 앱아이디로 서버연결
        PN.GameVersion = gameVersion;                                               // 게임 버전 *중요
        PN.AutomaticallySyncScene = true;                                           // 자동으로 씬 동기화
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // 겹치지 않는 난수 생성

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "번 플레이어";
        }
        //PN.JoinLobby();
    }

    public void InitRoom(int defaultRoomIndex)
    {        
        isTime = false;
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //PN.LoadLevel(roomSettings.sceneNum);
        //SceneManager.LoadScene(roomSettings.sceneNum);
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
    public void InitGun(int defaultRoomIndex)
    {       
        isTime = true;
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //PN.LoadLevel(roomSettings.sceneNum);
        //SceneManager.LoadScene(roomSettings.sceneNum);
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
    public void InitRoomRed(int defaultRoomIndex)
    {
        isRed = true;
        isTime = false;
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //PN.LoadLevel(roomSettings.sceneNum);
        //SceneManager.LoadScene(roomSettings.sceneNum);
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
    public void InitRoomBlue(int defaultRoomIndex)
    {
        isRed = false;
        isTime = false;
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //PN.LoadLevel(roomSettings.sceneNum);
        //SceneManager.LoadScene(roomSettings.sceneNum);
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
    public void InitGunRed(int defaultRoomIndex)
    {
        isRed = true;
        isTime = true;
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //PN.LoadLevel(roomSettings.sceneNum);
        //SceneManager.LoadScene(roomSettings.sceneNum);
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
    public void InitGunBlue(int defaultRoomIndex)
    {
        isRed = false;
        isTime = true;
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //PN.LoadLevel(roomSettings.sceneNum);
        //SceneManager.LoadScene(roomSettings.sceneNum);
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

    public override void OnConnectedToMaster()                                       // 포톤 서버에 접속되면 호출되는 메서드
    {
        
           teamSelectUI.SetActive(false);
           mapSelectUI.SetActive(true);

        if (isRed)
        {
            //isRed = true;
            mapRedUI.SetActive(true);
            teamSelectUI.SetActive(false);
            Debug.Log("레드팀UI 활성");
        }
        else
        {
            //isRed = false;
            mapBlueUI.SetActive(true);
            teamSelectUI.SetActive(false);
            Debug.Log("블루팀UI 활성");
        }



        Debug.Log($"{PN.LocalPlayer.NickName} 서버에 접속하였습니다.");
    }

    /*  public override void OnJoinedLobby()                                             // 로비에 들어갔을 때 호출되는 메서드
      {              
          Debug.Log($"{PN.LocalPlayer.NickName}님이 로비에 입장하였습니다.");
      }
  */
    public override void OnJoinedRoom()
    {
      if(!isTime)
        {
            PN.LoadLevel(1); // 튜토리얼
        }
      else
        {
            PN.LoadLevel(2); // 건슈팅
        }
    }






}
