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
    [SerializeField] RectTransform connectUI;

    //[Header("���� �г���")]
    //public TMP_InputField nick;

    [Header("�����÷��̾�")]
    [SerializeField] GameObject localPlayer;

    [Header("�ʼ��� â")]
    [SerializeField] RectTransform mapSelectUI;

    [Header("���� ������ â")]
    [SerializeField] RectTransform teamSelectUI_T;

    [Header("������ ������ â")]
    [SerializeField] RectTransform teamSelectUI_W;

    [Header("Ʃ�丮�� & ���� �����ο�")]
    [SerializeField] TextMeshProUGUI countText_TT;

    [Header("Ʃ�丮�� & ������ �����ο�")]
    [SerializeField] TextMeshProUGUI countText_TW;

    [Header("���̵��� ��ũ��")]
    [SerializeField] Canvas fadeScreen;  

    private readonly string gameVersion = "1.0";
    //private readonly string masterAddress = "125.134.36.239";
    // private readonly string appID = "698049ca-edd8-41f6-9c9b-b8561355930a";
    // private readonly int portNum = 5055;
    //private readonly int n = 1;
    //private readonly int maxCount = 10;
   
    [Header("�����ڿɼ�")]
    readonly private string adminName = "������";
    [SerializeField] GameObject adminPlayer;
    [SerializeField] RectTransform ad_ConnectUI;
    [SerializeField] RectTransform ad_MapUI;
    [SerializeField] RectTransform ad_ToyUI;
    [SerializeField] RectTransform ad_WesternUI;

    private void Awake()
    {
        if (NM != null && NM != this)
        {
            Destroy(this.gameObject);
        }
        NM = this;       

        PN.AutomaticallySyncScene = true;
        localPlayer.SetActive(true);
         // ������ ���α׷� ���� ��
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            adminPlayer.SetActive(true);
            localPlayer.SetActive(false);
        }
        // ����Ƽ �����Ϳ��� ��� ��
       /* if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            adminPlayer.SetActive(true);
            localPlayer.SetActive(false);
        }*/
    }

    private void Start()
    {
        DataManager.DM.startingNum++;
        if (DataManager.DM.startingNum >= 2)
        {
            connectUI.gameObject.SetActive(false);
              // ������ ���α׷� ���� ��
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                ad_ConnectUI.gameObject.SetActive(false);
            }

            // ����Ƽ �����Ϳ��� ��� ��
           /* if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                ad_ConnectUI.gameObject.SetActive(false);
            }*/
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { StartToServer_Admin(); }             // ������        ����
        else if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }           // ����
        else if (Input.GetKeyDown(KeyCode.T)) { InitTutoT(); }                       // ����
        else if (Input.GetKeyDown(KeyCode.W)) { InitTutoW(); }                       // ������
        else if (Input.GetKeyDown(KeyCode.Keypad0)) { InitRed(0); }                  //  ���� ����
        else if (Input.GetKeyDown(KeyCode.Keypad1)) { InitBlue(0); }                 //  ���� ���
        else if (Input.GetKeyDown(KeyCode.Keypad2)) { InitRed(2); }                  //  ������ ����
        else if (Input.GetKeyDown(KeyCode.Keypad3)) { InitBlue(2); }                 //  ������ ���
        else if (Input.GetKeyDown(KeyCode.A)) { InitAdmin(0); }                      // ���� ������    ����
        else if (Input.GetKeyDown(KeyCode.S)) { InitAdmin(2); }                      // ������ ������   ����
        // ������ ���α׷� ���� ��
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { StartToServer_Admin(); }             // ������        ����
            else if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }           // ����
            else if (Input.GetKeyDown(KeyCode.T)) { InitTutoT(); }                       // ����
            else if (Input.GetKeyDown(KeyCode.W)) { InitTutoW(); }                       // ������
            else if (Input.GetKeyDown(KeyCode.Keypad0)) { InitRed(0); }                  //  ���� ����
            else if (Input.GetKeyDown(KeyCode.Keypad1)) { InitBlue(0); }                 //  ���� ���
            else if (Input.GetKeyDown(KeyCode.Keypad2)) { InitRed(2); }                  //  ������ ����
            else if (Input.GetKeyDown(KeyCode.Keypad3)) { InitBlue(2); }                 //  ������ ���
            else if (Input.GetKeyDown(KeyCode.A)) { InitAdmin(0); }                      // ���� ������    ����
            else if (Input.GetKeyDown(KeyCode.S)) { InitAdmin(2); }                      // ������ ������   ����
        }
    }

    public void StartToServer()                                                     // �������� �޼���
    {
        PN.ConnectUsingSettings();                                                  // ����Ʈ ����
        //PN.ConnectToMaster(masterAddress, portNum, appID);                        // �����ּ�, ��Ʈ�ѹ�, �۾��̵�� ��������
        PN.GameVersion = gameVersion;                                               // ���� ���� *�߿�
        PN.AutomaticallySyncScene = true;                                           // �ڵ����� �� ����ȭ
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        /*int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // ��ġ�� �ʴ� ���� ����

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = "�÷��̾� "+ NickNumber[i];
            DataManager.DM.nickName= "�÷��̾� " + NickNumber[i];
        }*/
        /* string str = nick.text;
         PN.LocalPlayer.NickName = str.ToUpper();
         DataManager.DM.nickName = str;*/
    }

    public void StartToServer_Admin()                                                     // �������� �޼���
    {
        PN.ConnectUsingSettings();                                                  // ����Ʈ ����
        //PN.ConnectToMaster(masterAddress, portNum, appID);                        // �����ּ�, ��Ʈ�ѹ�, �۾��̵�� ��������
        PN.GameVersion = gameVersion;                                               // ���� ���� *�߿�
        PN.AutomaticallySyncScene = true;                                           // �ڵ����� �� ����ȭ
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        PN.LocalPlayer.NickName = adminName;
        //DataManager.DM.nickName = adminName;
    }

    public void InitAdmin(int defaultRoomIndex)                                       // ������ ����
    {
        DataManager.DM.currentTeam = Team.ADMIN;
        DataManager.DM.isSelected = true;

        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        Hashtable options = new Hashtable { { "Time", 240 } };

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
        DataManager.DM.teamInt = 1;
        DataManager.DM.isSelected = true;

        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        Hashtable options = new Hashtable { { "Time", 240 } };        

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
        DataManager.DM.teamInt = 0;
        DataManager.DM.isSelected = true;

        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        Hashtable options = new Hashtable { { "Time", 240 } };       

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


    /* public void InitToy(int defaultRoomIndex)
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

     }*/

    public override void OnConnectedToMaster()                                       // ���� ������ ���ӵǸ� ȣ��Ǵ� �޼���
    {
        //mapSelectUI.gameObject.SetActive(true);
        connectUI.gameObject.SetActive(false);

        switch (DataManager.DM.startingNum)
        {
            case 1:
            case 3:
                InitTutoT();
                // ����Ƽ �����Ϳ��� ��� ��
                /* if (Application.platform == RuntimePlatform.WindowsEditor)
                 {
                     ad_ConnectUI.gameObject.SetActive(false);
                     ad_MapUI.gameObject.SetActive(false);
                 }                */

                // ������ ���α׷� ���� ��
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    ad_ConnectUI.gameObject.SetActive(false);
                    ad_MapUI.gameObject.SetActive(false);
                }
                break;
            case 2:
            case 4:
                InitTutoW();
                // ����Ƽ �����Ϳ��� ��� ��
                /*    if (Application.platform == RuntimePlatform.WindowsEditor)
                    {
                        ad_ConnectUI.gameObject.SetActive(false);
                        ad_MapUI.gameObject.SetActive(false);
                    }               */
                // ������ ���α׷� ���� ��
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    ad_ConnectUI.gameObject.SetActive(false);
                    ad_MapUI.gameObject.SetActive(false);
                }
                break;
        }
        Debug.Log($"{PN.LocalPlayer.NickName}���� ������ �����Ͽ����ϴ�.");
    }

    public override void OnJoinedLobby()                                             // �κ� ���� �� ȣ��Ǵ� �޼���
    {
        switch (DataManager.DM.startingNum)
        {
            case 1:
            case 3:
                teamSelectUI_T.gameObject.SetActive(true);
                //mapSelectUI.gameObject.SetActive(false);
                //ad_MapUI.gameObject.SetActive(false);
                // ����Ƽ �����Ϳ��� ��� ��     
                /* if (Application.platform == RuntimePlatform.WindowsEditor)
                 {
                     ad_ToyUI.gameObject.SetActive(true);
                 }*/

                // ������ ���α׷� ���� ��                
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    ad_ToyUI.gameObject.SetActive(true);
                }

                if (DataManager.DM.isSelected && DataManager.DM.currentTeam == Team.BLUE)
                {
                    InitBlue(0);
                    teamSelectUI_T.gameObject.SetActive(false);
                }
                else if (DataManager.DM.isSelected && DataManager.DM.currentTeam == Team.RED)
                {
                    InitRed(0);
                    teamSelectUI_T.gameObject.SetActive(false);
                }
                break;
            case 2:
            case 4:
                //mapSelectUI.gameObject.SetActive(false);
                teamSelectUI_W.gameObject.SetActive(true);
                // ����Ƽ �����Ϳ��� ��� ��
                /* if (Application.platform == RuntimePlatform.WindowsEditor)
                 {
                     ad_WesternUI.gameObject.SetActive(false);
                     InitAdmin(2);
                 }*/
                // ������ ���α׷� ���� ��
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    ad_WesternUI.gameObject.SetActive(false);
                    InitAdmin(2);

                }

                if (DataManager.DM.isSelected && DataManager.DM.currentTeam == Team.BLUE)
                {
                    InitBlue(2);
                    teamSelectUI_W.gameObject.SetActive(false);
                }
                else if (DataManager.DM.isSelected && DataManager.DM.currentTeam == Team.RED)
                {
                    InitRed(2);
                    teamSelectUI_W.gameObject.SetActive(false);
                }
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
                //teamSelectUI_T.gameObject.SetActive(false);
                // ����Ƽ �����Ϳ��� ��� ��
               /* if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    ad_ToyUI.gameObject.SetActive(false);
                }     */         
                // ������ ���α׷� ���� ��
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    ad_ToyUI.gameObject.SetActive(false);
                }
               
                PN.LoadLevel(1); // Ʃ�丮��T
                break;
            case Map.TOY:
               
                PN.LoadLevel(2); // ����
                break;
            case Map.TUTORIAL_W:
                teamSelectUI_W.gameObject.SetActive(false);
                // ����Ƽ �����Ϳ��� ��� ��
               /* if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    ad_WesternUI.gameObject.SetActive(false);
                }*/
                 // ������ ���α׷� ���� ��
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    ad_WesternUI.gameObject.SetActive(false);
                }
               
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
            countText_TW.text = 0 + " ��";           
        }

        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room.Name);                   
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

}
