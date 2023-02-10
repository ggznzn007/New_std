using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using TMPro;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using static UnityEngine.Rendering.DebugUI;

public class GunShootManager : MonoBehaviourPunCallbacks                                      // 토이
{
    public static GunShootManager GSM;

    [Header("카운트다운 텍스트")]
    [SerializeField] TextMeshPro countText;
    [Header("게임 제한시간 텍스트")]
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
    [Header("NPC 프리팹")]                                            // 폭탄을 생성해주는 NPC => 크레인
    public GameObject npc;

    private GameObject spawnPlayer;                                  // 생성되는 플레이어
    [SerializeField] bool count = false;                             // 타이머 카운트 스위치
    [SerializeField] int limitedTime;                                // 게임제한시간
    Hashtable setTime = new Hashtable();                             // 세팅시간
    PhotonView PV;
    public Transform bSpawnPosBlue;                                  // 블루팀 진영의 폭탄 생성위치
    public Transform bSpawnPosRed;                                   // 레드팀 진영의 폭탄 생성위치
    public Transform adminPoint;                                     // 관리자 생성 위치
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
    public GameObject shield;                             // 폭탄 프리팹
    private GameObject bombBlue;
    private GameObject bombRed;
    private GameObject shieldCap_Blue;
    private GameObject shieldCap_Red;

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
            if (PN.IsMasterClient)
            {
                PV.RPC(nameof(StartBtnT), RpcTarget.AllViaServer);
                PN.InstantiateRoomObject(npc.name, new Vector3(-0.021f, 0.725f, -0.097f), Quaternion.identity);
                Invoke(nameof(Shield_Blue), 2);
                Invoke(nameof(Shield_Red), 2);
                //Shield_Blue();
                //Shield_Red();
            }
            SpawnPlayer();
            if (DataManager.DM.currentTeam != Team.ADMIN)  // 관리자 빌드시 필요한 코드
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
                    PN.AutomaticallySyncScene = true;
                    DataManager.DM.inGame = false;
                    DataManager.DM.gameOver = false;
                    spawnPlayer = PN.Instantiate(admin.name, adminPoint.position, adminPoint.rotation);
                    //spawnPlayer = admin;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Emp_Red();
            Emp_Blue();
        }

        /*if (PN.IsConnectedAndReady && PN.InRoom && PN.IsMasterClient)  // 윈도우 프로그램 빌드 시
        {


            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { PV.RPC("StartBtnT", RpcTarget.All); }
                else if (Input.GetKeyDown(KeyCode.Backspace)) { PV.RPC("EndGameT", RpcTarget.All); }
                else if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    Emp_Red();
                    Emp_Blue();
                }
            }

        }*/
    }
    public IEnumerator ExitGame()
    {
        yield return new WaitForSeconds(1);
        photonView.RPC(nameof(ForceOff), RpcTarget.AllViaServer);
    }
    void FixedUpdate()
    {
        SetScore();
        Timer();
    }

    public void SetScore()
    {
        blueScore.text = score_BlueKill.ToString();   // 블루팀 점수
        redScore.text = score_RedKill.ToString();     // 레드팀 점수
    }
    public void UpdateStats()
    {
        playerProp["kills"] = kills;             // 개인 킬 수                                                

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

    public void Shield_Red()
    {
        if (shieldCap_Red == null)
        {
            if (shieldCap_Red != null) return;
            shieldCap_Red = PN.InstantiateRoomObject(shield.name, bSpawnPosRed.position, bSpawnPosRed.rotation, 0);
        }
    }

    public void Shield_Blue()
    {
        if (shieldCap_Blue == null)
        {
            if (shieldCap_Blue != null) return;
            shieldCap_Blue = PN.InstantiateRoomObject(shield.name, bSpawnPosBlue.position, bSpawnPosBlue.rotation, 0);
        }
    }


    public void Emp_Red()
    {
        //PN.Instantiate(bomB.name, bSpawnPosRed.position, bSpawnPosRed.rotation, 0);
        bombRed = PN.InstantiateRoomObject(bomB.name, bSpawnPosRed.position, bSpawnPosRed.rotation, 0);
    }

    public void Emp_Blue()
    {
        //PN.Instantiate(bomB.name, bSpawnPosBlue.position, bSpawnPosBlue.rotation, 0);
        bombBlue = PN.InstantiateRoomObject(bomB.name, bSpawnPosBlue.position, bSpawnPosBlue.rotation, 0);
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
    public void StartBtnT()
    {
        StartCoroutine(StartTimer());
    }

    [PunRPC]
    public void EndGameT()
    {
        StartCoroutine(LeaveGame());
    }

    [PunRPC]
    public void Notice()
    {
        AudioManager.AM.PlaySE("GameInfo3");
    }

    [PunRPC]
    public void Notice1min()
    {
        AudioManager.AM.PlaySE("GameInfo9");
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
            PV.RPC(nameof(Notice1min), RpcTarget.AllViaServer);   // 게임 끝나기 60초전 알림
        }

        if (limitedTime == 8)                                 // 게임 끝나기 8초전 알림
        {
            PV.RPC(nameof(Notice), RpcTarget.AllViaServer);
        }

        if (limitedTime <= 0)
        {
            count = false;
            limitedTime = 0;
            PV.RPC(nameof(EndGameT), RpcTarget.AllViaServer);
            Debug.Log("타임오버");
        }
    }

    public IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(7);
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
        yield return new WaitForSeconds(2);
        VictoryTeam();
        yield return new WaitForSeconds(3);
        AudioManager.AM.PlaySE("GameInfo7");
        gameOverImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(3);
        countText.gameObject.SetActive(true);
        AudioManager.AM.PlaySE("GameInfo2");
        countText.text = string.Format("3초 뒤에 다음 스테이지로 이동합니다");
        yield return new WaitForSeconds(6);
        PN.LeaveRoom();
        StopCoroutine(LeaveGame());
    }

    public void VictoryTeam()
    {
        if (score_BlueKill > score_RedKill)
        {
            AudioManager.AM.PlaySE("GameInfo4");
            blueScore.gameObject.SetActive(false);
            redScore.gameObject.SetActive(false);
            bluewinImg.gameObject.SetActive(true);
            redloseImg.gameObject.SetActive(true);

        }
        else if (score_BlueKill < score_RedKill)
        {
            AudioManager.AM.PlaySE("GameInfo5");
            blueScore.gameObject.SetActive(false);
            redScore.gameObject.SetActive(false);
            blueloseImg.gameObject.SetActive(true);
            redwinImg.gameObject.SetActive(true);
        }
        else if (score_BlueKill == score_RedKill)
        {
            AudioManager.AM.PlaySE("GameInfo6");
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
}
