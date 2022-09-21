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

    [Header("�ʼ��� â")]
    [SerializeField] GameObject mapRedUI;   
    
    [Header("�ʼ��� â")]
    [SerializeField] GameObject mapBlueUI;    

    [Header("�����÷��̾�")]
    [SerializeField] GameObject localPlayer;

    [Header("���̵��� ��ũ��")]
    [SerializeField] GameObject fadeScreen;

    [Header("������ �Ǵ�")]
    public bool isRed;

    [Header("�ΰ��� �Ǵ�")]
    public bool inGame;

    [Header("���ӽð����� �Ǵ�")]
    public bool isTime;

    [Header("���÷��� �Ǵ�")]
    public bool isRePlay;

    private readonly string gameVersion = "1.0";
    //private readonly string masterAddress = "125.134.36.239";
   // private readonly string appID = "698049ca-edd8-41f6-9c9b-b8561355930a";
   // private readonly int portNum = 5055;
    private readonly int n = 1;
    private readonly int maxCount = 1000;

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
        if(PN.IsConnectedAndReady)
        {
           teamSelectUI.SetActive(false);
            if(isRed)
            {
                mapRedUI.SetActive(true);
            }
            else
            {
                mapBlueUI.SetActive(true);
            }
            
        }
    }

    /*public void StartToServer()                                                     // �������� �޼���
    {
        PN.ConnectUsingSettings();                                                // ����Ʈ ����
        //PN.ConnectToMaster(masterAddress, portNum, appID);                          // �����ּ�, ��Ʈ�ѹ�, �۾��̵�� ��������
        PN.GameVersion = gameVersion;                                               // ���� ���� *�߿�
        PN.AutomaticallySyncScene = true;                                           // �ڵ����� �� ����ȭ
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // ��ġ�� �ʴ� ���� ����

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "�� �÷��̾�";
        }
    }*/

    public void InitRed()       
    {
        isRed = true;      
        
        PN.ConnectUsingSettings();                                                // ����Ʈ ����
        //PN.ConnectToMaster(masterAddress, portNum, appID);                          // �����ּ�, ��Ʈ�ѹ�, �۾��̵�� ��������
        PN.GameVersion = gameVersion;                                               // ���� ���� *�߿�
        PN.AutomaticallySyncScene = true;                                           // �ڵ����� �� ����ȭ
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // ��ġ�� �ʴ� ���� ����

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "�� �÷��̾�";
        }
       // PN.JoinLobby();
    }
    public void InitBlue()
    {
        isRed = false;   
        
        PN.ConnectUsingSettings();                                                // ����Ʈ ����
        //PN.ConnectToMaster(masterAddress, portNum, appID);                          // �����ּ�, ��Ʈ�ѹ�, �۾��̵�� ��������
        PN.GameVersion = gameVersion;                                               // ���� ���� *�߿�
        PN.AutomaticallySyncScene = true;                                           // �ڵ����� �� ����ȭ
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // ��ġ�� �ʴ� ���� ����

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "�� �÷��̾�";
        }
        //PN.JoinLobby();
    }

    public void InitRoom(int defaultRoomIndex)
    {        
        isTime = false;
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //PN.LoadLevel(roomSettings.sceneNum);
        //SceneManager.LoadScene(roomSettings.sceneNum);
        RoomOptions roomOptions = new RoomOptions
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)roomSettings.maxPLayer,
            PlayerTtl = 235,
            EmptyRoomTtl = 236
        };

        PN.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
        //localPlayer.SetActive(false);
    }
    public void InitGun(int defaultRoomIndex)
    {       
        isTime = true;
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //PN.LoadLevel(roomSettings.sceneNum);
        //SceneManager.LoadScene(roomSettings.sceneNum);
        Hashtable options = new Hashtable
        {
            { "Time", 40 }
        };
        RoomOptions roomOptions = new RoomOptions
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)roomSettings.maxPLayer,
            PlayerTtl = 235,
            EmptyRoomTtl = 236,
            CustomRoomProperties = options
        };

        PN.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
        // localPlayer.SetActive(false);
    }
    public void InitRoomRed(int defaultRoomIndex)
    {
        isRed = true;
        isTime = false;
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //PN.LoadLevel(roomSettings.sceneNum);
        //SceneManager.LoadScene(roomSettings.sceneNum);
        RoomOptions roomOptions = new RoomOptions
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)roomSettings.maxPLayer,
            PlayerTtl = 235,
            EmptyRoomTtl = 236            
        };

        PN.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
        //localPlayer.SetActive(false);
    }
    public void InitRoomBlue(int defaultRoomIndex)
    {
        isRed = false;
        isTime = false;
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //PN.LoadLevel(roomSettings.sceneNum);
        //SceneManager.LoadScene(roomSettings.sceneNum);
        RoomOptions roomOptions = new RoomOptions
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)roomSettings.maxPLayer,
            PlayerTtl = 235,
            EmptyRoomTtl = 236
        };

        PN.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
        //localPlayer.SetActive(false);
    }
    public void InitGunRed(int defaultRoomIndex)
    {
        isRed = true;
        isTime = true;
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //PN.LoadLevel(roomSettings.sceneNum);
        //SceneManager.LoadScene(roomSettings.sceneNum);
        Hashtable options = new Hashtable
        {
            { "Time", 40 }
        };
        RoomOptions roomOptions = new RoomOptions
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)roomSettings.maxPLayer,
            PlayerTtl = 235,
            EmptyRoomTtl = 236,
            CustomRoomProperties = options
        };       

        PN.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
       // localPlayer.SetActive(false);
    }
    public void InitGunBlue(int defaultRoomIndex)
    {
        isRed = false;
        isTime = true;
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //PN.LoadLevel(roomSettings.sceneNum);
        //SceneManager.LoadScene(roomSettings.sceneNum);
        Hashtable options = new Hashtable
        {
            { "Time", 40 }
        };
        RoomOptions roomOptions = new RoomOptions
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)roomSettings.maxPLayer,
            PlayerTtl = 235,
            EmptyRoomTtl = 236,
            CustomRoomProperties = options
        };

        PN.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
        // localPlayer.SetActive(false);
    }

    public override void OnConnectedToMaster()                                       // ���� ������ ���ӵǸ� ȣ��Ǵ� �޼���
    {
        
           teamSelectUI.SetActive(false);
           mapSelectUI.SetActive(true);

        if (isRed)
        {
            //isRed = true;
            mapRedUI.SetActive(true);
            teamSelectUI.SetActive(false);
            Debug.Log("������UI Ȱ��");
        }
        else
        {
            //isRed = false;
            mapBlueUI.SetActive(true);
            teamSelectUI.SetActive(false);
            Debug.Log("�����UI Ȱ��");
        }



        Debug.Log($"{PN.LocalPlayer.NickName} ������ �����Ͽ����ϴ�.");
    }

    /*  public override void OnJoinedLobby()                                             // �κ� ���� �� ȣ��Ǵ� �޼���
      {              
          Debug.Log($"{PN.LocalPlayer.NickName}���� �κ� �����Ͽ����ϴ�.");
      }
  */
    public override void OnJoinedRoom()
    {
      if(!isTime)
        {
            PN.LoadLevel(1); // Ʃ�丮��
        }
      else
        {
            PN.LoadLevel(2); // �ǽ���
        }
    }






}
