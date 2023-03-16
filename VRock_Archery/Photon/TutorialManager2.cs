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

public class TutorialManager2 : MonoBehaviourPunCallbacks
{
    public static TutorialManager2 TM2;

    [SerializeField] GameObject redTeam;                // ������ ������
    [SerializeField] GameObject blueTeam;               // ����� ������
    [SerializeField] GameObject admin;                  // ������ ������
    public GameObject eagleNPC;
    //public GameObject eagleBlock;
    public Transform adminPoint;                        // ������ ������ġ
    public Transform eaglePoint;
    private GameObject spawnPlayer;                     // �����Ǵ� �÷��̾�
    public GameObject snowBlock;                             // �����Ǵ� ��ź   
    public Transform[] blockPoint;    
    private float curTime;
    private float limit = 35;                                // ����� �� ���� ������

    private void Awake()
    {
        TM2 = this;
    }
    private void Start()
    {
        DataManager.DM.isReady = false;
        if (!PN.IsConnectedAndReady)
        {
            SceneManager.LoadScene(0);
        }
        if (PN.IsConnectedAndReady && PN.InRoom)
        {            
            SpawnPlayer();

           /* if (DataManager.DM.currentTeam != Team.ADMIN)     // ������ ����� �ʿ��� �ڵ�
            {
                Destroy(admin);
            }*/

           
        }
    }
    private void Update()
    {
        // ������ ���α׷� ���� ��
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (!DataManager.DM.isReady)
            {
                DataManager.DM.isReady = true;
                photonView.RPC(nameof(Ready2), RpcTarget.AllViaServer);
            }
            else if (DataManager.DM.isReady)
            {
                DataManager.DM.isReady = false;
                PN.LoadLevel(4);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) { StartCoroutine(nameof(ExitGame)); }

        if(PN.IsMasterClient)
        {
            SpawnBlock();            
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
                if (Application.platform == RuntimePlatform.WindowsPlayer)// || Application.platform == RuntimePlatform.WindowsEditor)
                {
                    if (Application.platform != RuntimePlatform.WindowsPlayer) { return; }
                    PN.AutomaticallySyncScene = true;
                    DataManager.DM.inGame = false;
                    DataManager.DM.gameOver = false;
                    spawnPlayer = PN.Instantiate(admin.name, adminPoint.position, adminPoint.rotation);
                    Debug.Log($"{PN.CurrentRoom.Name} �濡 ������{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
                    Info();
                }
                break;

            default:
                return;
        }
    }

    [PunRPC]
    public void Ready2()
    {
        DataManager.DM.gameOver = true;
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
   
    public void SpawnBlock()                                                                       // ������ �ð����� �����Ǵ� ����
    {
        curTime += Time.deltaTime;

        if(curTime>=limit)
        {
            for (int i = 0; i < blockPoint.Length; i++)
            {
                PN.InstantiateRoomObject(snowBlock.name, blockPoint[i].position, blockPoint[i].rotation, 0);
                curTime = 0;
            }
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
