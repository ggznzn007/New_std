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
using static Pico.Platform.Message;

public class AvartarController : MonoBehaviourPunCallbacks, IPunObservable
{
    public static AvartarController ATC;                                     // �̱���

    [Header("�÷��̾� ����")]
    [SerializeField] TextMeshPro Nickname;                            // �÷��̾� �г���
    [SerializeField] PhotonView PV;                                   // �����
    public Slider HP;                                                 // �÷��̾� HP
    [SerializeField] int curHP = 100;
    [SerializeField] int inItHP = 100;
    [SerializeField] int actNumber = 0;
    [SerializeField] int attackPower = 15;
    [SerializeField] int attackPowerH = 30;
    [SerializeField] int grenadePower = 50;
    [SerializeField] XRDirectInteractor hand_Left;
    [SerializeField] XRDirectInteractor hand_Right;
    [SerializeField] GameObject at_hand_Left;
    [SerializeField] GameObject at_hand_Right;
    [SerializeField] GameObject FPS;
    [SerializeField] Camera myCam;

    [Header("�÷��̾� ������ ����")]
    [SerializeField] MeshRenderer head_Rend;                                           // �ƹ�Ÿ �Ӹ�     ������
    [SerializeField] MeshRenderer body_Rend;                                           // �ƹ�Ÿ ��          
    [SerializeField] MeshRenderer body_Rend_belt;                                           // �ƹ�Ÿ ��
    [SerializeField] MeshRenderer at_L_Rend;                                  // �ƹ�Ÿ �޼�   
    [SerializeField] MeshRenderer at_R_Rend;                                  // �ƹ�Ÿ �޼�   

    [Header("�÷��̾� ��Ƽ���� ����")]
    [SerializeField] Material head_Mat;                                             // �ƹ�Ÿ �Ӹ�     ��Ƽ����    
    [SerializeField] Material body_Mat;                                             // �ƹ�Ÿ ��          
    [SerializeField] Material body_Mat_B;                                             // ������ �ƹ�Ÿ ��        
                                                                                      // [SerializeField] Material glove_R;                                                 // �ƹ�Ÿ ������ �尩 
    [SerializeField] Material hand_R;                                                  // �ƹ�Ÿ ������    
    [SerializeField] Material DeadMat_Head;                                         // �Ӹ� ����   ��Ƽ����
    [SerializeField] Material DeadMat_Body;                                         // ��   ����   ��Ƽ����
    [SerializeField] Material DeadMat_Hand;                                            // ��   ����   ��Ƽ����

    [Header("�÷��̾� �ݶ��̴�")]
    public List<Collider> playerColls;                                       // �÷��̾� �ݶ��̴�

    [Header("�÷��̾� ��������")]
    public bool isAlive;                                                      // �÷��̾� ���� �Ǵܿ���

    [Header("�÷��̾� �ǰ�ȿ�� �̹���")]
    [SerializeField] Image damageScreen;
    [SerializeField] Image deadScreen;
    [SerializeField] Image threeScreen;

    [Header("�÷��̾� ��ƼŬ ȿ�� ����")]
    [SerializeField] GameObject[] effects;
    [SerializeField] bool isDeadLock;

