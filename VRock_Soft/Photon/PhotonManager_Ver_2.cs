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
public class PhotonManager_Ver_2 : MonoBehaviourPunCallbacks
{
    private AltTrackingXR AltXR;
   // private PhotonView PV;
    public static PhotonManager_Ver_2 Singleton;
    private readonly string version = "1.0"; // ���� ���� �Է� == ���� ������ �������� �������    
    //private string userID = "VRock";
    //string photnState;    

    private GameObject player;

    public string RoomName
    {
        get => PlayerPrefs.GetString("CurrentRoomName", "");
        set => PlayerPrefs.SetString("CurrentRoomName", value);
    }
    void Awake()
    {
        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            ConnectingPhoton();
        }
       // DontDestroyOnLoad(this);
    }


    void Update()
    {
        
        /* string currentPhotonState = PN.NetworkClientState.ToString();
         if (photnState != currentPhotonState)
         {
             photnState = currentPhotonState;
             Debug.Log(photnState);
         }*/
    }

    public void ConnectingPhoton()
    {
        PN.GameVersion = version;                                                 // ���ӹ��� ����        
        /*int nums = Random.Range(1, 4);
        PN.NickName = "VRock " + nums + "�� �÷��̾�";*/
        Debug.Log($"������ ���Ƚ�� �ʴ� : {PN.SendRate}");                            // ���� ������ ��� Ƚ�� ����. �ʴ� 30ȸ
                                                                              // Debug.Log($"�� ����ȭ = {PN.AutomaticallySyncScene}, ���� ���� = {PN.ConnectUsingSettings()}");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("���� ������ ���� ������ ���ӿϷ�");
        Debug.Log($"���� �κ� = {PN.InLobby}");
        PN.JoinLobby(TypedLobby.Default);                                           // �κ� ����
        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
    }

    public override void OnJoinedLobby()                                            // �κ� ���� �� ȣ��Ǵ� �Լ�
    {
        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        Debug.Log($"���� �κ� = {PN.InLobby}");

        if (RoomName == "") // �κ� ������ ���̸��� ������ ���ι��� �ɼǿ����� ����
        {
            PN.JoinOrCreateRoom("LobbyScene", new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 3000 }, TypedLobby.Default);
        }
        else // �κ� ���� �� ���� ������ �濡 ��
        {
            PN.JoinRoom(RoomName);
            PN.AutomaticallySyncScene = true; // �� �ڵ� ����ȭ
            Debug.Log("�κ� ���� �Ϸ�");
        }

    }
    public override void OnJoinRoomFailed(short returnCode, string message) // �濡 ���� ���н� ȣ��Ǵ� �Լ�
    {
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 3000 }; // �� �ɼ�
        Debug.Log("�濡 ������ �����Ͽ� ���� �����մϴ�.");
        PN.CreateRoom("LobbyScene", options); // ���� ����
    }
    public override void OnCreatedRoom()                                             // �� ���� �Ϸ�� �� ȣ��
    {
        Debug.Log($"{PN.CurrentRoom.Name} �� ���� ����!!!");
        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
    }

    public override void OnJoinedRoom()                                              // �濡 ���� �� ȣ��
    {
        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        SpawnPlayer();
        RoomName = PN.CurrentRoom.Name;
        Debug.Log($"��ȿ� �ִ��� ���� : {PN.InRoom}");
        Debug.Log("LobbyScene�濡 ���� ����");
        Debug.Log($"������ �÷��̾� �� : {PN.CurrentRoom.PlayerCount}");

    }    

    public void SpawnPlayer()
    {
        //yield return new WaitUntil(() => PN.IsConnected);
        //if (!PN.IsConnected) { PN.ConnectUsingSettings(); }
        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 

        //Vector3[] BlueTeamAltSpots = { new Vector3(-2, 0, 2), new Vector3(-2, 0, 0), new Vector3(-2, 0, -2) };
        //Vector3[] RedTeamAltSpots = { new Vector3(2, 0, 2), new Vector3(2, 0, 0), new Vector3(2, 0, -2) };
        //Transform[] BlueTeamSpots = GameObject.Find("BlueTeamSpots").GetComponentsInChildren<Transform>();
        //Transform[] RedTeamSpots = GameObject.Find("RedTeamSpots").GetComponentsInChildren<Transform>();
        if (PN.CountOfPlayersInRooms % 2 == 0 && PN.CountOfPlayersInRooms == 0&& PN.ReconnectAndRejoin()) // ������� �ο��� ¦���̰ų� 0�� �� 0,2,4 ��������� 1,3,5��° �÷��̾� ����
        {
            //player = PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity,0);
            // int blueSpawnAltspot = Random.Range(0, BlueTeamAltSpots.Length);
            //player = PN.Instantiate("AltBlue", BlueTeamAltSpots[blueSpawnAltspot], Quaternion.identity, 0);            
            player = PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity, 0);
            string nickBlue = PN.NickName;
            Debug.Log($"{nickBlue} ����� ���������� �����Ϸ�");
           
            /*int blueSpawnspot = Random.Range(0, BlueTeamSpots.Length);
            player = PN.Instantiate("AltPlayer", BlueTeamSpots[blueSpawnspot].position, BlueTeamSpots[blueSpawnspot].rotation, 0);
            string nickBlue = PN.NickName;
            Debug.Log($"{nickBlue} ����� ���������� �����Ϸ�");*/
        }
        else  // ������� �ο��� Ȧ�� �϶� 1,3,5 == ��������� 2,4,6��° �÷��̾� ����
        {
            //int redSpawnAltspot = Random.Range(0, RedTeamAltSpots.Length);
            //player = PN.Instantiate("AltRed", RedTeamAltSpots[redSpawnAltspot], Quaternion.identity, 0);
            player = PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity, 0);
            string nickRed = PN.NickName;
            Debug.Log($"{nickRed} ������ ���������� �����Ϸ�");
            /*int redSpawnspot = Random.Range(0, RedTeamSpots.Length);
            player= PN.Instantiate("RedTeamPlayer", RedTeamSpots[redSpawnspot].position, RedTeamSpots[redSpawnspot].rotation, 0);
            string nickRed = PN.NickName;
            Debug.Log($"{nickRed} ������ ���������� �����Ϸ�");*/
        }

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}��"); // $ == String.Format() ���� 
        }


    }

    public override void OnLeftRoom()
    {
        //SceneManager.LoadScene("LobbyScene");

         //PN.Disconnect();
         PN.Destroy(player);
        //SceneManager.LoadScene("GunShooting");

    }

    public void EnterGunShooting(string sceneName)
    {
        //PN.LeaveRoom();
        PN.AutomaticallySyncScene = true;
        if (PN.IsMasterClient)
        {            
            PN.LoadLevel(sceneName);
            
        }
        else
        {
            GameObject.Find("Button_Enter").SetActive(false);
        }

    }

    public void EnterLobbyScene(string sceneName)
    {

        if (PN.IsMasterClient)
        {
            PN.LoadLevel(sceneName);
        }
        else
        {
            GameObject.Find("Button_Enter").SetActive(false);
        }

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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {         
        
        if (PN.IsMasterClient)
        {
            
            Debug.Log($"�濡 ������ ������ Ŭ���̾�Ʈ�� {0},{ PN.IsMasterClient}");
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PN.IsMasterClient)
        {
            Debug.Log($"���� ���� ������ Ŭ���̾�Ʈ�� {0},{ PN.IsMasterClient}");
        }
        else
        {
            Debug.Log($"���� ���� Ŭ���̾�Ʈ�� {1},{ !PN.IsMasterClient}");
        }
    }


    public override void OnDisconnected(DisconnectCause cause) => PN.ReconnectAndRejoin();  // ���� ����� ������ �õ�


    public void OnApplicationQuit()
    {
        RoomName = "";

        PN.Disconnect();
    }


}
