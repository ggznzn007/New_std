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
public class TutorialManager : MonoBehaviourPunCallbacks
{
    public static TutorialManager TM;

    [SerializeField] GameObject redTeam;
    [SerializeField] GameObject blueTeam;

    private GameObject spawnPlayer;

    private void Awake()
    {
        TM = this;

    }
    private void Start()
    {
        if(!PN.IsConnectedAndReady)
        {
            SceneManager.LoadScene(0);
           // PN.LoadLevel(0);
        }
        
        if (PN.IsConnectedAndReady && DataManager.DM.currentTeam == Team.RED)
        {
            PN.AutomaticallySyncScene = true;                                           // 자동으로 씬 동기화
            NetworkManager.NM.inGame = false;
            spawnPlayer = PN.Instantiate(redTeam.name, Vector3.zero, Quaternion.identity, 0);
            Debug.Log($"{PN.CurrentRoom.Name} 방에 레드팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
            Info();
        }
        else
        {
            PN.AutomaticallySyncScene = true;                                           // 자동으로 씬 동기화
            NetworkManager.NM.inGame = false;
            spawnPlayer = PN.Instantiate(blueTeam.name, Vector3.zero, Quaternion.identity, 0);
            Debug.Log($"{PN.CurrentRoom.Name} 방에 블루팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
            Info();
        }
    }


   /* IEnumerator CreatePlayer()
    {
        yield return new WaitUntil(() => PN.IsConnectedAndReady);

        if (PN.IsConnectedAndReady&&DataManager.DM.currentTeam==Team.RED)
        {
            NetworkManager.NM.inGame = false;
            spawnPlayer = PN.Instantiate(redTeam.name, Vector3.zero, Quaternion.identity, 0);
            Debug.Log($"{PN.CurrentRoom.Name} 방에 레드팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
            Info();
        }
        else
        {
            NetworkManager.NM.inGame = false;
            spawnPlayer = PN.Instantiate(blueTeam.name, Vector3.zero, Quaternion.identity, 0);
            Debug.Log($"{PN.CurrentRoom.Name} 방에 블루팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
            Info();
        }
    }*/

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

    private void OnApplicationQuit()
    {
        PN.Disconnect();
    }
}
