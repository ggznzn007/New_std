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


public class GunShootManager : MonoBehaviourPunCallbacks                      // ����
{
    public static GunShootManager GSM;

    [Header("ī��Ʈ�ٿ� �ؽ�Ʈ")]
    [SerializeField] TextMeshPro countText;
    [Header("���� ���ѽð�")]
    [SerializeField] TextMeshPro timerText;
    [Header("���� ���� �ؽ�Ʈ(������)")]
    public TextMeshPro startText;
    [Header("��ŸƮ ��ư(������)")]
    public GameObject startBtn;
    [Header("������ ������")]
    [SerializeField] GameObject redTeam;
    [Header("����� ������")]
    [SerializeField] GameObject blueTeam;
    
    private GameObject spawnPlayer;
    [SerializeField] bool count = false;
    [SerializeField] int limitedTime;
    Hashtable setTime = new Hashtable();
    PhotonView PV;
    
    private void Awake()
    {
        GSM = this;
        DataManager.DM.currentMap = Map.TOY;        
    }
        
    private void Start()
    {
       PV = GetComponent<PhotonView>();
        if (PN.IsConnectedAndReady&&PN.InRoom)
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
                NetworkManager.NM.inGame = false;
                spawnPlayer = PN.Instantiate(redTeam.name, Vector3.zero, Quaternion.identity);                
                Debug.Log($"{PN.CurrentRoom.Name} �濡 ������{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
                Info();
                if (PN.IsMasterClient)
                {
                    startText.gameObject.SetActive(true);
                    startBtn.SetActive(true);
                }
                break;

            case Team.BLUE:
                PN.AutomaticallySyncScene = true;
                NetworkManager.NM.inGame = false;
                spawnPlayer = PN.Instantiate(blueTeam.name, Vector3.zero, Quaternion.identity);                
                Debug.Log($"{PN.CurrentRoom.Name} �濡 �����{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
                Info();
                if (PN.IsMasterClient)
                {
                    startText.gameObject.SetActive(true);
                    startBtn.SetActive(true);
                }
                
                break;

            default:
                return;
        }
    }
     void FixedUpdate()
    {
        if (PN.InRoom && PN.IsConnectedAndReady)
        {            
            limitedTime = (int)PN.CurrentRoom.CustomProperties["Time"];
            limitedTime = limitedTime < 0 ? 0 : limitedTime;
            float min = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] / 60);
            float sec = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] % 60);
            timerText.text = string.Format("�����ð� {0:00}�� {1:00}��", min, sec);
            if (limitedTime < 60)
            {
                timerText.text = string.Format("�����ð� {0:0}��", sec);
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

    
    public void StartBtnT()
    {
        StartCoroutine(StartTimer());
        startBtn.SetActive(false);
        startText.gameObject.SetActive(false);
    }

    [PunRPC]
    public void EndGameT()
    {
        StartCoroutine(LeaveGame());
    }

     IEnumerator PlayTimer()
    {
        yield return new WaitForSeconds(1);
        int nextTime = limitedTime -= 1;
        setTime["Time"] = nextTime;
        PN.CurrentRoom.SetCustomProperties(setTime);
        count = true;

        if (limitedTime <= 0)
        {
            count = false;
            limitedTime = 0;            
            PV.RPC("EndGameT", RpcTarget.All);
            Debug.Log("Ÿ�ӿ���");
        }
    }
      
    

   public  IEnumerator StartTimer()
    {        
        AudioManager.AM.EffectPlay(AudioManager.Effect.GAMESTART);
        countText.text = string.Format("������ 3�� �ڿ� ���۵˴ϴ�.");
        yield return new WaitForSeconds(3);
        AudioManager.AM.EffectPlay(AudioManager.Effect.Three);
        countText.text = string.Format("3");
        yield return new WaitForSeconds(1);
        AudioManager.AM.EffectPlay(AudioManager.Effect.Two);
        countText.text = string.Format("2");
        yield return new WaitForSeconds(1);
        AudioManager.AM.EffectPlay(AudioManager.Effect.One);
        countText.text = string.Format("1");
        yield return new WaitForSeconds(1);
        AudioManager.AM.EffectPlay(AudioManager.Effect.START);
        countText.text = string.Format("���� ��ŸƮ!!!");
        yield return new WaitForSeconds(1);
        count = true;
        NetworkManager.NM.inGame = true;
        timerText.gameObject.SetActive(true);
        countText.gameObject.SetActive(false);
    }

   
    public  IEnumerator LeaveGame()
    {
        timerText.gameObject.SetActive(false);
        countText.gameObject.SetActive(true);
        NetworkManager.NM.inGame = false;
        AudioManager.AM.EffectPlay(AudioManager.Effect.GAMEOVER);
        countText.text = string.Format("GAME OVER");
        yield return new WaitForSeconds(1);
        AudioManager.AM.EffectPlay(AudioManager.Effect.END);
        countText.text = string.Format("3�� �ڿ� �κ�� �̵��մϴ�");
        yield return new WaitForSeconds(5);            
        PN.LeaveRoom();
        StopCoroutine(LeaveGame());
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
        PN.LeaveRoom();
    }

    // ���� ĳ���ͻ��� if��
    /*if (PN.IsConnectedAndReady && DataManager.DM.currentTeam == Team.RED)
       {
           PN.AutomaticallySyncScene = true;
           NetworkManager.NM.inGame = true;
           spawnPlayer = PN.Instantiate(redTeam.name, Vector3.zero, Quaternion.identity);
           count = true;
           Debug.Log($"{PN.CurrentRoom.Name} �濡 ������{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
           Info();
       }
       else if (PN.IsConnectedAndReady && DataManager.DM.currentTeam == Team.BLUE)
       {
           PN.AutomaticallySyncScene = true;
           NetworkManager.NM.inGame = true;
           spawnPlayer = PN.Instantiate(blueTeam.name, Vector3.zero, Quaternion.identity);
           count = true;
           Debug.Log($"{PN.CurrentRoom.Name} �濡 �����{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
           Info();
       }*/
}
