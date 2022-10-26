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
//using static UnityEditor.PlayerSettings;

public class TutorialManager2 : MonoBehaviourPunCallbacks
{
    public static TutorialManager2 TM2;

    [SerializeField] GameObject redTeam;
    [SerializeField] GameObject blueTeam;
    [SerializeField] GameObject admin;
    public Transform adminPoint;
    private GameObject spawnPlayer;
    //public Vector3 adminPos = new Vector3(0, 3.5f, 5.11f);
    //public Quaternion adminRot = new Quaternion(30, 180, 0, 0);
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
            SpawnPlayer();
            if (DataManager.DM.currentTeam != Team.ADMIN)
            {
                Destroy(admin);
            }
        }
    }
    private void Update()
    {
        /* if (SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool pressed))
         {
             if (pressed)
             {
                 PN.LoadLevel(4);
             }
         }*/
#if UNITY_EDITOR_WIN            // ����Ƽ �����Ϳ��� ��� ��
        if (PN.InRoom && PN.IsMasterClient)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { PN.LoadLevel(4); }
            else if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
        }
#endif
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
#if UNITY_EDITOR_WIN            // ����Ƽ �����Ϳ��� ��� ��
            case Team.ADMIN:
                PN.AutomaticallySyncScene = true;
                DataManager.DM.inGame = false;
                spawnPlayer = PN.Instantiate(admin.name, adminPoint.position, adminPoint.rotation);
                Debug.Log($"{PN.CurrentRoom.Name} �濡 ������{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
                Info();
                break;
#endif
            default:
                return;
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

            string playerStr = "�濡 �ִ� �÷��̾� �̸� ";
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
