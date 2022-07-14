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

/*[System.Serializable]
public class DefaultRoom
{
    public string Name = "LobbyRoom";
    public int sceneIndex;
    public int maxPlayer;
}*/

public class StartManager : MonoBehaviourPunCallbacks                                    // StartScene 스크립트
{
    public static StartManager NetWorkMgr;
    public GameObject startUI;
    public GameObject teamSelectUI;
    //public List<DefaultRoom> defaultRooms;
    private readonly string gameVersion = "1.0";
    private readonly string masterAddress = "125.134.36.239";
    private readonly string appID = "698049ca-edd8-41f6-9c9b-b8561355930a";
    private readonly int portNum = 5055;
    private readonly int n = 1;
    private readonly int maxCount = 6;
    public bool isRed = false;
    private void Awake()
    {
        if (NetWorkMgr != null && NetWorkMgr != this)
        {
            Destroy(this.gameObject);
        }
        NetWorkMgr = this;
        StartToServer();
    }
  

    public void LobbyJoin()
    {
        if(PN.IsConnected)
        {
            PN.JoinLobby();
        }
    }


    public void StartToServer()
    {
        PN.ConnectUsingSettings();
        //PN.ConnectToMaster(masterAddress, portNum, appID);
        PN.GameVersion = gameVersion;
        int[] NickNumber = Utils.RandomNumbers(maxCount, n); // 겹치지 않는 랜덤한 수 생성

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "번 플레이어";
        }
       
    }

    public void InitiliazeRoomRedTeam()
    {
        isRed = true;
        PN.JoinRoom("LobbyRoom");        // PN.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobbyInfo.Default);
    }
    public void InitiliazeRoomBlueTeam()
    {
        isRed = false;
        PN.JoinRoom("LobbyRoom");      //PN.LoadLevel(roomSettings.sceneIndex);              
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log($"{PN.LocalPlayer.NickName} 서버에 접속하였습니다.");
        Debug.Log("서버상태 : " + PN.NetworkClientState);
       
    }


    public override void OnJoinedLobby()
    {
        startUI.SetActive(false);
        teamSelectUI.SetActive(true);
        Debug.Log($"{PN.LocalPlayer.NickName} 로비에 입장하였습니다.");
        // PN.LoadLevel("LobbyScene");
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
