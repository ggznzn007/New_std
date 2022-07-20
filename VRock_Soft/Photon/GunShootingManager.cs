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

public class GunShootingManager : MonoBehaviourPunCallbacks                               // StartScene ��ũ��Ʈ
{
    public static GunShootingManager gunShootingManager;                                          // �̱���
    
    
    public GameObject teamUI;
    public GameObject gameUI;
    public GameObject localPlayer;
    public GameObject fadeScreen;
    public GameObject RedTeam;
    public GameObject BlueTeam;

    //private GameObject ownPlayer;


    private readonly string gameVersion = "1.0";
    private readonly string masterAddress = "125.134.36.239";
    private readonly string appID = "698049ca-edd8-41f6-9c9b-b8561355930a";
    private readonly int portNum = 5055;
    private readonly int n = 1;
    private readonly int maxCount = 6;
    public bool isRed = false;

    #region ����Ƽ �޼��� ���� /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        if (gunShootingManager != null && gunShootingManager != this)
        {
            Destroy(this.gameObject);
        }
        gunShootingManager = this;
        StartToServer();                                                            // ���ӽ��۰� ���ÿ� ��������
    }
    public void StartToServer()                                                     // �������� �޼���
    {
        //PN.ConnectUsingSettings();
        PN.ConnectToMaster(masterAddress, portNum, appID);
        PN.GameVersion = gameVersion;
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // ��ġ�� �ʴ� ������ �� ����

        for (int i = 0; i < NickNumber.Length; i++)
        {
            //PN.LocalPlayer.NickName = NickNumber[i] + "�� VRock�÷��̾�";
            PN.NickName = NickNumber[i] + "�� VRock�÷��̾�";
        }

    }
    #endregion ����Ƽ �޼��� �� ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    #region UI ��Ʈ�� �޼��� ���� //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void InitiliazeRoomRedTeam()       // ������ ��ư                            // �κ� ���� �� ������ �гο��� ���������� �޼���
    {
        isRed = true;
        fadeScreen.SetActive(true);        
        PN.JoinRoom("LobbyRoom");
        //RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 1000 }; // �� �ɼ�
        //PN.JoinOrCreateRoom("LobbyRoom", options, TypedLobbyInfo.Default);        
    }
    public void InitiliazeRoomBlueTeam()      // ����� ��ư                            // �κ� ���� �� ������ �гο��� ��������� �޼���
    {
        isRed = false;
        fadeScreen.SetActive(true);
        PN.JoinRoom("LobbyRoom");
        //RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 1000 }; // �� �ɼ�
        // PN.JoinOrCreateRoom("LobbyRoom", options, TypedLobbyInfo.Default);
    }

    public void GoToReadyScene2()
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
   

    #endregion UI ��Ʈ�� �޼��� �� //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    #region ���� ���� �ݹ� �޼��� ����///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public override void OnConnectedToMaster()                                       // ���� ������ ���ӵǸ� ȣ��Ǵ� �޼���
    {
        //Debug.Log($"{PN.LocalPlayer.NickName} ������ �����Ͽ����ϴ�.");
        Debug.Log($"{PN.NickName} ������ �����Ͽ����ϴ�.");
        Debug.Log("�������� : " + PN.NetworkClientState);
        PN.JoinLobby();
    }

    public override void OnJoinedLobby()                                             // �κ� ���� �� ȣ��Ǵ� �޼���
    {
       
        Debug.Log($"{PN.NickName} �κ� �����Ͽ����ϴ�.");
        teamUI.SetActive(true);

    }

    public override void OnJoinRoomFailed(short returnCode, string message)          // �� ���� ���н� ȣ��Ǵ� �޼���
    {
        Debug.Log($"�ش��̸��� ���̾��� ���ο���� �����մϴ�.");
        CreateAndJoinRoom();
    }

    private void CreateAndJoinRoom()                                                  // ���� �����ϰ� ���� �޼���
    {
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 1000 }; // �� �ɼ�      

        PN.CreateRoom("LobbyRoom", options); // ���� ����
    }

    public override void OnCreatedRoom()                                              // �� ���� �Ϸ�� �� ȣ��Ǵ� �޼���
    {
        Debug.Log($"{PN.CurrentRoom.Name} ���� �����Ͽ����ϴ�.");

    }

    public override void OnJoinedRoom()                                               // �濡 ���� �� ȣ��Ǵ� �޼���
    {
        Debug.Log($"{PN.CurrentRoom.Name} �濡 {PN.NickName} ���� �����ϼ̽��ϴ�.");
        teamUI.SetActive(false);
        gameUI.SetActive(true);
        localPlayer.SetActive(false);

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

    public void SpawnRedPlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        }

        PN.Instantiate(RedTeam.name, Vector3.zero, Quaternion.identity);

        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ         

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}��"); // $ == String.Format() ���� 
        }
    }

    public void SpawnBluePlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        }

        PN.Instantiate(BlueTeam.name, Vector3.zero, Quaternion.identity);

        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ         

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}��"); // $ == String.Format() ���� 
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName}�� �����ο�:{PN.CurrentRoom.PlayerCount}");
    }


    public override void OnLeftRoom()
    {
        // PN.LoadLevel("StartScene2");

        SceneManager.LoadScene("readyscene_2");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PN.LoadLevel("GunShooting");
    }
    

    private void MigrateMaster()
    {
        var dict = PN.CurrentRoom.Players;
        if (PN.SetMasterClient(dict[dict.Count - 1]))
        {

            PN.LeaveRoom();
        }
    }
#endregion ���� ���� �ݹ� �޼��� �� ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
