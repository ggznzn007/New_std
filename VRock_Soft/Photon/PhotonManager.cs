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
public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;

    private readonly string version = "1.0";

    public int[] num = { 1, 2, 3, 4, 5, 6 };
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        Instance = this;

        DontDestroyOnLoad(this);
        PhotonUpdate();
    }

    public void PhotonUpdate()
    {
        PN.AutomaticallySyncScene = true;

        PN.GameVersion = version;

        for (int i = 0; i < num.Length; i++)
        {
            PN.NickName = "클라이언트 " + num[i];
        }

        Debug.Log(PN.SendRate);

        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
        }

    }

    public override void OnConnectedToMaster() // 포톤 서버에 접속 후 호출되는 콜백함수  
    {
        Debug.Log("서버에 연결 성공");
        Debug.Log($"PN.InLobby = {PN.InLobby}");
        PN.JoinLobby();
    }

    public override void OnJoinedLobby() // 로비에 접속 후 호출되는 콜백함수
    {
        Debug.Log($"PN.InLobby = {PN.InLobby}");
        PN.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message) // 랜덤 룸 입장이 실패했을 경우 호출되는 콜백함수
    {
        Debug.Log($"JoinRandom Failed {returnCode} : {message}");

        // 룸 속성 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 7;                 // 방 최대 인원
        roomOptions.IsOpen = true;                  // 방 오픈 여부
        roomOptions.IsVisible = true;               // 로비에서 방 목록에 노출 시킬지 여부

        // 룸 생성
        PN.CreateRoom("GunShooting", roomOptions);

    }

    public override void OnCreatedRoom() // 룸 생성이 완료된 후에 호출되는 콜백함수
    {
        Debug.Log("방 생성 완료");
        Debug.Log($"방제 : {PN.CurrentRoom.Name}");
    }

    public override void OnJoinedRoom() // 룸에 입장한 후 호출되는 콜백함수
    {
        Debug.Log($"InRoom : {PN.InRoom}");
        Debug.Log($"플레이어 수 : {PN.CurrentRoom.PlayerCount}");

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName} , {player.Value.ActorNumber}");
        }
        PN.LoadLevel("LobbyScene");            
    }

    
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        PN.LeaveRoom();
        Debug.Log("서버와 연결이 끊어졌습니다.");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        PN.LeaveRoom();
        Debug.Log("방에서 나왔습니다.");
    }

    /*  public override void OnRoomListUpdate(List<RoomInfo> roomList)
      {
         foreach(var room in roomList)
          {
              Debug.Log($"방 = {room.Name} ({room.PlayerCount} / {room.MaxPlayers})");
          }
      }*/


}
