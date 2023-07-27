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

public class GunShootingManager : MonoBehaviourPunCallbacks                             // StartScene ��ũ��Ʈ
{
    public static GunShootingManager GSM;                                          // �̱���

    [Header("������ â")]
    [SerializeField] GameObject teamSelectUI;

    [Header("���� ��")]
    [SerializeField] GameObject gunBG;

    [Header("������")]
    [SerializeField] GameObject scoreBoard;

    [Header("���� �÷��̾�")]
    [SerializeField] GameObject localPlayer;

    [Header("������ �÷��̾�")]
    [SerializeField] GameObject redTeam;

    [Header("����� �÷��̾�")]
    [SerializeField] GameObject blueTeam;

    [Header("���� ���ѽð�")]
    [SerializeField] TextMeshPro timerText;
   
    private bool count;
    private int limitedTime;
    readonly Hashtable setTime = new Hashtable();

    [Header("������ �Ǵ�")]
    public bool isRed = false;

    private readonly string gameVersion = "1.0";
    private readonly string masterAddress = "125.134.36.239";
    private readonly string appID = "698049ca-edd8-41f6-9c9b-b8561355930a";
    private readonly int portNum = 5055;
    private readonly int n = 1;
    private readonly int maxCount = 6;

    #region ����Ƽ �޼��� ���� /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {      
        if (GSM != null && GSM != this)
        {
            Destroy(this.gameObject);
        }
        GSM = this;
    }
    private void Start()
    {
        PN.SendRate = 60;
        PN.SerializationRate = 30;
        StartToServer();
    }

    private void Update()
    {
        if(PN.InRoom)
        {
            limitedTime = (int)PN.CurrentRoom.CustomProperties["Time"];
            float min = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] / 60);
            float sec = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] % 60);
            timerText.text = string.Format("�����ð� {0:00}�� {1:00}��", min, sec);
            if (limitedTime < 60)
            {
                timerText.text = string.Format("�����ð� {0:0}��", sec);
            }
            if (PN.IsMasterClient)
            {
                if (count)
                {
                    count = false;
                    StartCoroutine(timer());
                }
            }
        }
       
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

    public void InitiliazeRoomRedTeam()       // ������ ��ư                            // �κ� ���� �� ������ �гο��� ���������� �޼���
    {
        isRed = true;        
        //PN.JoinRoom("LobbyRoom");
        Hashtable option = new Hashtable();
        option.Add("Time", 180);
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 100, EmptyRoomTtl = 1000, CustomRoomProperties = option }; // �� �ɼ�
                                                                                                                                                          //options.CustomRoomProperties = option;        
        PN.JoinOrCreateRoom("GunRoom", options, TypedLobbyInfo.Default);
    }
    public void InitiliazeRoomBlueTeam()      // ����� ��ư                            // �κ� ���� �� ������ �гο��� ��������� �޼���
    {
        isRed = false;       
        //PN.JoinRoom("LobbyRoom");
        Hashtable option = new Hashtable();
        option.Add("Time", 180);
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 100, EmptyRoomTtl = 1000, CustomRoomProperties = option }; // �� �ɼ�
                                                                                                                                                          //options.CustomRoomProperties = option;        
        PN.JoinOrCreateRoom("GunRoom", options, TypedLobbyInfo.Default);
    }

    #endregion UI ��Ʈ�� �޼��� �� //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region ���� ���� �ݹ� �޼��� ����///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public override void OnConnectedToMaster()                                       // ���� ������ ���ӵǸ� ȣ��Ǵ� �޼���
    {
        Debug.Log($"{PN.LocalPlayer.NickName} ������ �����Ͽ����ϴ�.");
        //Debug.Log("�������� : " + PN.NetworkClientState);
        PN.JoinLobby();
    }

    public override void OnJoinedLobby()                                             // �κ� ���� �� ȣ��Ǵ� �޼���
    {
        teamSelectUI.SetActive(true);
        Debug.Log($"{PN.LocalPlayer.NickName} �κ� �����Ͽ����ϴ�.");
        //Debug.Log("�������� : " + PN.NetworkClientState);       
    }

    public override void OnJoinedRoom()                                               // �濡 ���� �� ȣ��Ǵ� �޼���
    {
        teamSelectUI.SetActive(false);
        localPlayer.SetActive(false);
        gunBG.SetActive(true);
        scoreBoard.SetActive(true);
        ReadySceneManager.RSM.inGame = true;
        count = true;

        if (isRed)
        {            
            PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity);
            Debug.Log($"{PN.CurrentRoom.Name} �濡 {PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");           
        }
        else
        {            
            PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);
            Debug.Log($"{PN.CurrentRoom.Name} �濡 {PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");           
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


    public override void OnLeftRoom()
    {
        PN.Disconnect();
        SceneManager.LoadScene(0);
        Debug.Log("���� �������ϴ�.");
        // PN.LoadLevel("StartScene2");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //Application.Quit();
        Debug.Log("�����������");
        //PN.LoadLevel("GunShooting");
        // SceneManager.LoadScene("readyscene_1");
        //PN.IsMessageQueueRunning = false;
        // Debug.Log("����1��������");
        //StartCoroutine(DelayLoadRD1());
    }
    public void OnApplicationQuit()
    {
        PN.Disconnect();
        //PN.LeaveRoom();
    }

   /* public void SpawnRedPlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        }

        PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity);

        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ         

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}��"); // $ == String.Format() ���� 
        }
    }

    public void SpawnBluePlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        }

        PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);

        PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ         

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}��"); // $ == String.Format() ���� 
        }
    }*/

    /* IEnumerator DelayLoadRD1()
     {
         //PN.LeaveRoom();

         yield return new WaitForSeconds(1);

     }*/

    /*private void MigrateMaster()
    {
        var dict = PN.CurrentRoom.Players;
        if (PN.SetMasterClient(dict[dict.Count - 1]))
        {

            PN.LeaveRoom();
        }
    }*/


    #endregion ���� ���� �ݹ� �޼��� �� ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region    �÷��� Ÿ�� �޼��� ���� ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public IEnumerator timer()
    {
        yield return new WaitForSeconds(1);
        int nextTime = limitedTime -= 1;
        setTime["Time"] = nextTime;
        PN.CurrentRoom.SetCustomProperties(setTime);
        count = true;

        if (limitedTime == 0)
        {
            limitedTime = 0;
            timerText.text = string.Format("�����ð� 0��");
            // StartCoroutine(LoadNext());

            Application.Quit();
            // PN.LeaveRoom();

            Debug.Log("Ÿ�ӿ���");
        }
    }

    IEnumerator LoadNext()
    {
        SceneManager.LoadScene(0);
        yield return new WaitForSeconds(1f);
        PN.Disconnect();
    }
    #endregion �÷��� Ÿ�� �޼��� �� /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
