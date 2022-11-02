using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using Antilatency;
using Antilatency.TrackingAlignment;
using Antilatency.DeviceNetwork;
using Antilatency.Alt;
using Antilatency.SDK;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;
using UnityEngine.SocialPlatforms.Impl;

public class AvartarController : MonoBehaviourPunCallbacks, IPunObservable
{
    public static AvartarController ATC;                                     // �̱��� 
    Player player;
    //PlayerStats stats;

    /* public static Action headShot;
     public static Action bodyShot;
     public static Action respawnP;*/
    [Header("�÷��̾� ����")]
    [SerializeField] TextMeshPro Nickname;                                                    // �÷��̾� �г���
    [SerializeField] PhotonView PV;                                                   // �����
                                                                                      // [SerializeField] Image HP;                                                         // �÷��̾� HP
    public Slider HP;                                                         // �÷��̾� HP
    [SerializeField] int curHP = 100;
    [SerializeField] int inItHP = 100;
    //[SerializeField] TextMeshPro hpText;                                                    // �÷��̾� �г���
    [SerializeField] int actNumber = 0;
    [SerializeField] int attackPower = 10;
    [SerializeField] int attackPowerH = 20;
    [SerializeField] int grenadePower = 40;
    [SerializeField] GameObject myGun;
    [SerializeField] GameObject hand_Left;
    [SerializeField] GameObject hand_Right;
    [SerializeField] GameObject at_hand_Left;
    [SerializeField] GameObject at_hand_Right;
    [SerializeField] GameObject FPS;
    [SerializeField] Camera myCam;

    [Header("�÷��̾� ������ ����")]
    [SerializeField] MeshRenderer head_Rend;                                           // �ƹ�Ÿ �Ӹ�     ������
    [SerializeField] MeshRenderer body_Rend;                                           // �ƹ�Ÿ ��         
    [SerializeField] SkinnedMeshRenderer glove_L_Rend;                                 // �ƹ�Ÿ �޼�   �尩     
    [SerializeField] SkinnedMeshRenderer glove_R_Rend;                                 // �ƹ�Ÿ ������ �尩     
    [SerializeField] SkinnedMeshRenderer hand_L_Rend;                                  // �ƹ�Ÿ �޼�   
    [SerializeField] SkinnedMeshRenderer hand_R_Rend;                                  // �ƹ�Ÿ ������   

    [Header("�÷��̾� ��Ƽ���� ����")]
    [SerializeField] Material[] head_Mats;                                             // �ƹ�Ÿ �Ӹ�     ��Ƽ����    
    [SerializeField] Material[] body_Mats;                                             // �ƹ�Ÿ ��      
    [SerializeField] Material[] body_colors;                                             // �ƹ�Ÿ ����    
    [SerializeField] Material glove_R;                                                 // �ƹ�Ÿ ������ �尩 
    [SerializeField] Material hand_R;                                                  // �ƹ�Ÿ ������    
    [SerializeField] Material[] DeadMat_Heads;                                         // �Ӹ� ����   ��Ƽ����
    [SerializeField] Material[] DeadMat_Bodys;                                         // ��   ����   ��Ƽ����
    [SerializeField] Material DeadMat_Hand;                                            // ��   ����   ��Ƽ����

    [Header("�÷��̾� �ݶ��̴�")]
    public List<Collider> playerColls;                                       // �÷��̾� �ݶ��̴�


    [Header("�÷��̾� ��������")]
    public bool isAlive;                                                      // �÷��̾� ���� �Ǵܿ���

    [Header("�÷��̾� �ǰ�ȿ�� �̹���")]
    [SerializeField] Image damageScreen;
    [SerializeField] Image deadScreen;


    [Header("�÷��̾� ��ƼŬ ȿ�� ����")]
    [SerializeField] GameObject[] effects;
    [SerializeField] bool isDeadLock;

    private float delayTime = 1f;
    public bool isDamaged = false;
    public int team;
    public GameObject quitBtn;

