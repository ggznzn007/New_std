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
using Unity.VisualScripting;
using static UnityEngine.UIElements.UxmlAttributeDescription;

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

    [Header("���� ����â")]
    [SerializeField] GameObject connectUI;

    [Header("������ â")]
    [SerializeField] GameObject teamSelectUI;

    [Header("�ʼ��� â")]
    [SerializeField] GameObject mapSelectUI;

    [Header("Ʃ�丮��1 �����ο�")]
    [SerializeField] TextMeshProUGUI countText_TT;

    /*[Header("TOY �����ο�")]
    [SerializeField] TextMeshProUGUI countText_T;*/

    [Header("Ʃ�丮��2 �����ο�")]
    [SerializeField] TextMeshProUGUI countText_TW;

    /*[Header("Western �����ο�")]
    [SerializeField] TextMeshProUGUI countText_W;*/

    [Header("�����÷��̾�")]
    [SerializeField] GameObject localPlayer;

    [Header("���̵��� ��ũ��")]
    [SerializeField] GameObject fadeScreen;

    [Header("�ΰ��� �Ǵ�")]
    public bool inGame;

    /*[Header("���ӽð����� �Ǵ�")]
    public bool gamehasTime;*/

    private readonly string gameVersion = "1.0";
    //private readonly string masterAddress = "125.134.36.239";
    // private readonly string appID = "698049ca-edd8-41f6-9c9b-b8561355930a";
    // private readonly int portNum = 5055;
    private readonly int n = 1;
    private readonly int maxCount = 100;
    public Hashtable team = new Hashtable();
    
    private void Awake()
    {
        if (NM != null && NM != this)
        {
            Destroy(this.gameObject);
        }
        NM = this;
        //DontDestroyOnLoad(this);
        PN.AutomaticallySyncScene = true;

    }
    private void Start()
    {
        DataManager.DM.startingNum++;
    }

    public void StartToServer()                                                     // �������� �޼���
    {
        PN.ConnectUsingSettings();                                                  // ����Ʈ ����
        //PN.ConnectToMaster(masterAddress, portNum, appID);                        // �����ּ�, ��Ʈ�ѹ�, �۾��̵�� ��������
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
        DataManager.DM.currentTeam = Team.RED;
        DataManager.DM.isSelected = true;
        PN.JoinLobby();
    }
    public void InitBlue()
    {
        DataManager.DM.currentTeam = Team.BLUE;
        DataManager.DM.isSelected = true;
        PN.JoinLobby();
    }

    public void InitTutoT(int defaultRoomIndex)
    {
        DataManager.DM.currentMap = Map.TUTORIAL_T;
        //PN.KeepAliveInBackground = 3;
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        Hashtable options = new Hashtable { { "Time", 300 } };

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

    }
    public void InitToy(int defaultRoomIndex)
    {
        DataManager.DM.currentMap = Map.TOY;
       
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        Hashtable options = new Hashtable { { "Time", 120 } };

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

    }

    public void InitTutoW(int defaultRoomIndex)
    {
        DataManager.DM.currentMap = Map.TUTORIAL_W;

        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        Hashtable options = new Hashtable { { "Time", 300 } };

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

    }

    public void InitWestern(int defaultRoomIndex)
    {
        DataManager.DM.currentMap = Map.WESTERN;
        
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        Hashtable options = new Hashtable { { "Time", 120 } };

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

    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public override void OnConnectedToMaster()                                       // ���� ������ ���ӵǸ� ȣ��Ǵ� �޼���
    {
        if (DataManager.DM.startingNum == 1)
        {
            connectUI.SetActive(false);
            teamSelectUI.SetActive(true);
        }
        else if (DataManager.DM.startingNum > 1)
        {
            connectUI.SetActive(false);
            teamSelectUI.SetActive(false);
            PN.JoinLobby();
        }

        Debug.Log($"{PN.LocalPlayer.NickName} ������ �����Ͽ����ϴ�.");
    }

    public override void OnJoinedLobby()                                             // �κ� ���� �� ȣ��Ǵ� �޼���
    {
        if (DataManager.DM.isSelected && PN.InLobby)
        {
            teamSelectUI.SetActive(false);
            mapSelectUI.SetActive(true);
        }


        Debug.Log($"{PN.LocalPlayer.NickName}���� �κ� �����Ͽ����ϴ�.");
    }

    public override void OnJoinedRoom()
    {
        mapSelectUI.SetActive(false);

        switch (DataManager.DM.currentMap)
        {
            case Map.TUTORIAL_T:
                PN.LoadLevel(1); // Ʃ�丮��T
                break;
            case Map.TOY:
                PN.LoadLevel(2); // ����
                break;
            case Map.TUTORIAL_W:
                PN.LoadLevel(3); // Ʃ�丮��W
                break;
            case Map.WESTERN:
                PN.LoadLevel(4); // ������
                break;
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList.Count == 0)                                 // �濡 �ƹ��� ���� ��
        {
            countText_TT.text = 0 + " ��";
            //countText_T.text = 0 + " ��";
            countText_TW.text = 0 + " ��";
           // countText_W.text = 0 + " ��";
        }

        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
            // DefaultRoom roomName = new DefaultRoom();            
            string roomString = room.Name.ToString();
            switch (roomString)
            {
                case "Tutorial_T":
                    Debug.Log("����Ʃ�� �ο� : " + room.PlayerCount);
                    countText_TT.text = room.PlayerCount + " ��";
                    break;
                case "Tutorial_W":
                    Debug.Log("������Ʃ�� �ο� : " + room.PlayerCount);
                    countText_TW.text = room.PlayerCount + " ��";
                    break;
               /* case "Toy":
                    Debug.Log("���̹� �ο� : " + room.PlayerCount);
                    countText_T.text = room.PlayerCount + " ��";
                    break;
                case "Western":
                    Debug.Log("�����Ϲ� �ο� : " + room.PlayerCount);
                    countText_W.text = room.PlayerCount + " ��";
                    break;*/
            }
        }
    }

    // �� �����ο� ������Ʈ if��
    /*if (room.Name.Contains(roomName.Name = "Tutorial"))
            {
               Debug.Log("Ʃ�丮��� �ο� : " + room.PlayerCount);
                    countText_TU.text = room.PlayerCount + " ��";
            }
            else if(room.Name.Contains(roomName.Name = "Toy"))
            {
                Debug.Log("���̹� �ο� : " + room.PlayerCount);
                    countText_T.text = room.PlayerCount + " ��";
            }
            else if (room.Name.Contains(roomName.Name = "Western"))
            {
                Debug.Log("�����Ϲ� �ο� : " + room.PlayerCount);
                    countText_W.text = room.PlayerCount + " ��";
            }*/

    // �ε� �� if��
    /* if (DataManager.DM.currentMap == Map.TUTORIAL1)
       {
           PN.LoadLevel(1); // Ʃ�丮��1
       }
       else if (DataManager.DM.currentMap == Map.TOY)
       {
           PN.LoadLevel(2); // ����
       }
       else if (DataManager.DM.currentMap == Map.WESTERN)
       {
           PN.LoadLevel(3); // ������
       }*/
}
