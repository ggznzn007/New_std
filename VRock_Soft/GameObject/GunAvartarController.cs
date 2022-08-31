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

public class GunAvartarController : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GunAvartarController GAC;
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
    public GameObject FPS;
    public Camera myCam;

    [Header("�÷��̾� ������ ����")]
    public MeshRenderer head_Rend;                                           // �ƹ�Ÿ �Ӹ�     ������
    public MeshRenderer body_Rend;                                           // �ƹ�Ÿ ��         
    public MeshRenderer hand_R_Rend;                                           // �ƹ�Ÿ ��         

    [Header("�÷��̾� ��Ƽ���� ����")]
    public Material head_Mat;                                             // �ƹ�Ÿ �Ӹ�     ��Ƽ����    
    public Material body_Mat;                                             // �ƹ�Ÿ ��          
  //  public Material glove_R;                                                 // �ƹ�Ÿ �尩
    public Material hand_R;                                                  // �ƹ�Ÿ ������    
    public Material DeadMat_Head;                                         // �Ӹ� ����   ��Ƽ����
   public Material DeadMat_Body;                                         // ��   ����   ��Ƽ����
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
        GAC = this;
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        Initialize();
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

        // �÷��̾� HP �ʱ�ȭ
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

    public void PlayerDead()                                                 // ���� �޼���
    {

        StartCoroutine(ShowDeadEffect());
        Nickname.gameObject.SetActive(false);
        HP.gameObject.SetActive(false);
        hand_Right.SetActive(false);
        FPS.SetActive(false);
        playerColls[2].enabled = false;
        playerColls[3].enabled = false;

        HP.fillAmount = 0f;
        curHP = 0f;


        head_Rend.material = DeadMat_Head;
        body_Rend.material= DeadMat_Body;
        
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
        FPS.SetActive(true);
        playerColls[2].enabled = true;
        playerColls[3].enabled = true;

        HP.fillAmount = inItHP;
        curHP = inItHP;

        head_Rend.material = head_Mat;
        body_Rend.material = body_Mat;
        
        hand_R_Rend.material = hand_R;
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
        /*if (collision.collider.CompareTag("Gun") && isAlive)
        {
            if(SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))
            {
                if (!griped)
                {
                    StartCoroutine(ShowDamageScreen());
                    //DamagedPlayer(10f);
                    PV.RPC("Damaged", RpcTarget.AllBuffered, attackPower);
                    Debug.Log("�ѿ� ����");
                }
            }
            
           
        }*/
    }

    public IEnumerator ShowDamageScreen()                                      // �ǰ� ��ũ��
    {

        damageScreen.gameObject.SetActive(true);
        damageScreen.color = new Color(1, 0, 0, 1.0f);
        yield return new WaitForSeconds(0.1f);
        damageScreen.color = Color.clear;
        damageScreen.gameObject.SetActive(false);
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
        AudioManager.AM.EffectPlay(AudioManager.Effect.PlayerDamaged);
        // string hitter = GetNickNameByActorNumber(actNumber);
        curHP = Mathf.Max(0, curHP - pow);
        //curHP -= attackPower;        
        HP.fillAmount = curHP / inItHP;
        if (PV.IsMine && curHP <= 0.0f)
        {
            isAlive = false;
            deadScreen.gameObject.SetActive(true);
            PV.RPC("DeadPlayer", RpcTarget.AllBuffered);
            Debug.Log("ų ����");
        }

    }
    [PunRPC]
    public void DeadPlayer()
    {
        if (!PV.IsMine)
        {
            AudioManager.AM.EffectPlay(AudioManager.Effect.PlayerKill);
        }

        AudioManager.AM.EffectPlay(AudioManager.Effect.PlayerDead);
        PlayerDead();
    }

    [PunRPC]
    public void RespawnPlayer()
    {
        if (PV.IsMine)
        {
            AudioManager.AM.EffectPlay(AudioManager.Effect.ReSpawn);
        }

        PlayerRespawn();
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
        }
    }
}
