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
using static ObjectPooler;
public class ReadySceneManager : MonoBehaviourPunCallbacks                               // StartScene 스크립트
{
    public static ReadySceneManager RSM;                                          // 싱글턴

    [Header("메인 맵")]
    [SerializeField] GameObject mainBG;

    [Header("팀선택 창")]
    [SerializeField] GameObject teamSelectUI;

    [Header("로컬플레이어")]
    [SerializeField] GameObject localPlayer;

    [Header("페이드인 스크린")]
    [SerializeField] GameObject fadeScreen;

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

    #region 유니티 메서드 시작 /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {

        if (RSM != null && RSM != this)
        {
            Destroy(this.gameObject);
        }
        RSM = this;
        // StartToServer();// 게임시작과 동시에 서버연결
    }

    private void Start()
    {
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        StartToServer();
    }
    public void StartToServer()                                                     // 서버연결 메서드
    {
        //PN.ConnectUsingSettings();
        PN.ConnectToMaster(masterAddress, portNum, appID);
        PN.GameVersion = gameVersion;
        PN.AutomaticallySyncScene = true;
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // 겹치지 않는 랜덤한 수 생성

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "번 플레이어";
        }


    }
    #endregion 유니티 메서드 끝 ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    #region UI 컨트롤 메서드 시작 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void LobbyJoin()                   // Start 버튼                           // 서버가 연결된 상태에서 로비진입 메서드
    {
        if (PN.IsConnected)
        {
            PN.JoinLobby();
        }
    }

    public void InitiliazeRoomRedTeam()       // 레드팀 버튼                            // 로비 진입 후 팀선택 패널에서 레드팀선택 메서드
    {
        isRed = true;
        fadeScreen.SetActive(true);
        teamSelectUI.SetActive(false);
        //PN.JoinRoom("LobbyRoom");
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 10, EmptyRoomTtl = 1000 }; // 방 옵션
        PN.JoinOrCreateRoom("LobbyRoom", options, TypedLobbyInfo.Default);
    }
    public void InitiliazeRoomBlueTeam()      // 블루팀 버튼                            // 로비 진입 후 팀선택 패널에서 블루팀선택 메서드
    {
        isRed = false;
        fadeScreen.SetActive(true);
        teamSelectUI.SetActive(false);
        //PN.JoinRoom("LobbyRoom");
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 10, EmptyRoomTtl = 1000 }; // 방 옵션
        PN.JoinOrCreateRoom("LobbyRoom", options, TypedLobbyInfo.Default);
    }



    #endregion UI 컨트롤 메서드 끝 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    #region 포톤 서버 콜백 메서드 시작///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public override void OnConnectedToMaster()                                       // 포톤 서버에 접속되면 호출되는 메서드
    {
        Debug.Log($"{PN.LocalPlayer.NickName} 서버에 접속하였습니다.");
       // Debug.Log("서버상태 : " + PN.NetworkClientState);
        PN.JoinLobby();

    }

    public override void OnJoinedLobby()                                             // 로비에 들어갔을 때 호출되는 메서드
    {
        teamSelectUI.SetActive(true);
        //Debug.Log($"{PN.LocalPlayer.NickName} 로비에 입장하였습니다.");        
        Debug.Log($"{PN.LocalPlayer.NickName}님이 로비에 입장하였습니다.");
    }

    /*public override void OnJoinRoomFailed(short returnCode, string message)          // 방 진입 실패시 호출되는 메서드
    {
        Debug.Log($"해당이름의 방이없어 새로운방을 생성합니다.");
        CreateAndJoinRoom();
    }

    private void CreateAndJoinRoom()                                                  // 방을 생성하고 들어가는 메서드
    {
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 10, EmptyRoomTtl = 1000 }; // 방 옵션      

        PN.CreateRoom("LobbyRoom", options); // 방을 생성
    }*/

    public override void OnCreatedRoom()                                              // 방 생성 완료된 후 호출되는 메서드
    {
       // Debug.Log($"{PN.CurrentRoom.Name} 방을 생성하였습니다.");

    }

    public override void OnJoinedRoom()                                               // 방에 들어갔을 때 호출되는 메서드
    {
        if (PN.InRoom && PN.IsConnectedAndReady)
        {
            if (isRed)
            {
                //SpawnRedPlayer();
                Debug.Log($"{PN.CurrentRoom.Name} 방에 {PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                localPlayer.SetActive(false);
                mainBG.SetActive(true);
                inGame = false;
                PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity);
            }
            else
            {
                //SpawnBluePlayer();
                Debug.Log($"{PN.CurrentRoom.Name} 방에 {PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                localPlayer.SetActive(false);
                mainBG.SetActive(true);
                inGame = false;
                PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);
            }
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName}님 현재인원:{PN.CurrentRoom.PlayerCount}");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName}님 현재인원:{PN.CurrentRoom.PlayerCount}");
    }
    /*public void SpawnRedPlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화 
        }

        //PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity);
        PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity);
        
        *//*PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화         

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}번"); // $ == String.Format() 약자 
        }*//*
    }

    public void SpawnBluePlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화 
        }

        PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);
        //PN.Instantiate(BlueTeam.name, Vector3.zero, Quaternion.identity);
        //GameObject myPlayer = PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);
       *//* 
        PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화         

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}번"); // $ == String.Format() 약자 
        }*//*
    }*/


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

    public GunManager FindGun()
    {
        foreach (GameObject Gun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            if (Gun.GetPhotonView().IsMine) return Gun.GetComponent<GunManager>();
        }
        return null;
    }

    public override void OnLeftRoom()
    {
        Debug.Log("방을 나갔습니다.");

        SceneManager.LoadScene("GunShooting");
        /*if (isRed)
        {
            PN.Destroy(GameObject.FindGameObjectWithTag("RedTeam"));
            //PN.Disconnect();
            SceneManager.LoadScene("GunShooting");
        }
        else
        {
            PN.Destroy(GameObject.FindGameObjectWithTag("BlueTeam"));
            //PN.Disconnect();
            SceneManager.LoadScene("GunShooting");
        }*/
        // PN.Disconnect();

        // Debug.LogError("방을 나갔습니다.");
        //PN.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("서버연결끊김");
        // Debug.Log("건슈팅으로진입");
        //StartCoroutine(DelayLoadGun());           
    }

    /* IEnumerator DelayLoadGun()
     {
         //PN.IsMessageQueueRunning = false;
         yield return new WaitForSeconds(1);
         SceneManager.LoadScene("GunShooting");
         Debug.Log("서버연결끊김");
         Debug.Log("건슈팅으로진입");
     }*/

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

        // PN.LeaveRoom();     
        /*if (PN.IsConnected)
        {            
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
