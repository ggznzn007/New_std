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
public class WesternManager : MonoBehaviourPunCallbacks
{
    public static WesternManager WM;
    [Header("게임 시작 텍스트")]
    [SerializeField] TextMeshPro startText;
    [Header("게임 제한시간")]
    [SerializeField] TextMeshPro timerText;

    //[Header("게임종료 UI")]
   // [SerializeField] GameObject quitUI;
    private GameObject spawnPlayer;

    [SerializeField] GameObject redTeam;
    [SerializeField] GameObject blueTeam;
    [SerializeField] bool count = false;
    [SerializeField] int limitedTime;
    Hashtable setTime = new Hashtable();
    private void Awake()
    {
        WM = this;
        DataManager.DM.currentMap = Map.WESTERN;
    }
    void Start()
    {
        if (PN.IsConnectedAndReady && PN.InRoom)
        {
            SpawnPlayer();
        }      
    }

    public void SpawnPlayer()
    {
        switch (DataManager.DM.currentTeam)
        {
            case Team.RED:
                PN.AutomaticallySyncScene = true;
                NetworkManager.NM.inGame = true;
                spawnPlayer = PN.Instantiate(redTeam.name, Vector3.zero, Quaternion.identity);                
                Debug.Log($"{PN.CurrentRoom.Name} 방에 레드팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                Info();
                if (PN.IsMasterClient)
                {
                    StartCoroutine(StartTimer());
                }
                break;

            case Team.BLUE:
                PN.AutomaticallySyncScene = true;
                NetworkManager.NM.inGame = true;
                spawnPlayer = PN.Instantiate(blueTeam.name, Vector3.zero, Quaternion.identity);                
                Debug.Log($"{PN.CurrentRoom.Name} 방에 블루팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                Info();
                if (PN.IsMasterClient)
                {
                    StartCoroutine(StartTimer());
                }
                break;

            default:
                return;
        }
    }
    void FixedUpdate()
    {
        if (PN.InRoom&& PN.IsConnectedAndReady)
        {           
            limitedTime = (int)PN.CurrentRoom.CustomProperties["Time"];
            float min = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] / 60);
            float sec = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] % 60);
            timerText.text = string.Format("남은시간 {0:00}분 {1:00}초", min, sec);
            if (limitedTime < 60)
            {
                timerText.text = string.Format("남은시간 {0:0}초", sec);
            }
            if (limitedTime <= 0)
            {
                count = false;
                limitedTime = 0;
                timerText.text = string.Format("GAME OVER\n 5초 뒤에 로비로 이동합니다.");                
                StartCoroutine(LeaveGame());
                Debug.Log("타임오버");
            }
            if (PN.IsMasterClient)
            {
                if (count)
                {
                    count = false;
                    StartCoroutine(PlayTimer());
                }
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
    }
    IEnumerator StartTimer()
    {
        startText.text = string.Format("게임이 5초 뒤에 시작됩니다.");
        yield return new WaitForSeconds(5);
        startText.text = string.Format("5");
        yield return new WaitForSeconds(1);
        startText.text = string.Format("4");
        yield return new WaitForSeconds(1);
        startText.text = string.Format("3");
        yield return new WaitForSeconds(1);
        startText.text = string.Format("2");
        yield return new WaitForSeconds(1);
        startText.text = string.Format("1");
        yield return new WaitForSeconds(1);
        startText.text = string.Format("게임 스타트!!!");
        yield return new WaitForSeconds(1);
        count = true;
        timerText.gameObject.SetActive(true);
        startText.gameObject.SetActive(false);
    }

    IEnumerator LeaveGame()
    {
        yield return new WaitForSeconds(5);
        /*if (PN.IsMasterClient)
        {
            PN.LoadLevel(3);
        }*/
         PN.LeaveRoom();
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
        PN.LeaveRoom();
    }

}
