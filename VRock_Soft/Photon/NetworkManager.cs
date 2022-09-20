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
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Security.Cryptography;

[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneNum;
    public int maxPLayer;    
}

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager NM;


    [Header("�� ���")]
    [SerializeField] List<DefaultRoom> defaultRooms;

    [Header("������ â")]
    [SerializeField] GameObject teamSelectUI;

    [Header("�ʼ��� â")]
    [SerializeField] GameObject mapSelectUI;

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

    private void Awake()
    {
        if (NM != null && NM != this)
        {
            Destroy(this.gameObject);
        }       
        NM = this;        
    }
    private void Start()
    {        
        StartToServer();
    }

    public void StartToServer()                                                     // �������� �޼���
    {
        //PN.ConnectUsingSettings();                                                // ����Ʈ ����
        PN.ConnectToMaster(masterAddress, portNum, appID);                          // �����ּ�, ��Ʈ�ѹ�, �۾��̵�� ��������
        PN.GameVersion = gameVersion;                                               // ���� ���� *�߿�
        PN.AutomaticallySyncScene = true;                                           // �ڵ����� �� ����ȭ
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // ��ġ�� �ʴ� ���� ����

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "�� �÷��̾�";
        }
    }

    public void InitRed()       
    {
        isRed = true;        
        teamSelectUI.SetActive(false);
        mapSelectUI.SetActive(true);
    }
    public void InitBlue()
    {
        isRed = false;
        teamSelectUI.SetActive(false);
        mapSelectUI.SetActive(true);
    }

    public void InitRoom(int defaultRoomIndex)
    {              
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        PN.LoadLevel(roomSettings.sceneNum);
        //SceneManager.LoadScene(roomSettings.sceneNum);
        RoomOptions roomOptions = new RoomOptions
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)roomSettings.maxPLayer,
            PlayerTtl = 60000,
            EmptyRoomTtl = 60000            
        };

        PN.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);        
    }

    public void InitGun(int defaultRoomIndex)
    {
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        PN.LoadLevel(roomSettings.sceneNum);
        //SceneManager.LoadScene(roomSettings.sceneNum);
        Hashtable options = new Hashtable
        {
            { "Time", 180 }
        };
        RoomOptions roomOptions = new RoomOptions
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)roomSettings.maxPLayer,
            PlayerTtl = 60000,
            EmptyRoomTtl = 60000,
            CustomRoomProperties = options
        };
       

        PN.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
    }

    public override void OnConnectedToMaster()                                       // ���� ������ ���ӵǸ� ȣ��Ǵ� �޼���
    {        
        Debug.Log($"{PN.LocalPlayer.NickName} ������ �����Ͽ����ϴ�.");
        PN.JoinLobby();
    }

    public override void OnJoinedLobby()                                             // �κ� ���� �� ȣ��Ǵ� �޼���
    {
        teamSelectUI.SetActive(true);        
        Debug.Log($"{PN.LocalPlayer.NickName}���� �κ� �����Ͽ����ϴ�.");
    }

    
   
   

  


}
