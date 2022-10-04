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

    [Header("���� �г���")]
    [SerializeField] TMP_InputField nick;

    [Header("�����÷��̾�")]
    [SerializeField] GameObject localPlayer;

    [Header("�ʼ��� â")]
    [SerializeField] GameObject mapSelectUI;     

    [Header("���� ������ â")]
    [SerializeField] GameObject teamSelectUI_T;

    [Header("������ ������ â")]
    [SerializeField] GameObject teamSelectUI_W;

    [Header("Ʃ�丮�� & ���� �����ο�")]
    [SerializeField] TextMeshProUGUI countText_TT;   

    [Header("Ʃ�丮�� & ������ �����ο�")]
    [SerializeField] TextMeshProUGUI countText_TW;    


    [Header("���̵��� ��ũ��")]
    [SerializeField] GameObject fadeScreen;

    [Header("�ΰ��� �Ǵ�")]
    public bool inGame;   

    private readonly string gameVersion = "1.0";
    //private readonly string masterAddress = "125.134.36.239";
    // private readonly string appID = "698049ca-edd8-41f6-9c9b-b8561355930a";
    // private readonly int portNum = 5055;
    //private readonly int n = 1;
    //private readonly int maxCount = 100;
    public Hashtable team = new Hashtable();

    [Header("������")]
    public GameObject adminPlayer;
    public GameObject ad_ConnectUI;
    public GameObject ad_MapUI;
    public GameObject ad_ToyUI;
    public GameObject ad_WesternUI;

    private void Awake()
    {
        if (NM != null && NM != this)
        {
            Destroy(this.gameObject);
        }
        NM = this;
        //DontDestroyOnLoad(this);
        PN.AutomaticallySyncScene = true;
        localPlayer.SetActive(true);
#if UNITY_EDITOR_WIN
        adminPlayer.SetActive(true);
        localPlayer.SetActive(false);
#endif
    }
    private void Start()
    {        
        // DataManager.DM.startingNum++;
    }

    private void Update()
    {
#if UNITY_EDITOR_WIN
        if (Input.GetKeyDown(KeyCode.Return)) { StartToServer_Admin(); }
        else if (Input.GetKeyDown(KeyCode.T)) { InitTutoT(); }
        else if (Input.GetKeyDown(KeyCode.W)) { InitTutoW(); }
        else if (Input.GetKeyDown(KeyCode.A)) { InitAdmin(0); }
        else if (Input.GetKeyDown(KeyCode.S)) { InitAdmin(2); }
#endif

    }

    public void StartToServer()                                                     // �������� �޼���
    {
        PN.ConnectUsingSettings();                                                  // ����Ʈ ����
        //PN.ConnectToMaster(masterAddress, portNum, appID);                        // �����ּ�, ��Ʈ�ѹ�, �۾��̵�� ��������
        PN.GameVersion = gameVersion;                                               // ���� ���� *�߿�
        PN.AutomaticallySyncScene = true;                                           // �ڵ����� �� ����ȭ
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        // int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // ��ġ�� �ʴ� ���� ����
        /*for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "�� �÷��̾�";
        }*/
        string str = nick.text;
        PN.LocalPlayer.NickName = str.ToUpper();

    }


    public void StartToServer_Admin()                                                     // �������� �޼���
    {
        PN.ConnectUsingSettings();                                                  // ����Ʈ ����
        //PN.ConnectToMaster(masterAddress, portNum, appID);                        // �����ּ�, ��Ʈ�ѹ�, �۾��̵�� ��������
        PN.GameVersion = gameVersion;                                               // ���� ���� *�߿�
        PN.AutomaticallySyncScene = true;                                           // �ڵ����� �� ����ȭ
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        // int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // ��ġ�� �ʴ� ���� ����
        /*for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "�� �÷��̾�";
        }*/
        string str = "Admin";
        PN.LocalPlayer.NickName = str.ToUpper();

    }

    public void InitAdmin(int defaultRoomIndex)                                       // ������ ����
    {
        DataManager.DM.currentTeam = Team.ADMIN;

        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        Hashtable options = new Hashtable { { "Time", 180 } };

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

    public void InitTutoT()                                                          // ���� ����
    {
        DataManager.DM.currentMap = Map.TUTORIAL_T;
        PN.JoinLobby();
    }

    public void InitTutoW()                                                         // ������ ����
    {
        DataManager.DM.currentMap = Map.TUTORIAL_W;
        PN.JoinLobby();
    }

   

    public void InitRed(int defaultRoomIndex)                                       // ������ ����
    {
        DataManager.DM.currentTeam = Team.RED;

        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        Hashtable options = new Hashtable { { "Time", 180 } };

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
    public void InitBlue(int defaultRoomIndex)                                      // ����� ����
    {
        DataManager.DM.currentTeam = Team.BLUE;

        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        Hashtable options = new Hashtable { { "Time", 180 } };

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
        connectUI.SetActive(false);
        mapSelectUI.SetActive(true);
#if UNITY_EDITOR_WIN
        ad_ConnectUI.SetActive(false);
        ad_MapUI.SetActive(true);
#endif
        Debug.Log($"{PN.LocalPlayer.NickName} ������ �����Ͽ����ϴ�.");
    }

    public override void OnJoinedLobby()                                             // �κ� ���� �� ȣ��Ǵ� �޼���
    {
        switch (DataManager.DM.currentMap)
        {
            case Map.TUTORIAL_T:
                teamSelectUI_T.SetActive(true);
                mapSelectUI.SetActive(false);
#if UNITY_EDITOR_WIN                
                ad_MapUI.SetActive(false);
                ad_ToyUI.SetActive(true);
#endif
                break;
            case Map.TUTORIAL_W:
                mapSelectUI.SetActive(false);
                teamSelectUI_W.SetActive(true);
#if UNITY_EDITOR_WIN
                ad_WesternUI.SetActive(true);
                ad_MapUI.SetActive(false);
#endif
                break;
            default:
                return;
        }
        Debug.Log($"{PN.LocalPlayer.NickName}���� �κ� �����Ͽ����ϴ�.");
    }

    public override void OnJoinedRoom()
    {
        switch (DataManager.DM.currentMap)
        {
            case Map.TUTORIAL_T:
            teamSelectUI_T.SetActive(false);
#if UNITY_EDITOR_WIN
                ad_ToyUI.SetActive(false);                
#endif
                PN.LoadLevel(1); // Ʃ�丮��T
                break;
            case Map.TOY:
                PN.LoadLevel(2); // ����
                break;
            case Map.TUTORIAL_W:
            teamSelectUI_W.SetActive(false);
#if UNITY_EDITOR_WIN
                ad_WesternUI.SetActive(false);
#endif
                PN.LoadLevel(3); // Ʃ�丮��W
                break;
            case Map.WESTERN:
                PN.LoadLevel(4); // ������
                break;
            default:
                return;
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
                default:
                    return;
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
