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
    [Header("���� ���ѽð�")]
    [SerializeField] TextMeshPro timerText;

    private GameObject spawnPlayer;
    [SerializeField] bool count;
    [SerializeField] int limitedTime;
    readonly Hashtable setTime = new Hashtable();
      
    private void Start()
    {        
        if (PN.IsConnected && NetworkManager.NM.isRed)
        {
            NetworkManager.NM.inGame = true;
            count = true;
            spawnPlayer = PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity);
            Debug.Log($"{PN.CurrentRoom.Name} �濡 ������{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
        }
        else
        {
            NetworkManager.NM.inGame = true;
            count = true;
            spawnPlayer = PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);
            Debug.Log($"{PN.CurrentRoom.Name} �濡 �����{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
        }
    }

    private void Update()
    {        
        if (PN.InRoom && PN.IsConnectedAndReady && NetworkManager.NM.inGame)
        {
            limitedTime = (int)PN.CurrentRoom.CustomProperties["Time"];
            float min = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] / 60);
            float sec = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] % 60);
            timerText.text = string.Format("�����ð� {0:00}�� {1:00}��", min, sec);
            if (PN.IsMasterClient)
            {
                if (count)
                {
                    count = false;
                    StartCoroutine(PlayTimer());
                }
            }
            if (limitedTime < 60)
            {
                timerText.text = string.Format("�����ð� {0:0}��", sec);
            }
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
            PN.LeaveRoom(spawnPlayer);
            limitedTime = 0;
            timerText.text = string.Format("�����ð� 0��");
            
            Debug.Log("Ÿ�ӿ���");
        }
    }

    public override void OnLeftRoom()
    {
        if (PN.IsMasterClient)
        {
            PN.DestroyAll();
        }

        NetworkManager.NM.isRePlay = true;
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

   
}
