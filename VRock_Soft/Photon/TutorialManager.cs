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
using static ObjectPooler;
using UnityEngine.XR;

public class TutorialManager : MonoBehaviourPunCallbacks
{
    public static TutorialManager TM;

    [SerializeField] GameObject redTeam;                // 레드팀 프리팹
    [SerializeField] GameObject blueTeam;               // 블루팀 프리팹
    [SerializeField] GameObject defaultTeam;
    [SerializeField] GameObject admin;                  // 관리자 프리팹
    public Transform adminPoint;                        // 관리자 생성위치
    private GameObject spawnPlayer;                     // 생성되는 플레이어
    public GameObject bomB;                             // 폭탄 프리팹
    public Transform[] bSpawnPosition;                  // 폭탄 생성위치
    private GameObject bomBs;                           // 생성되는 폭탄

    private void Awake()
    {
        TM = this;
    }

    private void Start()
    {
        if (!PN.IsConnectedAndReady)
        {
            SceneManager.LoadScene(0);
        }
        if (PN.IsConnectedAndReady && PN.InRoom)
        {
            if (PN.IsMasterClient)
            {
                InvokeRepeating(nameof(SpawnBomb), 20, 20);             // 게임 시작되고 20초후에 실행하고 그 후 15초마다 실행
            }

            switch (PN.CurrentRoom.PlayerCount)
            {
                case 1:
                    PN.LocalPlayer.NickName = "마스터";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 2:
                    PN.LocalPlayer.NickName = "플레이어 1";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 3:
                    PN.LocalPlayer.NickName = "플레이어 2";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 4:
                    PN.LocalPlayer.NickName = "플레이어 3";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 5:
                    PN.LocalPlayer.NickName = "플레이어 4";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 6:
                    PN.LocalPlayer.NickName = "플레이어 5";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                default:
                    PN.LocalPlayer.NickName = "마스터 플레이어";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
            }

           /* if (DataManager.DM.currentTeam != Team.ADMIN)  // 관리자 빌드시 필요한 코드
            {
                Destroy(admin);
            }*/
        }
    }

    private void Update()
    {
        if (PN.InRoom && PN.IsMasterClient)
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)  // 윈도우 빌드
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {  PN.LoadLevel(2); }
                else if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    SpawnBomb();
                }
            }
        }          
    }

    public void SpawnPlayer()
    {
        switch (DataManager.DM.currentTeam)
        {
            case Team.RED:
                PN.AutomaticallySyncScene = true;
                DataManager.DM.inGame = false;
                spawnPlayer = PN.Instantiate(redTeam.name, Vector3.zero, Quaternion.identity);
                Debug.Log($"{PN.CurrentRoom.Name} 방에 레드팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                Info();
                break;

            case Team.BLUE:
                PN.AutomaticallySyncScene = true;
                DataManager.DM.inGame = false;
                spawnPlayer = PN.Instantiate(blueTeam.name, Vector3.zero, Quaternion.identity);
                Debug.Log($"{PN.CurrentRoom.Name} 방에 블루팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                Info();
                break;
           
             // 윈도우 프로그램 빌드 시
            case Team.ADMIN:
                if (Application.platform == RuntimePlatform.WindowsPlayer)//|| Application.platform == RuntimePlatform.WindowsEditor)
                {
                    PN.AutomaticallySyncScene = true;
                    DataManager.DM.inGame = false;
                    spawnPlayer = PN.Instantiate(admin.name, adminPoint.position, adminPoint.rotation);
                    Debug.Log($"{PN.CurrentRoom.Name} 방에 관리자{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                    Info();
                }
                break;
            

            default:
                return;
        }
    }

    public void SpawnBomb()
    {
        for (int i = 0; i < bSpawnPosition.Length; i++)
        {
           bomBs = PN.Instantiate(bomB.name, bSpawnPosition[i].position, bSpawnPosition[i].rotation, 0);
        }
    }
    
    [ContextMenu("포톤 서버 정보")]
    void Info()
    {
        if (PN.InRoom)
        {
            print("현재 방 이름 : " + PN.CurrentRoom.Name);
            print("현재 방 인원 수 : " + PN.CurrentRoom.PlayerCount + "명");
            print("현재 방 MAX인원 : " + PN.CurrentRoom.MaxPlayers + "명");

            string playerStr = "방에 있는 플레이어 이름 \n";
            for (int i = 0; i < PN.PlayerList.Length; i++)
            {
                playerStr += PN.PlayerList[i].NickName + ", \n\t";
                print(playerStr);
            }
        }
        else
        {
            print("접속한 인원 수: " + PN.CountOfPlayers + "명");
            print("로비에 있는 여부: " + PN.InLobby);
            print("접속한 인원 수: " + PN.CountOfPlayers + "명");
            print("서버 연결여부: " + PN.IsConnected);
        }
    }

    public override void OnLeftRoom()
    {
        if (PN.IsMasterClient)
        {
            PN.DestroyAll();
            PN.RemoveBufferedRPCs();
        }

        PN.Destroy(spawnPlayer);
        SceneManager.LoadScene(0);
        //PN.LoadLevel(0);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName}님 현재인원:{PN.CurrentRoom.PlayerCount}");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName}님 현재인원:{PN.CurrentRoom.PlayerCount}");
    }

}
