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

public class TutorialManager : MonoBehaviourPunCallbacks
{
    public static TutorialManager TM;

    [SerializeField] GameObject redTeam;                // 레드팀 프리팹
    [SerializeField] GameObject blueTeam;               // 블루팀 프리팹
    [SerializeField] GameObject defaultTeam;
    [SerializeField] GameObject admin;                  // 관리자 프리팹
    public GameObject eagleNPC;
    public Transform adminPoint;                        // 관리자 생성위치
    public Transform eaglePoint;
    //public GameObject arrowSkilled;                             // 폭탄 프리팹
    //public GameObject arrowBomb;                             // 폭탄 프리팹
   
    public Transform[] wayPos;
    //public Transform[] aSpawnPosition;                  // 폭탄 생성위치
    //public ParticleSystem[] arrowSpawnFX;
    //public AcheryEagle aEagle;
    // int wayNum = 0;

    public GameObject eagleBomb;
    public Transform spawnPoint;
    public GameObject myBomb = null;

    private PhotonView PV;   
    private GameObject spawnPlayer;                     // 생성되는 플레이어   

    private void Awake()
    {
        TM = this;
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        DataManager.DM.isReady = false;
        if (!PN.IsConnectedAndReady)
        {
            SceneManager.LoadScene(0);
        }
        if (PN.IsConnectedAndReady && PN.InRoom)
        {
            if (DataManager.DM.currentTeam != Team.ADMIN)  // 관리자 빌드시 필요한 코드
            {
                Destroy(admin);
            }
            /*if (PN.IsMasterClient)
            {
                //InvokeRepeating(nameof(SpawnBomb), 1, 4);
                InvokeRepeating(nameof(SpawnB), 1, 4);
                InvokeRepeating(nameof(SpawnBB), 1, 4);
                InvokeRepeating(nameof(SpawnR), 1, 4);
                InvokeRepeating(nameof(SpawnRR), 1, 4);

                //SpawnEagle();
                //InvokeRepeating(nameof(SpawnEB), 2, 5);
            }*/

            switch (PN.CurrentRoom.PlayerCount)
            {
                case 1:
                    PN.LocalPlayer.NickName = "마스터";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 2:
                    PN.LocalPlayer.NickName = "플레이어 1";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 3:
                    PN.LocalPlayer.NickName = "플레이어 2";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 4:
                    PN.LocalPlayer.NickName = "플레이어 3";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 5:
                    PN.LocalPlayer.NickName = "플레이어 4";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 6:
                    PN.LocalPlayer.NickName = "플레이어 5";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                default:
                    PN.LocalPlayer.NickName = "마스터 플레이어";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
            }
        }
    }

    private void Update()
    {       
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (!DataManager.DM.isReady)
            {
                DataManager.DM.isReady = true;
                photonView.RPC(nameof(Ready1), RpcTarget.AllViaServer);
            }
            else if (DataManager.DM.isReady)
            {
                DataManager.DM.isReady = false;
                PN.LoadLevel(2);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) { StartCoroutine(nameof(ExitGame)); }        
       
    }

    /* public void SpawnEagle()
     {
         if (curEagle == null)
         {
             if (curEagle != null) return;
             curEagle = PN.InstantiateRoomObject(eagleNPC.name, wayPos[0].position, wayPos[0].rotation, 0);
         }
     }*/
    /*public void SpawnEB()
    {
        if (myBomb != null) return;
        if (myBomb == null)
        {
            myBomb = PN.InstantiateRoomObject(eagleBomb.name, eagleNPC.transform.position + new Vector3(0, 0.9f, 0), eagleNPC.transform.rotation, 0);
           
        }
    }*/