    private void Awake()
    {
        ATC = this;
        PV = GetComponent<PhotonView>();
        Initialize();
    }

    void Start()
    {
        isDeadLock = true;
        team = DataManager.DM.teamInt;

    }

    void Update()
    {
        if (!PV.IsMine) return;
        Nick_HP_Pos();
    }

    public void Nick_HP_Pos()
    {
        // HP.transform.SetPositionAndRotation(myCam.transform.position + new Vector3(0, 0.5f, 0), myCam.transform.rotation);
        // HP.transform.position = myCam.transform.position + new Vector3(0, 0.42f, 0);
        //Nickname.transform.SetPositionAndRotation(myCam.transform.position + new Vector3(0, 0.6f, 0), myCam.transform.rotation);
        Nickname.transform.position = myCam.transform.position + new Vector3(0, 0.54f, 0);

        // HP.transform.forward = -myCam.transform.forward;
        Nickname.transform.forward = -myCam.transform.forward;
    }
    public void Initialize()                                                  // �÷��̾� �ʱ�ȭ �޼���
    {
        // �÷��̾� HP & �г��� �ʱ�ȭ
        Nickname.text = PV.IsMine ? PN.NickName : PV.Owner.NickName;         // �÷��̾� ����䰡 �ڽ��̸� �г����� �ƴϸ� ���� �г���
        Nickname.color = PV.IsMine ? Color.white : Color.red;                // �÷��̾� ����䰡 �ڽ��̸� ��� �ƴϸ� ������
        actNumber = PV.Owner.ActorNumber;

        isAlive = true;                                                      // �÷��̾� ���� �ʱ�ȭ
        curHP = inItHP;                                                      // �÷��̾� HP �ʱ�ȭ
        HP.value = inItHP;
        HP.maxValue = inItHP;                                                  // ������ �������� HP�� �ʱ�ȭ
        //HP.fillAmount = inItHP;                                              // ������ �������� HP�� �ʱ�ȭ
        GetNickNameByActorNumber(actNumber);
    }

    public string GetNickNameByActorNumber(int actorNumber)   //�г��� ��������
    {
        //���� ���� �濡 ������ ����� �г����� �����´�   -- PlayerListOthers �ڱ� �ڽ��� �� ������ �� ������
        foreach (Player player in PN.PlayerListOthers)
        {
            if (player.ActorNumber == actorNumber)
            {
                return player.NickName;
            }
        }
        return "Ghost";
    }

    public IEnumerator PlayerDead()  ///////////////////////////////////////////���� �޼���//////////////////////////////////////////////////////////////////
    {
        StartCoroutine(ShowDeadEffect());
        yield return new WaitForSeconds(0.001f);
        isDeadLock = false;                                                  // �ߺ���������
        Nickname.gameObject.SetActive(false);                                // �÷��̾� �г���
        HP.gameObject.SetActive(false);                                      // �÷��̾� HP
        at_hand_Left.SetActive(false);                                       // �ƹ�Ÿ �޼�
        at_hand_Right.SetActive(false);                                      // �ƹ�Ÿ ������
        hand_Left.SetActive(false);                                          // �޼� ��Ʈ�ѷ�
        hand_Right.SetActive(false);                                         // ������ ��Ʈ�ѷ�
        FPS.SetActive(false);                                                // ������UI
                                                                             // �����۽����ڽ� ����
                                                                             // �����۽����ڽ� ����
        playerColls[0].enabled = true;                                       // ������ ���� �ݶ��̴�
        playerColls[1].enabled = false;                                      // �Ӹ� �ݶ��̴�
        playerColls[2].enabled = false;                                      // ���� �ݶ��̴�
        playerColls[3].enabled = false;                                      // �����۽����ڽ� ������
        playerColls[4].enabled = false;                                      // �����۽����ڽ� ������

        //HP.fillAmount = 0f;
        curHP = 0;
        HP.value = 0;
        HP.maxValue = inItHP;

        head_Rend.materials = DeadMat_Heads;                                 // �ƹ�Ÿ �Ӹ� ��Ƽ����
        body_Rend.materials = DeadMat_Bodys;                                 // �ƹ�Ÿ ���� ��Ƽ����
        glove_L_Rend.material = DeadMat_Hand;                                // �ƹ�Ÿ �޼� �尩 ��Ƽ����
        glove_R_Rend.material = DeadMat_Hand;                                // �ƹ�Ÿ ������ �尩 ��Ƽ����
        hand_L_Rend.material = DeadMat_Hand;                                 // �ƹ�Ÿ �޼� ��Ƽ���� 
        hand_R_Rend.material = DeadMat_Hand;                                 // �ƹ�Ÿ ������ ��Ƽ���� 
    }

