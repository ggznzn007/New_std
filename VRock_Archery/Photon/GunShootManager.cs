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

public class GunShootManager : MonoBehaviourPunCallbacks      // ��ó ���� ���� ��ũ��Ʈ
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
    [Header("Eagle NPC ������")]                                           
    public GameObject eagleNPC;
    public Transform[] wayPos;
    public GameObject eagleBomb;
    public Transform spawnPoint;
    public GameObject myBomb = null; 
    [SerializeField] bool count = false;                             // Ÿ�̸� ī��Ʈ ����ġ
    [SerializeField] int limitedTime;                                // �������ѽð�
    public Transform adminPoint;                                     // ������ ���� ��ġ

    public string GameInfo1;                                         // ���� �˸� ���������� ���� ���ڿ� "������ 3�ʵڿ� ���۵˴ϴ�."
    public string GameInfo2;                                         // ���� �˸� ���������� ���� ���ڿ� "3�� �ڿ� �������������� �̵��մϴ�"
    public string GameInfo3;                                         // ���� �˸� ���������� ���� ���ڿ� "������ �� ����˴ϴ�."
    public string GameInfo4;                                         // ���� �˸� ���������� ���� ���ڿ� "������� �¸��Ͽ����ϴ�."
    public string GameInfo5;                                         // ���� �˸� ���������� ���� ���ڿ� "�������� �¸��Ͽ����ϴ�."
    public string GameInfo6;                                         // ���� �˸� ���������� ���� ���ڿ� "���º��Դϴ�."
    public string GameInfo7;                                         // ���� �˸� ���������� ���� ���ڿ� "score_result."
    public string GameInfo9;                                         // ���� �˸� ���������� ���� ���ڿ� "������ 1�� ���ҽ��ϴ�."
    public string one;                                               // ���� �˸� ���������� ���� ���ڿ� "1"
    public string two;                                               // ���� �˸� ���������� ���� ���ڿ� "2"
    public string three;                                             // ���� �˸� ���������� ���� ���ڿ� "3"
    public string gameStart;                                         // ���� �˸� ���������� ���� ���ڿ� "��ŸƮ"
    public string gameover;                                          // ���� �˸� ���������� ���� ���ڿ� "���ӿ���"
    public int kills;                                                // ų ��
    public Image[] gameOverImage;                                    // ���� UI�� ������ ���ӿ��� �̹���
    public Image[] bluewinImg;                                       // ���� UI�� ������ ����� �̰��� �� �̹���
    public Image[] redwinImg;                                        // ���� UI�� ������ ������ �̰��� �� �̹���
    public Image[] blueloseImg;                                      // ���� UI�� ������ ����� ���� �� �̹���
    public Image[] redloseImg;                                       // ���� UI�� ������ ������ ���� �� �̹���
    public Image[] drawImg;                                          // ���� UI�� ������ ����� �� �̹���
    public TMP_Text[] blueScore;                                     // ���� UI�� ������ ����� ���� �ؽ�Ʈ
    public TMP_Text[] redScore;                                      // ���� UI�� ������ ������ ���� �ؽ�Ʈ
    public int score_BlueKill;                                       // ���� UI�� ������ ����� ����
    public int score_RedKill;                                        // ���� UI�� ������ ������ ����  
    PhotonView PV;                                                   // �����
    Hashtable setTime = new Hashtable();                             // ���ӽð�
    private GameObject spawnPlayer;                                  // �����Ǵ� �÷��̾�
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
        blueScore[1].text = score_RedKill.ToString();    // ������ ����
        redScore[0].text = score_BlueKill.ToString();    // ����� ����
        redScore[1].text = score_RedKill.ToString();     // ������ ����
    }
    public void UpdateStats()
    {
        playerProp["kills"] = kills;             // ų ��                                                

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
    public void EndGameT()                   // "���� ����"
    {
        StartCoroutine(LeaveGame());
    }

    [PunRPC]
    public void Notice()
    {
        AudioManager.AM.PlaySE(GameInfo3); // "������ �� ����˴ϴ�." �˸� ����� ���
    }

    [PunRPC]
    public void Notice1min()
    {
        AudioManager.AM.PlaySE(GameInfo9); // "������ 1�� ���ҽ��ϴ�." �˸� ����� ���
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
            PV.RPC(nameof(Notice1min), RpcTarget.AllViaServer);   // ���� ������ 1�� �� �˸� RPC�� ��ü �˸�
        }

        if (limitedTime == 8)                                     // ���� ������ 8������ �˸� RPC�� ��ü �˸�
        {
            PV.RPC(nameof(Notice), RpcTarget.AllViaServer);
        }

        if (limitedTime <= 0)
        {
            count = false;
            limitedTime = 0;
            PV.RPC(nameof(EndGameT), RpcTarget.AllViaServer);     // ���� ���Ḧ RPC�� ��ü �˸�            
        }
    }

    public IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(4);                            
        AudioManager.AM.PlaySE(GameInfo1);                             // "������ 3�� �ڿ� ���۵˴ϴ�." �˸� ����� ���
        countText[0].text = string.Format("������ 3�� �ڿ� ���۵˴ϴ�.");  // ����� UI�� ���ڷ� ����
        countText[1].text = string.Format("������ 3�� �ڿ� ���۵˴ϴ�.");  // ������ UI�� ���ڷ� ����
        yield return new WaitForSeconds(3);                            
        AudioManager.AM.PlaySE(three);                                 // "Three" �˸� ����� ���
        countText[0].text = string.Format("3");                        // ����� UI�� ���ڷ� ����
        countText[1].text = string.Format("3");                        // ������ UI�� ���ڷ� ����
        yield return new WaitForSeconds(1);                            
        AudioManager.AM.PlaySE(two);                                   // "Two" �˸� ����� ���
        countText[0].text = string.Format("2");                        // ����� UI�� ���ڷ� ����
        countText[1].text = string.Format("2");                        // ������ UI�� ���ڷ� ����
        yield return new WaitForSeconds(1);                            
        AudioManager.AM.PlaySE(one);                                   // "One" �˸� ����� ���
        countText[0].text = string.Format("1");                        // ����� UI�� ���ڷ� ����
        countText[1].text = string.Format("1");                        // ������ UI�� ���ڷ� ����
        yield return new WaitForSeconds(1);
        AudioManager.AM.PlaySE(gameStart);                             // "��ŸƮ" �˸� ����� ���
        countText[0].text = string.Format("���� ��ŸƮ!!!");             // ����� UI�� ���ڷ� ����
        countText[1].text = string.Format("���� ��ŸƮ!!!");             // ������ UI�� ���ڷ� ����
        yield return new WaitForSeconds(1);
        count = true;                                                  // ���� Ÿ�̸� ����
        DataManager.DM.inGame = true;                                  // ���� �� ���� ������ ����
        timerText[0].gameObject.SetActive(true);                       // ����� UI�� Ÿ�̸� on
        timerText[1].gameObject.SetActive(true);                       // ������ UI�� Ÿ�̸� on
        countText[0].gameObject.SetActive(false);                      // ����� UI�� �ȳ����� off
        countText[1].gameObject.SetActive(false);                      // ������ UI�� �ȳ����� off
    }

    public IEnumerator LeaveGame()
    {
        timerText[0].gameObject.SetActive(false);                      // ����� UI�� Ÿ�̸� off
        timerText[1].gameObject.SetActive(false);                      // ������ UI�� Ÿ�̸� off
        resultText[0].gameObject.SetActive(true);                      // ����� UI�� ��� on
        resultText[1].gameObject.SetActive(true);                      // ������ UI�� ��� on
        DataManager.DM.inGame = false;                                 // ���� �� ���� ������ ����
        DataManager.DM.gameOver = true;                                // ���ӿ��� ���� true
        AudioManager.AM.PlaySE(gameover);                              // "���ӿ���" �˸� ����� ���
        gameOverImage[0].gameObject.SetActive(true);                   // ����� UI�� ���ӿ��� �̹��� on
        gameOverImage[1].gameObject.SetActive(true);                   // ������ UI�� ���ӿ��� �̹��� on   
        yield return new WaitForSeconds(2);
        VictoryTeam();                                                 // ���� ��� ���� �޼��� ȣ��(�¸�,���º�)
        yield return new WaitForSeconds(3);
        AudioManager.AM.PlaySE(GameInfo7);                             // "���Ӱ�� ��ǥ ��" �˸� ����� ���
        gameOverImage[0].gameObject.SetActive(false);                  // ����� UI�� ���ӿ��� �̹��� off
        gameOverImage[1].gameObject.SetActive(false);                  // ������ UI�� ���ӿ��� �̹��� off  
        yield return new WaitForSeconds(3);
        countText[0].gameObject.SetActive(true);                       // ����� UI�� �ȳ����� on
        countText[1].gameObject.SetActive(true);                       // ������ UI�� �ȳ����� on
        AudioManager.AM.PlaySE(GameInfo2);                             // "3�� �ڿ� ���� ���������� �̵��մϴ�" �˸� ����� ���
        countText[0].text = string.Format("3�� �ڿ� ���� ���������� �̵��մϴ�");
        countText[1].text = string.Format("3�� �ڿ� ���� ���������� �̵��մϴ�");
        yield return new WaitForSeconds(5);
        PN.LeaveRoom();
        StopCoroutine(LeaveGame());                                    // �� ������ �޼��� ȣ��
    }

    public void VictoryTeam()
    {
        if (score_BlueKill > score_RedKill)                                   // ����� �¸� ��
        {
            AudioManager.AM.PlaySE(GameInfo4);                                // "������� �¸��Ͽ����ϴ�." �˸� ����� ���            
            blueScore[0].gameObject.SetActive(false);                         // ����� UI�� ����� ���ھ� off
            blueScore[1].gameObject.SetActive(false);                         // ������ UI�� ����� ���ھ� off
            redScore[0].gameObject.SetActive(false);                          // ����� UI�� ������ ���ھ� off
            redScore[1].gameObject.SetActive(false);                          // ������ UI�� ������ ���ھ� off 
            bluewinImg[0].gameObject.SetActive(true);                         // ����� UI�� ����� Win �̹��� on
            bluewinImg[1].gameObject.SetActive(true);                         // ������ UI�� ����� Win �̹��� on
            redloseImg[0].gameObject.SetActive(true);                         // ����� UI�� ������ Lose �̹��� on
            redloseImg[1].gameObject.SetActive(true);                         // ������ UI�� ������ Lose �̹��� on
        }
        else if (score_BlueKill < score_RedKill)                              // ������ �¸� ��
        {
            AudioManager.AM.PlaySE(GameInfo5);                                // "�������� �¸��Ͽ����ϴ�." �˸� ����� ��� 
            blueScore[0].gameObject.SetActive(false);                         // ����� UI�� ����� ���ھ� off
            blueScore[1].gameObject.SetActive(false);                         // ������ UI�� ����� ���ھ� off
            redScore[0].gameObject.SetActive(false);                          // ����� UI�� ������ ���ھ� off
            redScore[1].gameObject.SetActive(false);                          // ������ UI�� ������ ���ھ� off 
            blueloseImg[0].gameObject.SetActive(true);                        // ����� UI�� ����� Lose �̹��� on
            blueloseImg[1].gameObject.SetActive(true);                        // ������ UI�� ����� Lose �̹��� on
            redwinImg[0].gameObject.SetActive(true);                          // ����� UI�� ������ Win �̹��� on
            redwinImg[1].gameObject.SetActive(true);                          // ������ UI�� ������ Win �̹��� on
        }
        else if (score_BlueKill == score_RedKill)                             // ���º� ��
        {
            AudioManager.AM.PlaySE(GameInfo6);                                // "���º��Դϴ�." �˸� ����� ��� 
            blueScore[0].gameObject.SetActive(false);                         // ����� UI�� ����� ���ھ� off
            blueScore[1].gameObject.SetActive(false);                         // ������ UI�� ����� ���ھ� off
            redScore[0].gameObject.SetActive(false);                          // ����� UI�� ������ ���ھ� off
            redScore[1].gameObject.SetActive(false);                          // ������ UI�� ������ ���ھ� off
            drawImg[0].gameObject.SetActive(true);                            // ����� UI�� ������ Draw �̹��� on
            drawImg[1].gameObject.SetActive(true);                            // ������ UI�� ������ Draw �̹��� on
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