    private float delayTime = 0.7f;
    public bool isDamaged = false;
    public int team;
    //private int showCount = 0;
    /* [SerializeField] private GameObject arrowPrefab;
     [SerializeField] private Transform[] spawnArea;
     [SerializeField] private int arrowCount = 2;*/
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
        PN.UseRpcMonoBehaviourCache = true;
        //InvokeRepeating(nameof(SpawnArrow), 1, 10);
    }

    void Update()
    {
        if (!PV.IsMine) return;
        //PV.RefreshRpcMonoBehaviourCache();
        Nick_HP_Pos();
        Show_Frame();
        UnShow_Frame();
        GameOverInteract();
    }
    public void GameOverInteract()
    {
        if (DataManager.DM.gameOver)
        {
            hand_Left.interactionLayers = 0;
            hand_Right.interactionLayers = 0;
            for (int i = 3; i < playerColls.Count; i++)
            {
                playerColls[i].enabled = false;
            }
        }
    }
    /* public void SpawnArrow()
     {
         for (int i = 0; i < arrowCount; i++)
         {
             for (int j = 0; j < spawnArea.Length; j++)
             {
                 PN.Instantiate(arrowPrefab.name, spawnArea[j].position, spawnArea[j].rotation);
             }         

         }
     }
 */
    public void Show_Frame()
    {
        if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out bool pressed))
        {
            if (pressed) { FPS.SetActive(true); }
        }
    }

    public void UnShow_Frame()
    {
        if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.secondaryButton, out bool pressed))
        {
            if (pressed) { FPS.SetActive(false); }
        }
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
        threeScreen.gameObject.SetActive(false);

        //hand_Left.enabled = false;
       // hand_Right.enabled = false;
       /* hand_Left.interactionLayers = 0;                                     // ���ͷ��� ���̾ �ٲٴ� ������� ������ ����
        hand_Right.interactionLayers = 0;                                    // ���̾� �ѹ� 0 = ����Ʈ ,6 = ���ͷ��ͺ�, 12 = ����*/

        playerColls[0].enabled = true;
        for (int i = 1; i < playerColls.Count; i++)
        {
            playerColls[i].enabled = false;
        }
        // ������ ���� �ݶ��̴�       

        curHP = 0;
        HP.gameObject.SetActive(false);                                      // �÷��̾� HP
        HP.value = 0;
        HP.maxValue = inItHP;

        at_L_Rend.material = DeadMat_Hand;
        at_R_Rend.material = DeadMat_Hand;

        if (DataManager.DM.currentMap == Map.TOY)
        {
            head_Rend.material = DeadMat_Head;                                 // �ƹ�Ÿ �Ӹ� ��Ƽ����
            body_Rend.material = DeadMat_Body;                                 // �ƹ�Ÿ ���� ��Ƽ����
            body_Rend_belt.material = DeadMat_Body;
        }

        if (DataManager.DM.currentMap == Map.WESTERN)
        {
            head_Rend.material = DeadMat_Head;                                 // �ƹ�Ÿ �Ӹ� ��Ƽ����
            body_Rend.material = DeadMat_Body;                                 // �ƹ�Ÿ ���� ��Ƽ����
            body_Rend_belt.material = DeadMat_Body;
        }
        //glove_L_Rend.material = DeadMat_Hand;                                // �ƹ�Ÿ �޼� �尩 ��Ƽ����
        //  glove_R_Rend.material = DeadMat_Hand;                                // �ƹ�Ÿ ������ �尩 ��Ƽ����
        // hand_L_Rend.material = DeadMat_Hand;                                 // �ƹ�Ÿ �޼� ��Ƽ���� 
        // hand_R_Rend.material = DeadMat_Hand;                                 // �ƹ�Ÿ ������ ��Ƽ���� 
    }

    public IEnumerator PlayerRespawn() /////////////////////////////////////////������ �޼���/////////////////////////////////////////////////////////////////////
    {
        deadScreen.gameObject.SetActive(false);
        Nickname.gameObject.SetActive(true);

        //hand_Left.enabled = true; 
       // hand_Right.enabled = true;
       /* hand_Left.interactionLayers = 0 | 12 | 13 | 14 | 15;                          // ���ͷ��� ���̾ �ٲٴ� ������� ������ ����
        hand_Right.interactionLayers = 0 | 12 | 13 | 14 | 15;                         
        
       // ���̾� �ѹ� 0 = ����Ʈ ,6 = ���ͷ��ͺ�, 12 = Ȱ, 13 = ȭ��, 14 = ��ųȭ��, 15 = ��Ƽ��*/

        playerColls[0].enabled = false;
        for (int i = 1; i < playerColls.Count; i++)
        {
            playerColls[i].enabled = true;
        }

        at_L_Rend.material = hand_R;
        at_R_Rend.material = hand_R;

        if (DataManager.DM.currentMap == Map.TOY)
        {
            head_Rend.material = head_Mat;
            body_Rend.material = body_Mat;
            body_Rend_belt.material = body_Mat_B;
        }

        if (DataManager.DM.currentMap == Map.WESTERN)
        {
            head_Rend.material = head_Mat;
            body_Rend.material = body_Mat;
            body_Rend_belt.material = body_Mat_B;
        }
        // glove_L_Rend.material = glove_R;
        // glove_R_Rend.material = glove_R;
        //  hand_L_Rend.material = hand_R;
        //  hand_R_Rend.material = hand_R;

        effects[2].SetActive(true);                   // �ǵ� ȿ�� On
        yield return new WaitForSeconds(4.5f);           // 4�ʰ���
        effects[2].SetActive(false);                  // �ǵ� ȿ�� Off        
        curHP = inItHP;
        HP.value = inItHP;
        HP.maxValue = inItHP;
        HP.gameObject.SetActive(true);
        isAlive = true;
        isDeadLock = true;
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

        if (col.CompareTag("RespawnRed") && !isAlive && DataManager.DM.currentTeam == Team.RED)
        {
            PV.RPC("RespawnPlayer", RpcTarget.All);
            Debug.Log("������ ������");
        }
    }

    public void SkillDamage()                                              // ũ��Ƽ��(��弦) ������
    {
        StartCoroutine(ShowDamageScreen());

        if (isDeadLock)
        {
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, grenadePower);
        }
    }
    public void HeadShotDamage()                                              // ũ��Ƽ��(��弦) ������
    {
        StartCoroutine(ShowDamageScreen());
        if (isDeadLock)
        {
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, attackPowerH);
            // PV.RPC(nameof(HeadShot), RpcTarget.AllBuffered);
        }
    }
    public void NormalDamage()                                                // �Ϲ� ������
    {
        StartCoroutine(ShowDamageScreen());
        if (isDeadLock)
        {
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, attackPower);
            PV.RPC(nameof(BodyShot), RpcTarget.AllBuffered);
        }
    }
    public void Respawn()                                                     // ������ �޼���
    {
        PV.RPC(nameof(RespawnPlayer), RpcTarget.AllBuffered);
    }

    public IEnumerator ShowDamageScreen()                                      // �ǰ� ��ũ�� �����ֱ�
    {
        damageScreen.color = new Color(1, 0, 0, Random.Range(0.65f, 0.75f));
        yield return new WaitForSeconds(0.3f);
        damageScreen.color = Color.clear;
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

                if (HP.value <= 30)
                {
                    threeScreen.gameObject.SetActive(true);
                }

                if (HP.value <= 0)
                {
                    isAlive = false;
                    deadScreen.gameObject.SetActive(true);
                    if (DataManager.DM.currentMap == Map.TOY)                                                  // ����
                    {
                        PV.RPC(nameof(PlayerDeadT), RpcTarget.AllViaServer, team);
                    }
                    else if (DataManager.DM.currentMap == Map.WESTERN)                                         // ������
                    {
                        PV.RPC(nameof(PlayerDeadW), RpcTarget.AllViaServer, team);
                    }

                    PV.RPC(nameof(DeadPlayer), RpcTarget.AllBuffered);
                    Debug.Log("ų ����");
                }
            }
        }
    }

    [PunRPC]
    public void PlayerDeadT(int team)
    {
        if (team == 0)                    // �������� ������� �׿��� ��  == ����� ���
        {
            GunShootManager.GSM.score_RedKill++;
            Debug.Log("���忡�� ��簡 �׾���");
        }
        else                              // ������� �������� �׿��� ��  == ������ ���
        {
            GunShootManager.GSM.score_BlueKill++;
            Debug.Log("��翡�� ���尡 �׾���");
        }
    }

    [PunRPC]
    public void PlayerDeadW(int team)
    {
        if (team == 0)                   // �������� ������� �׿��� ��  == ����� ���
        {
            WesternManager.WM.score_RedKill++;
            Debug.Log("���忡�� ��簡 �׾���");
        }
        else                             // ������� �������� �׿��� ��  == ������ ���
        {
            WesternManager.WM.score_BlueKill++;
            Debug.Log("��翡�� ���尡 �׾���");
        }
    }

    /*  [PunRPC]
      public void HeadShot()
      {
          if (!PV.IsMine)
          {
              AudioManager.AM.PlaySE("HeadShot");
          }
      }*/

    [PunRPC]
    public void BodyShot()// ���� �¾��� �� ���� ���� �÷��̾ �������� ȿ���� �鸲
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
            AudioManager.AM.PlaySE("Shield");
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
            stream.SendNext(HP.value);
        }
        else
        {
            Nickname.transform.position = (Vector3)stream.ReceiveNext();
            Nickname.transform.forward = (Vector3)stream.ReceiveNext();
            Nickname.text = (string)stream.ReceiveNext();
            HP.value = (float)stream.ReceiveNext();
        }
    }


}
