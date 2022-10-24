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
#if UNITY_EDITOR_WIN            // 유니티 에디터에서 재생 시
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
                Debug.Log($"{PN.CurrentRoom.Name} 방에 레드팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                Info();
                break;

            case Team.BLUE:
                PN.AutomaticallySyncScene = true;
                DataManager.DM.inGame = false;
                spawnPlayer = PN.Instantiate(blueTeam.name, Vector3.zero, Quaternion.identity);
                Debug.Log($"{PN.CurrentRoom.Name} 방에 블루팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                Info();
                break;
#if UNITY_EDITOR_WIN            // 유니티 에디터에서 재생 시
            case Team.ADMIN:
                PN.AutomaticallySyncScene = true;
                DataManager.DM.inGame = false;
                spawnPlayer = PN.Instantiate(admin.name, adminPoint.position, adminPoint.rotation);
                Debug.Log($"{PN.CurrentRoom.Name} 방에 관리자{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                Info();
                break;
#endif
            default:
                return;
        }
    }

    [ContextMenu("포톤 서버 정보")]
    void Info()
    {
        if (PN.InRoom)
        {
            print("현재 방 이름 : " + PN.CurrentRoom.Name);
            print("현재 방 인원 수 : " + PN.CurrentRoom.PlayerCount + "명");
            print("현재 방 MAX인원 : " + PN.CurrentRoom.MaxPlayers + "명");

            string playerStr = "방에 있는 플레이어 이름 ";
            for (int i = 0; i < PN.PlayerList.Length; i++)
            {
                playerStr += PN.PlayerList[i].NickName + ", \n\t";
                print(playerStr);
            }
        }
        else
        {
            print("접속한 인원 수: " + PN.CountOfPlayers + "명");
            print("로비에 있는 여부: " + PN.InLobby);
            print("접속한 인원 수: " + PN.CountOfPlayers + "명");
            print("서버 연결여부: " + PN.IsConnected);
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
        Debug.Log($"{newPlayer.NickName}님 현재인원:{PN.CurrentRoom.PlayerCount}");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName}님 현재인원:{PN.CurrentRoom.PlayerCount}");
    }

}
