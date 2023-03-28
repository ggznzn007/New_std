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
using Unity.VisualScripting;
using UnityEngine.XR;
using UnityEngine.LowLevel;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using static Unity.XR.PXR.PXR_HandPoseConfig.BonesRecognizer;

public class GunShootManager : MonoBehaviourPunCallbacks      // 아처 게임 관리 스크립트
{
    public static GunShootManager GSM;

    [Header("카운트다운 텍스트")]
    [SerializeField] TextMeshPro[] countText;
    [Header("게임 제한시간 텍스트")]
    public TextMeshPro[] timerText;
    public TextMeshPro[] resultText;
    [Header("레드팀 프리팹")]
    [SerializeField] GameObject redTeam;
    [Header("블루팀 프리팹")]
    [SerializeField] GameObject blueTeam;
    [Header("관리자")]
    public GameObject admin;
    [Header("Eagle NPC 프리팹")]                                           
    public GameObject eagleNPC;
    public Transform[] wayPos;
    public GameObject eagleBomb;
    public Transform spawnPoint;
    public GameObject myBomb = null; 
    [SerializeField] bool count = false;                             // 타이머 카운트 스위치
    [SerializeField] int limitedTime;                                // 게임제한시간
    public Transform adminPoint;                                     // 관리자 생성 위치

