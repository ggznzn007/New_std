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
using System.Security.Cryptography;

public class ReadySceneManager0 : MonoBehaviourPunCallbacks
{
    public static ReadySceneManager0 RSM0;
    [Header("���̵��� ��ũ��")]
    [SerializeField] GameObject fadeScreen;    

    [Header("�����÷��̾�")]
    [SerializeField] GameObject localPlayer;    

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

    private void Awake()
    {
        if (RSM0 != null && RSM0 != this)
        {
            Destroy(this.gameObject);
        }
        RSM0 = this;       
    }

    private void Start()
    {
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        StartToServer();
    }

    public void StartToServer()                                                     // �������� �޼���
    {
        //PN.ConnectUsingSettings();                                                // ����Ʈ ����
        PN.ConnectToMaster(masterAddress, portNum, appID);                          // �����ּ�, ��Ʈ�ѹ�, �۾��̵�� ��������
        PN.GameVersion = gameVersion;                                               // ���� ���� *�߿�
        PN.AutomaticallySyncScene = true;                                           // �ڵ����� �� ����ȭ
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // ��ġ�� �ʴ� ���� ����

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "�� �÷��̾�";
        }
    }

    public void InitiliazeRedTeam()       // ������ ��ư                            // �κ� ���� �� ������ �гο��� ���������� �޼���
    {
        isRed = true;
        fadeScreen.SetActive(true);
        SceneManager.LoadScene(1);
        //mapSelectUI.SetActive(true);
        //localPlayer.SetActive(false);
        //PN.JoinRoom("LobbyRoom");
        /*RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 10, EmptyRoomTtl = 1000 }; // �� �ɼ�
        PN.JoinOrCreateRoom("LobbyRoom", options, TypedLobbyInfo.Default);*/
    }

    public void InitiliazeBlueTeam()      // ����� ��ư                            // �κ� ���� �� ������ �гο��� ��������� �޼���
    {
        isRed = false;
        fadeScreen.SetActive(true);
        SceneManager.LoadScene(1);
        //mapSelectUI.SetActive(true);
        //localPlayer.SetActive(false);
        //PN.JoinRoom("LobbyRoom");
        /* RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 10, EmptyRoomTtl = 1000 }; // �� �ɼ�
         PN.JoinOrCreateRoom("LobbyRoom", options, TypedLobbyInfo.Default);*/
    }


    #region ���� ���� �ݹ� �޼��� ����//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override void OnConnectedToMaster()                                       // ���� ������ ���ӵǸ� ȣ��Ǵ� �޼���
    {
        Debug.Log($"{PN.LocalPlayer.NickName} ������ �����Ͽ����ϴ�.");        
        PN.JoinLobby();
    }

    public override void OnJoinedLobby()                                             // �κ� ���� �� ȣ��Ǵ� �޼���
    {              
        Debug.Log($"{PN.LocalPlayer.NickName}���� �κ� �����Ͽ����ϴ�.");
    }

    #endregion ���� ���� �ݹ� �޼��� �� ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
