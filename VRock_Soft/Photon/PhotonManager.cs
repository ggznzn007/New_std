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
            PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화 
        }
        //yield return new WaitUntil(() => PN.IsConnected);
        //if (!PN.IsConnected) { PN.ConnectUsingSettings(); }


        //Vector3[] BlueTeamAltSpots = { new Vector3(-2, 0, 2), new Vector3(-2, 0, 0), new Vector3(-2, 0, -2) };
        //Vector3[] RedTeamAltSpots = { new Vector3(2, 0, 2), new Vector3(2, 0, 0), new Vector3(2, 0, -2) };
        //Transform[] BlueTeamSpots = GameObject.Find("BlueTeamSpots").GetComponentsInChildren<Transform>();
        //Transform[] RedTeamSpots = GameObject.Find("RedTeamSpots").GetComponentsInChildren<Transform>();
        if (PN.CountOfPlayersInRooms % 2 == 0 && PN.CountOfPlayersInRooms == 0) // 현재방의 인원이 짝수이거나 0일 때 0,2,4 결과적으로 1,3,5번째 플레이어 생성
        {
            //player = PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity,0);
            // int blueSpawnAltspot = Random.Range(0, BlueTeamAltSpots.Length);
            //player = PN.Instantiate("AltBlue", BlueTeamAltSpots[blueSpawnAltspot], Quaternion.identity, 0);            
            player = PN.Instantiate("AltBlue", new Vector3(0,10,0), Quaternion.identity, 0);
            string nickBlue = PN.NickName;
            Debug.Log($"{nickBlue} 블루팀 정상적으로 생성완료");
            /*if(PN.ReconnectAndRejoin())
             {
                 player = PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity, 0);
                 string renickBlue = PN.NickName;
                 Debug.Log($"{renickBlue} 블루팀 정상적으로 생성완료");
             }*/
            /*int blueSpawnspot = Random.Range(0, BlueTeamSpots.Length);
            player = PN.Instantiate("AltPlayer", BlueTeamSpots[blueSpawnspot].position, BlueTeamSpots[blueSpawnspot].rotation, 0);
            string nickBlue = PN.NickName;
            Debug.Log($"{nickBlue} 블루팀 정상적으로 생성완료");*/
        }
        else  // 현재방의 인원이 홀수 일때 1,3,5 == 결과적으로 2,4,6번째 플레이어 생성
        {
            //int redSpawnAltspot = Random.Range(0, RedTeamAltSpots.Length);
            //player = PN.Instantiate("AltRed", RedTeamAltSpots[redSpawnAltspot], Quaternion.identity, 0);
            player = PN.Instantiate("AltRed", new Vector3(0, 10, 0), Quaternion.identity, 0);
            string nickRed = PN.NickName;
            Debug.Log($"{nickRed} 레드팀 정상적으로 생성완료");
            /* if (PN.ReconnectAndRejoin())
             {
                 player = PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity, 0);
                 string renickRed = PN.NickName;
                 Debug.Log($"{renickRed} 레드팀 정상적으로 생성완료");
             }*/
            /*int redSpawnspot = Random.Range(0, RedTeamSpots.Length);
            player= PN.Instantiate("RedTeamPlayer", RedTeamSpots[redSpawnspot].position, RedTeamSpots[redSpawnspot].rotation, 0);
            string nickRed = PN.NickName;
            Debug.Log($"{nickRed} 레드팀 정상적으로 생성완료");*/
        }

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}번"); // $ == String.Format() 약자 
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
             PN.NickName = "클라이언트 " + num[i];
         }

         Debug.Log(PN.SendRate);

         if (!PN.IsConnected)
         {
             PN.ConnectUsingSettings();
         }

     }

     public override void OnConnectedToMaster() // 포톤 서버에 접속 후 호출되는 콜백함수  
     {
         Debug.Log("서버에 연결 성공");
         Debug.Log($"PN.InLobby = {PN.InLobby}");
         PN.JoinLobby();
     }

     public override void OnJoinedLobby() // 로비에 접속 후 호출되는 콜백함수
     {
         Debug.Log($"PN.InLobby = {PN.InLobby}");
         PN.JoinRandomRoom();
     }

     public override void OnJoinRandomFailed(short returnCode, string message) // 랜덤 룸 입장이 실패했을 경우 호출되는 콜백함수
     {
         Debug.Log($"JoinRandom Failed {returnCode} : {message}");

         // 룸 속성 정의
         RoomOptions roomOptions = new RoomOptions();
         roomOptions.MaxPlayers = 7;                 // 방 최대 인원
         roomOptions.IsOpen = true;                  // 방 오픈 여부
         roomOptions.IsVisible = true;               // 로비에서 방 목록에 노출 시킬지 여부

         // 룸 생성
         PN.CreateRoom("GunShooting", roomOptions);

     }

     public override void OnCreatedRoom() // 룸 생성이 완료된 후에 호출되는 콜백함수
     {
         Debug.Log("방 생성 완료");
         Debug.Log($"방제 : {PN.CurrentRoom.Name}");
     }

     public override void OnJoinedRoom() // 룸에 입장한 후 호출되는 콜백함수
     {
         Debug.Log($"InRoom : {PN.InRoom}");
         Debug.Log($"플레이어 수 : {PN.CurrentRoom.PlayerCount}");

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
         Debug.Log("서버와 연결이 끊어졌습니다.");
     }

     public override void OnPlayerLeftRoom(Player otherPlayer)
     {
         base.OnPlayerLeftRoom(otherPlayer);
         PN.LeaveRoom();
         Debug.Log("방에서 나왔습니다.");
     }

     */
    /*  public override void OnRoomListUpdate(List<RoomInfo> roomList)
       {
          foreach(var room in roomList)
           {
               Debug.Log($"방 = {room.Name} ({room.PlayerCount} / {room.MaxPlayers})");
           }
       }*/





}
