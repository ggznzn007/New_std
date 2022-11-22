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
using UnityEngine.XR;

public class TutorialManager : MonoBehaviourPunCallbacks
{
    public static TutorialManager TM;

    [SerializeField] GameObject redTeam;                // ������ ������
    [SerializeField] GameObject blueTeam;               // ����� ������
    [SerializeField] GameObject defaultTeam;
    [SerializeField] GameObject admin;                  // ������ ������
    public Transform adminPoint;                        // ������ ������ġ
    private GameObject spawnPlayer;                     // �����Ǵ� �÷��̾�
    public GameObject bomB;                             // ��ź ������
    public Transform[] bSpawnPosition;                  // ��ź ������ġ
    private GameObject bomBs;                           // �����Ǵ� ��ź

    private void Awake()
    {
        TM = this;
    }

    private void Start()
    {
        if (!PN.IsConnectedAndReady)
        {
            SceneManager.LoadScene(0);
        }
        if (PN.IsConnectedAndReady && PN.InRoom)
        {
            if (PN.IsMasterClient)
            {
                InvokeRepeating(nameof(SpawnBomb), 20, 20);             // ���� ���۵ǰ� 20���Ŀ� �����ϰ� �� �� 15�ʸ��� ����
            }

            switch (PN.CurrentRoom.PlayerCount)
            {
                case 1:
                    PN.LocalPlayer.NickName = "������";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 2:
                    PN.LocalPlayer.NickName = "�÷��̾� 1";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 3:
                    PN.LocalPlayer.NickName = "�÷��̾� 2";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 4:
                    PN.LocalPlayer.NickName = "�÷��̾� 3";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 5:
                    PN.LocalPlayer.NickName = "�÷��̾� 4";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 6:
                    PN.LocalPlayer.NickName = "�÷��̾� 5";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                default:
                    PN.LocalPlayer.NickName = "������ �÷��̾�";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
            }

           /* if (DataManager.DM.currentTeam != Team.ADMIN)  // ������ ����� �ʿ��� �ڵ�
            {
                Destroy(admin);
            }*/
        }
    }

    private void Update()
    {
        if (PN.InRoom && PN.IsMasterClient)
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)  // ������ ����
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {  PN.LoadLevel(2); }
                else if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    SpawnBomb();
                }
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
           
             // ������ ���α׷� ���� ��
            case Team.ADMIN:
                if (Application.platform == RuntimePlatform.WindowsPlayer)//|| Application.platform == RuntimePlatform.WindowsEditor)
                {
                    PN.AutomaticallySyncScene = true;
                    DataManager.DM.inGame = false;
                    spawnPlayer = PN.Instantiate(admin.name, adminPoint.position, adminPoint.rotation);
                    Debug.Log($"{PN.CurrentRoom.Name} �濡 ������{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
                    Info();
                }
                break;
            

            default:
                return;
        }
    }

    public void SpawnBomb()
    {
        for (int i = 0; i < bSpawnPosition.Length; i++)
        {
           bomBs = PN.Instantiate(bomB.name, bSpawnPosition[i].position, bSpawnPosition[i].rotation, 0);
        }
    }
    
    [ContextMenu("���� ���� ����")]
    void Info()
    {
        if (PN.InRoom)
        {
            print("���� �� �̸� : " + PN.CurrentRoom.Name);
            print("���� �� �ο� �� : " + PN.CurrentRoom.PlayerCount + "��");
            print("���� �� MAX�ο� : " + PN.CurrentRoom.MaxPlayers + "��");

            string playerStr = "�濡 �ִ� �÷��̾� �̸� \n";
            for (int i = 0; i < PN.PlayerList.Length; i++)
            {
                playerStr += PN.PlayerList[i].NickName + ", \n\t";
                print(playerStr);
            }
        }
        else
        {
            print("������ �ο� ��: " + PN.CountOfPlayers + "��");
            print("�κ� �ִ� ����: " + PN.InLobby);
            print("������ �ο� ��: " + PN.CountOfPlayers + "��");
            print("���� ���Ῡ��: " + PN.IsConnected);
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
        SceneManager.LoadScene(0);
        //PN.LoadLevel(0);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName}�� �����ο�:{PN.CurrentRoom.PlayerCount}");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName}�� �����ο�:{PN.CurrentRoom.PlayerCount}");
    }

}
