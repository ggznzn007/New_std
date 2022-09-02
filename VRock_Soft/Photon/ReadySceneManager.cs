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
using Antilatency.DeviceNetwork;

public class ReadySceneManager : MonoBehaviourPunCallbacks                               // StartScene 스크립트
{
    public static ReadySceneManager readySceneManager;                                          // 싱글턴
    public GameObject mainBG;
    public GameObject timerUI;
    public GameObject teamSelectUI;    

    public GameObject localPlayer;
    private GameObject myPlayer; 
   // public GameObject RedTeam;
   // public GameObject BlueTeam;
    public GameObject fadeScreen;

    private readonly string gameVersion = "1.0";
    private readonly string masterAddress = "125.134.36.239";
    private readonly string appID = "698049ca-edd8-41f6-9c9b-b8561355930a";
    private readonly int portNum = 5055;
    private readonly int n = 1;
    private readonly int maxCount = 6;
    public bool isRed = false;

    #region 유니티 메서드 시작 /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        if (readySceneManager != null && readySceneManager != this)
        {
            Destroy(this.gameObject);
        }
        readySceneManager = this;
        StartToServer();// 게임시작과 동시에 서버연결
        
    }
    private void Start()
    {
        //StartToServer();// 게임시작과 동시에 서버연결
        myPlayer = localPlayer;
    }

    
    public void StartToServer()                                                     // 서버연결 메서드
    {
        //PN.IsMessageQueueRunning = false;
        PN.ConnectUsingSettings();
        //PN.ConnectToMaster(masterAddress, portNum, appID);
        PN.GameVersion = gameVersion;
       // PN.AutomaticallySyncScene = true;
       if(!PN.IsConnected)
        {
            PN.Reconnect();
        }
    }
    #endregion 유니티 메서드 끝 ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    #region UI 컨트롤 메서드 시작 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
   /* public void LobbyJoin()                   // Start 버튼                           // 서버가 연결된 상태에서 로비진입 메서드
    {
        if (PN.IsConnected)
        {
            PN.JoinLobby();
        }
    }*/

    public void InitiliazeRoomRedTeam()       // 레드팀 버튼                            // 로비 진입 후 팀선택 패널에서 레드팀선택 메서드
    {        
        isRed = true;
        fadeScreen.SetActive(true);
        teamSelectUI.SetActive(false);
        //PN.JoinRoom("LobbyRoom");
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 1000 }; // 방 옵션
        PN.JoinOrCreateRoom("LobbyRoom", options, TypedLobbyInfo.Default);        
    }
    public void InitiliazeRoomBlueTeam()      // 블루팀 버튼                            // 로비 진입 후 팀선택 패널에서 블루팀선택 메서드
    {       
        isRed = false;
        fadeScreen.SetActive(true);
        teamSelectUI.SetActive(false);
        //PN.JoinRoom("LobbyRoom");
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 1000 }; // 방 옵션
        PN.JoinOrCreateRoom("LobbyRoom", options, TypedLobbyInfo.Default);
    }

    public IEnumerator InRoomAction()
    {
        localPlayer.SetActive(false);
        mainBG.SetActive(true);        
        yield return new WaitForSeconds(5f);
        timerUI.SetActive(true);
       // PN.AutomaticallySyncScene = true;
    }
       


    #endregion UI 컨트롤 메서드 끝 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    #region 포톤 서버 콜백 메서드 시작///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public override void OnConnectedToMaster()                                       // 포톤 서버에 접속되면 호출되는 메서드
    {
        
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // 겹치지 않는 랜덤한 수 생성

        for (int i = 0; i < NickNumber.Length; i++)
        {
            //PN.LocalPlayer.NickName = NickNumber[i] + "번 VRock플레이어";
           PN.NickName = NickNumber[i] + "번 플레이어";
        }
                
        Debug.Log($" {PN.LocalPlayer.NickName}님 서버 접속완료 !!!\n\t서버상태:{ PN.NetworkClientState}");        
        
        teamSelectUI.SetActive(true);
    }

    public override void OnJoinedLobby()                                             // 로비에 들어갔을 때 호출되는 메서드
    {
       
        //Debug.Log($"{PN.LocalPlayer.NickName} 로비에 입장하였습니다.");        
        Debug.Log($"{PN.NickName}님이 로비에 입장하였습니다.");

    }

    public override void OnJoinRoomFailed(short returnCode, string message)          // 방 진입 실패시 호출되는 메서드
    {
        Debug.Log($"해당이름의 방이없어 새로운방을 생성합니다.");
        CreateAndJoinRoom();
    }

    private void CreateAndJoinRoom()                                                  // 방을 생성하고 들어가는 메서드
    {
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 1000 }; // 방 옵션      

        PN.CreateRoom("LobbyRoom", options); // 방을 생성
    }

    public override void OnCreatedRoom()                                              // 방 생성 완료된 후 호출되는 메서드
    {
        Debug.Log($"  {PN.CurrentRoom.Name} 방 생성완료 !!!");

    }

    public override void OnJoinedRoom()                                               // 방에 들어갔을 때 호출되는 메서드
    {        
        if (PN.InRoom && PN.IsConnectedAndReady)
        {
            if (!isRed)
            {
                SpawnBluePlayer();
                StartCoroutine(InRoomAction());
                Debug.Log($" {PN.CurrentRoom.Name} 방에 {PN.NickName}님 입장완료 !!!\n\t  현재접속인원 : {PN.CurrentRoom.PlayerCount}명");
            }
            else
            {
                SpawnRedPlayer();
                StartCoroutine(InRoomAction());
                Debug.Log($" {PN.CurrentRoom.Name} 방에 {PN.NickName}님 입장완료 !!!\n\t  현재접속인원 : {PN.CurrentRoom.PlayerCount}명");
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"  {newPlayer.NickName}님 접속완료 !!!\n\t현재접속인원 : {PN.CurrentRoom.PlayerCount}명");
    }

    public void SpawnRedPlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화 
        }

        //PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity);
        PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity);
       
        PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화         

       /* foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}번"); // $ == String.Format() 약자 
        }*/
    }

    public void SpawnBluePlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화 
        }

        myPlayer = PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);
        PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화         
        //PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);
        //PN.Instantiate(BlueTeam.name, Vector3.zero, Quaternion.identity);
        

        /*foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}번"); // $ == String.Format() 약자 
        }*/
    }


    public string GetNickNameByActorNumber(int actorNumber)   //닉네임 가져오기
    {
        //지금 현재 방에 접속한 사람의 닉네임을 가져온다   -- PlayerListOthers 자기 자신을 뺀 나머지 다 가져옴
        foreach (Player player in PN.PlayerListOthers)
        {
            if (player.ActorNumber == actorNumber)
            {
                return player.NickName;
            }
        }
        return "Ghost";
    }

    /*public GunManager FindGun()
    {
        foreach (GameObject Gun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            if (Gun.GetPhotonView().IsMine) return Gun.GetComponent<GunManager>();
        }
        return null;
    }*/
       
    public override void OnLeftRoom()
    {
        PN.Disconnect();
        //SceneManager.LoadScene("GunShooting");
        // Debug.LogError("방을 나갔습니다.");
        //PN.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        
        SceneManager.LoadScene("GunShooting");
        Debug.Log(" 서버 접속 끊김");        
    }


    private void MigrateMaster()
    {
        var dict = PN.CurrentRoom.Players;
        if (PN.SetMasterClient(dict[dict.Count - 1]))
        {
            PN.LeaveRoom();
        }
    }

    public void OnApplicationQuit()
    {
       
        PN.Disconnect();

        /*if (PN.IsConnected)
        {
            //SceneManager.LoadScene("readyscene1");
            //PN.Destroy(gameObject);
            if (PN.IsMasterClient && PN.CurrentRoom.PlayerCount > 1)
            {
                MigrateMaster();
            }
            else
            {
                PN.LeaveRoom();
            }
        }*/
    }

    #endregion 포톤 서버 콜백 메서드 끝 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
