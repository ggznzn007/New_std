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
using Antilatency;
using Antilatency.TrackingAlignment;
using Antilatency.DeviceNetwork;
using Antilatency.Alt;
using Antilatency.SDK;
public class PhotonManager_Ver_2 : MonoBehaviourPunCallbacks  // ����� ���ӸŴ��� ���ҽ�ũ��Ʈ
{
    public static PhotonManager_Ver_2 Singleton;
    private readonly string version = "1.0"; // ���� ���� �Է� == ���� ������ �������� �������    
                                             //private string userID = "VRock";
    string photnState;
    public GameObject teamSelectPanel;
    public GameObject gameStartPanel;
    public GameObject lobbyPlayer;
    private GameObject player;
    public GameObject[] bgObjects;
    
   
    
    private PlayerListing _playerListing;
    private List<PlayerListing> _listings = new List<PlayerListing>();
    //private int nextTeam = 1;
    //private int myTeam = PN.CountOfPlayers;
    //private PhotonView PV;

   
    void Awake()
    {
        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            //ConnectingPhoton();
        }
        
    }


    void Update()
    {

        string currentPhotonState = PN.NetworkClientState.ToString();
        if (photnState != currentPhotonState)
        {
            photnState = currentPhotonState;
            Debug.Log(photnState);
        }
    }

   /* public void ConnectingPhoton()
    {
        PN.GameVersion = version;                                                 // ���ӹ��� ����        
        int nums = Random.Range(1, 4);
        PN.NickName = "VRock " + nums + "�� �÷��̾�";
        Debug.Log($"������ ���Ƚ�� �ʴ� : {PN.SendRate}");                            // ���� ������ ��� Ƚ�� ����. �ʴ� 30ȸ
    }
*/
    public override void OnConnectedToMaster()
    {
        Debug.Log("���� ������ ���� ������ ���ӿϷ�");
        Debug.Log($"���� �κ� = {PN.InLobby}");
        PN.JoinLobby(TypedLobby.Default);                                           // �κ� ����
        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 

        /* if (RoomName == "") // �κ� ������ ���̸��� ������ ���ι��� �ɼǿ����� ����
          {
              PN.JoinOrCreateRoom("LobbyScene", new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 3000 }, TypedLobby.Default);
          }
          else // �κ� ���� �� ���� ������ �濡 ��
          {
              PN.JoinRoom(RoomName);
              PN.AutomaticallySyncScene = true; // �� �ڵ� ����ȭ
              //Debug.Log("�κ� ���� �Ϸ�");
          }*/
 
    }

    public void SelectToBlue()
    {
        //DestroyImmediate(lobbyPlayer);
        lobbyPlayer.SetActive(false);
        teamSelectPanel.SetActive(false);
        gameStartPanel.SetActive(true);

        SpawnBluePlayer();

    }

    public void SelectToRed()
    {
        //DestroyImmediate(lobbyPlayer);
        lobbyPlayer.SetActive(false);
        teamSelectPanel.SetActive(false);
        gameStartPanel.SetActive(true);

        SpawnRedPlayer();

    }

    public string RoomName
    {
        get => PlayerPrefs.GetString("CurrentRoomName", "");
        set => PlayerPrefs.SetString("CurrentRoomName", value);
    }


    
    public override void OnJoinedLobby()                                            // �κ� ���� �� ȣ��Ǵ� �Լ�
    {
        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        Debug.Log($"���� �κ� = {PN.InLobby}");

        if (RoomName == "") // �κ� ������ ���̸��� ������ ���ι��� �ɼǿ����� ����
        {
            PN.JoinOrCreateRoom("Lobby", new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 3000 }, TypedLobby.Default);
        }
        else // �κ� ���� �� ���� ������ �濡 ��
        {
            PN.JoinRoom(RoomName);
            PN.AutomaticallySyncScene = true; // �� �ڵ� ����ȭ

        }

    }

    public override void OnJoinRoomFailed(short returnCode, string message) // �濡 ���� ���н� ȣ��Ǵ� �Լ�
    {
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 3000 }; // �� �ɼ�
        Debug.Log("�濡 ������ �����Ͽ� ���� �����մϴ�.");
        PN.CreateRoom("Lobby", options); // ���� ����
    }
    public override void OnCreatedRoom()                                             // �� ���� �Ϸ�� �� ȣ��
    {
        Debug.Log($"{PN.CurrentRoom.Name} �� ���� ����!!!");
        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
    }

    public override void OnJoinedRoom()                                              // �濡 ���� �� ȣ��
    {
        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ        
        RoomName = PN.CurrentRoom.Name;
        /*int playerNum = Random.Range(0, 10);
        PN.NickName = "VRock " + playerNum + "�� " + PN.CountOfPlayersInRooms + 1.ToString();*/
        Debug.Log($"��ȿ� �ִ��� ���� : {PN.InRoom}");
        Debug.Log("Lobby�濡 ���� ����");
        Debug.Log($"������ �÷��̾� �� : {PN.CurrentRoom.PlayerCount}");

    }

    public void SyncScene()
    {
        PN.AutomaticallySyncScene = true;
    }
    public void SpawnBluePlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        }

        player = PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity, 0);
        string nickAlt = PN.NickName;
        Debug.Log($"{nickAlt} ���������� �����Ϸ�");

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}��"); // $ == String.Format() ���� 
        }
    }
    public void SpawnRedPlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        }

        player = PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity, 0);
        string nickAlt = PN.NickName;
        Debug.Log($"{nickAlt} ���������� �����Ϸ�");

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}��"); // $ == String.Format() ���� 
        }
    }


    public override void OnLeftRoom()
    {
        PN.Destroy(player);
    }

    public void EnterGunShooting()
    {

        bgObjects[0].SetActive(false);
        bgObjects[1].SetActive(true);
        Debug.Log($"{PN.NickName} �ǽ��ù����� �̵��Ϸ�");
        PN.AutomaticallySyncScene = true;        


    }

    public void EnterLobbyScene()
    {

        bgObjects[0].SetActive(true);
        bgObjects[1].SetActive(false);
        Debug.Log($"{PN.NickName} �κ�� �̵��Ϸ�");
        PN.AutomaticallySyncScene = true;        


    }
    public void ChangeMasterClientifAvailble()
    {
        if (!PN.IsMasterClient)
        {
            return;
        }
        if (PN.CurrentRoom.PlayerCount <= 1)
        {
            return;
        }

        PN.SetMasterClient(PN.MasterClient.GetNext());
    }

    /*public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if(info.RemovedFromList)
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if(index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
        }
    }*/

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        /*PlayerListing listing = Instantiate(_playerListing, player.transform);
        if (listing != null)
        {
            listing.SetPlayerInfo(newPlayer);
            _listings.Add(listing);
            Debug.Log($"�濡 ������ Ŭ���̾�Ʈ�� {0},{ PN.NickName}");
        }*/
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PN.LeaveRoom(player);
        /* int index = _listings.FindIndex(x => x.Player == otherPlayer);
         if (index != -1)
         {
             PN.Destroy(_listings[index].gameObject);
             _listings.RemoveAt(index);
             PN.LeaveRoom(player);
             //PN.Destroy(player);
             Debug.Log($"���� ���� Ŭ���̾�Ʈ�� {0},{ PN.NickName}");
         }*/
    }


    public override void OnDisconnected(DisconnectCause cause) => PN.ConnectUsingSettings();  // ���� ����� ������ �õ�


    public void OnApplicationQuit()
    {
        PN.LeaveRoom();

        PN.Destroy(player);
        RoomName = "";
        PN.Disconnect();
        Debug.Log("���� ���� �� ���� ���� ����");
    }


    /*public void UpdateTeam()
    {
        if (myTeam == 1)
        {
            myTeam = 2;
        }
        else
        {
            myTeam = 1;
        }
    }

    [PunRPC]
    void RPC_GetTeam()
    {
        UpdateTeam();
        PV.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);

    }

    [PunRPC]
    void RPC_SentTeam(int whichTeam)
    {
        myTeam = whichTeam;
    }*/
   





}
