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

public class TutorialManager : MonoBehaviourPunCallbacks  // 아처 튜토리얼 포톤 관리 스크립트
{
    public static TutorialManager TM;

    [SerializeField] GameObject redTeam;                // 레드팀 프리팹
    [SerializeField] GameObject blueTeam;               // 블루팀 프리팹
    [SerializeField] GameObject defaultTeam;            // 디폴트 프리팹 - 예외가 생겼을 때
    [SerializeField] GameObject admin;                  // 관리자 프리팹
    public GameObject eagleNPC;                         // 독수리
    public Transform adminPoint;                        // 관리자 생성위치
    public Transform eaglePoint;                        // 독수리 첫 위치 -> 웨이포인트의 첫 포인트 
    public Transform[] wayPos;                          // 독수리 이동경로
    public GameObject eagleBomb;                        // 독수리 밑에 생성되는 폭탄 프리팹
    public Transform spawnPoint;                        // 독수리 폭탄 생성 포인트
    public GameObject myBomb = null;                    // 독수리 폭탄 초기화

    private PhotonView PV;                              // 포톤뷰
    private GameObject spawnPlayer;                     // 현재 생성되는 플레이어   

    private void Awake()
    {
        TM = this;
        PV = GetComponent<PhotonView>();
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
            if (DataManager.DM.currentTeam != Team.ADMIN)  // 관리자 빌드시 필요한 코드
            {
                Destroy(admin);                            // 현재 관리자가 아니면 관리자를 삭제
            }            

            switch (PN.CurrentRoom.PlayerCount)            // 현재방에 플레이어 들어오는 순서에 따라 닉네임 배급
            {
                case 1:
                    PN.LocalPlayer.NickName = "Master";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 2:
                    PN.LocalPlayer.NickName = "Player 1";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 3:
                    PN.LocalPlayer.NickName = "Player 2";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 4:
                    PN.LocalPlayer.NickName = "Player 3";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 5:
                    PN.LocalPlayer.NickName = "Player 4";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 6:
                    PN.LocalPlayer.NickName = "Player 5";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                default:
                    PN.LocalPlayer.NickName = "Master Player";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
            }
        }
    }

    private void Update()
    {       
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (!DataManager.DM.isReady)
            {
                DataManager.DM.isReady = true;
                photonView.RPC(nameof(Ready1), RpcTarget.AllViaServer);
            }
            else if (DataManager.DM.isReady)
            {
                DataManager.DM.isReady = false;
                PN.LoadLevel(2);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) { StartCoroutine(nameof(ExitGame)); }        
       
    }   

    public void SpawnPlayer()                   // 플레이어 생성 메서드
    {
        switch (DataManager.DM.currentTeam)     // 선택한 팀에 따라 해당 포톤 플레이어 생성
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

            // 윈도우 프로그램 빌드 시 PC버전 관리자
            case Team.ADMIN:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    if(Application.platform != RuntimePlatform.WindowsPlayer) { return; }
                    PN.AutomaticallySyncScene = true;
                    DataManager.DM.inGame = false;                    
                    DataManager.DM.gameOver = false;
                    spawnPlayer = PN.Instantiate(admin.name, adminPoint.position, adminPoint.rotation);
                    spawnPlayer.transform.SetParent(adminPoint.transform, true);
                    Debug.Log($"{PN.CurrentRoom.Name} 방에 관리자{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                    Info();
                }
                break;

            default:
                return;
        }
    }   

    [PunRPC]
    public void Ready1()    // 게임 준비
    {
        DataManager.DM.gameOver = true;
    }

    public IEnumerator ExitGame()  // 게임 강제 종료 - 모든플레이어 한꺼번에 아웃
    {
        yield return new WaitForSeconds(1);
        photonView.RPC(nameof(ForceOff), RpcTarget.AllViaServer);
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
}
