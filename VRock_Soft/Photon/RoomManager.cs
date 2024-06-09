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


public class RoomManager : MonoBehaviourPunCallbacks
{
   /* public static RoomManager Instance =null;

    private string mapType;

    [SerializeField] TextMeshProUGUI OccupRateText_GunShooting;
    [SerializeField] TextMeshProUGUI OccupRateText_SlingShot;
    [SerializeField] GameObject host;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
    }
    void Start()
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
    #region UI Callback Methods
    public void JoinRandomRoom()
    {
        PN.JoinRandomRoom();
    }
    public void OnEnterButtonClicked_Gun_Shooting()
    {
       *//* mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_GUNSHOOTING;
        ExitGames.Client.Photon.Hashtable expectCustomRoomProp 
            = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY,mapType } };
        PN.JoinRandomRoom(expectCustomRoomProp,0);*//*
    }

    public void OnEnterButtonClicked_Sling_Shot()
    {
       *//* mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_SLINGSHOT;
        ExitGames.Client.Photon.Hashtable expectCustomRoomProp
            = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
        PN.JoinRandomRoom(expectCustomRoomProp, 0);*//*
    }

    #endregion

    #region Photon Callback Methods
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        CreateAndJoinRoom();
    }

    public override void OnConnectedToMaster()
    {
        print("서버에 접속을 다시 시도하는 중.");
        PN.JoinLobby();
    }

    public override void OnCreatedRoom()
    {

        print("방 생성완료!!! 방이름:" + PN.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        print("접속한 플레이어 이름:" + PN.NickName + "\n 방이름 : " + PN.CurrentRoom.Name + "접속한 플레이어 수:" + PN.CurrentRoom.PlayerCount);



        if (PN.CurrentRoom.CustomProperties.ContainsKey(MultiplayerVRConstants.MAP_TYPE_KEY))
        {
            object mapType;
            if (PN.CurrentRoom.CustomProperties.TryGetValue(MultiplayerVRConstants.MAP_TYPE_KEY, out mapType))
            {
               *//* print("접속된 방: " + (string)mapType);
                if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_GUNSHOOTING)
                {
                    PN.LoadLevel("GunShooting");
                }
                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_SLINGSHOT)
                {
                    PN.LoadLevel("SlingShot");
                }*//*
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print(newPlayer.NickName + "접속한 플레이어 수: " + PN.CurrentRoom.PlayerCount);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList.Count == 0)
        {
            OccupRateText_GunShooting.text = 0 + " / " + 7;
            OccupRateText_SlingShot.text = 0 + " / " + 7;
        }

        foreach (RoomInfo room in roomList)
        {
           *//* print(room.Name);
            if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_GUNSHOOTING))
            {
                print("현재 접속한 방은 건슈팅, 접속한 플레이어는 " + room.PlayerCount + "명 입니다.");
                OccupRateText_GunShooting.text = room.PlayerCount + " / " + 7;
            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_SLINGSHOT))
            {
                print("현재 접속한 방은 스노우, 접속한 플레이어는 " + room.PlayerCount + "명 입니다.");
                OccupRateText_SlingShot.text = room.PlayerCount + " / " + 7;
            }*//*
        }
    }

    public override void OnJoinedLobby()
    {
        print("로비에 접속");
    }
    #endregion

    #region Private Methods
    private void CreateAndJoinRoom()
    {
        string randomRoomName = mapType + Random.Range(1, 2) + "번 방";
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 7;

        string[] roomPropsInLobby = { MultiplayerVRConstants.MAP_TYPE_KEY };

        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };

        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;

        PN.CreateRoom(randomRoomName, roomOptions);
    }

    #endregion*/
}
