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

public class WesternManager : MonoBehaviourPunCallbacks
{
    public static WesternManager WM;

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
    [SerializeField] bool count = false;
    [SerializeField] int limitedTime;
    Hashtable setTime = new Hashtable();
    PhotonView PV;
    //public Vector3 adminPos = new Vector3(8.28f, 20, 0f);
    //public Quaternion adminRot = new Quaternion(52, -90, 0, 0);
    public Transform[] bSpawnPosition;
    public Transform adminPoint;
    private void Awake()
    {
        WM = this;
        DataManager.DM.currentMap = Map.WESTERN;
    }
    void Start()
    {
        //StartCoroutine(SpawnDynamite());
        PV = GetComponent<PhotonView>();
        if (PN.IsConnectedAndReady && PN.InRoom)
        {
            SpawnPlayer();

            if (PN.IsMasterClient)
            {
                PV.RPC("StartBtnW", RpcTarget.AllViaServer);
            }
            if (DataManager.DM.currentTeam != Team.ADMIN)
            {
                admin.SetActive(false);
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
            /*#if UNITY_EDITOR_WIN
                        case Team.ADMIN:
                            PN.AutomaticallySyncScene = true;
                            NetworkManager.NM.inGame = false;
                            spawnPlayer = PN.Instantiate(admin.name,adminPoint.position,adminPoint.rotation);
                            Debug.Log($"{PN.CurrentRoom.Name} �濡 ������{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
                            Info();
                            break;
            #endif*/
            default:
                return;
        }
    }

    private void Update()
    {
        if (SpawnWeapon_RW.RW.DeviceR.TryGetFeatureValue(CommonUsages.primaryButton, out bool pressed))
        {
            if (pressed)
            {
                spawnBomb = PN.Instantiate(bomB.name, bSpawnPosition[0].position, bSpawnPosition[0].rotation, 0);
                spawnBomb = PN.Instantiate(bomB.name, bSpawnPosition[1].position, bSpawnPosition[1].rotation, 0);
            }
        }
        
#if UNITY_EDITOR_WIN
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { PV.RPC("StartBtnW", RpcTarget.All); }        
        else if (Input.GetKeyDown(KeyCode.Backspace)) { PV.RPC("EndGameW", RpcTarget.All); }
        else if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
        else if (Input.GetKeyDown(KeyCode.Space)) { spawnBomb = PN.Instantiate(bomB.name, bSpawnPosition[0].position, bSpawnPosition[0].rotation, 0); }
        else if (Input.GetKeyDown(KeyCode.Space)) { spawnBomb = PN.Instantiate(bomB.name, bSpawnPosition[1].position, bSpawnPosition[1].rotation, 0); }
#endif
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

    public IEnumerator SpawnDynamite()
    {
        yield return new WaitForSeconds(3);
        spawnBomb = PN.Instantiate(bomB.name, bSpawnPosition[0].position, bSpawnPosition[0].rotation, 0);
        spawnBomb = PN.Instantiate(bomB.name, bSpawnPosition[1].position, bSpawnPosition[1].rotation, 0);
        StartCoroutine(SpawnDynamite());
    }

    [PunRPC]
    public void StartBtnW()
    {
        StartCoroutine(StartTimer());
        //startBtn.SetActive(false);
        //startText.gameObject.SetActive(false);
    }

    [PunRPC]
    public void EndGameW()
    {
        StartCoroutine(LeaveGame());
    }
    public IEnumerator PlayTimer()
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
            PV.RPC("EndGameW", RpcTarget.All);
            Debug.Log("Ÿ�ӿ���");
        }
    }
    IEnumerator StartTimer()
    {
        //yield return new WaitForSeconds(10);
        AudioManager.AM.EffectPlay(AudioManager.Effect.GAMESTART);
        countText.text = string.Format("������ 3�� �ڿ� ���۵˴ϴ�.");
        yield return new WaitForSeconds(3);
        AudioManager.AM.EffectPlay(AudioManager.Effect.Three);
        countText.text = string.Format("3");
        yield return new WaitForSeconds(1);
        AudioManager.AM.EffectPlay(AudioManager.Effect.Two);
        countText.text = string.Format("2");
        yield return new WaitForSeconds(1);
        AudioManager.AM.EffectPlay(AudioManager.Effect.One);
        countText.text = string.Format("1");
        yield return new WaitForSeconds(1);
        AudioManager.AM.EffectPlay(AudioManager.Effect.START);
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
        AudioManager.AM.EffectPlay(AudioManager.Effect.GAMEOVER);
        countText.text = string.Format("GAME OVER");
        yield return new WaitForSeconds(1);
        AudioManager.AM.EffectPlay(AudioManager.Effect.END);
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

}
