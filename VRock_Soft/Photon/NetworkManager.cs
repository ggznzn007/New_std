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

    [Header("���� ����â")]
    [SerializeField] GameObject connectUI;

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

    /*[Header("�����÷��� ������ �Ǵ�")]
    public bool isRed;*/

    [Header("������ ���� �Ǵ�")]
    public bool inGame;

    [Header("���ӽð� ���� �Ǵ�")]
    public bool isTime;    

    private readonly string gameVersion = "1.0";                                           // ���ӹ���
    //private readonly string masterAddress = "125.134.36.239";                            // �����ּ�
    // private readonly string appID = "698049ca-edd8-41f6-9c9b-b8561355930a";             // �۾��̵�
    // private readonly int portNum = 5055;                                                // ��Ʈ�ѹ�
    private readonly int n = 1;                                                            // ���� n�� ����
    private readonly int maxCount = 1000;                                                  // ���� �ִ� ����

    #region ######################################### ����Ƽ & UI �޼��� ���� #####################################################################
    private void Awake()
    {
        if (NM != null && NM != this)
        {
            Destroy(this.gameObject);
        }        
        NM = this;
        PN.AutomaticallySyncScene = true;
    }
    private void Start()
    {       
        DataManager.DM.startingNum++;                                                // ���� �ε� �� ������ ����       
    }

    public void StartToServer()                                                     // �������� �޼���
    {
        PN.ConnectUsingSettings();                                                  // ����Ʈ ���� ����Ŭ����
        //PN.ConnectToMaster(masterAddress, portNum, appID);                        // ���漭���ּ�, ��Ʈ�ѹ�, �۾��̵�� ��������
        PN.GameVersion = gameVersion;                                               // ���� ���� *�߿�
        PN.AutomaticallySyncScene = true;                                           // �ڵ����� �� ����ȭ
        PN.SendRate = 60;                                                           // �������� ����
        PN.SerializationRate = 30;                                                  // ����ȭ   ����
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // ��ġ�� �ʴ� ���� ����

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "�� �÷��̾�";                  // �г��� n�� ���� �� �÷��̾�
        }
    }
 
    public void InitRed()                                                           // ������ ����
    {
        DataManager.DM.currentTeam = Team.RED;
        DataManager.DM.isSelected= true;           
        
        PN.JoinLobby();
    }
    public void InitBlue()                                                          // ����� ����
    {
        DataManager.DM.currentTeam = Team.BLUE;
        DataManager.DM.isSelected = true;
        
        PN.JoinLobby();
    }

    public void InitRoomNormal(int defaultRoomIndex)                                      // ���ӽð��� ���� �� ���� �޼���
    {
        isTime = false;                                                                   // �ð� ������Ƽ ����
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];
       
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
    public void InitRoomHasTime(int defaultRoomIndex)                                       // ���ӽð��� �ִ� �� ���� �޼���
    {
        isTime = true;                                                                      // �ð� ������Ƽ ����
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        
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

    #endregion ######################################### ����Ƽ & UI �޼��� �� ###################################################################

    #region @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ ���� ���� �ݹ� �޼��� ���� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public override void OnConnectedToMaster()                                       // ���� ������ ���ӵǸ� ȣ��Ǵ� �޼���
    {              
        if(DataManager.DM.startingNum==1)                                            // ���� ó�� ���۽ÿ��� ȣ��
        {
            connectUI.SetActive(false);
            teamSelectUI.SetActive(true);            
        }
        else if (DataManager.DM.startingNum > 1)                                     // �ι�°���ʹ� �ʼ��ø� ȣ��
        {
            connectUI.SetActive(false);
            teamSelectUI.SetActive(false);
            PN.JoinLobby();
        }

        Debug.Log($"{PN.LocalPlayer.NickName} ������ �����Ͽ����ϴ�.");
    }

    public override void OnJoinedLobby()                                             // �κ� ���� �� ȣ��Ǵ� �޼���
    {        
        if (DataManager.DM.isSelected)                                               // �� ������ ���� �� 
        {
            teamSelectUI.SetActive(false);
            mapSelectUI.SetActive(true);
        }            

        Debug.Log($"{PN.LocalPlayer.NickName}���� �κ� �����Ͽ����ϴ�.");
    }

    public override void OnJoinedRoom()
    {
        mapSelectUI.SetActive(false);
        if (!isTime)                                                                 // ���Ӿ��� �ð� ������Ƽ ���� ����
        {
            PN.LoadLevel(1); // Ʃ�丮���
        }
        else
        {
            PN.LoadLevel(2); // �ǽ��þ�
        }

    }
    

    #endregion @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ ���� ���� �ݹ� �޼��� �� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


    private void OnApplicationQuit()
    {
        PN.Disconnect();
    }

}
