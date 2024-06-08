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

public class LobbyManager : MonoBehaviourPunCallbacks                          // LobbyScene 스크립트
{
    public static LobbyManager LobbyIns;
    [SerializeField] GameObject RedTeam;
    [SerializeField] GameObject BlueTeam;
    public bool isRed = false;

    private string mapType;

    private void Awake()
    {       
        if (LobbyIns == null)
        {
            LobbyIns = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (LobbyIns != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Start()
    {
        PN.AutomaticallySyncScene = true;
        if(!PN.IsConnectedAndReady)
        {
            PN.ConnectUsingSettings();
        }
        else
        {
            PN.JoinLobby();
        }      
    }

    public void RedTeamSelected()
    {        
        isRed = true;
        PN.JoinRoom("LobbyRoom");
    }

    public void BlueTeamSelected()
    {        
        isRed = false;
        PN.JoinRoom("LobbyRoom");
    }
  
    public string RoomName
    {
        get => PlayerPrefs.GetString("CurrentRoomName", "");
        set => PlayerPrefs.SetString("CurrentRoomName", value);
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"해당이름의 방이없어 새로운방을 생성합니다.");
        CreateAndJoinRoom();
    }

    private void CreateAndJoinRoom()
    {
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 3000 }; // 방 옵션      

        PN.CreateRoom("LobbyRoom", options); // 방을 생성
    }
  
    public override void OnCreatedRoom()                                             // 방 생성 완료된 후 호출
    {
        Debug.Log($"{PN.CurrentRoom.Name} 방을 생성하였습니다.");

    }

    public override void OnJoinedRoom()                                              // 방에 들어갔을 때 호출
    {                                                                                                 
        Debug.Log($"{PN.CurrentRoom.Name} 방에 {PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
       
        PN.LoadLevel("LobbyScene_Real");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        newPlayer = PN.LocalPlayer;
        Debug.Log($"{newPlayer.NickName}님 현재인원:{PN.CurrentRoom.PlayerCount}");
    }
}
