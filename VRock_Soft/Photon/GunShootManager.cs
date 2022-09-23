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

    [Header("게임 제한시간")]
    [SerializeField] TextMeshPro timerText;

    [Header("게임종료 UI")]
    [SerializeField] GameObject quitUI;

    [Header("레드팀")]
    [SerializeField] GameObject redTeam;

    [Header("블루팀")]
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
            PN.AutomaticallySyncScene = true;                                           // 자동으로 씬 동기화
            NetworkManager.NM.inGame = true;
            count = true;
            spawnPlayer = PN.Instantiate(redTeam.name, Vector3.zero, Quaternion.identity, 0);
            Debug.Log($"{PN.CurrentRoom.Name} 방에 레드팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
            Info();
        }
        else
        {
            PN.AutomaticallySyncScene = true;                                           // 자동으로 씬 동기화
            NetworkManager.NM.inGame = true;
            count = true;
            spawnPlayer = PN.Instantiate(blueTeam.name, Vector3.zero, Quaternion.identity, 0);
            Debug.Log($"{PN.CurrentRoom.Name} 방에 블루팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
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
            timerText.text = string.Format("남은시간 {0:00}분 {1:00}초", min, sec);
            if (limitedTime < 60)
            {
                timerText.text = string.Format("남은시간 {0:0}초", sec);
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
            timerText.text = string.Format("남은시간 0초");
            timerText.gameObject.SetActive(false);
            quitUI.SetActive(true);
            Debug.Log("타임오버");
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

    [ContextMenu("포톤 서버 정보")]
    void Info()
    {
        if (PN.InRoom)
        {
            print("현재 방 이름: " + PN.CurrentRoom.Name);
            print("현재 방 인원 수: " + PN.CurrentRoom.PlayerCount);
            print("현재 방 MAX인원: " + PN.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록";
            for (int i = 0; i < PN.PlayerList.Length; i++)
            {
                playerStr += PN.PlayerList[i].NickName + ",";
                print(playerStr);
            }

        }
        else
        {
            print("접속한 인원 수: " + PN.CountOfPlayers);
            print("로비에 있는 여부: " + PN.InLobby);
            print("접속한 인원 수: " + PN.CountOfPlayers);
            print("서버 연결여부: " + PN.IsConnected);
        }
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName}님 현재인원:{PN.CurrentRoom.PlayerCount}");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName}님 현재인원:{PN.CurrentRoom.PlayerCount}");
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