    public IEnumerator PlayerRespawn() /////////////////////////////////////////������ �޼���/////////////////////////////////////////////////////////////////////
    {
        Debug.Log("������");
        deadScreen.gameObject.SetActive(false);
        Nickname.gameObject.SetActive(true);
        at_hand_Left.SetActive(true);
        at_hand_Right.SetActive(true);
        hand_Left.SetActive(true);
        hand_Right.SetActive(true);
        FPS.SetActive(true);


        playerColls[0].enabled = false;
        playerColls[1].enabled = true;
        playerColls[2].enabled = true;
        playerColls[3].enabled = true;
        playerColls[4].enabled = true;

        head_Rend.materials = head_Mats;
        body_Rend.materials = body_Mats;
        glove_L_Rend.material = glove_R;
        glove_R_Rend.material = glove_R;
        hand_L_Rend.material = hand_R;
        hand_R_Rend.material = hand_R;

        effects[2].SetActive(true);                   // �ǵ� ȿ�� On
        yield return new WaitForSeconds(4.5f);           // 4�ʰ���
        effects[2].SetActive(false);                  // �ǵ� ȿ�� Off
        isAlive = true;
        isDeadLock = true;
        curHP = inItHP;
        HP.value = inItHP;
        HP.maxValue = inItHP;
        HP.gameObject.SetActive(true);
    }

    /*  public void DamagedPlayer(float pow)
      {
          string hitter = GetNickNameByActorNumber(actNumber);
          curHP = Mathf.Max(0, curHP - pow);
          HP.fillAmount = inItHP;
          if (PV.IsMine && curHP <= 0.0f)
          {
              deadScreen.gameObject.SetActive(true);
              isAlive = false;
              PV.RPC("DeadPlayer", RpcTarget.AllBuffered);
              Debug.Log("ų ����" + hitter);
          }
      }*/

    private void OnTriggerEnter(Collider col)                                 // ������ �±� �� �޼���
    {
        if (col.CompareTag("RespawnBlue") && !isAlive && DataManager.DM.currentTeam == Team.BLUE)
        {
            PV.RPC("RespawnPlayer", RpcTarget.All);
            Debug.Log("����� ������");
        }

        else if (col.CompareTag("RespawnRed") && !isAlive && DataManager.DM.currentTeam == Team.RED)
        {
            PV.RPC("RespawnPlayer", RpcTarget.All);
            Debug.Log("������ ������");
        }

       /* else if(col.CompareTag("BlueTeam"))
        {
            DataManager.DM.currentTeam = Team.BLUE;
            body_Rend.materials[0] = body_colors[0];          
        }
        else if(col.CompareTag("RedTeam"))
        {
            DataManager.DM.currentTeam = Team.RED;
            body_Rend.materials[0] = body_colors[1];           
        }*/
    }

