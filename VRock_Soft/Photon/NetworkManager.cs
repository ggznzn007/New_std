using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PhotonNetwork;
using Random = UnityEngine.Random;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    /*public Transform spawnPoint;
    public GameObject player;*/
    [SerializeField] Vector3 playerPosition;
    [SerializeField] Quaternion playerRotation;
    [SerializeField] Transform[] spawnPoints;
    private void Start()
     {
         ConnectToServer();
         
    }

    void ConnectToServer() => PN.ConnectUsingSettings(); // 포톤 서버에 연결    

     public override void OnConnectedToMaster() // 포톤 서버에 연결되었을 때 
     {
         base.OnConnectedToMaster();
         print("서버 접속 완료");
         RoomOptions roomOptions = new RoomOptions(); // 룸 할당
         roomOptions.MaxPlayers = 6;                  // 룸 인원 설정 
         roomOptions.IsVisible = true;                // 룸 비주얼 여부
         roomOptions.IsOpen = true;                   // 룸 개방 여부

         PN.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default); // 로비에서 방 생성
         print("방만들고 접속 완료");
        // 방 참가하는데 방이 없으면 생성하고 참가

    }

     public override void OnJoinedRoom() // 방안에 있을 때
     {
        //PN.Instantiate("NormalPlayer", new Vector3(-11.48f,4.03f,36.1f), Quaternion.identity, 0);  
        //PN.Instantiate("NormalPlayer", transform.position, Quaternion.identity,0);  // 플레이어 생성
        CreatePlayer();
     }


    void CreatePlayer()
    {
       
        /*spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();

        Vector3 position = spawnPoints[PN.CurrentRoom.PlayerCount].position;
        Quaternion rotation = spawnPoints[PN.CurrentRoom.PlayerCount].rotation;*/

        GameObject playerClone = PN.Instantiate("NewPlayer", spawnPoints[Random.Range(0,6)].position, Quaternion.identity, 0);
        print("플레이어가 정상적으로 랜덤스폰되었습니다.");
    }

    [ContextMenu("포톤 서버 정보")]
    void Info()
    {
        if(PN.InRoom)
        {
            print("현재 방 이름: " + PN.CurrentRoom.Name);
            print("현재 방 인원 수: " + PN.CurrentRoom.PlayerCount);
            print("현재 방 MAX인원: " + PN.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록";
            for (int i = 0; i < PN.PlayerList.Length; i++)
            {
                playerStr += PN.PlayerList[i].NickName + ",";
                print(playerStr);
            }

        }
        else
        {
            print("접속한 인원 수: " + PN.CountOfPlayers);
            print("로비에 있는 여부: " + PN.InLobby);
            print("접속한 인원 수: " + PN.CountOfPlayers);
            print("서버 연결여부: " + PN.IsConnected);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        
    }

   

    /*  public override void OnPlayerEnteredRoom(Player newPlayer)
      {
          base.OnPlayerEnteredRoom(newPlayer);
      }*/

    /* string _room = "Tutorial";

     // Use this for initialization
     void Start()
     {

         Debug.Log("Network Controller Start");

         PN.ConnectUsingSettings();
     }

   public override void OnJoinedLobby()
     {
         Debug.Log("Joined Lobby");

         PN.JoinRandomRoom();

         //RoomOptions roomOptions = new RoomOptions() {};
         //PhotonNetwork.JoinOrCreateRoom(_room, roomOptions, TypedLobby.Default);
     }


     public override void OnJoinedRoom()
     {
         Debug.Log("Joined Room");

         PN.Instantiate("NormalPlayer", Vector3.one, Quaternion.identity, 0);
     }*/




}
