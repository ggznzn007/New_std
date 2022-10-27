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

public class GunShootManager : MonoBehaviourPunCallbacks //,IPunObservable                     // ����
{
    public static GunShootManager GSM;

    [Header("ī��Ʈ�ٿ� �ؽ�Ʈ")]
    [SerializeField] TextMeshPro countText;
    [Header("���� ���ѽð�")]
    [SerializeField] TextMeshPro timerText;
    /*[Header("���� ���� �ؽ�Ʈ(������)")]
    public TextMeshPro startText;
    [Header("��ŸƮ ��ư(������)")]
    public GameObject startBtn;*/
    [Header("������ ������")]
    [SerializeField] GameObject redTeam;
    [Header("����� ������")]
    [SerializeField] GameObject blueTeam;
    [Header("������")]
    public GameObject admin;
    [Header("��ź ������")]
    public GameObject bomB;

    private GameObject spawnPlayer;
    private GameObject spawnBomb;
    private GameObject spawnBomb1;
    [SerializeField] bool count = false;
    [SerializeField] int limitedTime;
    Hashtable setTime = new Hashtable();
    PhotonView PV;
    //public Vector3 adminPos = new Vector3(-10.72f, 15, 0.55f);
    // public Quaternion adminRot = new Quaternion(40, 90, 0, 0);
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
    public int deaths;
    public int ranking;
    private ExitGames.Client.Photon.Hashtable playerProp = new ExitGames.Client.Photon.Hashtable();
    public TMP_Text blueScore;
    public TMP_Text redScore;   
    public int score_Blue;
    public int score_Red;
    public RectTransform blueRect;
    public RectTransform redRect;
    private void Awake()
    {
        GSM = this;
        DataManager.DM.currentMap = Map.TOY;
    }

