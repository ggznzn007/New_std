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

public class AvartarController : MonoBehaviourPunCallbacks, IPunObservable
{
    public static AvartarController ATC;                                     // �̱��� 

    [Header("�÷��̾� ����")]
    public Text Nickname;                                                    // �÷��̾� �г���
    public Image HP;                                                         // �÷��̾� HP
    public float curHP = 100.0f;
    public readonly float inItHP = 100.0f;
    public PhotonView PV;                                                   // �����
    public int actNumber;
    public float attackPower = 10f;
    public GameObject myGun;
    public GameObject hand_Right;   

    [Header("�÷��̾� ������ ����")]
    public MeshRenderer head_Rend;                                           // �ƹ�Ÿ �Ӹ�     ������
    public MeshRenderer body_Rend;                                           // �ƹ�Ÿ ��         
    public SkinnedMeshRenderer glove_R_Rend;                                 // �ƹ�Ÿ �尩     
    public SkinnedMeshRenderer hand_R_Rend;                                  // �ƹ�Ÿ ������   

    [Header("�÷��̾� ��Ƽ���� ����")]
    public Material[] head_Mats;                                             // �ƹ�Ÿ �Ӹ�     ��Ƽ����    
    public Material[] body_Mats;                                             // �ƹ�Ÿ ��          
    public Material glove_R;                                                 // �ƹ�Ÿ �尩
    public Material hand_R;                                                  // �ƹ�Ÿ ������    
    public Material[] DeadMat_Heads;                                         // �Ӹ� ����   ��Ƽ����
    public Material[] DeadMat_Bodys;                                         // ��   ����   ��Ƽ����
    public Material DeadMat_Hand;                                           // ��   ����   ��Ƽ����

    [Header("�÷��̾� �ݶ��̴�")]
    public List<Collider> playerColls;                                       // �÷��̾� �ݶ��̴�

    [Header("�÷��̾� ��������")]
    public bool isAlive;                                                      // �÷��̾� ���� �Ǵܿ���

    [Header("�÷��̾� �ǰ�ȿ�� �̹���")]
    public Image damageScreen;
    public Image deadScreen;

    [Header("�÷��̾� ��ƼŬ ȿ�� ����")]
    public GameObject[] effects;

