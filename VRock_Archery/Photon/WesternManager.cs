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
using Unity.XR.PXR;
using Unity.VisualScripting;

public class WesternManager : MonoBehaviourPunCallbacks
{
    public static WesternManager WM;

    [Header("카운트다운 텍스트")]
    [SerializeField] TextMeshPro[] countText;
    [Header("게임 제한시간")]
    public TextMeshPro[] timerText;
    public TextMeshPro[] btimerText;
    public TextMeshPro[] resultText;
    [Header("레드팀 프리팹")]
    [SerializeField] GameObject redTeam;
    [Header("블루팀 프리팹")]
    [SerializeField] GameObject blueTeam;
    [Header("관리자")]
    public GameObject admin;
    [Header("스노우블럭 프리팹")]
    public GameObject snowBlock;
    [Header("Eagle NPC 프리팹")]
    public GameObject eagleNPC;  

    public Transform[] blockPoint;                                   // 스노우 블럭 생성 위치
    [SerializeField] bool count = false;                             // 타이머 카운트 스위치
    [SerializeField] bool bCount = false;                            // 타이머 카운트 스위치_빌드타임
    [SerializeField] int limitedTime;                                // 게임제한시간
    [SerializeField] float buildTime;                                // 빌드제한시간_ 스노우 블럭 쌓는시간
    public Transform adminPoint;                                     // 관리자 생성 위치
    public string BuildTime;                                         // 게임 알림 오디오재생을 위한 문자열 "빌드타임에 대한 설명 멘트"
    public string BuildTime2;                                        // 게임 알림 오디오재생을 위한 문자열 "빌드 타임!"
    public string GameInfo1;                                         // 게임 알림 오디오재생을 위한 문자열 "게임이 3초뒤에 시작됩니다."
    public string GameInfo3;                                         // 게임 알림 오디오재생을 위한 문자열 "게임이 곧 종료됩니다."
    public string GameInfo4;                                         // 게임 알림 오디오재생을 위한 문자열 "블루팀이 승리하였습니다."
    public string GameInfo5;                                         // 게임 알림 오디오재생을 위한 문자열 "레드팀이 승리하였습니다."
    public string GameInfo6;                                         // 게임 알림 오디오재생을 위한 문자열 "무승부입니다."
    public string GameInfo7;                                         // 게임 알림 오디오재생을 위한 문자열 "score_result."
    public string GameInfo8;                                         // 게임 알림 오디오재생을 위한 문자열 "게임이 종료되었습니다. 헤드셋을 벗어주세요."
    public string GameInfo9;                                         // 게임 알림 오디오재생을 위한 문자열 "게임이 1분 남았습니다."
    public string one;                                               // 게임 알림 오디오재생을 위한 문자열 "1"
    public string two;                                               // 게임 알림 오디오재생을 위한 문자열 "2"
    public string three;                                             // 게임 알림 오디오재생을 위한 문자열 "3"
    public string gameStart;                                         // 게임 알림 오디오재생을 위한 문자열 "스타트"
    public string gameover;                                          // 게임 알림 오디오재생을 위한 문자열 "게임오버"
    public int kills;                                                // 킬 수
    public Image[] buildTImage;                                      // 양쪽 UI에 나오는 빌드타임 이미지
    public Image[] gameOverImage;                                    // 양쪽 UI에 나오는 게임오버 이미지
    public Image[] bluewinImg;                                       // 양쪽 UI에 나오는 블루팀 이겼을 때 이미지
    public Image[] redwinImg;                                        // 양쪽 UI에 나오는 레드팀 이겼을 때 이미지
    public Image[] blueloseImg;                                      // 양쪽 UI에 나오는 블루팀 졌을 때 이미지
    public Image[] redloseImg;                                       // 양쪽 UI에 나오는 레드팀 졌을 때 이미지
    public Image[] drawImg;                                          // 양쪽 UI에 나오는 비겼을 때 이미지
    public TMP_Text[] blueScore;                                     // 양쪽 UI에 나오는 블루팀 점수 텍스트
    public TMP_Text[] redScore;                                      // 양쪽 UI에 나오는 레드팀 점수 텍스트
    public int score_BlueKill;                                       // 양쪽 UI에 나오는 블루팀 점수
    public int score_RedKill;                                        // 양쪽 UI에 나오는 레드팀 점수    
    PhotonView PV;                                                   // 포톤뷰
    Hashtable setTime = new Hashtable();                             // 게임시간
    private GameObject spawnPlayer;                                  // 생성되는 플레이어
    private ExitGames.Client.Photon.Hashtable playerProp = new ExitGames.Client.Photon.Hashtable();

