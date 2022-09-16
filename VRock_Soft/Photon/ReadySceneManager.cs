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
using static ObjectPooler;
public class ReadySceneManager : MonoBehaviourPunCallbacks                               // StartScene ��ũ��Ʈ
{
    public static ReadySceneManager RSM;                                          // �̱���

    [Header("���� ��")]
    [SerializeField] GameObject mainBG;

    [Header("������ â")]
    [SerializeField] GameObject teamSelectUI;

    [Header("�����÷��̾�")]
    [SerializeField] GameObject localPlayer;

    [Header("���̵��� ��ũ��")]
    [SerializeField] GameObject fadeScreen;

    [Header("������ �Ǵ�")]
    public bool isRed = false;

    [Header("�ΰ��� �Ǵ�")]
    public bool inGame;

    private readonly string gameVersion = "1.0";
    private readonly string masterAddress = "125.134.36.239";
    private readonly string appID = "698049ca-edd8-41f6-9c9b-b8561355930a";
    private readonly int portNum = 5055;
    private readonly int n = 1;
    private readonly int maxCount = 6;

    #region ����Ƽ �޼��� ���� /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {

        if (RSM != null && RSM != this)
        {
            Destroy(this.gameObject);
        }
        RSM = this;
        // StartToServer();// ���ӽ��۰� ���ÿ� ��������
    }

    private void Start()
    {
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        StartToServer();
    }
    public void StartToServer()                                                     // �������� �޼���
    {
        //PN.ConnectUsingSettings();
        PN.ConnectToMaster(masterAddress, portNum, appID);
        PN.GameVersion = gameVersion;
        PN.AutomaticallySyncScene = true;
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // ��ġ�� �ʴ� ������ �� ����

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "�� �÷��̾�";
        }


    }
    #endregion ����Ƽ �޼��� �� ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    #region UI ��Ʈ�� �޼��� ���� //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void LobbyJoin()                   // Start ��ư                           // ������ ����� ���¿��� �κ����� �޼���
    {
        if (PN.IsConnected)
        {
            PN.JoinLobby();
        }
    }

    public void InitiliazeRoomRedTeam()       // ������ ��ư                            // �κ� ���� �� ������ �гο��� ���������� �޼���
    {
        isRed = true;
        fadeScreen.SetActive(true);
        teamSelectUI.SetActive(false);
        //PN.JoinRoom("LobbyRoom");
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 10, EmptyRoomTtl = 1000 }; // �� �ɼ�
        PN.JoinOrCreateRoom("LobbyRoom", options, TypedLobbyInfo.Default);
    }
    public void InitiliazeRoomBlueTeam()      // ����� ��ư                            // �κ� ���� �� ������ �гο��� ��������� �޼���
    {
        isRed = false;
        fadeScreen.SetActive(true);
        teamSelectUI.SetActive(false);
        //PN.JoinRoom("LobbyRoom");
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 10, EmptyRoomTtl = 1000 }; // �� �ɼ�
        PN.JoinOrCreateRoom("LobbyRoom", options, TypedLobbyInfo.Default);
    }



    #endregion UI ��Ʈ�� �޼��� �� //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    #region ���� ���� �ݹ� �޼��� ����///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public override void OnConnectedToMaster()                                       // ���� ������ ���ӵǸ� ȣ��Ǵ� �޼���
    {
        Debug.Log($"{PN.LocalPlayer.NickName} ������ �����Ͽ����ϴ�.");
       // Debug.Log("�������� : " + PN.NetworkClientState);
        PN.JoinLobby();

    }

    public override void OnJoinedLobby()                                             // �κ� ���� �� ȣ��Ǵ� �޼���
    {
        teamSelectUI.SetActive(true);
        //Debug.Log($"{PN.LocalPlayer.NickName} �κ� �����Ͽ����ϴ�.");        
        Debug.Log($"{PN.LocalPlayer.NickName}���� �κ� �����Ͽ����ϴ�.");
    }

    /*public override void OnJoinRoomFailed(short returnCode, string message)          // �� ���� ���н� ȣ��Ǵ� �޼���
    {
        Debug.Log($"�ش��̸��� ���̾��� ���ο���� �����մϴ�.");
        CreateAndJoinRoom();
    }

    private void CreateAndJoinRoom()                                                  // ���� �����ϰ� ���� �޼���
    {
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 10, EmptyRoomTtl = 1000 }; // �� �ɼ�      

        PN.CreateRoom("LobbyRoom", options); // ���� ����
    }*/

    public override void OnCreatedRoom()                                              // �� ���� �Ϸ�� �� ȣ��Ǵ� �޼���
    {
       // Debug.Log($"{PN.CurrentRoom.Name} ���� �����Ͽ����ϴ�.");

    }

    public override void OnJoinedRoom()                                               // �濡 ���� �� ȣ��Ǵ� �޼���
    {
        if (PN.InRoom && PN.IsConnectedAndReady)
        {
            if (isRed)
            {
                //SpawnRedPlayer();
                Debug.Log($"{PN.CurrentRoom.Name} �濡 {PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
                localPlayer.SetActive(false);
                mainBG.SetActive(true);
                inGame = false;
                PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity);
            }
            else
            {
                //SpawnBluePlayer();
                Debug.Log($"{PN.CurrentRoom.Name} �濡 {PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
                localPlayer.SetActive(false);
                mainBG.SetActive(true);
                inGame = false;
                PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);
            }
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName}�� �����ο�:{PN.CurrentRoom.PlayerCount}");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName}�� �����ο�:{PN.CurrentRoom.PlayerCount}");
    }
    /*public void SpawnRedPlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        }

        //PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity);
        PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity);
        
        *//*PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ         

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}��"); // $ == String.Format() ���� 
        }*//*
    }

    public void SpawnBluePlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        }

        PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);
        //PN.Instantiate(BlueTeam.name, Vector3.zero, Quaternion.identity);
        //GameObject myPlayer = PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);
       *//* 
        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ         

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}��"); // $ == String.Format() ���� 
        }*//*
    }*/


    public string GetNickNameByActorNumber(int actorNumber)   //�г��� ��������
    {
        //���� ���� �濡 ������ ����� �г����� �����´�   -- PlayerListOthers �ڱ� �ڽ��� �� ������ �� ������
        foreach (Player player in PN.PlayerListOthers)
        {
            if (player.ActorNumber == actorNumber)
            {
                return player.NickName;
            }
        }
        return "Ghost";
    }

    public GunManager FindGun()
    {
        foreach (GameObject Gun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            if (Gun.GetPhotonView().IsMine) return Gun.GetComponent<GunManager>();
        }
        return null;
    }

    public override void OnLeftRoom()
    {
        Debug.Log("���� �������ϴ�.");

        SceneManager.LoadScene("GunShooting");
        /*if (isRed)
        {
            PN.Destroy(GameObject.FindGameObjectWithTag("RedTeam"));
            //PN.Disconnect();
            SceneManager.LoadScene("GunShooting");
        }
        else
        {
            PN.Destroy(GameObject.FindGameObjectWithTag("BlueTeam"));
            //PN.Disconnect();
            SceneManager.LoadScene("GunShooting");
        }*/
        // PN.Disconnect();

        // Debug.LogError("���� �������ϴ�.");
        //PN.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("�����������");
        // Debug.Log("�ǽ�����������");
        //StartCoroutine(DelayLoadGun());           
    }

    /* IEnumerator DelayLoadGun()
     {
         //PN.IsMessageQueueRunning = false;
         yield return new WaitForSeconds(1);
         SceneManager.LoadScene("GunShooting");
         Debug.Log("�����������");
         Debug.Log("�ǽ�����������");
     }*/

    private void MigrateMaster()
    {
        var dict = PN.CurrentRoom.Players;
        if (PN.SetMasterClient(dict[dict.Count - 1]))
        {
            PN.LeaveRoom();
        }
    }

    public void OnApplicationQuit()
    {
        PN.Disconnect();

        // PN.LeaveRoom();     
        /*if (PN.IsConnected)
        {            
            if (PN.IsMasterClient && PN.CurrentRoom.PlayerCount > 1)
            {
                MigrateMaster();
            }
            else
            {
                PN.LeaveRoom();
            }
        }*/
    }

    #endregion ���� ���� �ݹ� �޼��� �� ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