    private void Awake()
    {
        ATC = this;
        PV = GetComponent<PhotonView>();       
    }

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        if (!PV.IsMine) return;
    }

    public void Initialize()                                                  // �÷��̾� �ʱ�ȭ �޼���
    {
        
        // �÷��̾� HP �ʱ�ȭ
        Nickname.text = PV.IsMine ? PN.NickName : PV.Owner.NickName;         // �÷��̾� ����䰡 �ڽ��̸� �г����� �ƴϸ� ���� �г���
        Nickname.color = PV.IsMine ? Color.white : Color.red;                // �÷��̾� ����䰡 �ڽ��̸� ��� �ƴϸ� ������
        actNumber = PV.Owner.ActorNumber;
       
       
        isAlive = true;                                                      // �÷��̾� ���� �ʱ�ȭ
        curHP = inItHP;                                                      // �÷��̾� HP �ʱ�ȭ
        HP.fillAmount = curHP / inItHP;                                      // ������ �������� HP�� �ʱ�ȭ
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

    public void PlayerDead()                                                 // ���� �޼���
    {
        
        StartCoroutine(ShowDeadEffect());
        Nickname.gameObject.SetActive(false);
        HP.gameObject.SetActive(false);
        hand_Right.SetActive(false);        
        playerColls[2].enabled = false;
        playerColls[3].enabled = false;

        curHP = inItHP;
        HP.fillAmount = curHP / inItHP;

        head_Rend.materials = DeadMat_Heads;       
        body_Rend.materials = DeadMat_Bodys;        
        glove_R_Rend.material = DeadMat_Hand;
        hand_R_Rend.material = DeadMat_Hand;
    }

    public void PlayerRespawn()                                               // ������ �޼���
    {
        StartCoroutine(ShowRespawnEffect());        
        deadScreen.gameObject.SetActive(false);
        isAlive = true;
        HP.gameObject.SetActive(true);
        Nickname.gameObject.SetActive(true);
        hand_Right.SetActive(true);
        playerColls[2].enabled = true;
        playerColls[3].enabled = true;

        head_Rend.materials = head_Mats;
        body_Rend.materials = body_Mats;
        glove_R_Rend.material = glove_R;
        hand_R_Rend.material = hand_R;
    }

    public void DamagedPlayer(float pow)
    {
        string hitter = GetNickNameByActorNumber(actNumber);
        curHP = Mathf.Max(0, curHP - pow);
        HP.fillAmount = curHP / inItHP;
        if (PV.IsMine && curHP <= 0.0f)
        {
            deadScreen.gameObject.SetActive(true);
            isAlive = false;
            PV.RPC("DeadPlayer", RpcTarget.AllBuffered);
            Debug.Log("ų ����" + hitter);
        }
    }
    private void OnTriggerEnter(Collider col)                                 // ������ �±� �� �޼���
    {
        if (col.CompareTag("Respawn") && !isAlive)
        {
            PV.RPC("RespawnPlayer", RpcTarget.AllBuffered);
            Debug.Log("������");
        }
    }
    private void OnCollisionEnter(Collision collision)                         // �Ѿ� �±� �� �޼���
    {
        if (collision.collider.CompareTag("Bullet") && isAlive)
        {            
            StartCoroutine(ShowDamageScreen());
            //DamagedPlayer(10f);
            PV.RPC("Damaged", RpcTarget.AllBuffered, attackPower);
            Debug.Log("�Ѿ˿� ����");
        }
    }

    public IEnumerator ShowDamageScreen()
    {
        AudioManager.AM.EffectPlay(AudioManager.Effect.PlayerDamaged);
        damageScreen.gameObject.SetActive(true);
        damageScreen.color = new Color(1, 0, 0, 1.0f);
        yield return new WaitForSeconds(0.1f);
        damageScreen.color = Color.clear;
        damageScreen.gameObject.SetActive(false);
    }


    public IEnumerator ShowDeadEffect()
    {
        effects[0].SetActive(true);
        yield return new WaitForSeconds(3f);
        effects[0].SetActive(false);
    }

    public IEnumerator ShowRespawnEffect()
    {
        effects[1].SetActive(true);
        yield return new WaitForSeconds(3f);
        effects[1].SetActive(false);
    }

    [PunRPC]
    public void Damaged(float pow)
    {
        string hitter = GetNickNameByActorNumber(actNumber);
        curHP = Mathf.Max(0, curHP - pow);
        HP.fillAmount = curHP / inItHP;
        if (PV.IsMine && curHP <= 0.0f)
        {                        
            deadScreen.gameObject.SetActive(true);
            isAlive = false;
            PV.RPC("DeadPlayer", RpcTarget.AllBuffered);
            Debug.Log("ų ����" + hitter);
        }

    }
    [PunRPC]
    public void DeadPlayer()
    {
        if(PV.IsMine)
        {
            AudioManager.AM.EffectPlay(AudioManager.Effect.PlayerKill);
        }
        
        AudioManager.AM.EffectPlay(AudioManager.Effect.PlayerDead);        
        PlayerDead();
    }

    [PunRPC]
    public void RespawnPlayer()
    {
        AudioManager.AM.EffectPlay(AudioManager.Effect.ReSpawn);
        PlayerRespawn();
    }

    [PunRPC]
    public void KillPlayer()
    {
        AudioManager.AM.EffectPlay(AudioManager.Effect.PlayerKill);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(HP.transform.position);
            stream.SendNext(HP.transform.rotation);
            stream.SendNext(HP.fillAmount);
            stream.SendNext(Nickname.gameObject.transform.position);
            stream.SendNext(Nickname.gameObject.transform.rotation);
            stream.SendNext(Nickname.text);
        }
        else
        {
            HP.transform.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            HP.fillAmount = (float)stream.ReceiveNext();
            Nickname.gameObject.transform.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            Nickname.text = (string)stream.ReceiveNext();
        }
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