    public string GameInfo1;                                         // 게임 알림 오디오재생을 위한 문자열 "게임이 3초뒤에 시작됩니다."
    public string GameInfo2;                                         // 게임 알림 오디오재생을 위한 문자열 "3초 뒤에 다음스테이지로 이동합니다"
    public string GameInfo3;                                         // 게임 알림 오디오재생을 위한 문자열 "게임이 곧 종료됩니다."
    public string GameInfo4;                                         // 게임 알림 오디오재생을 위한 문자열 "블루팀이 승리하였습니다."
    public string GameInfo5;                                         // 게임 알림 오디오재생을 위한 문자열 "레드팀이 승리하였습니다."
    public string GameInfo6;                                         // 게임 알림 오디오재생을 위한 문자열 "무승부입니다."
    public string GameInfo7;                                         // 게임 알림 오디오재생을 위한 문자열 "score_result."
    public string GameInfo9;                                         // 게임 알림 오디오재생을 위한 문자열 "게임이 1분 남았습니다."
    public string one;                                               // 게임 알림 오디오재생을 위한 문자열 "1"
    public string two;                                               // 게임 알림 오디오재생을 위한 문자열 "2"
    public string three;                                             // 게임 알림 오디오재생을 위한 문자열 "3"
    public string gameStart;                                         // 게임 알림 오디오재생을 위한 문자열 "스타트"
    public string gameover;                                          // 게임 알림 오디오재생을 위한 문자열 "게임오버"
    public int kills;                                                // 킬 수
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
        GSM = this;
        DataManager.DM.currentMap = Map.TOY;
        SetScore();
    }

    private void Start()
    {
        //PN.UseRpcMonoBehaviourCache = true;
        PV = GetComponent<PhotonView>();
        if (PN.IsConnectedAndReady && PN.InRoom)
        {
            if (DataManager.DM.currentTeam != Team.ADMIN)  // 관리자 빌드 시 필요한 코드
            {
                Destroy(admin);
            }
            SpawnPlayer();
            if (PN.IsMasterClient)
            {
                PV.RPC(nameof(StartBtnT), RpcTarget.AllViaServer);                
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
                if (Application.platform == RuntimePlatform.WindowsPlayer)//|| Application.platform == RuntimePlatform.WindowsEditor)
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
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { PV.RPC("StartBtnT", RpcTarget.All); }
        if (Input.GetKeyDown(KeyCode.Backspace)) { PV.RPC("EndGameT", RpcTarget.All); }
        if (Input.GetKeyDown(KeyCode.Escape)) { StartCoroutine(nameof(ExitGame)); }
        /*  if (PN.IsConnectedAndReady && PN.InRoom && PN.IsMasterClient)  // 윈도우 프로그램 빌드 시
          {
              if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
              {


              }
          }*/
    }



    void FixedUpdate()
    {
        SetScore();
        Timer();
    }

    public void SetScore()
    {
        blueScore[0].text = score_BlueKill.ToString();   // 블루팀 점수
        blueScore[1].text = score_RedKill.ToString();    // 레드팀 점수
        redScore[0].text = score_BlueKill.ToString();    // 블루팀 점수
        redScore[1].text = score_RedKill.ToString();     // 레드팀 점수
    }
    public void UpdateStats()
    {
        playerProp["kills"] = kills;             // 킬 수                                                

        PN.LocalPlayer.CustomProperties = playerProp;
        PN.SetPlayerCustomProperties(playerProp);
    }

    public void Timer()
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

   /* public void SpawnEagle()
    {
        if (curEagle == null)
        {
            if (curEagle != null) return;
            curEagle = PN.InstantiateRoomObject(eagleNPC.name, wayPos[0].position, wayPos[0].rotation, 0);
        }
    }

    public void SpawnEB()
    {
        if (myBomb != null) return;
        if (myBomb == null)
        {
            myBomb = PN.InstantiateRoomObject(eagleBomb.name, eagleNPC.transform.position + new Vector3(0, 0.9f, 0), eagleNPC.transform.rotation, 0);
            myBomb.transform.SetParent(eagleNPC.transform, true);
            myBomb.GetComponentInChildren<Rigidbody>().useGravity = false;
            myBomb.GetComponentInChildren<Collider>().enabled = false;
        }
    }

    public void SpawnArrowB()
    {        
        if (curArrowB == null)
        {
            if (curArrowB != null) return;
            bool skilled = RandomArrow.RandArrowPer(50); // 30%
            if (skilled)
            {
                curArrowB = PN.InstantiateRoomObject(arrowSkilled.name, aSpawnPosition[0].position, aSpawnPosition[0].rotation, 0);
                Debug.Log("스킬 생성");
            }
            else
            {
                bool bomb = RandomArrow.RandArrowPer(100); // 35%, 35%
                if (bomb)
                {
                    curArrowB = PN.InstantiateRoomObject(arrowBomb.name, aSpawnPosition[0].position, aSpawnPosition[0].rotation, 0);
                    Debug.Log("폭탄 생성");
                }
                else
                {
                    curArrowB = PN.InstantiateRoomObject(arrowMulti.name, aSpawnPosition[0].position, aSpawnPosition[0].rotation, 0);
                    Debug.Log("멀티샷 생성");
                }
            }
        }
    }

    public void SpawnArrowBB()
    {
        if (curArrowBB == null)
        {
            if (curArrowBB != null) return;
            bool skilled = RandomArrow.RandArrowPer(50);// 10%
            if (skilled)
            {
                curArrowBB = PN.InstantiateRoomObject(arrowSkilled.name, aSpawnPosition[1].position, aSpawnPosition[1].rotation, 0);
                Debug.Log("스킬 생성");
            }
            else
            {
                bool bomb = RandomArrow.RandArrowPer(100);// 45%, 45%
                if (bomb)
                {
                    curArrowBB = PN.InstantiateRoomObject(arrowBomb.name, aSpawnPosition[1].position, aSpawnPosition[1].rotation, 0);
                    Debug.Log("폭탄 생성");
                }
                else
                {
                    curArrowBB = PN.InstantiateRoomObject(arrowMulti.name, aSpawnPosition[1].position, aSpawnPosition[1].rotation, 0);
                    Debug.Log("멀티샷 생성");
                }
            }
        }
    }

    public void SpawnArrowR()
    {
        if (curArrowR == null)
        {
            if (curArrowR != null) return;
            bool skilled = RandomArrow.RandArrowPer(50); 
            if (skilled)
            {
                curArrowR = PN.InstantiateRoomObject(arrowSkilled.name, aSpawnPosition[2].position, aSpawnPosition[2].rotation, 0);
                Debug.Log("스킬 생성");
            }
            else
            {
                bool bomb = RandomArrow.RandArrowPer(100);
                if (bomb)
                {
                    curArrowR = PN.InstantiateRoomObject(arrowBomb.name, aSpawnPosition[2].position, aSpawnPosition[2].rotation, 0);
                    Debug.Log("폭탄 생성");
                }
                else
                {
                    curArrowR = PN.InstantiateRoomObject(arrowMulti.name, aSpawnPosition[2].position, aSpawnPosition[2].rotation, 0);
                    Debug.Log("멀티샷 생성");
                }
            }
        }
    }
    public void SpawnArrowRR()
    {
        if (curArrowRR == null)
        {
            if (curArrowRR != null) return;
            bool skilled = RandomArrow.RandArrowPer(50); 
            if (skilled)
            {
                curArrowRR = PN.InstantiateRoomObject(arrowSkilled.name, aSpawnPosition[3].position, aSpawnPosition[3].rotation, 0);
                Debug.Log("스킬 생성");
            }
            else
            {
                bool bomb = RandomArrow.RandArrowPer(100);
                if (bomb)
                {
                    curArrowRR = PN.InstantiateRoomObject(arrowBomb.name, aSpawnPosition[3].position, aSpawnPosition[3].rotation, 0);
                    Debug.Log("폭탄 생성");
                }
                else
                {
                    curArrowRR = PN.InstantiateRoomObject(arrowMulti.name, aSpawnPosition[3].position, aSpawnPosition[3].rotation, 0);
                    Debug.Log("멀티샷 생성");
                }
            }
        }
    }*/


    [PunRPC]
    public void StartBtnT()
    {
        StartCoroutine(StartTimer());
    }

    [PunRPC]
    public void EndGameT()                   // "게임 종료"
    {
        StartCoroutine(LeaveGame());
    }

    [PunRPC]
    public void Notice()
    {
        AudioManager.AM.PlaySE(GameInfo3); // "게임이 곧 종료됩니다." 알림 오디오 재생
    }

    [PunRPC]
    public void Notice1min()
    {
        AudioManager.AM.PlaySE(GameInfo9); // "게임이 1분 남았습니다." 알림 오디오 재생
    }

    IEnumerator PlayTimer()
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
            PV.RPC(nameof(Notice), RpcTarget.AllViaServer);
        }

        if (limitedTime <= 0)
        {
            count = false;
            limitedTime = 0;
            PV.RPC(nameof(EndGameT), RpcTarget.AllViaServer);     // 게임 종료를 RPC로 전체 알림            
        }
    }

    public IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(4);                            
        AudioManager.AM.PlaySE(GameInfo1);                             // "게임이 3초 뒤에 시작됩니다." 알림 오디오 재생
        countText[0].text = string.Format("게임이 3초 뒤에 시작됩니다.");  // 블루팀 UI에 문자로 띄우기
        countText[1].text = string.Format("게임이 3초 뒤에 시작됩니다.");  // 레드팀 UI에 문자로 띄우기
        yield return new WaitForSeconds(3);                            
        AudioManager.AM.PlaySE(three);                                 // "Three" 알림 오디오 재생
        countText[0].text = string.Format("3");                        // 블루팀 UI에 문자로 띄우기
        countText[1].text = string.Format("3");                        // 레드팀 UI에 문자로 띄우기
        yield return new WaitForSeconds(1);                            
        AudioManager.AM.PlaySE(two);                                   // "Two" 알림 오디오 재생
        countText[0].text = string.Format("2");                        // 블루팀 UI에 문자로 띄우기
        countText[1].text = string.Format("2");                        // 레드팀 UI에 문자로 띄우기
        yield return new WaitForSeconds(1);                            
        AudioManager.AM.PlaySE(one);                                   // "One" 알림 오디오 재생
        countText[0].text = string.Format("1");                        // 블루팀 UI에 문자로 띄우기
        countText[1].text = string.Format("1");                        // 레드팀 UI에 문자로 띄우기
        yield return new WaitForSeconds(1);
        AudioManager.AM.PlaySE(gameStart);                             // "스타트" 알림 오디오 재생
        countText[0].text = string.Format("게임 스타트!!!");             // 블루팀 UI에 문자로 띄우기
        countText[1].text = string.Format("게임 스타트!!!");             // 레드팀 UI에 문자로 띄우기
        yield return new WaitForSeconds(1);
        count = true;                                                  // 게임 타이머 시작
        DataManager.DM.inGame = true;                                  // 게임 중 여부 데이터 저장
        timerText[0].gameObject.SetActive(true);                       // 블루팀 UI에 타이머 on
        timerText[1].gameObject.SetActive(true);                       // 레드팀 UI에 타이머 on
        countText[0].gameObject.SetActive(false);                      // 블루팀 UI에 안내문구 off
        countText[1].gameObject.SetActive(false);                      // 레드팀 UI에 안내문구 off
    }

    public IEnumerator LeaveGame()
    {
        timerText[0].gameObject.SetActive(false);                      // 블루팀 UI에 타이머 off
        timerText[1].gameObject.SetActive(false);                      // 레드팀 UI에 타이머 off
        resultText[0].gameObject.SetActive(true);                      // 블루팀 UI에 결과 on
        resultText[1].gameObject.SetActive(true);                      // 레드팀 UI에 결과 on
        DataManager.DM.inGame = false;                                 // 게임 중 여부 데이터 해제
        DataManager.DM.gameOver = true;                                // 게임오버 여부 true
        AudioManager.AM.PlaySE(gameover);                              // "게임오버" 알림 오디오 재생
        gameOverImage[0].gameObject.SetActive(true);                   // 블루팀 UI에 게임오버 이미지 on
        gameOverImage[1].gameObject.SetActive(true);                   // 레드팀 UI에 게임오버 이미지 on   
        yield return new WaitForSeconds(2);
        VictoryTeam();                                                 // 게임 결과 판정 메서드 호출(승리,무승부)
        yield return new WaitForSeconds(3);
        AudioManager.AM.PlaySE(GameInfo7);                             // "게임결과 발표 후" 알림 오디오 재생
        gameOverImage[0].gameObject.SetActive(false);                  // 블루팀 UI에 게임오버 이미지 off
        gameOverImage[1].gameObject.SetActive(false);                  // 레드팀 UI에 게임오버 이미지 off  
        yield return new WaitForSeconds(3);
        countText[0].gameObject.SetActive(true);                       // 블루팀 UI에 안내문구 on
        countText[1].gameObject.SetActive(true);                       // 레드팀 UI에 안내문구 on
        AudioManager.AM.PlaySE(GameInfo2);                             // "3초 뒤에 다음 스테이지로 이동합니다" 알림 오디오 재생
        countText[0].text = string.Format("3초 뒤에 다음 스테이지로 이동합니다");
        countText[1].text = string.Format("3초 뒤에 다음 스테이지로 이동합니다");
        yield return new WaitForSeconds(5);
        PN.LeaveRoom();
        StopCoroutine(LeaveGame());                                    // 방 나가기 메서드 호출
    }

    public void VictoryTeam()
    {
        if (score_BlueKill > score_RedKill)                                   // 블루팀 승리 시
        {
            AudioManager.AM.PlaySE(GameInfo4);                                // "블루팀이 승리하였습니다." 알림 오디오 재생            
            blueScore[0].gameObject.SetActive(false);                         // 블루팀 UI에 블루팀 스코어 off
            blueScore[1].gameObject.SetActive(false);                         // 레드팀 UI에 블루팀 스코어 off
            redScore[0].gameObject.SetActive(false);                          // 블루팀 UI에 레드팀 스코어 off
            redScore[1].gameObject.SetActive(false);                          // 레드팀 UI에 레드팀 스코어 off 
            bluewinImg[0].gameObject.SetActive(true);                         // 블루팀 UI에 블루팀 Win 이미지 on
            bluewinImg[1].gameObject.SetActive(true);                         // 레드팀 UI에 블루팀 Win 이미지 on
            redloseImg[0].gameObject.SetActive(true);                         // 블루팀 UI에 레드팀 Lose 이미지 on
            redloseImg[1].gameObject.SetActive(true);                         // 레드팀 UI에 레드팀 Lose 이미지 on
        }
        else if (score_BlueKill < score_RedKill)                              // 레드팀 승리 시
        {
            AudioManager.AM.PlaySE(GameInfo5);                                // "레드팀이 승리하였습니다." 알림 오디오 재생 
            blueScore[0].gameObject.SetActive(false);                         // 블루팀 UI에 블루팀 스코어 off
            blueScore[1].gameObject.SetActive(false);                         // 레드팀 UI에 블루팀 스코어 off
            redScore[0].gameObject.SetActive(false);                          // 블루팀 UI에 레드팀 스코어 off
            redScore[1].gameObject.SetActive(false);                          // 레드팀 UI에 레드팀 스코어 off 
            blueloseImg[0].gameObject.SetActive(true);                        // 블루팀 UI에 블루팀 Lose 이미지 on
            blueloseImg[1].gameObject.SetActive(true);                        // 레드팀 UI에 블루팀 Lose 이미지 on
            redwinImg[0].gameObject.SetActive(true);                          // 블루팀 UI에 레드팀 Win 이미지 on
            redwinImg[1].gameObject.SetActive(true);                          // 레드팀 UI에 레드팀 Win 이미지 on
        }
        else if (score_BlueKill == score_RedKill)                             // 무승부 시
        {
            AudioManager.AM.PlaySE(GameInfo6);                                // "무승부입니다." 알림 오디오 재생 
            blueScore[0].gameObject.SetActive(false);                         // 블루팀 UI에 블루팀 스코어 off
            blueScore[1].gameObject.SetActive(false);                         // 레드팀 UI에 블루팀 스코어 off
            redScore[0].gameObject.SetActive(false);                          // 블루팀 UI에 레드팀 스코어 off
            redScore[1].gameObject.SetActive(false);                          // 레드팀 UI에 레드팀 스코어 off
            drawImg[0].gameObject.SetActive(true);                            // 블루팀 UI에 레드팀 Draw 이미지 on
            drawImg[1].gameObject.SetActive(true);                            // 레드팀 UI에 레드팀 Draw 이미지 on
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
        DataManager.DM.isReady = false;
        SceneManager.LoadScene(0);
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

   /* [PunRPC]
    public void Ready1()
    {
        DataManager.DM.gameOver = true;
    }
*/
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