    private void Awake()
    {
        WM = this;
        DataManager.DM.currentMap = Map.WESTERN;
        SetScore();
        
    }

    void Start()
    {
        DataManager.DM.inBuild = true;
        DataManager.DM.activeBall = false;
        PV = GetComponent<PhotonView>();
        if (PN.IsConnectedAndReady && PN.InRoom)
        {
            SpawnPlayer();

            if (PN.IsMasterClient)
            {                
                PV.RPC(nameof(StartBtnW), RpcTarget.AllViaServer);                              // 타이머 동기화                                                                                                
                SpawnBlock();                                                                  // 스노우블럭 시작시  지정위치에 생성 총 10개
            }

            if (DataManager.DM.currentTeam != Team.ADMIN)      // 관리자 빌드시 필요한 코드
            {
                Destroy(admin);
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
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    if (Application.platform != RuntimePlatform.WindowsPlayer) { return; }
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { PV.RPC("StartBtnW", RpcTarget.All); }
        if (Input.GetKeyDown(KeyCode.Backspace)) { PV.RPC("EndGameW", RpcTarget.All); }
        if (Input.GetKeyDown(KeyCode.Escape)) { StartCoroutine(nameof(ExitGame)); }      

        /*if (PN.IsConnectedAndReady && PN.InRoom && PN.IsMasterClient)  // 윈도우 프로그램 빌드 시
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                
            }
        }*/
    }

    void FixedUpdate()
    {
        SetScore(); // 스코어 셋팅
        TimerW();   // 게임 타이머 메서드 호출
        TimerB();   // 빌드 타이머 메서드 호출 
    }

    public void SetScore()
    {
        blueScore[0].text = score_BlueKill.ToString();   // 블루팀 점수
        blueScore[1].text = score_RedKill.ToString();     // 레드팀 점수
        redScore[0].text = score_BlueKill.ToString();   // 블루팀 점수
        redScore[1].text = score_RedKill.ToString();     // 레드팀 점수
    }

    public void UpdateStats()
    {
        playerProp["kills"] = kills;             // 개인 킬 수
                                                 // playerProp["deaths"] = deaths;           // 개인 데스 수
        PN.LocalPlayer.CustomProperties = playerProp;
        PN.SetPlayerCustomProperties(playerProp);
    }

    public void TimerW()
    {
        if (PN.InRoom && PN.IsConnectedAndReady)
        {
            limitedTime = (int)PN.CurrentRoom.CustomProperties["Time"];
            limitedTime = limitedTime < 0 ? 0 : limitedTime;
            float min = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] / 60);
            float sec = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] % 60);
            timerText[0].text = string.Format("{0:00} : {1:00}", min, sec);
            timerText[1].text = string.Format("{0:00} : {1:00}", min, sec);

