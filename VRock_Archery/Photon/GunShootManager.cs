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

public class GunShootManager : MonoBehaviourPunCallbacks                                      // ����
{
    public static GunShootManager GSM;

    [Header("ī��Ʈ�ٿ� �ؽ�Ʈ")]
    [SerializeField] TextMeshPro[] countText;
    [Header("���� ���ѽð� �ؽ�Ʈ")]
    public TextMeshPro[] timerText;
    public TextMeshPro[] resultText;
    [Header("������ ������")]
    [SerializeField] GameObject redTeam;
    [Header("����� ������")]
    [SerializeField] GameObject blueTeam;
    [Header("������")]
    public GameObject admin;
    [Header("��ųȭ�� ������")]
    public GameObject arrowSkilled;                            // ��ų ȭ��
    public GameObject arrowMulti;                              // ��Ƽ��
    public GameObject arrowBomb;                               // ��ź ȭ��
    [Header("Eagle NPC ������")]                                            // ��ź�� �������ִ� NPC => ũ����
    public GameObject eagleNPC;
    public Transform[] wayPos;
    public Transform[] aSpawnPosition;                         // Ư��ȭ�� ������ġ
    public ParticleSystem[] arrowSpawnFX;
    public GameObject eagleBomb;
    public Transform spawnPoint;
    public GameObject myBomb = null;

    private GameObject spawnPlayer;                                  // �����Ǵ� �÷��̾�
    [SerializeField] bool count = false;                             // Ÿ�̸� ī��Ʈ ����ġ
    [SerializeField] int limitedTime;                                // �������ѽð�
    Hashtable setTime = new Hashtable();                             // ���ýð�
    PhotonView PV;
    public Transform adminPoint;                                     // ������ ���� ��ġ
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
            if (DataManager.DM.currentTeam != Team.ADMIN)  // ������ ���� �� �ʿ��� �ڵ�
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
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { PV.RPC("StartBtnT", RpcTarget.All); }
        if (Input.GetKeyDown(KeyCode.Backspace)) { PV.RPC("EndGameT", RpcTarget.All); }
        if (Input.GetKeyDown(KeyCode.Escape)) { StartCoroutine(nameof(ExitGame)); }
        /*  if (PN.IsConnectedAndReady && PN.InRoom && PN.IsMasterClient)  // ������ ���α׷� ���� ��
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
        blueScore[0].text = score_BlueKill.ToString();   // ����� ����
        blueScore[1].text = score_RedKill.ToString();     // ������ ����
        redScore[0].text = score_BlueKill.ToString();   // ����� ����
        redScore[1].text = score_RedKill.ToString();     // ������ ����
    }
    public void UpdateStats()
    {
        playerProp["kills"] = kills;             // ���� ų ��                                                

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
                Debug.Log("��ų ����");
            }
            else
            {
                bool bomb = RandomArrow.RandArrowPer(100); // 35%, 35%
                if (bomb)
                {
                    curArrowB = PN.InstantiateRoomObject(arrowBomb.name, aSpawnPosition[0].position, aSpawnPosition[0].rotation, 0);
                    Debug.Log("��ź ����");
                }
                else
                {
                    curArrowB = PN.InstantiateRoomObject(arrowMulti.name, aSpawnPosition[0].position, aSpawnPosition[0].rotation, 0);
                    Debug.Log("��Ƽ�� ����");
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
                Debug.Log("��ų ����");
            }
            else
            {
                bool bomb = RandomArrow.RandArrowPer(100);// 45%, 45%
                if (bomb)
                {
                    curArrowBB = PN.InstantiateRoomObject(arrowBomb.name, aSpawnPosition[1].position, aSpawnPosition[1].rotation, 0);
                    Debug.Log("��ź ����");
                }
                else
                {
                    curArrowBB = PN.InstantiateRoomObject(arrowMulti.name, aSpawnPosition[1].position, aSpawnPosition[1].rotation, 0);
                    Debug.Log("��Ƽ�� ����");
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
                Debug.Log("��ų ����");
            }
            else
            {
                bool bomb = RandomArrow.RandArrowPer(100);
                if (bomb)
                {
                    curArrowR = PN.InstantiateRoomObject(arrowBomb.name, aSpawnPosition[2].position, aSpawnPosition[2].rotation, 0);
                    Debug.Log("��ź ����");
                }
                else
                {
                    curArrowR = PN.InstantiateRoomObject(arrowMulti.name, aSpawnPosition[2].position, aSpawnPosition[2].rotation, 0);
                    Debug.Log("��Ƽ�� ����");
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
                Debug.Log("��ų ����");
            }
            else
            {
                bool bomb = RandomArrow.RandArrowPer(100);
                if (bomb)
                {
                    curArrowRR = PN.InstantiateRoomObject(arrowBomb.name, aSpawnPosition[3].position, aSpawnPosition[3].rotation, 0);
                    Debug.Log("��ź ����");
                }
                else
                {
                    curArrowRR = PN.InstantiateRoomObject(arrowMulti.name, aSpawnPosition[3].position, aSpawnPosition[3].rotation, 0);
                    Debug.Log("��Ƽ�� ����");
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
            PV.RPC(nameof(Notice1min), RpcTarget.AllViaServer);   // ���� ������ 60���� �˸�
        }

        if (limitedTime == 8)                                 // ���� ������ 8������ �˸�
        {
            PV.RPC(nameof(Notice), RpcTarget.AllViaServer);
        }

        if (limitedTime <= 0)
        {
            count = false;
            limitedTime = 0;
            PV.RPC(nameof(EndGameT), RpcTarget.AllViaServer);
            Debug.Log("Ÿ�ӿ���");
        }
    }

    public IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(4);
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
        countText[0].text = string.Format("3�� �ڿ� ���� ���������� �̵��մϴ�");
        countText[1].text = string.Format("3�� �ڿ� ���� ���������� �̵��մϴ�");
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
