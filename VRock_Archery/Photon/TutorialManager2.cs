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
using UnityEngine.XR;

public class TutorialManager2 : MonoBehaviourPunCallbacks
{
    public static TutorialManager2 TM2;

    [SerializeField] GameObject redTeam;                // 레드팀 프리팹
    [SerializeField] GameObject blueTeam;               // 블루팀 프리팹
    [SerializeField] GameObject admin;                  // 관리자 프리팹
    public GameObject eagleNPC;
    //public GameObject eagleBlock;
    public Transform adminPoint;                        // 관리자 생성위치
    public Transform eaglePoint;
    private GameObject spawnPlayer;                     // 생성되는 플레이어
    public GameObject snowBlock;                             // 생성되는 폭탄   
    public Transform[] blockPoint;    
    private float curTime;
    private float limit = 35;                                // 스노우 블럭 생성 딜레이

    private void Awake()
    {
        TM2 = this;
    }
    private void Start()
    {
        DataManager.DM.isReady = false;
        if (!PN.IsConnectedAndReady)
        {
            SceneManager.LoadScene(0);
        }
        if (PN.IsConnectedAndReady && PN.InRoom)
        {            
            SpawnPlayer();

           /* if (DataManager.DM.currentTeam != Team.ADMIN)     // 관리자 빌드시 필요한 코드
            {
                Destroy(admin);
            }*/

           
        }
    }
    private void Update()
    {
        // 윈도우 프로그램 빌드 시
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (!DataManager.DM.isReady)
            {
                DataManager.DM.isReady = true;
                photonView.RPC(nameof(Ready2), RpcTarget.AllViaServer);
            }
            else if (DataManager.DM.isReady)
            {
                DataManager.DM.isReady = false;
                PN.LoadLevel(4);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) { StartCoroutine(nameof(ExitGame)); }

        if(PN.IsMasterClient)
        {
            SpawnBlock();            
        }
        
    }

    public void SpawnPlayer()
    {
        switch (DataManager.DM.currentTeam)
        {
            case Team.RED:
                PN.AutomaticallySyncScene = true;
                DataManager.DM.inGame = false;
                DataManager.DM.gameOver = false;
                spawnPlayer = PN.Instantiate(redTeam.name, Vector3.zero, Quaternion.identity);
                Debug.Log($"{PN.CurrentRoom.Name} 방에 레드팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                Info();
                break;

            case Team.BLUE:
                PN.AutomaticallySyncScene = true;
                DataManager.DM.inGame = false;
                DataManager.DM.gameOver = false;
                spawnPlayer = PN.Instantiate(blueTeam.name, Vector3.zero, Quaternion.identity);
                Debug.Log($"{PN.CurrentRoom.Name} 방에 블루팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                Info();
                break;

            // 윈도우 프로그램 빌드 시
            case Team.ADMIN:
                if (Application.platform == RuntimePlatform.WindowsPlayer)// || Application.platform == RuntimePlatform.WindowsEditor)
                {
                    if (Application.platform != RuntimePlatform.WindowsPlayer) { return; }
                    PN.AutomaticallySyncScene = true;
                    DataManager.DM.inGame = false;
                    DataManager.DM.gameOver = false;
                    spawnPlayer = PN.Instantiate(admin.name, adminPoint.position, adminPoint.rotation);
                    Debug.Log($"{PN.CurrentRoom.Name} 방에 관리자{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                    Info();
                }
                break;

            default:
                return;
        }
    }

    [PunRPC]
    public void Ready2()
    {
        DataManager.DM.gameOver = true;
    }

    public IEnumerator ExitGame()
    {
        yield return new WaitForSeconds(1);
        photonView.RPC(nameof(ForceOff), RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void ForceOff()
    {
        Application.Quit();
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Application.Quit();
            if (PN.IsMasterClient)
            {
                PN.DestroyAll();
            }
            PN.Destroy(spawnPlayer);
        }
    }   
   
    public void SpawnBlock()                                                                       // 정해진 시간마다 생성되는 눈블럭
    {
        curTime += Time.deltaTime;

        if(curTime>=limit)
        {
            for (int i = 0; i < blockPoint.Length; i++)
            {
                PN.InstantiateRoomObject(snowBlock.name, blockPoint[i].position, blockPoint[i].rotation, 0);
                curTime = 0;
            }
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