            if (limitedTime < 60)
            {
                timerText[0].text = string.Format("{0:0}", sec);
                timerText[1].text = string.Format("{0:0}", sec);
            }
            if (PN.IsMasterClient)
            {
                if (count)
                {
                    count = false;
                    StartCoroutine(PlayTimer());
                }
            }
        }
    }

    public void TimerB()
    {
        if (PN.InRoom && PN.IsConnectedAndReady)
        {
            btimerText[0].text = Mathf.Round(buildTime).ToString();
            btimerText[1].text = Mathf.Round(buildTime).ToString();
            if (PN.IsMasterClient)
            {
                if (bCount)
                {
                    bCount = false;
                    PV.RPC(nameof(CountB), RpcTarget.AllViaServer);
                }
            }
        }
    }

    [PunRPC]
    public void CountB()
    {
        StartCoroutine(BuildTimer());
    }

    public void SpawnBlock()  // 스노우블럭 생성 메서드                                                                      
    {
        for (int i = 0; i < blockPoint.Length; i++)
        {
            PN.InstantiateRoomObject(snowBlock.name, blockPoint[i].position, blockPoint[i].rotation, 0);          
        }      
    }

    [PunRPC]
    public void StartBtnW()
    {
        StartCoroutine(StartTimer());
    }

    [PunRPC]
    public void EndGameW()                   // "게임 종료"
    {
        StartCoroutine(LeaveGame());
    }

    [PunRPC]
    public void NoticeW()
    {
        AudioManager.AM.PlaySE(GameInfo3); // "게임이 곧 종료됩니다." 알림 오디오 재생
    }

    [PunRPC]
    public void Notice1min()
    {
        AudioManager.AM.PlaySE(GameInfo9); // "게임이 1분 남았습니다." 알림 오디오 재생
    }

    public IEnumerator PlayTimer()
    {
        yield return new WaitForSeconds(1);
        int nextTime = limitedTime -= 1;
        setTime["Time"] = nextTime;
        PN.CurrentRoom.SetCustomProperties(setTime);
        count = true;

        if (limitedTime == 60)
        {
            PV.RPC(nameof(Notice1min), RpcTarget.AllViaServer);   // 게임 끝나기 1분 전 알림 RPC로 전체 알림
        }

        if (limitedTime == 8)                                     // 게임 끝나기 8초전에 알림 RPC로 전체 알림
        {
            PV.RPC(nameof(NoticeW), RpcTarget.AllViaServer);
        }

        if (limitedTime <= 0)
        {
            count = false;
            limitedTime = 0;
            PV.RPC(nameof(EndGameW), RpcTarget.AllViaServer);     // 게임 종료를 RPC로 전체 알림            
        }
    }


    public IEnumerator BuildTimer()
    {
        yield return new WaitForSeconds(1);
        bCount = true;
        buildTime -= 1;
        if (buildTime < 0)
        {
            buildTime = 0;
            bCount = false;
            StartCoroutine(RealTimer());
        }

    }

    IEnumerator StartTimer()                                           // 빌드 타임 타이머
    {
        yield return new WaitForSeconds(4);
        AudioManager.AM.PlaySE(BuildTime2);                            // "빌드 타임 !" 알림 오디오 재생
        countText[0].gameObject.SetActive(false);                      // 블루팀 UI에 알림문구 off
        countText[1].gameObject.SetActive(false);                      // 레드팀 UI에 알림문구 off
        buildTImage[0].gameObject.SetActive(true);                     // 블루팀 UI에 빌드타임 이미지 on
        buildTImage[1].gameObject.SetActive(true);                     // 레드팀 UI에 빌드타임 이미지 on
        DataManager.DM.inBuild = true;                                 // 빌드 타임 중 여부 데이터 저장
        yield return new WaitForSeconds(1);
        AudioManager.AM.PlaySE(BuildTime);                             // "빌드 타임에 대한 멘트 재생" 알림 오디오 재생
        buildTime = 30;                                                // 빌드제한시간
        bCount = true;                                                 // 빌드 타이머 시작
    }

    public IEnumerator RealTimer()                                     // 게임 타이머
    {
        buildTImage[0].gameObject.SetActive(false);                    // 블루팀 UI에 빌드타임 이미지 off
        buildTImage[1].gameObject.SetActive(false);                    // 레드팀 UI에 빌드타임 이미지 off
        btimerText[0].gameObject.SetActive(false);                     // 블루팀 UI에 빌드타임 타이머 문자 off
        btimerText[1].gameObject.SetActive(false);                     // 레드팀 UI에 빌드타임 타이머 문자 off
        countText[0].gameObject.SetActive(true);                       // 블루팀 UI에 안내문구 on
        countText[1].gameObject.SetActive(true);                       // 레드팀 UI에 안내문구 on
        yield return new WaitForSeconds(1);
        AudioManager.AM.PlaySE(GameInfo1);                             // "게임이 3초 뒤에 시작됩니다." 알림 오디오 재생
        countText[0].text = string.Format("게임이 3초 뒤에 시작됩니다.");
        countText[1].text = string.Format("게임이 3초 뒤에 시작됩니다.");
        yield return new WaitForSeconds(3);
        AudioManager.AM.PlaySE(three);                                 // "Three" 알림 오디오 재생
        countText[0].text = string.Format("3");
        countText[1].text = string.Format("3");
        yield return new WaitForSeconds(1);
        AudioManager.AM.PlaySE(two);                                   // "Two" 알림 오디오 재생
        countText[0].text = string.Format("2");
        countText[1].text = string.Format("2");
        yield return new WaitForSeconds(1);
        AudioManager.AM.PlaySE(one);                                   // "One" 알림 오디오 재생
        countText[0].text = string.Format("1");
        countText[1].text = string.Format("1");
        yield return new WaitForSeconds(1);
        AudioManager.AM.PlaySE(gameStart);                             // "스타트" 알림 오디오 재생
        countText[0].text = string.Format("게임 스타트!!!");
        countText[1].text = string.Format("게임 스타트!!!");
        DataManager.DM.inBuild = false;                                // 빌드 타임 중 여부 데이터 해제
        DataManager.DM.activeBall = true;
        yield return new WaitForSeconds(1);
        count = true;                                                  // 게임 타이머 시작
        DataManager.DM.inGame = true;                                  // 게임 중 여부 데이터 저장
        timerText[0].gameObject.SetActive(true);
        timerText[1].gameObject.SetActive(true);
        countText[0].gameObject.SetActive(false);
        countText[1].gameObject.SetActive(false);
    }

    public IEnumerator LeaveGame()
    {
        timerText[0].gameObject.SetActive(false);
        timerText[1].gameObject.SetActive(false);
        resultText[0].gameObject.SetActive(true);
        resultText[1].gameObject.SetActive(true);
        DataManager.DM.inGame = false;                                 // 게임 중 여부 데이터 해제
        DataManager.DM.gameOver = true;                                // 게임오버 여부 true
        AudioManager.AM.PlaySE(gameover);                              // "게임오버" 알림 오디오 재생
        gameOverImage[0].gameObject.SetActive(true);
        gameOverImage[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        VictoryTeam();                                                 // 게임 결과 판정 메서드 호출(승리,무승부)
        yield return new WaitForSeconds(3);
        AudioManager.AM.PlaySE(GameInfo7);                             // "게임결과 발표 후" 알림 오디오 재생
        gameOverImage[0].gameObject.SetActive(false);
        gameOverImage[1].gameObject.SetActive(false);
        yield return new WaitForSeconds(3);
        countText[0].gameObject.SetActive(true);
        countText[1].gameObject.SetActive(true);
        AudioManager.AM.PlaySE(GameInfo8);                           // "게임이 종료되었습니다." 알림 오디오 재생
        countText[0].text = string.Format("게임이 종료되었습니다\n 헤드셋을 벗어주세요");
        countText[1].text = string.Format("게임이 종료되었습니다\n 헤드셋을 벗어주세요");
        yield return new WaitForSeconds(4);
        PN.LeaveRoom();
        StopCoroutine(LeaveGame());                                    // 방 나가기 메서드 호출
    }

    public void VictoryTeam()
    {
        if (score_BlueKill > score_RedKill)                                   // 블루팀 승리 시
        {
            AudioManager.AM.PlaySE(GameInfo4);                                // "블루팀이 승리하였습니다." 알림 오디오 재생
            blueScore[0].gameObject.SetActive(false);
            blueScore[1].gameObject.SetActive(false);
            redScore[0].gameObject.SetActive(false);
            redScore[1].gameObject.SetActive(false);
            bluewinImg[0].gameObject.SetActive(true);
            bluewinImg[1].gameObject.SetActive(true);
            redloseImg[0].gameObject.SetActive(true);
            redloseImg[1].gameObject.SetActive(true);

        }
        else if (score_BlueKill < score_RedKill)                              // 레드팀 승리 시
        {
            AudioManager.AM.PlaySE(GameInfo5);                                // "레드팀이 승리하였습니다." 알림 오디오 재생 
            blueScore[0].gameObject.SetActive(false);
            blueScore[1].gameObject.SetActive(false);
            redScore[0].gameObject.SetActive(false);
            redScore[1].gameObject.SetActive(false);
            blueloseImg[0].gameObject.SetActive(true);
            blueloseImg[1].gameObject.SetActive(true);
            redwinImg[0].gameObject.SetActive(true);
            redwinImg[1].gameObject.SetActive(true);
        }
        else if (score_BlueKill == score_RedKill)                             // 무승부 시
        {
            AudioManager.AM.PlaySE(GameInfo6);                                // "무승부입니다." 알림 오디오 재생 
            blueScore[0].gameObject.SetActive(false);
            blueScore[1].gameObject.SetActive(false);
            redScore[0].gameObject.SetActive(false);
            redScore[1].gameObject.SetActive(false);
            drawImg[0].gameObject.SetActive(true);
            drawImg[1].gameObject.SetActive(true);
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

    [ContextMenu("포톤 서버 정보")]
    void Info()
    {
        if (PN.InRoom)
        {
            print("현재 방 이름: " + PN.CurrentRoom.Name);
            print("현재 방 인원 수: " + PN.CurrentRoom.PlayerCount);
            print("현재 방 MAX인원: " + PN.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 \n";
            for (int i = 0; i < PN.PlayerList.Length; i++)
            {
                playerStr += PN.PlayerList[i].NickName + ", ";
                print(playerStr);
            }

        }
        else
        {
            print("접속한 인원 수: " + PN.CountOfPlayers);
            print("로비에 있는 여부: " + PN.InLobby);
            print("접속한 인원 수: " + PN.CountOfPlayers);
            print("서버 연결여부: " + PN.IsConnected);
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
}
