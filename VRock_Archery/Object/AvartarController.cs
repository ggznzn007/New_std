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
    public static AvartarController ATC;                                               // �̱���

    [Header("�÷��̾� ����")]
    [SerializeField] TextMeshPro Nickname;                                             // �÷��̾� �г���
    [SerializeField] PhotonView PV;                                                    // �����
    public Slider HP;                                                                  // �÷��̾� HP
    public Slider myHp;                                                                // ����� �Ծ��� �� ���̴� �÷��̾� HP
    public Image hpBack;                                                               // ����� �Ծ��� �� ���̴� HP background ������
    public Image hpFront;                                                              // ����� �Ծ��� �� ���̴� HP front ���
    public Image hpText;                                                               // ����� �Ծ��� �� ���̴� HP �ؽ�Ʈ
    public bool isDamaged = false;                                                     // �÷��̾� ����� ����
    public int team;                                                                   // �� ������ ���� ����
    public string damage;                                                              // ����� ����� ����� ���� ���ڿ�
    public string respawn;                                                             // ��Ȱ ����� ����� ���� ���ڿ�
    public string shield;                                                              // �ǵ� ����� ����� ���� ���ڿ�
    public float animTime = 1;                                                         // ����� �� HP ��� �ִϸ��̼� �ð�
    public AnimationCurve fadeCurve;                                                   // ����� �� HP ��� �ִϸ��̼� ����

    private readonly int damNormal = 15;                                               // ��� �����
    private readonly int damCritic = 30;                                               // ��弦 �����
    private readonly int damDot = 10;                                                  // ��Ʈ �����
    private readonly int damSkill = 40;                                                // ��ź,��ų �����
    private readonly int damIce = 30;                                                  // ���̽� �����
    private int curHP = 100;                                                           // ���� HP
    private readonly int inItHP = 100;                                                 // �ʱ� HP
    private float delayTime = 0.7f;                                                    // �����ǰݹ����� ���� ������    
    [SerializeField] private int actNumber = 0;                                        // ���� ���ͳѹ� 
    [SerializeField] XRDirectInteractor hand_Left;                                     // �޼� ��Ʈ�ѷ� - ���ͷ���
    [SerializeField] XRDirectInteractor hand_Right;                                    // ������ ��Ʈ�ѷ� - ���ͷ���
    [SerializeField] GameObject at_hand_Left;                                          // �ƹ�Ÿ �޼�
    [SerializeField] GameObject at_hand_Right;                                         // �ƹ�Ÿ ������
    [SerializeField] GameObject FPS;                                                   // ������
    [SerializeField] Camera myCam;                                                     // �ƹ�Ÿ �Ӹ� ���� HP�ٸ� ���� 
    [SerializeField] GameObject warningScreen;                                         // ������� ��ũ��

    [Header("�÷��̾� ������ ����")]
    [SerializeField] MeshRenderer head_Rend;                                           // �ƹ�Ÿ �Ӹ�
    [SerializeField] MeshRenderer body_Rend;                                           // �ƹ�Ÿ ��          
    [SerializeField] MeshRenderer body_Rend_belt;                                      // �ƹ�Ÿ ��_��Ʈ
    [SerializeField] MeshRenderer at_L_Rend;                                           // �ƹ�Ÿ �޼�   
    [SerializeField] MeshRenderer at_R_Rend;                                           // �ƹ�Ÿ �޼�   

    [Header("�÷��̾� ��Ƽ���� ����")]
    [SerializeField] Material head_Mat;                                                // �ƹ�Ÿ �Ӹ�    
    [SerializeField] Material body_Mat;                                                // �ƹ�Ÿ ��          
    [SerializeField] Material body_Mat_B;                                              // �ƹ�Ÿ ��_��Ʈ      
    [SerializeField] Material hand_R;                                                  // �ƹ�Ÿ ��   
    [SerializeField] Material DeadMat_Head;                                            // �Ӹ� ����   ��Ƽ����
    [SerializeField] Material DeadMat_Body;                                            // ��   ����   ��Ƽ����
    [SerializeField] Material DeadMat_Hand;                                            // ��   ����   ��Ƽ����

    [Header("�÷��̾� �ݶ��̴�")]
    public List<Collider> playerColls;                                                 // �÷��̾� �ݶ��̴�

    [Header("�÷��̾� ��������")]
    public bool isAlive;                                                               // �÷��̾� ���� �Ǵܿ���
    public bool isDeadLock;                                                            // �÷��̾� �����ǰ� ����

    [Header("�÷��̾� �ǰ�ȿ�� �̹���")]
    [SerializeField] Image damageScreen;                                               // ����� �Ծ��� �� ȭ��
    [SerializeField] Image deadScreen;                                                 // �׾��� �� ȭ��
    [SerializeField] Image thirtyScreen;                                               // HP�� 30%������ �� ȭ��

    [Header("�÷��̾� ��ƼŬ ȿ�� ����")]
    [SerializeField] GameObject[] effects;                                             // ����, ��Ȱ, �ǵ� ��ƼŬ ����                                  
    [SerializeField] GameObject wording_Cr;                                            // �Ӹ� ����� �� ���̴� ��ƼŬ
    [SerializeField] GameObject wording_Hit;                                           // �� ����� �� ���̴� ��ƼŬ

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
    }

    void Update()
    {
        if (!PV.IsMine) return;        
        Nick_HP_Pos();
        Show_Frame();
        UnShow_Frame();
        GameOverInteract();
    }

    public void GameOverInteract()                                             // ���ӿ����� ���ͷ��� ��ȿȭ
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
    public void Show_Frame()                                                   // ������ �����ֱ�
    {
        if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out bool pressed))
        {
            if (pressed) { FPS.SetActive(true); }
        }
    }
    public void UnShow_Frame()                                                 // ������ �����
    {
        if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.secondaryButton, out bool pressed))
        {
            if (pressed) { FPS.SetActive(false); }
        }
    }
    public void Nick_HP_Pos()                                                  // �г���,HP ��ġ �޼���
    {        
        Nickname.transform.position = myCam.transform.position + new Vector3(0, 0.54f, 0);       
        Nickname.transform.forward = -myCam.transform.forward;
    }
    public void Initialize()                                                   // �÷��̾� �ʱ�ȭ �޼���
    {
        // �÷��̾� HP & �г��� �ʱ�ȭ
        Nickname.text = PV.IsMine ? PN.NickName : PV.Owner.NickName;         // �÷��̾� ����䰡 �ڽ��̸� �г����� �ƴϸ� ���� �г���
        Nickname.color = PV.IsMine ? Color.white : Color.red;                // �÷��̾� ����䰡 �ڽ��̸� ��� �ƴϸ� ������
        actNumber = PV.Owner.ActorNumber;
        isAlive = true;                                                      // �÷��̾� ���� �ʱ�ȭ
        curHP = inItHP;                                                      // �÷��̾� HP �ʱ�ȭ
        HP.value = inItHP;
        HP.maxValue = inItHP;                                                  // ������ �������� HP�� �ʱ�ȭ
        myHp.value = inItHP;
        myHp.maxValue = inItHP;

        GetNickNameByActorNumber(actNumber);
    }
    public string GetNickNameByActorNumber(int actorNumber)                    // �г��� ��������
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
    public void SkillDamage()                                                  // ��ųȭ�� �����
    {
        if (isDeadLock)
        {
            //PV.RPC(nameof(Wording_H), RpcTarget.AllBuffered);
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, damSkill);
        }
    }
    public void BombDamage()                                                   // ��źȭ�� �����
    {
        if (isDeadLock)
        {
            PV.RPC(nameof(Wording_H), RpcTarget.AllBuffered);
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, damSkill);
        }
    }
    public void DotDamage()                                                    // NPC ��Ʈ �����
    {
        if (isDeadLock)
        {
            /*PV.RPC(nameof(Wording_C), RpcTarget.AllBuffered); */
            PV.RPC(nameof(Wording_H), RpcTarget.AllBuffered);
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, damDot);
        }
    }
    public void IceDamage()                                                    // ���̽� ��ź �����
    {
        if (isDeadLock)
        {
            /*PV.RPC(nameof(Wording_C), RpcTarget.AllBuffered); */
            PV.RPC(nameof(Wording_H), RpcTarget.AllBuffered);
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, damIce);
        }
    }
    public void HeadShotDamage()                                               // ��弦 �����
    {
        // PV.RPC(nameof(HeadShot), RpcTarget.AllBuffered);
        if (isDeadLock)
        {
            //PV.RPC(nameof(Wording_C), RpcTarget.AllBuffered);
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, damCritic);
        }
    }
    public void NormalDamage()                                                 // ��� �����
    {        
        if (isDeadLock)
        {
            //PV.RPC(nameof(Wording_H), RpcTarget.AllBuffered);
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, damNormal);
        }
    }
    public void Respawn()                                                      // ������ �޼���
    {
        PV.RPC(nameof(RespawnPlayer), RpcTarget.AllBuffered);
    }

    public IEnumerator PlayerDead()  ///////////////////////////////////////////���� �޼���//////////////////////////////////////////////////////////////////
    {
        StartCoroutine(ShowDeadEffect());
        yield return new WaitForSeconds(0.001f);
        isDeadLock = false;                                                  // �ߺ���������
        Nickname.gameObject.SetActive(false);                                // �÷��̾� �г���
        thirtyScreen.gameObject.SetActive(false);

        hand_Left.enabled = false;
        hand_Right.enabled = false;

        for (int i = 1; i < playerColls.Count; i++)
        {
            playerColls[i].enabled = false;
        }

        curHP = 0;
        HP.gameObject.SetActive(false);                                      // �÷��̾� HP
        HP.value = 0;
        HP.maxValue = inItHP;
        myHp.value = 0;
        myHp.maxValue = inItHP;

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

    }
    public IEnumerator PlayerRespawn() /////////////////////////////////////////������ �޼���/////////////////////////////////////////////////////////////////////
    {
        deadScreen.gameObject.SetActive(false);
        Nickname.gameObject.SetActive(true);

        hand_Left.enabled = true;
        hand_Right.enabled = true;

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

        effects[2].SetActive(true);                   // �ǵ� ȿ�� On        
        yield return new WaitForSeconds(1.7f);           // 2�ʰ���       
        effects[2].SetActive(false);                  // �ǵ� ȿ�� Off
        AudioManager.AM.StopSE(shield);
        curHP = inItHP;
        HP.value = inItHP;
        HP.maxValue = inItHP;
        HP.gameObject.SetActive(true);
        myHp.value = inItHP;
        myHp.maxValue = inItHP;
        isAlive = true;
        isDeadLock = true;
    }
    public IEnumerator ShowDamageScreen()                                      // �ǰ� ��ũ�� �����ֱ�
    {
        AudioManager.AM.PlaySE(damage);
        damageScreen.color = new Color(1, 0, 0, 0.7f);
        yield return new WaitForSeconds(0.45f);
        damageScreen.color = Color.clear;
    }
    public IEnumerator ShowDeadEffect()                                        // ���� ȿ�� �����ֱ�
    {
        effects[0].SetActive(true);
        yield return new WaitForSeconds(3f);
        effects[0].SetActive(false);
    }
    public IEnumerator ShowRespawnEffect()                                     // ��Ȱ ȿ�� �����ֱ�
    {
        effects[1].SetActive(true);
        yield return new WaitForSeconds(2);
        effects[1].SetActive(false);
    }
    public IEnumerator ShowCri()                                               // ũ��Ƽ�� ȿ�� �����ֱ�
    {
        wording_Cr.SetActive(true);
        yield return new WaitForSeconds(1);
        wording_Cr.SetActive(false);
    }
    public IEnumerator ShowHit()                                               // Ÿ�� ȿ�� �����ֱ�
    {
        wording_Hit.SetActive(true);
        yield return new WaitForSeconds(1);
        wording_Hit.SetActive(false);
    }
    public IEnumerator DamagedDelay()                                          // �������� �����ǰ� ����(������)
    {
        while (delayTime >= 0)
        {
            delayTime -= Time.deltaTime;
            //Debug.Log("�ݶ��̴� OFF");
            playerColls[1].enabled = false;                                      // �Ӹ� �ݶ��̴�
            playerColls[2].enabled = false;                                      // ���� �ݶ��̴�           

            isDamaged = true;
        }
        yield return new WaitForSeconds(0.16f);
        playerColls[1].enabled = true;                                      // �Ӹ� �ݶ��̴�
        playerColls[2].enabled = true;                                      // ���� �ݶ��̴�

        isDamaged = false;
        //Debug.Log("�ݶ��̴� ON");
        if (HP.value <= 0)
        {
            playerColls[1].enabled = false;                                      // �Ӹ� �ݶ��̴�
            playerColls[2].enabled = false;                                      // ���� �ݶ��̴�
        }
    }
    public IEnumerator MyHpOnOff()                                             // �������� ����HP �����ֱ�
    {
        myHp.gameObject.SetActive(true);
        //yield return StartCoroutine(Fade(0,1));       // ���̵���
        yield return StartCoroutine(Fade(1, 0));       // ���̵�ƿ�
        myHp.gameObject.SetActive(false);
    }
    public IEnumerator Fade(float start, float end)                            // ������ ���̵� ȿ�� �����ֱ�
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            // fadeTime���� ����� fadeTime �ð� ����
            // percent ���� 0 ~ 1�� �����ϵ��� ��
            currentTime += Time.deltaTime;
            percent = currentTime / animTime;

            // ���İ��� ���ۺ��� ������ fadeTime �ð� ���� ��ȭ
            Color c = hpFront.color;
            Color c2 = hpBack.color;
            Color c3 = hpText.color;
            // color.a = Mathf.Lerp(start, end, percent);
            c.a = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));
            c2.a = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));
            c3.a = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));

            hpFront.color = c;
            hpBack.color = c2;
            hpText.color = c3;

            yield return null;
        }
    }

    /* public IEnumerator ShowEmergency()
     {
         while(true)           // Color(R,G,B,alphaValue)
         {
             damageScreen.color = new Color(1, 0, 0,1f);
             yield return new WaitForSeconds(0.5f);
             damageScreen.color = Color.clear;
             yield return new WaitForSeconds(0.5f);
             damageScreen.color = new Color(1, 0, 0, 1f);
             yield return new WaitForSeconds(0.5f);
             damageScreen.color = Color.clear;
             yield return new WaitForSeconds(0.5f);
             if (HP.value<=0)
             {
                 break;
             }
         }
     }*/

    private void OnTriggerEnter(Collider col)                                 // ������ �±� �� �޼���
    {
        if (col.CompareTag("RespawnBlue") && !isAlive && DataManager.DM.currentTeam == Team.BLUE)
        {
            PV.RPC(nameof(RespawnPlayer), RpcTarget.All);
            //Debug.Log("����� ������");
        }

        if (col.CompareTag("RespawnRed") && !isAlive && DataManager.DM.currentTeam == Team.RED)
        {
            PV.RPC(nameof(RespawnPlayer), RpcTarget.All);
            //Debug.Log("������ ������");
        }

        if (col.CompareTag("Warning"))
        {
            warningScreen.SetActive(true);
            //Debug.Log("��輱 ���!!!!!!!!");
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Warning"))
        {
            warningScreen.SetActive(false);
            //Debug.Log("��輱�� ���");
        }
    }

    [PunRPC]
    public void Damaged(int pow)                                                  // �÷��̾� ������ �޾��� ��
    {
        if (isAlive && !isDamaged)
        {
            if (PV.IsMine)
            {
                StartCoroutine(MyHpOnOff());
                if (isDamaged || !isAlive) { return; }
                curHP -= pow;
                HP.value = Mathf.Round(curHP * 10) * 0.1f;
                HP.maxValue = inItHP;
                myHp.value = HP.value;
                myHp.maxValue = HP.maxValue;                
                StartCoroutine(DamagedDelay());
                StartCoroutine(ShowDamageScreen());
                delayTime = 1f;
                //Debug.Log("���� HP : " + HP.value.ToString() + "%");

                if (HP.value <= 30)
                {
                    thirtyScreen.gameObject.SetActive(true);
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
                   // Debug.Log("ų ����");
                }
            }
        }
    }

    [PunRPC]
    public void PlayerDeadT(int team)                                           // ù��° ���ӿ��� ��� ��
    {
        if (team == 0)                    // �������� ������� �׿��� ��  == ����� ���
        {
            GunShootManager.GSM.score_RedKill++;
            //Debug.Log("���忡�� ��簡 �׾���");
        }
        else                              // ������� �������� �׿��� ��  == ������ ���
        {
            GunShootManager.GSM.score_BlueKill++;
            //Debug.Log("��翡�� ���尡 �׾���");
        }
    }

    [PunRPC]
    public void PlayerDeadW(int team)                                           // �ι�° ���ӿ��� ��� ��
    {
        if (team == 0)                   // �������� ������� �׿��� ��  == ����� ���
        {
            WesternManager.WM.score_RedKill++;
            //Debug.Log("���忡�� ��簡 �׾���");
        }
        else                             // ������� �������� �׿��� ��  == ������ ���
        {
            WesternManager.WM.score_BlueKill++;
            //Debug.Log("��翡�� ���尡 �׾���");
        }
    }

    [PunRPC]
    public void DeadPlayer()                                                    // �÷��̾� �׾�����
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
    public void RespawnPlayer()                                                 // �÷��̾� �������� ��
    {
        if (PV.IsMine)
        {
            AudioManager.AM.PlaySE(respawn);
            AudioManager.AM.PlaySE(shield);
        }
        StartCoroutine(ShowRespawnEffect());

        StartCoroutine(PlayerRespawn());
    }

    [PunRPC]
    public void Wording_C()                                                     // ũ��Ƽ�� �ؽ�Ʈ �����ֱ�
    {
        StartCoroutine(ShowCri());
    }
    [PunRPC]
    public void Wording_H()                                                     // Ÿ�� �ؽ�Ʈ �����ֱ�
    {
        StartCoroutine(ShowHit());
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

    /*  [PunRPC]
    public void HeadShot()
    {
        if (PV.IsMine)
        {
            AudioManager.AM.PlaySE("HeadShot");
        }
    }

    [PunRPC]
    public void BodyShot()// ���� �¾��� �� ���� ���� �÷��̾ �������� ȿ���� �鸲
    {
        if (!PV.IsMine)
        {
            AudioManager.AM.PlaySE("Hit");
        }
    }*/
}
