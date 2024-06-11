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

public class WesternManager : MonoBehaviourPunCallbacks
{
    public static WesternManager WM;

    [Header("카운트다운 텍스트")]
    [SerializeField] TextMeshPro countText;
    [Header("게임 제한시간")]
    public TextMeshPro timerText;
    public TextMeshPro resultText;
    [Header("레드팀 프리팹")]
    [SerializeField] GameObject redTeam;
    [Header("블루팀 프리팹")]
    [SerializeField] GameObject blueTeam;
    [Header("관리자")]
    public GameObject admin;
    [Header("폭탄 프리팹")]
    public GameObject bomB;
    [Header("NPC 프리팹")]                                      // 폭탄을 생성해주는 NPC => 카우보이
    public GameObject npc;
    public GameObject shield;                             // 방패 프리팹
    private GameObject spawnPlayer;
    [SerializeField] bool count = false;
    [SerializeField] int limitedTime;
    Hashtable setTime = new Hashtable();
    PhotonView PV;
    public Transform[] bSpawnPosition;
    public Transform adminPoint;
    public string GameInfo1;
    public string GameInfo2;
    public string one;
    public string two;
    public string three;
    public string startGame;
    public string gameover;
    public int kills;
    private ExitGames.Client.Photon.Hashtable playerProp = new ExitGames.Client.Photon.Hashtable();
    public Image gameOverImage;
    public Image bluewinImg;
    public Image redwinImg;
    public Image blueloseImg;
    public Image redloseImg;
    public Image drawImg;
    public TMP_Text blueScore;
    public TMP_Text redScore;
    public int score_BlueKill;
    public int score_RedKill;
    private GameObject bombRed;
    private GameObject bombBlue;
    private GameObject barrels_Blue;
    private GameObject barrels_Red;

