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
    private GameObject player;

    //PhotonManager_Ver_2 photonMgr;


    private void Awake()
    {
        SpawnPlayer();
    }
    public void SpawnPlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        }
        //yield return new WaitUntil(() => PN.IsConnected);
        //if (!PN.IsConnected) { PN.ConnectUsingSettings(); }


        //Vector3[] BlueTeamAltSpots = { new Vector3(-2, 0, 2), new Vector3(-2, 0, 0), new Vector3(-2, 0, -2) };
        //Vector3[] RedTeamAltSpots = { new Vector3(2, 0, 2), new Vector3(2, 0, 0), new Vector3(2, 0, -2) };
        //Transform[] BlueTeamSpots = GameObject.Find("BlueTeamSpots").GetComponentsInChildren<Transform>();
        //Transform[] RedTeamSpots = GameObject.Find("RedTeamSpots").GetComponentsInChildren<Transform>();
        if (PN.CountOfPlayersInRooms % 2 == 0 && PN.CountOfPlayersInRooms == 0) // ������� �ο��� ¦���̰ų� 0�� �� 0,2,4 ��������� 1,3,5��° �÷��̾� ����
        {
            //player = PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity,0);
            // int blueSpawnAltspot = Random.Range(0, BlueTeamAltSpots.Length);
            //player = PN.Instantiate("AltBlue", BlueTeamAltSpots[blueSpawnAltspot], Quaternion.identity, 0);            
            player = PN.Instantiate("AltBlue", new Vector3(0,10,0), Quaternion.identity, 0);
            string nickBlue = PN.NickName;
            Debug.Log($"{nickBlue} ����� ���������� �����Ϸ�");
            /*if(PN.ReconnectAndRejoin())
             {
                 player = PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity, 0);
                 string renickBlue = PN.NickName;
                 Debug.Log($"{renickBlue} ����� ���������� �����Ϸ�");
             }*/
            /*int blueSpawnspot = Random.Range(0, BlueTeamSpots.Length);
            player = PN.Instantiate("AltPlayer", BlueTeamSpots[blueSpawnspot].position, BlueTeamSpots[blueSpawnspot].rotation, 0);
            string nickBlue = PN.NickName;
            Debug.Log($"{nickBlue} ����� ���������� �����Ϸ�");*/
        }
        else  // ������� �ο��� Ȧ�� �϶� 1,3,5 == ��������� 2,4,6��° �÷��̾� ����
        {
            //int redSpawnAltspot = Random.Range(0, RedTeamAltSpots.Length);
            //player = PN.Instantiate("AltRed", RedTeamAltSpots[redSpawnAltspot], Quaternion.identity, 0);
            player = PN.Instantiate("AltRed", new Vector3(0, 10, 0), Quaternion.identity, 0);
            string nickRed = PN.NickName;
            Debug.Log($"{nickRed} ������ ���������� �����Ϸ�");
            /* if (PN.ReconnectAndRejoin())
             {
                 player = PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity, 0);
                 string renickRed = PN.NickName;
                 Debug.Log($"{renickRed} ������ ���������� �����Ϸ�");
             }*/
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
    public void SyncScenes()
    {
       // photonMgr.SpawnPlayer();
    }

    /* public static PhotonManager Instance;

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

     */
    /*  public override void OnRoomListUpdate(List<RoomInfo> roomList)
       {
          foreach(var room in roomList)
           {
               Debug.Log($"�� = {room.Name} ({room.PlayerCount} / {room.MaxPlayers})");
           }
       }*/





}
