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
public class GunShootManager : MonoBehaviourPunCallbacks
{
    public static GunShootManager GSM;

    [Header("���� ���ѽð�")]
    [SerializeField] TextMeshPro timerText;

    [Header("�������� UI")]
    [SerializeField] GameObject quitUI;

    [Header("������")]
    [SerializeField] GameObject redTeam;

    [Header("�����")]
    [SerializeField] GameObject blueTeam;

    private GameObject spawnPlayer;
    [SerializeField] int limitedTime;

    public bool count;
    readonly Hashtable setTime = new Hashtable();    

    private void Awake()
    {
        GSM = this;
    }
    private void Start()
    {
        if (!PN.IsConnectedAndReady)
        {
            SceneManager.LoadScene(0);
            //PN.LoadLevel(0);
        }
        if (PN.IsConnectedAndReady && DataManager.DM.currentTeam == Team.RED)
        {
            PN.AutomaticallySyncScene = true;                                           // �ڵ����� �� ����ȭ
            NetworkManager.NM.inGame = true;
            count = true;
            spawnPlayer = PN.Instantiate(redTeam.name, Vector3.zero, Quaternion.identity, 0);
            Debug.Log($"{PN.CurrentRoom.Name} �濡 ������{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
            Info();
        }
        else
        {
            PN.AutomaticallySyncScene = true;                                           // �ڵ����� �� ����ȭ
            NetworkManager.NM.inGame = true;
            count = true;
            spawnPlayer = PN.Instantiate(blueTeam.name, Vector3.zero, Quaternion.identity, 0);
            Debug.Log($"{PN.CurrentRoom.Name} �濡 �����{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
            Info();
        }
         
    }

    private void Update()
    {
        if (PN.InRoom)
        {
            limitedTime = (int)PN.CurrentRoom.CustomProperties["Time"];
            float min = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] / 60);
            float sec = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] % 60);
            timerText.text = string.Format("�����ð� {0:00}�� {1:00}��", min, sec);
            if (limitedTime < 60)
            {
                timerText.text = string.Format("�����ð� {0:0}��", sec);
            }
            if (count)
            {
                count = false;
                StartCoroutine(PlayTimer());
            }
            /*if (PN.IsMasterClient)
            {
               
            }*/
        }
    }

    public IEnumerator PlayTimer()
    {
        yield return new WaitForSeconds(1);
        int nextTime = limitedTime -= 1;
        setTime["Time"] = nextTime;
        PN.CurrentRoom.SetCustomProperties(setTime);
        count = true;

        if (limitedTime == 0)
        {            
            limitedTime = 0;
            timerText.text = string.Format("�����ð� 0��");
            timerText.gameObject.SetActive(false);
            quitUI.SetActive(true);
            Debug.Log("Ÿ�ӿ���");
        }
    }

    public override void OnLeftRoom()
    {
        if (PN.IsMasterClient)
        {
            PN.DestroyAll();
        }

        
         PN.Destroy(spawnPlayer);
         SceneManager.LoadScene(0);
        //PN.LoadLevel(0);
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
        PN.LeaveRoom(spawnPlayer);
    }

    private void OnApplicationQuit()
    {
        PN.Disconnect();
    }
}