    public void SpawnPlayer()
    {
        switch (DataManager.DM.currentTeam)
        {
            case Team.RED:
                PN.AutomaticallySyncScene = true;
                DataManager.DM.inGame = false;                
                DataManager.DM.gameOver = false;
                spawnPlayer = PN.Instantiate(redTeam.name, Vector3.zero, Quaternion.identity);
                Debug.Log($"{PN.CurrentRoom.Name} 방에 레드팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                //StartCoroutine(DeleteBullet());
                Info();
                break;

            case Team.BLUE:
                PN.AutomaticallySyncScene = true;
                DataManager.DM.inGame = false;                
                DataManager.DM.gameOver = false;
                spawnPlayer = PN.Instantiate(blueTeam.name, Vector3.zero, Quaternion.identity);
                Debug.Log($"{PN.CurrentRoom.Name} 방에 블루팀{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                //StartCoroutine(DeleteBullet());
                Info();
                break;

            // 윈도우 프로그램 빌드 시
            case Team.ADMIN:
                if (Application.platform == RuntimePlatform.WindowsPlayer)//|| Application.platform == RuntimePlatform.WindowsEditor)
                {
                    if(Application.platform != RuntimePlatform.WindowsPlayer) { return; }
                    PN.AutomaticallySyncScene = true;
                    DataManager.DM.inGame = false;                    
                    DataManager.DM.gameOver = false;
                    spawnPlayer = PN.Instantiate(admin.name, adminPoint.position, adminPoint.rotation);
                    spawnPlayer.transform.SetParent(adminPoint.transform, true);
                    Debug.Log($"{PN.CurrentRoom.Name} 방에 관리자{PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
                    Info();
                }
                break;


            default:
                return;
        }
    }


    /*public void SpawnBomb()
    {
        if (curArrow == null)
        {
            if (curArrow != null) return;
            timeBomb += Time.deltaTime;
            if (timeBomb >= 3)
            {
                bool skilled = RandomArrow.RandArrowPer(40);
                if (skilled)
                {
                    curArrow = PN.InstantiateRoomObject(arrowSkilled.name, aSpawnPosition[0].position, aSpawnPosition[0].rotation, 0);
                    Debug.Log("스킬 생성");
                    timeBomb = 0;
                }
                else
                {
                    bool bomb = RandomArrow.RandArrowPer(100);
                    if (bomb)
                    {
                        curArrow = PN.InstantiateRoomObject(arrowBomb.name, aSpawnPosition[0].position, aSpawnPosition[0].rotation, 0);
                        Debug.Log("폭탄 생성");
                        timeBomb = 0;
                    }
                    else
                    {
                        curArrow = PN.InstantiateRoomObject(arrowMulti.name, aSpawnPosition[0].position, aSpawnPosition[0].rotation, 0);
                        Debug.Log("멀티샷 생성");
                        timeBomb = 0;
                    }
                }
            }
        }
    }
    public void SpawnB()
    {        
        if (curArrowB == null)
        {
            if (curArrowB != null) return;
            timeB += Time.deltaTime;
            if (timeB >= 3)
            {
                bool skilled = RandomArrow.RandArrowPer(50);
                if (skilled)
                {
                    
                    curArrowB = PN.InstantiateRoomObject(arrowSkilled.name, aSpawnPosition[1].position, aSpawnPosition[1].rotation, 0);
                    Debug.Log("스킬 생성");
                    timeB = 0;
                }
                else
                {
                    bool bomb = RandomArrow.RandArrowPer(100);
                    if (bomb)
                    {
                        curArrowB = PN.InstantiateRoomObject(arrowBomb.name, aSpawnPosition[1].position, aSpawnPosition[1].rotation, 0);
                        Debug.Log("폭탄 생성");
                        timeB = 0;
                    }
                    else
                    {
                        curArrowB = PN.InstantiateRoomObject(arrowMulti.name, aSpawnPosition[1].position, aSpawnPosition[1].rotation, 0);
                        Debug.Log("멀티샷 생성");
                        timeB = 0;
                    }
                }
            }
        }
    }
    public void SpawnBB()
    {       
        if (curArrowBB == null)
        {
            if (curArrowBB != null) return;
            timeBB += Time.deltaTime;
            if (timeBB >= 3)
            {

                bool skilled = RandomArrow.RandArrowPer(50);
                if (skilled)
                {
                    curArrowBB = PN.InstantiateRoomObject(arrowSkilled.name, aSpawnPosition[2].position, aSpawnPosition[2].rotation, 0);
                    Debug.Log("스킬 생성");
                    timeBB = 0;
                }
                else
                {
                    bool bomb = RandomArrow.RandArrowPer(100);
                    if (bomb)
                    {
                        curArrowBB = PN.InstantiateRoomObject(arrowBomb.name, aSpawnPosition[2].position, aSpawnPosition[2].rotation, 0);
                        Debug.Log("폭탄 생성");
                        timeBB = 0;
                    }
                    else
                    {
                        curArrowBB = PN.InstantiateRoomObject(arrowMulti.name, aSpawnPosition[2].position, aSpawnPosition[2].rotation, 0);
                        Debug.Log("멀티샷 생성");
                        timeBB = 0;
                    }
                }
            }
        }
    }
    public void SpawnR()
    {
        if (curArrowR == null)
        {
            if (curArrowR != null) return;
            timeR += Time.deltaTime;
            if (timeR >= 3)
            {

                bool skilled = RandomArrow.RandArrowPer(50);
                if (skilled)
                {
                    curArrowR = PN.InstantiateRoomObject(arrowSkilled.name, aSpawnPosition[3].position, aSpawnPosition[3].rotation, 0);
                    Debug.Log("스킬 생성");
                    timeR = 0;
                }
                else
                {
                    bool bomb = RandomArrow.RandArrowPer(100);
                    if (bomb)
                    {
                        curArrowR = PN.InstantiateRoomObject(arrowBomb.name, aSpawnPosition[3].position, aSpawnPosition[3].rotation, 0);
                        Debug.Log("폭탄 생성");
                        timeR = 0;
                    }
                    else
                    {
                        curArrowR = PN.InstantiateRoomObject(arrowMulti.name, aSpawnPosition[3].position, aSpawnPosition[3].rotation, 0);
                        Debug.Log("멀티샷 생성");
                        timeR = 0;
                    }
                }
            }
        }
    }
    public void SpawnRR()
    {
        if (curArrowRR == null)
        {
            if (curArrowRR != null) return;
            timeRR += Time.deltaTime;
            if (timeRR >= 3)
            {

                bool skilled = RandomArrow.RandArrowPer(50);
                if (skilled)
                {
                    curArrowRR = PN.InstantiateRoomObject(arrowSkilled.name, aSpawnPosition[4].position, aSpawnPosition[4].rotation, 0);
                    Debug.Log("스킬 생성");
                    timeRR = 0;
                }
                else
                {
                    bool bomb = RandomArrow.RandArrowPer(100);
                    if (bomb)
                    {
                        curArrowRR = PN.InstantiateRoomObject(arrowBomb.name, aSpawnPosition[4].position, aSpawnPosition[4].rotation, 0);
                        Debug.Log("폭탄 생성");
                        timeRR = 0;
                    }
                    else
                    {
                        curArrowRR = PN.InstantiateRoomObject(arrowMulti.name, aSpawnPosition[4].position, aSpawnPosition[4].rotation, 0);
                        Debug.Log("멀티샷 생성");
                        timeRR = 0;
                    }
                }
            }
        }
    }*/

  

    [PunRPC]
    public void Ready1()
    {
        DataManager.DM.gameOver = true;
    }

    public IEnumerator ExitGame()
    {
        yield return new WaitForSeconds(1);
        photonView.RPC(nameof(ForceOff), RpcTarget.AllViaServer);
    }
    /* IEnumerator DeleteBullet()
     {
         yield return new WaitForSeconds(0.3f);
         foreach (GameObject bull in GameObject.FindGameObjectsWithTag("Arrow")) bull.GetComponent<PhotonView>().RPC("DestroyArrow", RpcTarget.AllBuffered);
     }*/

    [ContextMenu("포톤 서버 정보")]
    void Info()
    {
        if (PN.InRoom)
        {
            print("현재 방 이름 : " + PN.CurrentRoom.Name);
            print("현재 방 인원 수 : " + PN.CurrentRoom.PlayerCount + "명");
            print("현재 방 MAX인원 : " + PN.CurrentRoom.MaxPlayers + "명");

            string playerStr = "방에 있는 플레이어 이름 \n";
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
            PN.RemoveBufferedRPCs();
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


    [PunRPC]
    public void ForceOff()
    {
        Application.Quit();
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Application.Quit();
            if (PN.IsMasterClient)
            {
                PN.DestroyAll();
            }
            PN.Destroy(spawnPlayer);
        }
    }
}
