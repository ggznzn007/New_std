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

public class GunShootingManager : MonoBehaviourPunCallbacks                             // StartScene 스크립트
{
    public static GunShootingManager GSM;                                          // 싱글턴

    [Header("팀선택 창")]
    [SerializeField] GameObject teamSelectUI;

    [Header("게임 맵")]
    [SerializeField] GameObject gunBG;

    [Header("점수판")]
    [SerializeField] GameObject scoreBoard;

    [Header("로컬 플레이어")]
    [SerializeField] GameObject localPlayer;

    [Header("레드팀 플레이어")]
    [SerializeField] GameObject redTeam;

    [Header("블루팀 플레이어")]
    [SerializeField] GameObject blueTeam;

    [Header("게임 제한시간")]
    [SerializeField] TextMeshPro timerText;
   
    private bool count;
    private int limitedTime;
    readonly Hashtable setTime = new Hashtable();

    [Header("팀선택 판단")]
    public bool isRed = false;

    private readonly string gameVersion = "1.0";
    private readonly string masterAddress = "125.134.36.239";
    private readonly string appID = "698049ca-edd8-41f6-9c9b-b8561355930a";
    private readonly int portNum = 5055;
    private readonly int n = 1;
    private readonly int maxCount = 6;

    #region 유니티 메서드 시작 /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
            timerText.text = string.Format("남은시간 {0:00}분 {1:00}초", min, sec);
            if (limitedTime < 60)
            {
                timerText.text = string.Format("남은시간 {0:0}초", sec);
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

    public void StartToServer()                                                     // 서버연결 메서드
    {
        //PN.ConnectUsingSettings();
        PN.ConnectToMaster(masterAddress, portNum, appID);
        PN.GameVersion = gameVersion;
        PN.AutomaticallySyncScene = true;
        int[] NickNumber = Utils.RandomNumbers(maxCount, n);                        // 겹치지 않는 랜덤한 수 생성

        for (int i = 0; i < NickNumber.Length; i++)
        {
            PN.LocalPlayer.NickName = NickNumber[i] + "번 플레이어";
        }

    }

    #endregion 유니티 메서드 끝 ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region UI 컨트롤 메서드 시작 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void InitiliazeRoomRedTeam()       // 레드팀 버튼                            // 로비 진입 후 팀선택 패널에서 레드팀선택 메서드
    {
        isRed = true;        
        //PN.JoinRoom("LobbyRoom");
        Hashtable option = new Hashtable();
        option.Add("Time", 180);
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 100, EmptyRoomTtl = 1000, CustomRoomProperties = option }; // 방 옵션
                                                                                                                                                          //options.CustomRoomProperties = option;        
        PN.JoinOrCreateRoom("GunRoom", options, TypedLobbyInfo.Default);
    }
    public void InitiliazeRoomBlueTeam()      // 블루팀 버튼                            // 로비 진입 후 팀선택 패널에서 블루팀선택 메서드
    {
        isRed = false;       
        //PN.JoinRoom("LobbyRoom");
        Hashtable option = new Hashtable();
        option.Add("Time", 180);
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 100, EmptyRoomTtl = 1000, CustomRoomProperties = option }; // 방 옵션
                                                                                                                                                          //options.CustomRoomProperties = option;        
        PN.JoinOrCreateRoom("GunRoom", options, TypedLobbyInfo.Default);
    }

    #endregion UI 컨트롤 메서드 끝 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region 포톤 서버 콜백 메서드 시작///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public override void OnConnectedToMaster()                                       // 포톤 서버에 접속되면 호출되는 메서드
    {
        Debug.Log($"{PN.LocalPlayer.NickName} 서버에 접속하였습니다.");
        //Debug.Log("서버상태 : " + PN.NetworkClientState);
        PN.JoinLobby();
    }

    public override void OnJoinedLobby()                                             // 로비에 들어갔을 때 호출되는 메서드
    {
        teamSelectUI.SetActive(true);
        Debug.Log($"{PN.LocalPlayer.NickName} 로비에 접속하였습니다.");
        //Debug.Log("서버상태 : " + PN.NetworkClientState);       
    }

    public override void OnJoinedRoom()                                               // 방에 들어갔을 때 호출되는 메서드
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
            Debug.Log($"{PN.CurrentRoom.Name} 방에 {PN.LocalPlayer.NickName} 님이 입장하셨습니다.");           
        }
        else
        {            
            PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);
            Debug.Log($"{PN.CurrentRoom.Name} 방에 {PN.LocalPlayer.NickName} 님이 입장하셨습니다.");           
        }       
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName}님 현재인원:{PN.CurrentRoom.PlayerCount}");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName}님 현재인원:{PN.CurrentRoom.PlayerCount}");
    }


    public override void OnLeftRoom()
    {
        PN.Disconnect();
        SceneManager.LoadScene(0);
        Debug.Log("방을 나갔습니다.");
        // PN.LoadLevel("StartScene2");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //Application.Quit();
        Debug.Log("서버연결끊김");
        //PN.LoadLevel("GunShooting");
        // SceneManager.LoadScene("readyscene_1");
        //PN.IsMessageQueueRunning = false;
        // Debug.Log("레디1으로진입");
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
            PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화 
        }

        PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity);

        PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화         

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}번"); // $ == String.Format() 약자 
        }
    }

    public void SpawnBluePlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화 
        }

        PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);

        PN.AutomaticallySyncScene = true;                                           // 같은 룸의 유저들에게 자동으로 씬 동기화         

        foreach (var player in PN.CurrentRoom.Players)
        {
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}번"); // $ == String.Format() 약자 
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


    #endregion 포톤 서버 콜백 메서드 끝 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region    플레이 타임 메서드 시작 ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
            timerText.text = string.Format("남은시간 0초");
            // StartCoroutine(LoadNext());

            Application.Quit();
            // PN.LeaveRoom();

            Debug.Log("타임오버");
        }
    }

    IEnumerator LoadNext()
    {
        SceneManager.LoadScene(0);
        yield return new WaitForSeconds(1f);
        PN.Disconnect();
    }
    #endregion 플레이 타임 메서드 끝 /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