    private void Awake()
    {
        WM = this;
        DataManager.DM.currentMap = Map.WESTERN;
        SetScore();
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PN.IsConnectedAndReady && PN.InRoom)
        {
            SpawnPlayer();

            if (PN.IsMasterClient)
            {
                PV.RPC(nameof(StartBtnW), RpcTarget.AllViaServer);
                PN.InstantiateRoomObject(npc.name, new Vector3(8.25f, 0.02f, -0.62f), Quaternion.identity);     // 말
                Invoke(nameof(SpawnShield), 2);
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
      /*  if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnDynamite();
        }*/
        /*if (PN.IsConnectedAndReady && PN.InRoom && PN.IsMasterClient) // 윈도우 프로그램 빌드 시
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { PV.RPC("StartBtnW", RpcTarget.All); }
                else if (Input.GetKeyDown(KeyCode.Backspace)) { PV.RPC("EndGameW", RpcTarget.All); }
                else if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    SpawnDynamite();
                }
            }
        }     */
    }

    void FixedUpdate()
    {
        SetScore();
        TimerW();
    }

    public void SetScore()
    {
        blueScore.text = score_BlueKill.ToString();   // 블루팀 점수
        redScore.text = score_RedKill.ToString();     // 레드팀 점수
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
            timerText.text = string.Format("{0:00} : {1:00}", min, sec);
            if (limitedTime < 60)
            {
                timerText.text = string.Format("{0:0}", sec);
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

    public void SpawnDynamite()
    {
        bombBlue = PN.InstantiateRoomObject(bomB.name, bSpawnPosition[0].position, bSpawnPosition[0].rotation, 0);
        bombRed = PN.InstantiateRoomObject(bomB.name, bSpawnPosition[1].position, bSpawnPosition[1].rotation, 0);
        /* bombBlue = PN.Instantiate(bomB.name, bSpawnPosition[0].position, bSpawnPosition[0].rotation, 0);
         bombRed  = PN.Instantiate(bomB.name, bSpawnPosition[1].position, bSpawnPosition[1].rotation, 0);*/
        /*PN.Instantiate(bomB.name, bSpawnPosition[0].position, bSpawnPosition[0].rotation, 0);
        PN.Instantiate(bomB.name, bSpawnPosition[1].position, bSpawnPosition[1].rotation, 0); */
    }

    public void SpawnShield()
    {
        barrels_Blue = PN.InstantiateRoomObject(shield.name, bSpawnPosition[0].position, bSpawnPosition[0].rotation, 0);
        barrels_Red = PN.InstantiateRoomObject(shield.name, bSpawnPosition[1].position, bSpawnPosition[1].rotation, 0);
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

    [PunRPC]
    public void StartBtnW()
    {
        StartCoroutine(StartTimer());
    }

    [PunRPC]
    public void EndGameW()
    {
        StartCoroutine(LeaveGame());
    }

    [PunRPC]
    public void NoticeW()
    {
        AudioManager.AM.PlaySE("GameInfo3");
    }

    [PunRPC]
    public void Notice1minW()
    {
        AudioManager.AM.PlaySE("GameInfo9");
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
            PV.RPC(nameof(Notice1minW), RpcTarget.AllViaServer);   // 게임 끝나기 60초전 알림
        }

        if (limitedTime == 8)
        {
            PV.RPC(nameof(NoticeW), RpcTarget.AllViaServer);
        }

        if (limitedTime <= 0)
        {
            count = false;
            limitedTime = 0;
            PV.RPC(nameof(EndGameW), RpcTarget.AllViaServer);
            Debug.Log("타임오버");
        }
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(4);
        AudioManager.AM.PlaySE("GameInfo1");
        countText.text = string.Format("게임이 3초 뒤에 시작됩니다.");
        yield return new WaitForSeconds(3);
        AudioManager.AM.PlaySE("Three");
        countText.text = string.Format("3");
        yield return new WaitForSeconds(1);
        AudioManager.AM.PlaySE("Two");
        countText.text = string.Format("2");
        yield return new WaitForSeconds(1);
        AudioManager.AM.PlaySE("One");
        countText.text = string.Format("1");
        yield return new WaitForSeconds(1);
        AudioManager.AM.PlaySE("Start");
        countText.text = string.Format("게임 스타트!!!");
        yield return new WaitForSeconds(1);
        count = true;
        DataManager.DM.inGame = true;
        timerText.gameObject.SetActive(true);
        countText.gameObject.SetActive(false);
    }

    public IEnumerator LeaveGame()
    {
        timerText.gameObject.SetActive(false);
        resultText.gameObject.SetActive(true);
        DataManager.DM.inGame = false;
        DataManager.DM.gameOver = true;
        AudioManager.AM.PlaySE("Gameover");
        gameOverImage.gameObject.SetActive(true);
        //countText.text = string.Format("GAME OVER");
        yield return new WaitForSeconds(2);
        VictoryTeam();
        yield return new WaitForSeconds(3);
        AudioManager.AM.PlaySE("GameInfo7");
        gameOverImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(3);
        countText.gameObject.SetActive(true);
        AudioManager.AM.PlaySE("GameInfo8");
        countText.text = string.Format("게임이 종료되었습니다\n 헤드셋을 벗어주세요");
        // resultText.gameObject.SetActive(false);
        yield return new WaitForSeconds(4);
        PN.LeaveRoom();
        StopCoroutine(LeaveGame());
    }

    public void VictoryTeam()
    {
        if (score_BlueKill > score_RedKill)
        {
            AudioManager.AM.PlaySE("GameInfo4");
            //countText.text = string.Format("블루팀이 승리하였습니다");
            blueScore.gameObject.SetActive(false);
            redScore.gameObject.SetActive(false);
            bluewinImg.gameObject.SetActive(true);
            redloseImg.gameObject.SetActive(true);

        }
        else if (score_BlueKill < score_RedKill)
        {
            AudioManager.AM.PlaySE("GameInfo5");
            //countText.text = string.Format("레드팀이 승리하였습니다");
            blueScore.gameObject.SetActive(false);
            redScore.gameObject.SetActive(false);
            blueloseImg.gameObject.SetActive(true);
            redwinImg.gameObject.SetActive(true);
        }
        else if (score_BlueKill == score_RedKill)
        {
            AudioManager.AM.PlaySE("GameInfo6");
            // countText.text = string.Format("무승부입니다");
            blueScore.gameObject.SetActive(false);
            redScore.gameObject.SetActive(false);
            drawImg.gameObject.SetActive(true);
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
}
