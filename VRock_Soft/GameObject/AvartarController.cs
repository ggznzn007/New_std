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
    public static AvartarController ATC;                                     // 싱글턴 

    [Header("플레이어 정보")]
    public Text Nickname;                                                    // 플레이어 닉네임
    public Image HP;                                                         // 플레이어 HP
    public float curHP = 100.0f;
    public readonly float inItHP = 100.0f;
    public PhotonView PV;                                                   // 포톤뷰
    public int actNumber;
    public float attackPower = 10f;
    public GameObject myGun;
    public GameObject hand_Right;   

    [Header("플레이어 렌더러 묶음")]
    public MeshRenderer head_Rend;                                           // 아바타 머리     렌더러
    public MeshRenderer body_Rend;                                           // 아바타 몸         
    public SkinnedMeshRenderer glove_R_Rend;                                 // 아바타 장갑     
    public SkinnedMeshRenderer hand_R_Rend;                                  // 아바타 오른손   

    [Header("플레이어 머티리얼 묶음")]
    public Material[] head_Mats;                                             // 아바타 머리     머티리얼    
    public Material[] body_Mats;                                             // 아바타 몸          
    public Material glove_R;                                                 // 아바타 장갑
    public Material hand_R;                                                  // 아바타 오른손    
    public Material[] DeadMat_Heads;                                         // 머리 죽음   머티리얼
    public Material[] DeadMat_Bodys;                                         // 몸   죽음   머티리얼
    public Material DeadMat_Hand;                                           // 손   죽음   머티리얼

    [Header("플레이어 콜라이더")]
    public List<Collider> playerColls;                                       // 플레이어 콜라이더

    [Header("플레이어 죽음여부")]
    public bool isAlive;                                                      // 플레이어 죽음 판단여부

    [Header("플레이어 피격효과 이미지")]
    public Image damageScreen;
    public Image deadScreen;

    [Header("플레이어 파티클 효과 묶음")]
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

    public void Initialize()                                                  // 플레이어 초기화 메서드
    {
        
        // 플레이어 HP 초기화
        Nickname.text = PV.IsMine ? PN.NickName : PV.Owner.NickName;         // 플레이어 포톤뷰가 자신이면 닉네임을 아니면 오너 닉네임
        Nickname.color = PV.IsMine ? Color.white : Color.red;                // 플레이어 포톤뷰가 자신이면 흰색 아니면 빨간색
        actNumber = PV.Owner.ActorNumber;
       
       
        isAlive = true;                                                      // 플레이어 죽음 초기화
        curHP = inItHP;                                                      // 플레이어 HP 초기화
        HP.fillAmount = curHP / inItHP;                                      // 실제로 보여지는 HP양 초기화
        GetNickNameByActorNumber(actNumber);
       
    }

    public string GetNickNameByActorNumber(int actorNumber)   //닉네임 가져오기
    {
        //지금 현재 방에 접속한 사람의 닉네임을 가져온다   -- PlayerListOthers 자기 자신을 뺀 나머지 다 가져옴
        foreach (Player player in PN.PlayerListOthers)
        {
            if (player.ActorNumber == actorNumber)
            {
                return player.NickName;
            }
        }
        return "Ghost";
    }   

    public void PlayerDead()                                                 // 죽음 메서드
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

    public void PlayerRespawn()                                               // 리스폰 메서드
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
            Debug.Log("킬 성공" + hitter);
        }
    }
    private void OnTriggerEnter(Collider col)                                 // 리스폰 태그 시 메서드
    {
        if (col.CompareTag("Respawn") && !isAlive)
        {
            PV.RPC("RespawnPlayer", RpcTarget.AllBuffered);
            Debug.Log("리스폰");
        }
    }
    private void OnCollisionEnter(Collision collision)                         // 총알 태그 시 메서드
    {
        if (collision.collider.CompareTag("Bullet") && isAlive)
        {            
            StartCoroutine(ShowDamageScreen());
            //DamagedPlayer(10f);
            PV.RPC("Damaged", RpcTarget.AllBuffered, attackPower);
            Debug.Log("총알에 맞음");
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
            Debug.Log("킬 성공" + hitter);
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
