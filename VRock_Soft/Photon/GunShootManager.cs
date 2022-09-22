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
    private GameObject spawnPlayer;
    [SerializeField] bool count;
    [SerializeField] int limitedTime;
    readonly Hashtable setTime = new Hashtable();
    public Hashtable isRed = new Hashtable();

    private void Awake()
    {
        GSM = this;
    }
    private void Start()
    {        
        if (PN.IsConnected && NetworkManager.NM.isRed)
        {
            isRed["isRed"]=true;
            NetworkManager.NM.inGame = true;
            count = true;
            spawnPlayer = PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity);
            Debug.Log($"{PN.CurrentRoom.Name} �濡 ������{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
            Info();
        }
        else
        {
            isRed["isRed"] = false;
            NetworkManager.NM.inGame = true;
            count = true;
            spawnPlayer = PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);
            Debug.Log($"{PN.CurrentRoom.Name} �濡 �����{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
            Info();
        }
    }

    private void Update()
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
}
