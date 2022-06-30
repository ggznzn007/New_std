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
public class PhotonManager_Ver_2 : MonoBehaviourPunCallbacks
{
    public static PhotonManager_Ver_2 Singleton;
    private readonly string version = "1.0"; // ���� ���� �Է� == ���� ������ �������� �������    
    private string userID = "VRock";

    private GameObject player;
    int[] nums = { 1, 1, 2, 2, 3, 3 };
    //string photnState;    
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
        PN.GameVersion = version;                                                   // ���ӹ��� ����
        PN.NickName = userID;                                                        // ���� ���� ���� �õ�       
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
        SpawnPlayer();
        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
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
        Transform[] BlueTeamSpots = GameObject.Find("BlueTeamSpots").GetComponentsInChildren<Transform>();
        Transform[] RedTeamSpots = GameObject.Find("RedTeamSpots").GetComponentsInChildren<Transform>();
        PN.NickName = "VRock" + PN.CurrentRoom.PlayerCount + "�� Player";

        int blueSpawnspot = Random.Range(0, BlueTeamSpots.Length);
        PN.Instantiate("BlueTeamPlayer", BlueTeamSpots[blueSpawnspot].position, BlueTeamSpots[blueSpawnspot].rotation, 0);
        string nickBlue = PN.NickName;
        Debug.Log($"{nickBlue} ���������� �����Ϸ�");

        if (PN.CurrentRoom.PlayerCount % 2 == 0 && PN.CurrentRoom.PlayerCount != 0)
        {
            GameObject.Find("BlueTeamPlayer(Clone)/Avatar/Body").GetComponent<MeshRenderer>().materials[0].color = Color.red;
        }

        /*if (PN.CurrentRoom.PlayerCount == 2 )
        {
            PN.NickName = "VRock ������" + PN.CurrentRoom.PlayerCount + "�� Player";
            int redSpawnspot = Random.Range(0, RedTeamSpots.Length);
            PN.Instantiate("RedTeamPlayer", RedTeamSpots[redSpawnspot].position, RedTeamSpots[redSpawnspot].rotation, 0);
            string nickRed = PN.NickName;
            Debug.Log($"{nickRed} ���������� �����Ϸ�");
        }*/
        /*else if (PN.CurrentRoom.PlayerCount == 3)
        {
            PN.NickName = "VRock ������" + nums[2] + "�� Player";
            int blueSpawnspot = Random.Range(0, BlueTeamSpots.Length);
            PN.Instantiate("BlueTeamPlayer", BlueTeamSpots[blueSpawnspot].position, BlueTeamSpots[blueSpawnspot].rotation, 0);
            string nickBlue = PN.NickName;
            Debug.Log($"{nickBlue} ���������� �����Ϸ�");
        }
        else if (PN.CurrentRoom.PlayerCount == 4)
        {
            PN.NickName = "VRock ������" + nums[3] + "�� Player";
            int redSpawnspot = Random.Range(0, RedTeamSpots.Length);
            PN.Instantiate("RedTeamPlayer", RedTeamSpots[redSpawnspot].position, RedTeamSpots[redSpawnspot].rotation, 0);
            string nickRed = PN.NickName;
            Debug.Log($"{nickRed} ���������� �����Ϸ�");
        }
        else if (PN.CurrentRoom.PlayerCount == 5)
        {
            PN.NickName = "VRock ������" + nums[4] + "�� Player";
            int blueSpawnspot = Random.Range(0, BlueTeamSpots.Length);
            PN.Instantiate("BlueTeamPlayer", BlueTeamSpots[blueSpawnspot].position, BlueTeamSpots[blueSpawnspot].rotation, 0);
            string nickBlue = PN.NickName;
            Debug.Log($"{nickBlue} ���������� �����Ϸ�");
        }
        else if (PN.CurrentRoom.PlayerCount == 6)
        {
            PN.NickName = "VRock ������" + nums[5] + "�� Player";
            int redSpawnspot = Random.Range(0, RedTeamSpots.Length);
            PN.Instantiate("RedTeamPlayer", RedTeamSpots[redSpawnspot].position, RedTeamSpots[redSpawnspot].rotation, 0);
            string nickRed = PN.NickName;
            Debug.Log($"{nickRed} ���������� �����Ϸ�");
        }*/

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}��"); // $ == String.Format() ���� 
        }


    }

    public override void OnLeftRoom()
    {
        //SceneManager.LoadScene("LobbyScene");
        PN.LeaveRoom();
        PN.Disconnect();
        PN.DestroyAll(player);

    }

    public void EnterGunShooting(string sceneName)
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

    /*public override void OnPlayerEnteredRoom(Player newPlayer)
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
    }*/


    public override void OnDisconnected(DisconnectCause cause) => PN.ConnectUsingSettings();  // ���� ����� ������ �õ�


    public void OnApplicationQuit()
    {
        RoomName = "";
        PN.LeaveRoom();
        PN.Disconnect();
    }


}
