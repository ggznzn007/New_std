using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using PN = Photon.Pun.PN;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialManager : MonoBehaviourPunCallbacks
{
    public static TutorialManager TM;

    [SerializeField] GameObject redTeam;                // ������ ������
    [SerializeField] GameObject blueTeam;               // ����� ������
    [SerializeField] GameObject defaultTeam;
    [SerializeField] GameObject admin;                  // ������ ������
    public Transform adminPoint;                        // ������ ������ġ
    private GameObject spawnPlayer;                     // �����Ǵ� �÷��̾�
    public GameObject bomB;                             // ��ź ������
    public GameObject shield;                             // ���� ������
    public Transform[] bSpawnPosition;                  // ��ź ������ġ
    private GameObject bomBs;                           // �����Ǵ� ��ź
    private GameObject shieldCap;

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
            switch (PN.CurrentRoom.PlayerCount)
            {
                case 1:
                    PN.LocalPlayer.NickName = "������";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 2:
                    PN.LocalPlayer.NickName = "�÷��̾� 1";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 3:
                    PN.LocalPlayer.NickName = "�÷��̾� 2";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 4:
                    PN.LocalPlayer.NickName = "�÷��̾� 3";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 5:
                    PN.LocalPlayer.NickName = "�÷��̾� 4";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                case 6:
                    PN.LocalPlayer.NickName = "�÷��̾� 5";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;
                default:
                    PN.LocalPlayer.NickName = "������ �÷��̾�";
                    DataManager.DM.nickName = PN.LocalPlayer.NickName;
                    SpawnPlayer();
                    break;

            }
            //SpawnPlayer();
            if (DataManager.DM.currentTeam != Team.ADMIN)  // ������ ����� �ʿ��� �ڵ� 
            {
                Destroy(admin);
            }

            if (PN.IsMasterClient)
            {
                InvokeRepeating(nameof(SpawnBomb), 10, 35);
                SpawnShield();
            }

        }
    }



    public void SpawnPlayer()
    {
        switch (DataManager.DM.currentTeam)
        {
            case Team.RED:
                PN.AutomaticallySyncScene = true;
                DataManager.DM.inGame = false;
                DataManager.DM.gameOver = false;
                spawnPlayer = PN.Instantiate(redTeam.name, Vector3.zero, Quaternion.identity);
                Debug.Log($"{PN.CurrentRoom.Name} �濡 ������{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
                //StartCoroutine(DeleteBullet());
                Info();
                break;

            case Team.BLUE:
                PN.AutomaticallySyncScene = true;
                DataManager.DM.inGame = false;
                DataManager.DM.gameOver = false;
                spawnPlayer = PN.Instantiate(blueTeam.name, Vector3.zero, Quaternion.identity);
                Debug.Log($"{PN.CurrentRoom.Name} �濡 �����{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
                //StartCoroutine(DeleteBullet());
                Info();
                break;

            // ������ ���α׷� ���� ��            
            case Team.ADMIN:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    PN.AutomaticallySyncScene = true;
                    DataManager.DM.inGame = false;
                    DataManager.DM.gameOver = false;
                    spawnPlayer = PN.Instantiate(admin.name, adminPoint.position, adminPoint.rotation);
                    Debug.Log($"{PN.CurrentRoom.Name} �濡 ������{PN.LocalPlayer.NickName} ���� �����ϼ̽��ϴ�.");
                    Info();
                }
                break;


            default:
                return;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { DataManager.DM.gameOver = true; PN.LoadLevel(2); }
        if (Input.GetKey(KeyCode.Escape)) { StartCoroutine(nameof(ExitGame)); }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBomb();
        }
        /*if (PN.InRoom && PN.IsMasterClient)
        {

            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {

                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { PN.LoadLevel(2); }
                else if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    SpawnBomb();
                }
            }

        }*/
    }
    /*IEnumerator DeleteBullet()
    {
        yield return new WaitForSeconds(0.3f);
        foreach (GameObject bull in GameObject.FindGameObjectsWithTag("Bullet")) bull.GetComponent<PhotonView>().RPC("DestroyBullet", RpcTarget.AllBuffered);
    }*/

    public IEnumerator ExitGame()
    {
        yield return new WaitForSeconds(1);
        photonView.RPC(nameof(ForceOff), RpcTarget.AllViaServer);
    }


    public void SpawnBomb()
    {
        for (int i = 0; i < bSpawnPosition.Length; i++)
        {
            bomBs = PN.InstantiateRoomObject(bomB.name, bSpawnPosition[i].position, bSpawnPosition[i].rotation, 0);
        }
    }

    public void SpawnShield()
    {
        for (int i = 0; i < bSpawnPosition.Length; i++)
        {
            shieldCap = PN.InstantiateRoomObject(shield.name, bSpawnPosition[i].position, bSpawnPosition[i].rotation, 0);
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

            string playerStr = "�濡 �ִ� �÷��̾� �̸� \n";
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
            PN.RemoveBufferedRPCs();
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
