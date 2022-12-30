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
using static UnityEngine.Rendering.DebugUI;

public class TutorialManager2 : MonoBehaviourPunCallbacks
{
    public static TutorialManager2 TM2;

    [SerializeField] GameObject redTeam;                // ������ ������
    [SerializeField] GameObject blueTeam;               // ����� ������
    [SerializeField] GameObject admin;                  // ������ ������
    public GameObject shield;                             // ���� ������
    public Transform adminPoint;                        // ������ ������ġ
    private GameObject spawnPlayer;                     // �����Ǵ� �÷��̾�
    public GameObject bomB;                             // �����Ǵ� ��ź
    public Transform[] bSpawnPosition;                  // ��ź ������ġ
    private GameObject bomBs;
    private GameObject shieldCap;
    private void Awake()
    {
        TM2 = this;
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
                InvokeRepeating(nameof(SpawnDynamite), 10, 30);                // ���� ���۵ǰ� 20���Ŀ� �����ϰ� �� �� 15�ʸ��� ����
                SpawnShield();
            }
            SpawnPlayer();

            if (DataManager.DM.currentTeam != Team.ADMIN)     // ������ ����� �ʿ��� �ڵ�
            {
                Destroy(admin);
            }
        }
    }
    private void Update()
    {       
         // ������ ���α׷� ���� ��
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { PN.LoadLevel(4); }
            else if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnDynamite();
            }
       /* if (PN.InRoom && PN.IsMasterClient)
        {

            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { PN.LoadLevel(4); }
                else if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    SpawnDynamite();
                }
            }
        }        */
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
                if (Application.platform == RuntimePlatform.WindowsPlayer)
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

    public void SpawnDynamite()
    {
        for (int i = 0; i < bSpawnPosition.Length; i++)
        {
           bomBs = PN.InstantiateRoomObject(bomB.name, bSpawnPosition[i].position, bSpawnPosition[i].rotation, 0);
        }
    }

    public void SpawnShield()
    {
        for (int i = 0; i < bSpawnPosition.Length; i++)
        {
            shieldCap = PN.InstantiateRoomObject(shield.name, bSpawnPosition[i].position, bSpawnPosition[i].rotation, 0);
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
