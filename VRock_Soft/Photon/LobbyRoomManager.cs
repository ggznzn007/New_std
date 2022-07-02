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

public class LobbyRoomManager : MonoBehaviourPunCallbacks
{
    public static LobbyRoomManager instance = null;
    private string mapType;
    public bool master() => PN.LocalPlayer.IsMasterClient;

    /*[SerializeField] GameObject host;
    [SerializeField] GameObject RedTeamPlayer;
    [SerializeField] GameObject BlueTeamPlayer;*/
    [SerializeField] TextMeshProUGUI OccupRateText_GunShooting;
    private string gameVersion = "1.0";
    int[] nums = { 1, 2, 3, 4, 5, 6 };

    #region 유니티 콜백 함수들
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        PhotonInfoUpdate();
    }
    void Start()
    {

    }

    void PhotonInfoUpdate()
    {
        PN.AutomaticallySyncScene = true;  // 구성씬 동기화
        PN.GameVersion = gameVersion;
        PN.ConnectUsingSettings();

        PN.NickName = nums[Random.Range(0,6)] + "번 플레이어";


    }

    private void FixedUpdate()
    {
        /* if(PN.InRoom && master())
         {
             if (Input.GetKeyDown(KeyCode.Space))
             {
                 PN.LoadLevel("GunShooting");
             }
         }*/

    }

    public void OnEnterButtonClicked_Gun_Shooting()
    {

    }

    public void OnEnterButtonClicked_Sling_Shot()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_SLINGSHOT;
        ExitGames.Client.Photon.Hashtable expectCustomRoomProp
            = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
        PN.JoinRandomRoom(expectCustomRoomProp, 0);
    }
    #endregion

    #region 포톤 콜백 함수들
    public override void OnConnectedToMaster()
    {
        PN.JoinLobby(); // 서버로 접속 후 로비로 접속
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료");
        RoomOptions roomOp = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 6 };

        PN.JoinOrCreateRoom("건슈팅게임", roomOp, TypedLobby.Default); // 로비 접속완료 후 방 접속
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom()
    {
        // Transform[] points = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
        //Transform point = GameObject.Find("hostSpot").GetComponent<Transform>();        
        //int rand = Random.Range(0, points.Length);
        if (PN.IsConnectedAndReady)
        {
            Transform[] RedTeamSpots = GameObject.Find("RedTeamSpots").GetComponentsInChildren<Transform>();
            Transform[] BlueTeamSpots = GameObject.Find("BlueTeamSpots").GetComponentsInChildren<Transform>();
            int red = Random.Range(0, RedTeamSpots.Length);
            int Blue = Random.Range(0, BlueTeamSpots.Length);
            int num = Random.Range(1, 7);

            if (num % 2 == 1) // 홀수 레드
            {

                PN.Instantiate("RedTeamPlayer", RedTeamSpots[red].position, RedTeamSpots[red].rotation, 0);
                Debug.Log("레드팀 " + PN.NickName + "가 입장하셨습니다.");
            }
            else  // 짝수 블루
            {

                PN.Instantiate("BlueTeamPlayer", BlueTeamSpots[Blue].position, BlueTeamSpots[Blue].rotation, 0);
                Debug.Log("블루팀 " + PN.NickName + "가 입장하셨습니다.");
            }

        }
        /*else
        {
           Transform[] RedTeamSpots = GameObject.Find("RedTeamSpots").GetComponentsInChildren<Transform>();
           Transform[] BlueTeamSpots = GameObject.Find("BlueTeamSpots").GetComponentsInChildren<Transform>();
           int red = Random.Range(0, RedTeamSpots.Length);
           int Blue = Random.Range(0, BlueTeamSpots.Length);

           for (int i = 0; i < num.Length; i++)
           {
               if (num[i] % 2 == 1) // 홀수 레드
               {
                   PN.Instantiate("RedTeamPlayer", RedTeamSpots[red].position, RedTeamSpots[red].rotation, 0);
                   Debug.Log("레드팀 " + PN.NickName + "가 입장하셨습니다.");
               }
               else  // 짝수 블루
               {
                   PN.Instantiate("BlueTeamPlayer", BlueTeamSpots[Blue].position, BlueTeamSpots[Blue].rotation, 0);
                   Debug.Log("블루팀 " + PN.NickName + "가 입장하셨습니다.");
               }
           }

       }*/

    }

    public override void OnCreatedRoom()
    {

        Debug.Log("방 생성완료!!! 방이름:" + PN.CurrentRoom.Name);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        Debug.Log(newPlayer.NickName + "접속한 플레이어 수: " + PN.CurrentRoom.PlayerCount);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList.Count == 0)
        {
            OccupRateText_GunShooting.text = 0 + " / " + 7;
            //OccupRateText_SlingShot.text = 0 + " / " + 7;
        }

        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
            if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_GUNSHOOTING))
            {
                Debug.Log("현재 접속한 방은 건슈팅, 접속한 플레이어는 " + room.PlayerCount + "명 입니다.");
                OccupRateText_GunShooting.text = room.PlayerCount + " / " + 7;
            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_SLINGSHOT))
            {
                Debug.Log("현재 접속한 방은 스노우, 접속한 플레이어는 " + room.PlayerCount + "명 입니다.");
                // OccupRateText_SlingShot.text = room.PlayerCount + " / " + 7;
            }
        }
    }

    #endregion

    #region Private Methods
    private void CreateAndJoinRoom()
    {
        string randomRoomName = mapType + Random.Range(1, 2) + "번 방";
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6;

        string[] roomPropsInLobby = { MultiplayerVRConstants.MAP_TYPE_KEY };

        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };

        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;

        PN.CreateRoom(randomRoomName, roomOptions);
    }

    #endregion
}