    public void GrenadeDamage()                                              // ũ��Ƽ��(��弦) ������
    {
        StartCoroutine(ShowDamageScreen());
        if (isDeadLock)
        {
            PV.RPC("Damaged", RpcTarget.All, grenadePower);
            //PV.RPC("Damaged", PV.Owner,grenadePower);
        }
    }
    public void CriticalDamage()                                              // ũ��Ƽ��(��弦) ������
    {
        StartCoroutine(ShowDamageScreen());
        if (isDeadLock)
        {
            //PV.RPC("Damaged", PV.Owner, attackPowerH);   
            PV.RPC("Damaged", RpcTarget.All, attackPowerH);
            // PV.RPC("HeadShot", PV.Owner);
            PV.RPC("HeadShot", RpcTarget.All);
        }
    }
    public void NormalDamage()                                                // �Ϲ� ������
    {
        StartCoroutine(ShowDamageScreen());
        if (isDeadLock)
        {
            //PV.RPC("Damaged", PV.Owner, attackPower);
            PV.RPC("Damaged", RpcTarget.All, attackPower);
            //PV.RPC("BodyShot", PV.Owner);
            PV.RPC("BodyShot", RpcTarget.All);
        }
    }
    public void Respawn()                                                     // ������ �޼���
    {
        PV.RPC("RespawnPlayer", RpcTarget.All);
    }

    public IEnumerator ShowDamageScreen()                                      // �ǰ� ��ũ�� �����ֱ�
    {
        //damageScreen.gameObject.SetActive(true);
        damageScreen.color = new Color(1, 0, 0, Random.Range(0.65f, 0.75f));
        yield return new WaitForSeconds(0.3f);
        damageScreen.color = Color.clear;
        //yield return new WaitForSeconds(0.1f);
        // damageScreen.gameObject.SetActive(false);
    }

    public IEnumerator ShowDeadEffect()                                       // ���� ȿ�� �����ֱ�
    {
        effects[0].SetActive(true);
        yield return new WaitForSeconds(3f);
        effects[0].SetActive(false);
    }

    public IEnumerator ShowRespawnEffect()                                     // ��Ȱ ȿ�� �����ֱ�
    {
        effects[1].SetActive(true);
        yield return new WaitForSeconds(3f);
        effects[1].SetActive(false);
    }

    public IEnumerator DamagedDelay()                                          // �������� �����ǰ� ����
    {
        while (delayTime >= 0)
        {
            delayTime -= Time.deltaTime;
            Debug.Log("�ݶ��̴� OFF");
            playerColls[1].enabled = false;                                      // �Ӹ� �ݶ��̴�
            playerColls[2].enabled = false;                                      // ���� �ݶ��̴�           

            isDamaged = true;
        }
        yield return new WaitForSeconds(0.17f);
        playerColls[1].enabled = true;                                      // �Ӹ� �ݶ��̴�
        playerColls[2].enabled = true;                                      // ���� �ݶ��̴�

        isDamaged = false;
        Debug.Log("�ݶ��̴� ON");
        if (HP.value <= 0)
        {
            playerColls[1].enabled = false;                                      // �Ӹ� �ݶ��̴�
            playerColls[2].enabled = false;                                      // ���� �ݶ��̴�
        }
    }

    [PunRPC]
    public void Damaged(int pow)                                                  // �÷��̾� ������ �޾��� ��
    {
        if (isAlive && !isDamaged)
        {
            if (PV.IsMine)
            {
                if (isDamaged) { return; }
                AudioManager.AM.PlaySE("Damage");
                curHP -= pow;
                HP.value = Mathf.Round(curHP * 10) * 0.1f;
                HP.maxValue = inItHP;
                StartCoroutine(DamagedDelay());
                delayTime = 1f;
                Debug.Log("���� HP : " + HP.value.ToString() + "%");

                if (HP.value <= 0)
                {
                    isAlive = false;
                    deadScreen.gameObject.SetActive(true);
                    if(DataManager.DM.currentMap==Map.TOY)               // ����
                    {
                        PV.RPC("PlayerKilledT", RpcTarget.All, team);
                    }
                    else  if(DataManager.DM.currentMap==Map.WESTERN)                                               // ������
                    {
                        PV.RPC("PlayerKilledW", RpcTarget.All, team);
                    }
                    
                    PV.RPC("DeadPlayer", RpcTarget.All);
                    // PV.RPC("DeadPlayer", PV.Owner);                    
                    Debug.Log("ų ����");
                }

            }


        }
    }

