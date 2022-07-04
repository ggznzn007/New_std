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

    #region ����Ƽ �ݹ� �Լ���
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
        PN.AutomaticallySyncScene = true;  // ������ ����ȭ
        PN.GameVersion = gameVersion;
        PN.ConnectUsingSettings();

        PN.NickName = nums[Random.Range(0,6)] + "�� �÷��̾�";


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

    #region ���� �ݹ� �Լ���
    public override void OnConnectedToMaster()
    {
        PN.JoinLobby(); // ������ ���� �� �κ�� ����
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� ���� �Ϸ�");
        RoomOptions roomOp = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 6 };

        PN.JoinOrCreateRoom("�ǽ��ð���", roomOp, TypedLobby.Default); // �κ� ���ӿϷ� �� �� ����
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

            if (num % 2 == 1) // Ȧ�� ����
            {

                PN.Instantiate("RedTeamPlayer", RedTeamSpots[red].position, RedTeamSpots[red].rotation, 0);
                Debug.Log("������ " + PN.NickName + "�� �����ϼ̽��ϴ�.");
            }
            else  // ¦�� ���
            {

                PN.Instantiate("BlueTeamPlayer", BlueTeamSpots[Blue].position, BlueTeamSpots[Blue].rotation, 0);
                Debug.Log("����� " + PN.NickName + "�� �����ϼ̽��ϴ�.");
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
               if (num[i] % 2 == 1) // Ȧ�� ����
               {
                   PN.Instantiate("RedTeamPlayer", RedTeamSpots[red].position, RedTeamSpots[red].rotation, 0);
                   Debug.Log("������ " + PN.NickName + "�� �����ϼ̽��ϴ�.");
               }
               else  // ¦�� ���
               {
                   PN.Instantiate("BlueTeamPlayer", BlueTeamSpots[Blue].position, BlueTeamSpots[Blue].rotation, 0);
                   Debug.Log("����� " + PN.NickName + "�� �����ϼ̽��ϴ�.");
               }
           }

       }*/

    }

    public override void OnCreatedRoom()
    {

        Debug.Log("�� �����Ϸ�!!! ���̸�:" + PN.CurrentRoom.Name);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        Debug.Log(newPlayer.NickName + "������ �÷��̾� ��: " + PN.CurrentRoom.PlayerCount);
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
                Debug.Log("���� ������ ���� �ǽ���, ������ �÷��̾�� " + room.PlayerCount + "�� �Դϴ�.");
                OccupRateText_GunShooting.text = room.PlayerCount + " / " + 7;
            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_SLINGSHOT))
            {
                Debug.Log("���� ������ ���� �����, ������ �÷��̾�� " + room.PlayerCount + "�� �Դϴ�.");
                // OccupRateText_SlingShot.text = room.PlayerCount + " / " + 7;
            }
        }
    }

    #endregion

    #region Private Methods
    private void CreateAndJoinRoom()
    {
        string randomRoomName = mapType + Random.Range(1, 2) + "�� ��";
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
