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

public class StartManager : MonoBehaviourPunCallbacks                                    // StartScene ��ũ��Ʈ
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
        int[] NickNumber = Utils.RandomNumbers(maxCount, n); // ��ġ�� �ʴ� ������ �� ����

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "�� �÷��̾�";
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
        Debug.Log($"{PN.LocalPlayer.NickName} ������ �����Ͽ����ϴ�.");
        Debug.Log("�������� : " + PN.NetworkClientState);
       
    }


    public override void OnJoinedLobby()
    {
        startUI.SetActive(false);
        teamSelectUI.SetActive(true);
        Debug.Log($"{PN.LocalPlayer.NickName} �κ� �����Ͽ����ϴ�.");
        // PN.LoadLevel("LobbyScene");
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
