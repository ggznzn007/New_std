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

public class GunShootManager : MonoBehaviourPunCallbacks                                      // 토이
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
    [Header("스킬화살 프리팹")]
    public GameObject arrowSkilled;                            // 스킬 화살
    public GameObject arrowMulti;                              // 멀티샷
    public GameObject arrowBomb;                               // 폭탄 화살
    [Header("Eagle NPC 프리팹")]                                            // 폭탄을 생성해주는 NPC => 크레인
    public GameObject eagleNPC;
    public Transform[] wayPos;
    public Transform[] aSpawnPosition;                         // 특수화살 생성위치
    public ParticleSystem[] arrowSpawnFX;
    public GameObject eagleBomb;
    public Transform spawnPoint;
    public GameObject myBomb = null;

    private GameObject spawnPlayer;                                  // 생성되는 플레이어
    [SerializeField] bool count = false;                             // 타이머 카운트 스위치
    [SerializeField] int limitedTime;                                // 게임제한시간
    Hashtable setTime = new Hashtable();                             // 세팅시간
    PhotonView PV;
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
    public Image[] gameOverImage;
    public Image[] bluewinImg;
    public Image[] redwinImg;
    public Image[] blueloseImg;
    public Image[] redloseImg;
    public Image[] drawImg;
    public TMP_Text[] blueScore;
    public TMP_Text[] redScore;
    public int score_BlueKill;
    public int score_RedKill;
    /*private GameObject curArrowB;
    private GameObject curArrowBB;
    private GameObject curArrowR;
    private GameObject curArrowRR;
    private GameObject curEagle;              */       


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
        blueScore[1].text = score_RedKill.ToString();     // 레드팀 점수
        redScore[0].text = score_BlueKill.ToString();   // 블루팀 점수
        redScore[1].text = score_RedKill.ToString();     // 레드팀 점수
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

        if (limitedTime == 8)                                 // 게임 끝나기 8초전에 알림
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
        yield return new WaitForSeconds(4);
        AudioManager.AM.PlaySE("GameInfo1");
        countText[0].text = string.Format("게임이 3초 뒤에 시작됩니다.");
        countText[1].text = string.Format("게임이 3초 뒤에 시작됩니다.");
        yield return new WaitForSeconds(3);
        AudioManager.AM.PlaySE("Three");
        countText[0].text = string.Format("3");
        countText[1].text = string.Format("3");
        yield return new WaitForSeconds(1);
        AudioManager.AM.PlaySE("Two");
        countText[0].text = string.Format("2");
        countText[1].text = string.Format("2");
        yield return new WaitForSeconds(1);
        AudioManager.AM.PlaySE("One");
        countText[0].text = string.Format("1");
        countText[1].text = string.Format("1");
        yield return new WaitForSeconds(1);
        AudioManager.AM.PlaySE("Start");
        countText[0].text = string.Format("게임 스타트!!!");
        countText[1].text = string.Format("게임 스타트!!!");
        yield return new WaitForSeconds(1);
        count = true;
        DataManager.DM.inGame = true;
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
        DataManager.DM.inGame = false;
        DataManager.DM.gameOver = true;
        AudioManager.AM.PlaySE("Gameover");
        gameOverImage[0].gameObject.SetActive(true);        
        gameOverImage[1].gameObject.SetActive(true);        
        yield return new WaitForSeconds(2);
        VictoryTeam();
        yield return new WaitForSeconds(3);
        AudioManager.AM.PlaySE("GameInfo7");
        gameOverImage[0].gameObject.SetActive(false);
        gameOverImage[1].gameObject.SetActive(false);
        yield return new WaitForSeconds(3);
        countText[0].gameObject.SetActive(true);
        countText[1].gameObject.SetActive(true);
        AudioManager.AM.PlaySE("GameInfo2");
        countText[0].text = string.Format("3초 뒤에 다음 스테이지로 이동합니다");
        countText[1].text = string.Format("3초 뒤에 다음 스테이지로 이동합니다");
        yield return new WaitForSeconds(3);
        PN.LeaveRoom();
        StopCoroutine(LeaveGame());
    }

    public void VictoryTeam()
    {
        if (score_BlueKill > score_RedKill)
        {
            AudioManager.AM.PlaySE("GameInfo4");            
            blueScore[0].gameObject.SetActive(false);
            blueScore[1].gameObject.SetActive(false);
            redScore[0].gameObject.SetActive(false);
            redScore[1].gameObject.SetActive(false);
            bluewinImg[0].gameObject.SetActive(true);
            bluewinImg[1].gameObject.SetActive(true);
            redloseImg[0].gameObject.SetActive(true);
            redloseImg[1].gameObject.SetActive(true);

        }
        else if (score_BlueKill < score_RedKill)
        {
            AudioManager.AM.PlaySE("GameInfo5");
            blueScore[0].gameObject.SetActive(false);
            blueScore[1].gameObject.SetActive(false);
            redScore[0].gameObject.SetActive(false);
            redScore[1].gameObject.SetActive(false);
            blueloseImg[0].gameObject.SetActive(true);
            blueloseImg[1].gameObject.SetActive(true);
            redwinImg[0].gameObject.SetActive(true);
            redwinImg[1].gameObject.SetActive(true);
        }
        else if (score_BlueKill == score_RedKill)
        {
            AudioManager.AM.PlaySE("GameInfo6");
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