    private void Start()
    {
        StartCoroutine(SpawnBomb());
        PV = GetComponent<PhotonView>();
        if (PN.IsConnectedAndReady && PN.InRoom)
        {
            SpawnPlayer();
            if (PN.IsMasterClient)
            {
                PV.RPC("StartBtnT", RpcTarget.AllViaServer);
            }

          /*  if (DataManager.DM.currentTeam != Team.ADMIN)
            {
                Destroy(admin);
            }*/
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
                Debug.Log($"{PN.CurrentRoom.Name} �濡 ������{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
                Info();

                break;

            case Team.BLUE:
                PN.AutomaticallySyncScene = true;
                DataManager.DM.inGame = false;
                spawnPlayer = PN.Instantiate(blueTeam.name, Vector3.zero, Quaternion.identity);
                Debug.Log($"{PN.CurrentRoom.Name} �濡 �����{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
                Info();

                break;
/*#if UNITY_EDITOR          // ����Ƽ �����Ϳ��� ��� ��
            case Team.ADMIN:
                PN.AutomaticallySyncScene = true;
                DataManager.DM.inGame = false;
                spawnPlayer = PN.Instantiate(admin.name, adminPoint.position, adminPoint.rotation);               
                Debug.Log($"{PN.CurrentRoom.Name} �濡 ������{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
                Info();

                break;
#endif*/

#if UNITY_STANDALONE_WIN          // ������ ���α׷� ���� ��
        case Team.ADMIN:
                PN.AutomaticallySyncScene = true;
                DataManager.DM.inGame = false;
                spawnPlayer = PN.Instantiate(admin.name, adminPoint.position, adminPoint.rotation);                
                Debug.Log($"{PN.CurrentRoom.Name} �濡 ������{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
                Info();

                break;
#endif
            default:
                return;
        }
    }

    private void Update()
    {
        UpdateStats();
        /*#if UNITY_ANDROID
                if (SpawnWeapon_RW.RW.DeviceR.TryGetFeatureValue(CommonUsages.primaryButton, out bool pressed))
                {
                    if (pressed)
                    {
                        spawnBomb = PN.Instantiate(bomB.name, bSpawnPosition[0].position, bSpawnPosition[0].rotation, 0);
                        spawnBomb1 = PN.Instantiate(bomB.name, bSpawnPosition[1].position, bSpawnPosition[1].rotation, 0);
                    }
                }
        #endif*/
#if UNITY_EDITOR          // ����Ƽ �����Ϳ��� ��� ��
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { PV.RPC("StartBtnT", RpcTarget.All); }
        else if (Input.GetKeyDown(KeyCode.Backspace)) { PV.RPC("EndGameT", RpcTarget.All); }
        else if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            spawnBomb = PN.Instantiate(bomB.name, bSpawnPosition[0].position, bSpawnPosition[0].rotation, 0);
            spawnBomb1 = PN.Instantiate(bomB.name, bSpawnPosition[1].position, bSpawnPosition[1].rotation, 0);
        }

#endif
#if UNITY_STANDALONE_WIN          // ������ ���α׷� ���� ��
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { PV.RPC("StartBtnW", RpcTarget.All); }
        else if (Input.GetKeyDown(KeyCode.Backspace)) { PV.RPC("EndGameW", RpcTarget.All); }
        else if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            spawnBomb = PN.Instantiate(bomB.name, bSpawnPosition[0].position, bSpawnPosition[0].rotation, 0);
            spawnBomb1 = PN.Instantiate(bomB.name, bSpawnPosition[1].position, bSpawnPosition[1].rotation, 0);           
        }
#endif
    }


    public void UpdateStats()
    {
        playerProp["rank"] = ranking;
        playerProp["kills"] = kills;             // ���� ų ��
        playerProp["deaths"] = deaths;           // ���� ���� ��

        PN.LocalPlayer.CustomProperties = playerProp;
        PN.SetPlayerCustomProperties(playerProp);
        blueScore.text = score_Blue.ToString();   // ����� ����
        redScore.text = score_Red.ToString();     // ������ ����

    }

    [PunRPC]
    public void AddScoreBlue(int scorePlus)
    {
        score_Blue += scorePlus;
    }

    [PunRPC]
    public void AddScoreRed(int scorePlus)
    {
        score_Red += scorePlus;
    }

    void FixedUpdate()
    {
        if (PN.InRoom && PN.IsConnectedAndReady)
        {

            limitedTime = (int)PN.CurrentRoom.CustomProperties["Time"];
            limitedTime = limitedTime < 0 ? 0 : limitedTime;
            float min = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] / 60);
            float sec = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] % 60);
            timerText.text = string.Format("�����ð� {0:00}�� {1:00}��", min, sec);
            if (limitedTime < 60)
            {
                timerText.text = string.Format("�����ð� {0:0}��", sec);
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

    public IEnumerator SpawnBomb()
    {
       
            yield return new WaitForSecondsRealtime(30);
            spawnBomb = PN.Instantiate(bomB.name, bSpawnPosition[0].position, bSpawnPosition[0].rotation, 0);
            spawnBomb1 = PN.Instantiate(bomB.name, bSpawnPosition[1].position, bSpawnPosition[1].rotation, 0);
            StartCoroutine(SpawnBomb());
       
       
    }



    [PunRPC]
    public void StartBtnT()
    {
        StartCoroutine(StartTimer());
        //startBtn.SetActive(false);
        //  startText.gameObject.SetActive(false);
    }

    [PunRPC]
    public void EndGameT()
    {
        StartCoroutine(LeaveGame());
    }

    IEnumerator PlayTimer()
    {
        yield return new WaitForSeconds(1);
        int nextTime = limitedTime -= 1;
        setTime["Time"] = nextTime;
        PN.CurrentRoom.SetCustomProperties(setTime);
        count = true;
       
        if (limitedTime <= 0)
        {
            count = false;
            limitedTime = 0;
            PV.RPC("EndGameT", RpcTarget.All);
            Debug.Log("Ÿ�ӿ���");
        }
    }



    public IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(10);
        AudioManager.AM.PlaySE("GameInfo1");
        countText.text = string.Format("������ 3�� �ڿ� ���۵˴ϴ�.");
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
        countText.text = string.Format("���� ��ŸƮ!!!");
        yield return new WaitForSeconds(1);
        count = true;
        DataManager.DM.inGame = true;
        timerText.gameObject.SetActive(true);
        countText.gameObject.SetActive(false);
    }


    public IEnumerator LeaveGame()
    {
        timerText.gameObject.SetActive(false);
        countText.gameObject.SetActive(true);
        DataManager.DM.inGame = false;
        AudioManager.AM.PlaySE("Gameover");
        countText.text = string.Format("GAME OVER");
        yield return new WaitForSeconds(1);
        AudioManager.AM.PlaySE("GameInfo2");
        countText.text = string.Format("3�� �ڿ� �κ�� �̵��մϴ�");
        yield return new WaitForSeconds(4);
        PN.LeaveRoom();
        StopCoroutine(LeaveGame());
    }

    public override void OnLeftRoom()
    {
        if (PN.IsMasterClient)
        {
            PN.DestroyAll();
        }


        PN.Destroy(spawnPlayer);
        SceneManager.LoadScene(0);

    }

    [ContextMenu("���� ���� ����")]
    void Info()
    {
        if (PN.InRoom)
        {
            print("���� �� �̸�: " + PN.CurrentRoom.Name);
            print("���� �� �ο� ��: " + PN.CurrentRoom.PlayerCount);
            print("���� �� MAX�ο�: " + PN.CurrentRoom.MaxPlayers);

            string playerStr = "�濡 �ִ� �÷��̾� ���";
            for (int i = 0; i < PN.PlayerList.Length; i++)
            {
                playerStr += PN.PlayerList[i].NickName + ",";
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

    public void QuitGame()
    {
        PN.LeaveRoom();
    }

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(score_Blue);
            stream.SendNext(score_Red);
            stream.SendNext(blueRect.transform.position);
            stream.SendNext(redRect.transform.position);
        }
        else
        {
            score_Blue = (int)stream.ReceiveNext();
            score_Red = (int)stream.ReceiveNext();
            blueRect.transform.position = (Vector3)stream.ReceiveNext();
            redRect.transform.position = (Vector3)stream.ReceiveNext();
        }
    }*/

    // ���� ĳ���ͻ��� if��
    /*if (PN.IsConnectedAndReady && DataManager.DM.currentTeam == Team.RED)
       {
           PN.AutomaticallySyncScene = true;
           NetworkManager.NM.inGame = true;
           spawnPlayer = PN.Instantiate(redTeam.name, Vector3.zero, Quaternion.identity);
           count = true;
           Debug.Log($"{PN.CurrentRoom.Name} �濡 ������{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
           Info();
       }
       else if (PN.IsConnectedAndReady && DataManager.DM.currentTeam == Team.BLUE)
       {
           PN.AutomaticallySyncScene = true;
           NetworkManager.NM.inGame = true;
           spawnPlayer = PN.Instantiate(blueTeam.name, Vector3.zero, Quaternion.identity);
           count = true;
           Debug.Log($"{PN.CurrentRoom.Name} �濡 �����{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
           Info();
       }*/
}