    [PunRPC]
    public void PlayerKilledT(int team)
    {
        if (team == 0)                   // ������� �������� �׿��� ��
        {
            GunShootManager.GSM.score_Red++;
            Debug.Log("���-->���� ų");
        }
        else                             // �������� ������� �׿��� ��
        {
            GunShootManager.GSM.score_Blue++;
            Debug.Log("����-->��� ų");
        }
    }

    [PunRPC]
    public void PlayerKilledW(int team)
    {
        if (team == 0)                   // ������� �������� �׿��� ��
        {
            WesternManager.WM.score_Red++;
            Debug.Log("���-->���� ų");
        }
        else                             // �������� ������� �׿��� ��
        {
            WesternManager.WM.score_Blue++;
            Debug.Log("����-->��� ų");
        }
    }

    [PunRPC]
    public void HeadShot()
    {
        if (!PV.IsMine)
        {
            AudioManager.AM.PlaySE("HeadShot");
        }
    }
    [PunRPC]
    public void BodyShot()
    {
        if (!PV.IsMine)
        {
            AudioManager.AM.PlaySE("Hit");
        }
    }

    [PunRPC]
    public void DeadPlayer()                                                     // �÷��̾� �׾�����
    {        
       StartCoroutine(PlayerDead());
        if (!PV.IsMine)
        {
            AudioManager.AM.PlaySE("Kill");
           
        }
        else
        {                 
            AudioManager.AM.PlaySE("Dead");            
        }
    }

    [PunRPC]
    public void RespawnPlayer()                                                 //  �÷��̾� �������� ��
    {
        if (PV.IsMine)
        {
            AudioManager.AM.PlaySE("Respawn");
        }
        StartCoroutine(ShowRespawnEffect());

        StartCoroutine(PlayerRespawn());
    }


    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Nickname.transform.position);
            stream.SendNext(Nickname.transform.forward);
            stream.SendNext(Nickname.text);
            // stream.SendNext(HP.transform.rotation);
            //stream.SendNext(HP.transform.position);
            //stream.SendNext(HP.transform.forward);
            stream.SendNext(HP.value);
            //stream.SendNext(HP.maxValue);
            // stream.SendNext(Nickname.gameObject.transform.rotation);

            // stream.SendNext(isAlive);
        }
        else
        {
            Nickname.transform.position = (Vector3)stream.ReceiveNext();
            Nickname.transform.forward = (Vector3)stream.ReceiveNext();
            Nickname.text = (string)stream.ReceiveNext();
            //HP.transform.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            //HP.transform.position = (Vector3)stream.ReceiveNext();
            // HP.transform.forward = (Vector3)stream.ReceiveNext();
            HP.value = (float)stream.ReceiveNext();
            // HP.maxValue = (int)stream.ReceiveNext();

            // isAlive = (bool)stream.ReceiveNext();
        }
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    /*IEnumerator Damaged()
    {
        // SetPlayerInVisible(false);

        Controll_R.modelPrefab = deathHand_R.transform;
        Controll_L.modelPrefab = deathHand_L.transform;
        gunPrefab.SetActive(false);
        deathHead.SetActive(true);
        deathBody.SetActive(true);
        deathHand_L.SetActive(true);
        deathHand_R.SetActive(true);

        yield return new WaitForSeconds(5f);
        Controll_R.gameObject.SetActive(true);
        Controll_L.modelPrefab = Hand_L.transform;
        deathHead.SetActive(false);
        deathBody.SetActive(false);
        deathHand_L.SetActive(false);
        deathHand_R.SetActive(false);
        Hand_R.SetActive(true);
        Hand_L.SetActive(true);
        //  SetPlayerVisible(true);
        // SetPlayerInVisible(true);
        HP.gameObject.SetActive(true);
        Nickname.gameObject.SetActive(true);
        HP.fillAmount = 1f;
        gunPrefab.SetActive(true);

    }*/
}
