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
        ConnectingPhoton();        
        //DontDestroyOnLoad(this);
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
        PN.NickName = userID;
        PN.ConnectUsingSettings();                                                  // ���� ���� ���� �õ�       
        Debug.Log($"������ ���Ƚ�� �ʴ� : {PN.SendRate}");                            // ���� ������ ��� Ƚ�� ����. �ʴ� 30ȸ
        Debug.Log($"�� ����ȭ = {PN.AutomaticallySyncScene}, ���� ���� = {PN.ConnectUsingSettings()}");
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
            PN.JoinOrCreateRoom("LobbyScene", new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 6 }, TypedLobby.Default);
        }
        else // �κ� ���� �� ���� ������ �濡 ��
        {
            PN.JoinRoom(RoomName);
            PN.AutomaticallySyncScene = true; // �� �ڵ� ����ȭ
            Debug.Log("�κ� ���� �Ϸ�");
        }

    }
    public override void OnCreatedRoom()                                             // �� ���� �Ϸ�� �� ȣ��
    {
        Debug.Log($"{PN.CurrentRoom.Name} �� ���� ����!!!");
        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        

    }
    public override void OnJoinedRoom()                                              // �濡 ���� �� ȣ��
    {
        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        RoomName = PN.CurrentRoom.Name;
        //UpdatePlayerCounts();
        Debug.Log($"��ȿ� �ִ��� ���� : {PN.InRoom}");
        Debug.Log("LobbyScene�濡 ���� ����");
        Debug.Log($"������ �÷��̾� �� : {PN.CurrentRoom.PlayerCount}");

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}��"); // $ == String.Format() ���� 
        }

          
        if (PN.IsMasterClient)
        {
            
            switch (PN.CurrentRoom.PlayerCount)
            {
                case 1:
                    PN.NickName = "VRock �����" + nums[0] + "�� Player";
                    break;
                case 2:
                    PN.NickName = "VRock ������" + nums[1] + "�� Player";
                    break;
                case 3:
                    PN.NickName = "VRock �����" + nums[2] + "�� Player";
                    break;
                case 4:
                    PN.NickName = "VRock ������" + nums[3] + "�� Player";
                    break;
                case 5:
                    PN.NickName = "VRock �����" + nums[4] + "�� Player";
                    break;
                case 6:
                    PN.NickName = "VRock ������" + nums[5] + "�� Player";
                    break;
            }
            StartCoroutine(nameof(CreatePlayer));
        }
        else
        {
            switch (PN.CurrentRoom.PlayerCount)
            {
                case 1:
                    PN.NickName = "VRock �����" + nums[0] + "�� Player";
                    break;
                case 2:
                    PN.NickName = "VRock ������" + nums[1] + "�� Player";
                    break;
                case 3:
                    PN.NickName = "VRock �����" + nums[2] + "�� Player";
                    break;
                case 4:
                    PN.NickName = "VRock ������" + nums[3] + "�� Player";
                    break;
                case 5:
                    PN.NickName = "VRock �����" + nums[4] + "�� Player";
                    break;
                case 6:
                    PN.NickName = "VRock ������" + nums[5] + "�� Player";
                    break;
            }
            StartCoroutine(nameof(CreatePlayer));
        }
        


    }

   

    IEnumerator CreatePlayer()
    {
        yield return new WaitUntil(() => PN.IsConnected);
        if (!PN.IsConnected) { PN.ConnectUsingSettings(); }
        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        switch (PN.CurrentRoom.PlayerCount)
            {
                case 1:
                case 3:
                case 5:
                    Transform[] BlueTeamSpots = GameObject.Find("BlueTeamSpots").GetComponentsInChildren<Transform>();
                    int blueSpawnspot = Random.Range(0, BlueTeamSpots.Length);
                    PN.Instantiate("BlueTeamPlayer", BlueTeamSpots[blueSpawnspot].position, BlueTeamSpots[blueSpawnspot].rotation, 0);
                    //GameObject.Find("BlueTeamPlayer(Clone)/Avatar/Body").GetComponent<MeshRenderer>().materials[0].color = Color.blue;
                    Debug.Log($"{PN.NickName} ���������� �����Ϸ�");

                    break;
                case 2:
                case 4:
                case 6:
                    Transform[] RedTeamSpots = GameObject.Find("RedTeamSpots").GetComponentsInChildren<Transform>();
                    int redSpawnspot = Random.Range(0, RedTeamSpots.Length);
                    PN.Instantiate("RedTeamPlayer", RedTeamSpots[redSpawnspot].position, RedTeamSpots[redSpawnspot].rotation, 0);
                    //GameObject.Find("RedTeamPlayer(Clone)/Avatar/Body").GetComponent<MeshRenderer>().materials[0].color = Color.red;
                    Debug.Log($"{PN.NickName} ���������� �����Ϸ�");

                    break;

            }
        
       
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PN.DestroyAll(player);

    }
   
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        /*if (PN.IsMasterClient)
        {
            PN.LoadLevel("GunShooting");
        }*/
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        //UpdatePlayerCounts();
    }
    public override void OnJoinRoomFailed(short returnCode, string message) // �濡 ���� ���н� ȣ��Ǵ� �Լ�
    {
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 6, EmptyRoomTtl = 3000 }; // �� �ɼ�
        Debug.Log("�濡 ������ �����Ͽ� ���� �����մϴ�.");
        PN.CreateRoom("LobbyScene", options); // ���� ����
    }

    public override void OnDisconnected(DisconnectCause cause) => PN.Reconnect();  // ���� ����� ������ �õ�


    public void OnApplicationQuit()
    {
        RoomName = "";
        PN.LeaveRoom();
        PN.Disconnect();
    }

    public void EnterGunShooting()
    {
       if (PN.IsMasterClient)
        {
            PN.CurrentRoom.IsOpen = false;
            PN.CurrentRoom.IsVisible = false;
            PN.LoadLevel("GunShooting");
        }
        Debug.LogFormat("PN:Loading Level :{0}", PN.CurrentRoom.PlayerCount);
        //PN.LoadLevel("GunShooting");
        Destroy(this);
        //PN.LeaveRoom();        
        //SceneManager.LoadScene("GunShooting");
       
    }

   
}
