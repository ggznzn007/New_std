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

public class LobbyManager : MonoBehaviourPunCallbacks                          // LobbyScene ��ũ��Ʈ
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
        Debug.Log($"�ش��̸��� ���̾��� ���ο���� �����մϴ�.");
        CreateAndJoinRoom();
    }

    private void CreateAndJoinRoom()
    {
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 3000 }; // �� �ɼ�      

        PN.CreateRoom("LobbyRoom", options); // ���� ����
    }
  
    public override void OnCreatedRoom()                                             // �� ���� �Ϸ�� �� ȣ��
    {
        Debug.Log($"{PN.CurrentRoom.Name} ���� �����Ͽ����ϴ�.");

    }

    public override void OnJoinedRoom()                                              // �濡 ���� �� ȣ��
    {                                                                                                 
        Debug.Log($"{PN.CurrentRoom.Name} �濡 {PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
       
        PN.LoadLevel("LobbyScene_Real");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        newPlayer = PN.LocalPlayer;
        Debug.Log($"{newPlayer.NickName}�� �����ο�:{PN.CurrentRoom.PlayerCount}");
    }
}
