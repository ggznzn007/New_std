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
    [SerializeField] GameObject admin;

    public Vector3 adminPos = new Vector3(0,3.5f,5.11f);
    public Quaternion adminRot = new Quaternion(30,180,0,0);

    private GameObject spawnPlayer;    
    
    private void Awake()
    {
        TM = this;
        
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
            if(DataManager.DM.currentTeam!=Team.ADMIN)
            {
                admin.SetActive(false);                
            }
        }

    }

    private void Update()
    {
#if UNITY_EDITOR_WIN
        if (PN.InRoom&&PN.IsMasterClient)
        {
           if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.KeypadEnter)) { PN.LoadLevel(2); }
            else if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit();}           
        }
#endif
    }

    public void SpawnPlayer()
    {
        switch (DataManager.DM.currentTeam)
        {
            case Team.RED:
                PN.AutomaticallySyncScene = true;
                NetworkManager.NM.inGame = false;
                spawnPlayer = PN.Instantiate(redTeam.name, Vector3.zero, Quaternion.identity);
                Debug.Log($"{PN.CurrentRoom.Name} 방에 레드팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");                
                Info();
                break;

            case Team.BLUE:
                PN.AutomaticallySyncScene = true;
                NetworkManager.NM.inGame = false;
                spawnPlayer = PN.Instantiate(blueTeam.name, Vector3.zero, Quaternion.identity);
                Debug.Log($"{PN.CurrentRoom.Name} 방에 블루팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");                
                Info();
                break;
/*#if UNITY_EDITOR_WIN
            case Team.ADMIN:
                PN.AutomaticallySyncScene = true;
                NetworkManager.NM.inGame = false;
                //spawnPlayer = admin;
                spawnPlayer = PN.Instantiate(admin.name, adminPos, adminRot);
                Debug.Log($"{PN.CurrentRoom.Name} 방에 관리자{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                Info();
                break;
#endif*/

            default:
                return;
        }
    }

   /*public IEnumerator MasterKey()
    {
        yield return new WaitForSeconds(5);
        startBtn.SetActive(true);       
    }
*/

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
