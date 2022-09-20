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
public class TutorialManager : MonoBehaviourPunCallbacks
{
    private GameObject spawnPlayer;
    
    public override void OnJoinedRoom()
    {
        if(NetworkManager.NM.isRed)
        {
            NetworkManager.NM.inGame = false;
            spawnPlayer =  PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity);
            Debug.Log($"{PN.CurrentRoom.Name} �濡 {PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
        }
        else
        {
            NetworkManager.NM.inGame = false;
            spawnPlayer =  PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);
            Debug.Log($"{PN.CurrentRoom.Name} �濡 {PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
        }
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

    public override void OnLeftRoom()
    {
        if(PN.IsMasterClient)
        {
            PN.DestroyAll();
        }
        
        PN.Destroy(spawnPlayer);
       
        SceneManager.LoadScene(0);
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
