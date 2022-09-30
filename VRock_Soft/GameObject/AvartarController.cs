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
    [SerializeField] Text Nickname;                                                    // �÷��̾� �г���
    [SerializeField] Image HP;                                                         // �÷��̾� HP
    [SerializeField] float curHP = 100.0f;
    [SerializeField] float inItHP = 100.0f;
    [SerializeField] PhotonView PV;                                                   // �����
    [SerializeField] int actNumber = 0;
    [SerializeField] float attackPower = 10f;
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
    [SerializeField] Material glove_R;                                                 // �ƹ�Ÿ ������ �尩 
    [SerializeField] Material hand_R;                                                  // �ƹ�Ÿ ������    
    [SerializeField] Material[] DeadMat_Heads;                                         // �Ӹ� ����   ��Ƽ����
    [SerializeField] Material[] DeadMat_Bodys;                                         // ��   ����   ��Ƽ����
    [SerializeField] Material DeadMat_Hand;                                            // ��   ����   ��Ƽ����

    [Header("�÷��̾� �ݶ��̴�")]
    [SerializeField] List<Collider> playerColls;                                       // �÷��̾� �ݶ��̴�

    [Header("�÷��̾� ��������")]
    public bool isAlive;                                                      // �÷��̾� ���� �Ǵܿ���

    [Header("�÷��̾� �ǰ�ȿ�� �̹���")]
    [SerializeField] Image damageScreen;
    [SerializeField] Image deadScreen;


    [Header("�÷��̾� ��ƼŬ ȿ�� ����")]
    [SerializeField] GameObject[] effects;
    [SerializeField] bool isDeadLock;

    private void Awake()
    {
        ATC = this;
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        Initialize();
        isDeadLock = true;
    }

    void Update()
    {
        if (!PV.IsMine) return;
        Nick_HP_Pos();
    }

    public void Nick_HP_Pos()
    {
        // HP.transform.SetPositionAndRotation(myCam.transform.position + new Vector3(0, 0.5f, 0), myCam.transform.rotation);
        HP.transform.position = myCam.transform.position + new Vector3(0, 0.4f, 0);
        //Nickname.transform.SetPositionAndRotation(myCam.transform.position + new Vector3(0, 0.6f, 0), myCam.transform.rotation);
        Nickname.transform.position = myCam.transform.position + new Vector3(0, 0.5f, 0);

        HP.transform.forward = -myCam.transform.forward;
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
        HP.fillAmount = inItHP;                                              // ������ �������� HP�� �ʱ�ȭ
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
        yield return null;
        isDeadLock = false;                                                  // �ߺ���������
        Nickname.gameObject.SetActive(false);                                // �÷��̾� �г���
        HP.gameObject.SetActive(false);                                      // �÷��̾� HP
        at_hand_Left.SetActive(false);                                       // �ƹ�Ÿ �޼�
        at_hand_Right.SetActive(false);                                      // �ƹ�Ÿ ������
        hand_Left.SetActive(false);                                          // �޼� ��Ʈ�ѷ�
        hand_Right.SetActive(false);                                         // ������ ��Ʈ�ѷ�
        FPS.SetActive(false);                                                // ������UI
        playerColls[2].enabled = false;                                      // �����۽����ڽ� ����
        playerColls[3].enabled = false;                                      // �����۽����ڽ� ������

        HP.fillAmount = 0f;
        curHP = 0f;

        head_Rend.materials = DeadMat_Heads;                                 // �ƹ�Ÿ �Ӹ� ��Ƽ����
        body_Rend.materials = DeadMat_Bodys;                                 // �ƹ�Ÿ ���� ��Ƽ����
        glove_L_Rend.material = DeadMat_Hand;                                // �ƹ�Ÿ �޼� �尩 ��Ƽ����
        glove_R_Rend.material = DeadMat_Hand;                                // �ƹ�Ÿ ������ �尩 ��Ƽ����
        hand_L_Rend.material = DeadMat_Hand;                                 // �ƹ�Ÿ �޼� ��Ƽ���� 
        hand_R_Rend.material = DeadMat_Hand;                                 // �ƹ�Ÿ ������ ��Ƽ���� 
    }

    public IEnumerator PlayerRespawn() /////////////////////////////////////////������ �޼���/////////////////////////////////////////////////////////////////////
    {
        deadScreen.gameObject.SetActive(false);
        Nickname.gameObject.SetActive(true);
        at_hand_Left.SetActive(true);
        at_hand_Right.SetActive(true);
        hand_Left.SetActive(true);
        hand_Right.SetActive(true);
        FPS.SetActive(true);
        playerColls[2].enabled = true;
        playerColls[3].enabled = true;

        head_Rend.materials = head_Mats;
        body_Rend.materials = body_Mats;
        glove_L_Rend.material = glove_R;
        glove_R_Rend.material = glove_R;
        hand_L_Rend.material = hand_R;
        hand_R_Rend.material = hand_R;

        effects[2].SetActive(true);                   // �ǵ� ȿ�� On
        yield return new WaitForSeconds(4);           // 4�ʰ���
        effects[2].SetActive(false);                  // �ǵ� ȿ�� Off
        isAlive = true;
        isDeadLock = true;
        curHP = inItHP;
        HP.fillAmount = inItHP;
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
            Debug.Log("������");
        }

        else if (col.CompareTag("RespawnRed") && !isAlive && DataManager.DM.currentTeam == Team.RED)
        {
            PV.RPC("RespawnPlayer", RpcTarget.All);
            Debug.Log("������");
        }
    }
    private void OnCollisionEnter(Collision collision)                         // �Ѿ� �±� �� �޼���
    {
        if (collision.collider.CompareTag("Bullet") && isAlive && NetworkManager.NM.inGame)
        {
            StartCoroutine(ShowDamageScreen());
            if (isDeadLock)
            {
                PV.RPC("Damaged", RpcTarget.All, attackPower);
                AudioManager.AM.EffectPlay(AudioManager.Effect.PlayerHit);
                Debug.Log("�Ѿ˿� ����");
            }
        }
    }

    public IEnumerator ShowDamageScreen()                                      // �ǰ� ��ũ�� �����ֱ�
    {
        //damageScreen.gameObject.SetActive(true);
        damageScreen.color = new Color(1, 0, 0, Random.Range(0.65f,0.75f));
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

    [PunRPC]
    public void Damaged(float pow)
    {
        if (PV.IsMine && isAlive)
        {
            AudioManager.AM.EffectPlay(AudioManager.Effect.PlayerDamaged);
        }
        //curHP = Mathf.Max(0, curHP - pow);                
        curHP -= pow;
        HP.fillAmount = curHP / inItHP;
        if (PV.IsMine && curHP <= 0.0f)
        {
            isAlive = false;
            deadScreen.gameObject.SetActive(true);
            PV.RPC("DeadPlayer", RpcTarget.All);
            Debug.Log("ų ����");
        }
    }

    [PunRPC]
    public void DeadPlayer()
    {
        StartCoroutine(ShowDeadEffect());
        if (!PV.IsMine)
        {
            AudioManager.AM.EffectPlay(AudioManager.Effect.PlayerKill);
        }
        AudioManager.AM.EffectPlay(AudioManager.Effect.PlayerDead);

        StartCoroutine(PlayerDead());
    }

    [PunRPC]
    public void RespawnPlayer()
    {
        if (PV.IsMine)
        {
            AudioManager.AM.EffectPlay(AudioManager.Effect.ReSpawn);
        }
        StartCoroutine(ShowRespawnEffect());

        StartCoroutine(PlayerRespawn());
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // stream.SendNext(HP.transform.rotation);
            stream.SendNext(HP.transform.position);
            stream.SendNext(HP.transform.forward);
            stream.SendNext(HP.fillAmount);
            // stream.SendNext(Nickname.gameObject.transform.rotation);
            stream.SendNext(Nickname.transform.position);
            stream.SendNext(Nickname.transform.forward);
            stream.SendNext(Nickname.text);
            // stream.SendNext(isAlive);
        }
        else
        {
            //HP.transform.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            HP.transform.position = (Vector3)stream.ReceiveNext();
            HP.transform.forward = (Vector3)stream.ReceiveNext();
            HP.fillAmount = (float)stream.ReceiveNext();
            Nickname.transform.position = (Vector3)stream.ReceiveNext();
            Nickname.transform.forward = (Vector3)stream.ReceiveNext();
            Nickname.text = (string)stream.ReceiveNext();
            // isAlive = (bool)stream.ReceiveNext();
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
