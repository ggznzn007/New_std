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
            PN.NickName = "Ŭ���̾�Ʈ " + num[i];
        }

        Debug.Log(PN.SendRate);

        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
        }

    }

    public override void OnConnectedToMaster() // ���� ������ ���� �� ȣ��Ǵ� �ݹ��Լ�  
    {
        Debug.Log("������ ���� ����");
        Debug.Log($"PN.InLobby = {PN.InLobby}");
        PN.JoinLobby();
    }

    public override void OnJoinedLobby() // �κ� ���� �� ȣ��Ǵ� �ݹ��Լ�
    {
        Debug.Log($"PN.InLobby = {PN.InLobby}");
        PN.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message) // ���� �� ������ �������� ��� ȣ��Ǵ� �ݹ��Լ�
    {
        Debug.Log($"JoinRandom Failed {returnCode} : {message}");

        // �� �Ӽ� ����
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 7;                 // �� �ִ� �ο�
        roomOptions.IsOpen = true;                  // �� ���� ����
        roomOptions.IsVisible = true;               // �κ񿡼� �� ��Ͽ� ���� ��ų�� ����

        // �� ����
        PN.CreateRoom("GunShooting", roomOptions);

    }

    public override void OnCreatedRoom() // �� ������ �Ϸ�� �Ŀ� ȣ��Ǵ� �ݹ��Լ�
    {
        Debug.Log("�� ���� �Ϸ�");
        Debug.Log($"���� : {PN.CurrentRoom.Name}");
    }

    public override void OnJoinedRoom() // �뿡 ������ �� ȣ��Ǵ� �ݹ��Լ�
    {
        Debug.Log($"InRoom : {PN.InRoom}");
        Debug.Log($"�÷��̾� �� : {PN.CurrentRoom.PlayerCount}");

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
        Debug.Log("������ ������ ���������ϴ�.");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        PN.LeaveRoom();
        Debug.Log("�濡�� ���Խ��ϴ�.");
    }

    /*  public override void OnRoomListUpdate(List<RoomInfo> roomList)
      {
         foreach(var room in roomList)
          {
              Debug.Log($"�� = {room.Name} ({room.PlayerCount} / {room.MaxPlayers})");
          }
      }*/


}
