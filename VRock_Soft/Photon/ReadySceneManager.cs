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

public class ReadySceneManager : MonoBehaviourPunCallbacks                               // StartScene 스크립트
{
    public static ReadySceneManager readySceneManager;                                          // 싱글턴
    public GameObject mainBG;
    public GameObject startUI;
    public GameObject teamSelectUI;

    public GameObject localPlayer;
    public GameObject RedTeam;
    public GameObject BlueTeam;
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
        if (readySceneManager != null && readySceneManager != this)
        {
            Destroy(this.gameObject);
        }
        readySceneManager = this;
        StartToServer();                                                            // 게임시작과 동시에 서버연결
    }
    public void StartToServer()                                                     // 서버연결 메서드
    {
        //PN.ConnectUsingSettings();
        PN.ConnectToMaster(masterAddress, portNum, appID);
        PN.GameVersion = gameVersion;
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // 겹치지 않는 랜덤한 수 생성

        for (int i = 0; i < NickNumber.Length; i++)
        {
            //PN.LocalPlayer.NickName = NickNumber[i] + "번 VRock플레이어";
            PN.NickName = NickNumber[i] + "번 VRock플레이어";
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
        PN.JoinRoom("LobbyRoom");
        //RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 1000 }; // 방 옵션
        //PN.JoinOrCreateRoom("LobbyRoom", options, TypedLobbyInfo.Default);        
    }
    public void InitiliazeRoomBlueTeam()      // 블루팀 버튼                            // 로비 진입 후 팀선택 패널에서 블루팀선택 메서드
    {
        isRed = false;
        fadeScreen.SetActive(true);
        teamSelectUI.SetActive(false);
        PN.JoinRoom("LobbyRoom");
        //RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 1000 }; // 방 옵션
        // PN.JoinOrCreateRoom("LobbyRoom", options, TypedLobbyInfo.Default);
    }

    public void EnterGunShooting()
    {
        /* if (PN.InRoom)
         {
             if (PN.IsMasterClient && PN.CurrentRoom.PlayerCount > 1)
             {
                 MigrateMaster();
             }
             else
             {
                 //PN.Destroy(RedTeam);                
                 // PN.Destroy(BlueTeam);                
                 PN.LeaveRoom();
             }
         }*/
        if(PN.InRoom)
        {
            if (PN.IsMasterClient)
            {
                PN.LoadLevel("GunShooting");                
                PN.AutomaticallySyncScene = true;
            }
        }
        
       
    }


    #endregion UI 컨트롤 메서드 끝 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    #region 포톤 서버 콜백 메서드 시작///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public override void OnConnectedToMaster()                                       // 포톤 서버에 접속되면 호출되는 메서드
    {
        //Debug.Log($"{PN.LocalPlayer.NickName} 서버에 접속하였습니다.");
        Debug.Log($"{PN.NickName} 서버에 접속하였습니다.");
        Debug.Log("서버상태 : " + PN.NetworkClientState);
        PN.JoinLobby();
    }

    public override void OnJoinedLobby()                                             // 로비에 들어갔을 때 호출되는 메서드
    {
        teamSelectUI.SetActive(true);
        //Debug.Log($"{PN.LocalPlayer.NickName} 로비에 입장하였습니다.");        
        Debug.Log($"{PN.NickName} 로비에 입장하였습니다.");

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
        Debug.Log($"{PN.CurrentRoom.Name} 방을 생성하였습니다.");

    }

    public override void OnJoinedRoom()                                               // 방에 들어갔을 때 호출되는 메서드
    {
        Debug.Log($"{PN.CurrentRoom.Name} 방에 {PN.NickName} 님이 입장하셨습니다.");
        localPlayer.SetActive(false);
        mainBG.SetActive(true);
        startUI.SetActive(true);
        if (PN.InRoom && PN.IsConnectedAndReady)
        {
            if (isRed)
            {
                SpawnRedPlayer();
            }
            else
            {
                SpawnBluePlayer();
            }
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName}님 현재인원:{PN.CurrentRoom.PlayerCount}");
    }

    public void SpawnRedPlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화 
        }

        //PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity);
        GameObject myPlayer = PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity);

        PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화         

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}번"); // $ == String.Format() 약자 
        }
    }

    public void SpawnBluePlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화 
        }

        //PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);
        GameObject myPlayer = PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);
        
        PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화         

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}번"); // $ == String.Format() 약자 
        }
    }

    public GunManager FindGun()
    {
        foreach (GameObject gun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            if (gun.GetPhotonView().IsMine) return gun.GetComponent<GunManager>();
        }
        return null;
    }

    void OnPhotonInstaniate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this;
    }
    public override void OnLeftRoom()
    {

        //Debug.LogError("방을 나갔습니다.");

        SceneManager.LoadScene("GunShooting");
        // Debug.LogError("방을 나갔습니다.");

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // PN.LoadLevel("StartScene");
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
        if (PN.InRoom)
        {
            if (PN.IsMasterClient && PN.CurrentRoom.PlayerCount > 1)
            {
                MigrateMaster();
            }
            else
            {
                //PN.Destroy(RedTeam);                
                // PN.Destroy(BlueTeam);                
                PN.LeaveRoom();
            }
        }
    }

    #endregion 포톤 서버 콜백 메서드 끝 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
