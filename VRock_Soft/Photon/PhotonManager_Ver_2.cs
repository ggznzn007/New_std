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
    private readonly string version = "1.0"; // 게임 버전 입력 == 같은 버전의 유저끼리 접속허용    
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
        PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화 
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
        PN.GameVersion = version;                                                 // 게임버전 설정        
        /*int nums = Random.Range(1, 4);
        PN.NickName = "VRock " + nums + "번 플레이어";*/
        Debug.Log($"서버와 통신횟수 초당 : {PN.SendRate}");                            // 포톤 서버와 통신 횟수 설정. 초당 30회
                                                                              // Debug.Log($"씬 동기화 = {PN.AutomaticallySyncScene}, 서버 연결 = {PN.ConnectUsingSettings()}");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("포톤 마스터 서버 정상적 접속완료");
        Debug.Log($"포톤 로비 = {PN.InLobby}");
        PN.JoinLobby(TypedLobby.Default);                                           // 로비 입장
        PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화 
    }

    public override void OnJoinedLobby()                                            // 로비 접속 후 호출되는 함수
    {
        PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화 
        Debug.Log($"포톤 로비 = {PN.InLobby}");

        if (RoomName == "") // 로비 접속후 방이름이 없으면 새로방을 옵션에따라 생성
        {
            PN.JoinOrCreateRoom("LobbyScene", new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 3000 }, TypedLobby.Default);
        }
        else // 로비 접속 후 방이 있으면 방에 들어감
        {
            PN.JoinRoom(RoomName);
            PN.AutomaticallySyncScene = true; // 씬 자동 동기화
            Debug.Log("로비 접속 완료");
        }

    }
    public override void OnJoinRoomFailed(short returnCode, string message) // 방에 입장 실패시 호출되는 함수
    {
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 3000 }; // 방 옵션
        Debug.Log("방에 입장을 실패하여 방을 생성합니다.");
        PN.CreateRoom("LobbyScene", options); // 방을 생성
    }
    public override void OnCreatedRoom()                                             // 방 생성 완료된 후 호출
    {
        Debug.Log($"{PN.CurrentRoom.Name} 방 생성 성공!!!");
        PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화 
    }

    public override void OnJoinedRoom()                                              // 방에 있을 때 호출
    {
        PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화 
        SpawnPlayer();
        RoomName = PN.CurrentRoom.Name;
        Debug.Log($"방안에 있는지 여부 : {PN.InRoom}");
        Debug.Log("LobbyScene방에 입장 성공");
        Debug.Log($"입장한 플레이어 수 : {PN.CurrentRoom.PlayerCount}");

    }    

    public void SpawnPlayer()
    {
        //yield return new WaitUntil(() => PN.IsConnected);
        //if (!PN.IsConnected) { PN.ConnectUsingSettings(); }
        PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화 

        //Vector3[] BlueTeamAltSpots = { new Vector3(-2, 0, 2), new Vector3(-2, 0, 0), new Vector3(-2, 0, -2) };
        //Vector3[] RedTeamAltSpots = { new Vector3(2, 0, 2), new Vector3(2, 0, 0), new Vector3(2, 0, -2) };
        //Transform[] BlueTeamSpots = GameObject.Find("BlueTeamSpots").GetComponentsInChildren<Transform>();
        //Transform[] RedTeamSpots = GameObject.Find("RedTeamSpots").GetComponentsInChildren<Transform>();
        if (PN.CountOfPlayersInRooms % 2 == 0 && PN.CountOfPlayersInRooms == 0&& PN.ReconnectAndRejoin()) // 현재방의 인원이 짝수이거나 0일 때 0,2,4 결과적으로 1,3,5번째 플레이어 생성
        {
            //player = PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity,0);
            // int blueSpawnAltspot = Random.Range(0, BlueTeamAltSpots.Length);
            //player = PN.Instantiate("AltBlue", BlueTeamAltSpots[blueSpawnAltspot], Quaternion.identity, 0);            
            player = PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity, 0);
            string nickBlue = PN.NickName;
            Debug.Log($"{nickBlue} 블루팀 정상적으로 생성완료");
           
            /*int blueSpawnspot = Random.Range(0, BlueTeamSpots.Length);
            player = PN.Instantiate("AltPlayer", BlueTeamSpots[blueSpawnspot].position, BlueTeamSpots[blueSpawnspot].rotation, 0);
            string nickBlue = PN.NickName;
            Debug.Log($"{nickBlue} 블루팀 정상적으로 생성완료");*/
        }
        else  // 현재방의 인원이 홀수 일때 1,3,5 == 결과적으로 2,4,6번째 플레이어 생성
        {
            //int redSpawnAltspot = Random.Range(0, RedTeamAltSpots.Length);
            //player = PN.Instantiate("AltRed", RedTeamAltSpots[redSpawnAltspot], Quaternion.identity, 0);
            player = PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity, 0);
            string nickRed = PN.NickName;
            Debug.Log($"{nickRed} 레드팀 정상적으로 생성완료");
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
            
            Debug.Log($"방에 입장한 마스터 클라이언트는 {0},{ PN.IsMasterClient}");
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PN.IsMasterClient)
        {
            Debug.Log($"방을 나간 마스터 클라이언트는 {0},{ PN.IsMasterClient}");
        }
        else
        {
            Debug.Log($"방을 나간 클라이언트는 {1},{ !PN.IsMasterClient}");
        }
    }


    public override void OnDisconnected(DisconnectCause cause) => PN.ReconnectAndRejoin();  // 접속 끊기면 재접속 시도


    public void OnApplicationQuit()
    {
        RoomName = "";

        PN.Disconnect();
    }


}
