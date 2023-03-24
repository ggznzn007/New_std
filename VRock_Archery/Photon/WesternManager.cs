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

    [Header("ī��Ʈ�ٿ� �ؽ�Ʈ")]
    [SerializeField] TextMeshPro[] countText;
    [Header("���� ���ѽð�")]
    public TextMeshPro[] timerText;
    public TextMeshPro[] btimerText;
    public TextMeshPro[] resultText;
    [Header("������ ������")]
    [SerializeField] GameObject redTeam;
    [Header("����� ������")]
    [SerializeField] GameObject blueTeam;
    [Header("������")]
    public GameObject admin;
    [Header("������ ������")]
    public GameObject snowBlock;
    [Header("NPC ������")]
    public GameObject eagleNPC;  

    public Transform[] blockPoint;
    private GameObject spawnPlayer;
    [SerializeField] bool count = false;
    [SerializeField] bool bCount = false;
    [SerializeField] int limitedTime;
    [SerializeField] float buildTime;
    Hashtable setTime = new Hashtable();
    PhotonView PV;
    private float curTime;
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
    public Image[] buildTImage;
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
                //PV.RPC(nameof(BuildTimer), RpcTarget.AllViaServer);                              // Ÿ�̸� ����ȭ
                PV.RPC(nameof(StartBtnW), RpcTarget.AllViaServer);                              // Ÿ�̸� ����ȭ
                                                                                                // BuildTimer();
                SpawnBlock();                                                                  // ������ ���۽�  ������ġ�� ���� �� 10��
            }

            if (DataManager.DM.currentTeam != Team.ADMIN)      // ������ ����� �ʿ��� �ڵ�
            {
                Destroy(admin);
            }
        }
    }

    /*[PunRPC]
    public void BuildTimer()
    {       
       StartCoroutine(BuildT());        
    }

    public IEnumerator BuildT()
    {
        AudioManager.AM.PlaySE("GameInfo1");
        buildTime = 30;
        yield return new WaitForSeconds(4);
        if (buildTime>0)
        {
            buildTime -= 1;
        }
        if(buildTime<=0)
        {
            StartCoroutine(StartTimer());
        }
        else
        {
            yield break;
        }
        
        photonView.RPC(nameof(ShowBuildT), RpcTarget.All,buildTime);

        yield return new WaitForSeconds(1);
        StartCoroutine(BuildT());        
    }

    [PunRPC]
    public void ShowBuildT(int number)
    {
        btimerText[0].text = number.ToString();
        btimerText[1].text = number.ToString();
    }*/

    public void SpawnPlayer()
    {
        switch (DataManager.DM.currentTeam)
        {
            case Team.RED:
                PN.AutomaticallySyncScene = true;
                DataManager.DM.inGame = false;
                DataManager.DM.gameOver = false;
                spawnPlayer = PN.Instantiate(redTeam.name, Vector3.zero, Quaternion.identity);
                Debug.Log($"{PN.CurrentRoom.Name} �濡 ������{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
                Info();
                break;

            case Team.BLUE:
                PN.AutomaticallySyncScene = true;
                DataManager.DM.inGame = false;
                DataManager.DM.gameOver = false;
                spawnPlayer = PN.Instantiate(blueTeam.name, Vector3.zero, Quaternion.identity);
                Debug.Log($"{PN.CurrentRoom.Name} �濡 �����{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
                Info();
                break;
            // ������ ���α׷� ���� ��
            case Team.ADMIN:
                if (Application.platform == RuntimePlatform.WindowsPlayer)//|| Application.platform == RuntimePlatform.WindowsEditor)
                {
                    if (Application.platform != RuntimePlatform.WindowsPlayer) { return; }
                    PN.AutomaticallySyncScene = true;
                    DataManager.DM.inGame = false;
                    DataManager.DM.gameOver = false;
                    spawnPlayer = PN.Instantiate(admin.name, adminPoint.position, adminPoint.rotation);
                    spawnPlayer.transform.SetParent(adminPoint.transform, true);
                    Debug.Log($"{PN.CurrentRoom.Name} �濡 ������{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
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

        /*if (PN.IsMasterClient)
        {
            SpawnBlock();
        }*/

        /*if (PN.IsConnectedAndReady && PN.InRoom && PN.IsMasterClient)  // ������ ���α׷� ���� ��
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                
            }
        }*/
    }
    void FixedUpdate()
    {
        SetScore();
        TimerW();
        TimerB();
    }

    public void SetScore()
    {
        blueScore[0].text = score_BlueKill.ToString();   // ����� ����
        blueScore[1].text = score_RedKill.ToString();     // ������ ����
        redScore[0].text = score_BlueKill.ToString();   // ����� ����
        redScore[1].text = score_RedKill.ToString();     // ������ ����
    }
    public void UpdateStats()
    {
        playerProp["kills"] = kills;             // ���� ų ��
                                                 // playerProp["deaths"] = deaths;           // ���� ���� ��
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

    public void SpawnBlock()  // ������ ���� �޼���                                                                      
    {
        for (int i = 0; i < blockPoint.Length; i++)
        {
            PN.InstantiateRoomObject(snowBlock.name, blockPoint[i].position, blockPoint[i].rotation, 0);          
        }
      /*  curTime += Time.deltaTime;
        if (curTime >= limit)
        {
           
        }
*/
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
    public void Notice1min()
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
            PV.RPC(nameof(Notice1min), RpcTarget.AllViaServer);   // ���� ������ 60���� �˸�
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
            Debug.Log("Ÿ�ӿ���");
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

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(4);
        AudioManager.AM.PlaySE("BuildTime2");
        countText[0].gameObject.SetActive(false);
        countText[1].gameObject.SetActive(false);
        buildTImage[0].gameObject.SetActive(true);
        buildTImage[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        AudioManager.AM.PlaySE("BuildTime");        
        buildTime = 30;
        bCount = true;        
    }

    public IEnumerator RealTimer()
    {
        buildTImage[0].gameObject.SetActive(false);
        buildTImage[1].gameObject.SetActive(false);
        btimerText[0].gameObject.SetActive(false);
        btimerText[1].gameObject.SetActive(false);
        countText[0].gameObject.SetActive(true);
        countText[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        AudioManager.AM.PlaySE("GameInfo1");
        countText[0].text = string.Format("������ 3�� �ڿ� ���۵˴ϴ�.");
        countText[1].text = string.Format("������ 3�� �ڿ� ���۵˴ϴ�.");
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
        countText[0].text = string.Format("���� ��ŸƮ!!!");
        countText[1].text = string.Format("���� ��ŸƮ!!!");
        yield return new WaitForSeconds(1);
        count = true;                                             // ���� Ÿ�̸� ��ŸƮ
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
        AudioManager.AM.PlaySE("GameInfo8");
        countText[0].text = string.Format("������ ����Ǿ����ϴ�\n ������ �����ּ���");
        countText[1].text = string.Format("������ ����Ǿ����ϴ�\n ������ �����ּ���");
        yield return new WaitForSeconds(4);
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

    [ContextMenu("���� ���� ����")]
    void Info()
    {
        if (PN.InRoom)
        {
            print("���� �� �̸�: " + PN.CurrentRoom.Name);
            print("���� �� �ο� ��: " + PN.CurrentRoom.PlayerCount);
            print("���� �� MAX�ο�: " + PN.CurrentRoom.MaxPlayers);

            string playerStr = "�濡 �ִ� �÷��̾� ��� \n";
            for (int i = 0; i < PN.PlayerList.Length; i++)
            {
                playerStr += PN.PlayerList[i].NickName + ", ";
                print(playerStr);
            }

        }
        else
        {
            print("������ �ο� ��: " + PN.CountOfPlayers);
            print("�κ� �ִ� ����: " + PN.InLobby);
            print("������ �ο� ��: " + PN.CountOfPlayers);
            print("���� ���Ῡ��: " + PN.IsConnected);
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
