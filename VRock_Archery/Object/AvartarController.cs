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
    public static AvartarController ATC;                                               // 싱글턴

    [Header("플레이어 정보")]
    [SerializeField] TextMeshPro Nickname;                                             // 플레이어 닉네임
    [SerializeField] PhotonView PV;                                                    // 포톤뷰
    public Slider HP;                                                                  // 플레이어 HP
    public Slider myHp;                                                                // 대미지 입었을 때 보이는 플레이어 HP
    public Image hpBack;                                                               // 대미지 입었을 때 보이는 HP background 빨간색
    public Image hpFront;                                                              // 대미지 입었을 때 보이는 HP front 녹색
    public Image hpText;                                                               // 대미지 입었을 때 보이는 HP 텍스트
    public bool isDamaged = false;                                                     // 플레이어 대미지 여부
    public int team;                                                                   // 팀 구분을 위한 변수
    public string damage;                                                              // 대미지 오디오 재생을 위한 문자열
    public string respawn;                                                             // 부활 오디오 재생을 위한 문자열
    public string shield;                                                              // 실드 오디오 재생을 위한 문자열
    public float animTime = 1;                                                         // 대미지 시 HP 재생 애니메이션 시간
    public AnimationCurve fadeCurve;                                                   // 대미지 시 HP 재생 애니메이션 강도

    private readonly int damNormal = 15;                                               // 노멀 대미지
    private readonly int damCritic = 30;                                               // 헤드샷 대미지
    private readonly int damDot = 10;                                                  // 도트 대미지
    private readonly int damSkill = 40;                                                // 폭탄,스킬 대미지
    private readonly int damIce = 30;                                                  // 아이스 대미지
    private int curHP = 100;                                                           // 현재 HP
    private readonly int inItHP = 100;                                                 // 초기 HP
    private float delayTime = 0.7f;                                                    // 이중피격방지를 위한 딜레이    
    [SerializeField] private int actNumber = 0;                                        // 포톤 액터넘버 
    [SerializeField] XRDirectInteractor hand_Left;                                     // 왼손 컨트롤러 - 인터렉션
    [SerializeField] XRDirectInteractor hand_Right;                                    // 오른손 컨트롤러 - 인터렉션
    [SerializeField] GameObject at_hand_Left;                                          // 아바타 왼손
    [SerializeField] GameObject at_hand_Right;                                         // 아바타 오른손
    [SerializeField] GameObject FPS;                                                   // 프레임
    [SerializeField] Camera myCam;                                                     // 아바타 머리 위에 HP바를 위한 
    [SerializeField] GameObject warningScreen;                                         // 경고지역 스크린

    [Header("플레이어 렌더러 묶음")]
    [SerializeField] MeshRenderer head_Rend;                                           // 아바타 머리
    [SerializeField] MeshRenderer body_Rend;                                           // 아바타 몸          
    [SerializeField] MeshRenderer body_Rend_belt;                                      // 아바타 몸_벨트
    [SerializeField] MeshRenderer at_L_Rend;                                           // 아바타 왼손   
    [SerializeField] MeshRenderer at_R_Rend;                                           // 아바타 왼손   

    [Header("플레이어 머티리얼 묶음")]
    [SerializeField] Material head_Mat;                                                // 아바타 머리    
    [SerializeField] Material body_Mat;                                                // 아바타 몸          
    [SerializeField] Material body_Mat_B;                                              // 아바타 몸_벨트      
    [SerializeField] Material hand_R;                                                  // 아바타 손   
    [SerializeField] Material DeadMat_Head;                                            // 머리 죽음   머티리얼
    [SerializeField] Material DeadMat_Body;                                            // 몸   죽음   머티리얼
    [SerializeField] Material DeadMat_Hand;                                            // 손   죽음   머티리얼

    [Header("플레이어 콜라이더")]
    public List<Collider> playerColls;                                                 // 플레이어 콜라이더

    [Header("플레이어 죽음여부")]
    public bool isAlive;                                                               // 플레이어 죽음 판단여부
    public bool isDeadLock;                                                            // 플레이어 이중피격 방지

    [Header("플레이어 피격효과 이미지")]
    [SerializeField] Image damageScreen;                                               // 대미지 입었을 때 화면
    [SerializeField] Image deadScreen;                                                 // 죽었을 때 화면
    [SerializeField] Image thirtyScreen;                                               // HP가 30%이하일 때 화면

    [Header("플레이어 파티클 효과 묶음")]
    [SerializeField] GameObject[] effects;                                             // 죽음, 부활, 실드 파티클 모음                                  
    [SerializeField] GameObject wording_Cr;                                            // 머리 대미지 시 보이는 파티클
    [SerializeField] GameObject wording_Hit;                                           // 몸 대미지 시 보이는 파티클

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

    public void GameOverInteract()                                             // 게임오버시 인터렉션 무효화
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
    public void Show_Frame()                                                   // 프레임 보여주기
    {
        if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out bool pressed))
        {
            if (pressed) { FPS.SetActive(true); }
        }
    }
    public void UnShow_Frame()                                                 // 프레임 숨기기
    {
        if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.secondaryButton, out bool pressed))
        {
            if (pressed) { FPS.SetActive(false); }
        }
    }
    public void Nick_HP_Pos()                                                  // 닉네임,HP 위치 메서드
    {        
        Nickname.transform.position = myCam.transform.position + new Vector3(0, 0.54f, 0);       
        Nickname.transform.forward = -myCam.transform.forward;
    }
    public void Initialize()                                                   // 플레이어 초기화 메서드
    {
        // 플레이어 HP & 닉네임 초기화
        Nickname.text = PV.IsMine ? PN.NickName : PV.Owner.NickName;         // 플레이어 포톤뷰가 자신이면 닉네임을 아니면 오너 닉네임
        Nickname.color = PV.IsMine ? Color.white : Color.red;                // 플레이어 포톤뷰가 자신이면 흰색 아니면 빨간색
        actNumber = PV.Owner.ActorNumber;
        isAlive = true;                                                      // 플레이어 죽음 초기화
        curHP = inItHP;                                                      // 플레이어 HP 초기화
        HP.value = inItHP;
        HP.maxValue = inItHP;                                                  // 실제로 보여지는 HP양 초기화
        myHp.value = inItHP;
        myHp.maxValue = inItHP;

        GetNickNameByActorNumber(actNumber);
    }
    public string GetNickNameByActorNumber(int actorNumber)                    // 닉네임 가져오기
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
    public void SkillDamage()                                                  // 스킬화살 대미지
    {
        if (isDeadLock)
        {
            //PV.RPC(nameof(Wording_H), RpcTarget.AllBuffered);
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, damSkill);
        }
    }
    public void BombDamage()                                                   // 폭탄화살 대미지
    {
        if (isDeadLock)
        {
            PV.RPC(nameof(Wording_H), RpcTarget.AllBuffered);
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, damSkill);
        }
    }
    public void DotDamage()                                                    // NPC 도트 대미지
    {
        if (isDeadLock)
        {
            /*PV.RPC(nameof(Wording_C), RpcTarget.AllBuffered); */
            PV.RPC(nameof(Wording_H), RpcTarget.AllBuffered);
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, damDot);
        }
    }
    public void IceDamage()                                                    // 아이스 폭탄 대미지
    {
        if (isDeadLock)
        {
            /*PV.RPC(nameof(Wording_C), RpcTarget.AllBuffered); */
            PV.RPC(nameof(Wording_H), RpcTarget.AllBuffered);
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, damIce);
        }
    }
    public void HeadShotDamage()                                               // 헤드샷 대미지
    {
        // PV.RPC(nameof(HeadShot), RpcTarget.AllBuffered);
        if (isDeadLock)
        {
            //PV.RPC(nameof(Wording_C), RpcTarget.AllBuffered);
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, damCritic);
        }
    }
    public void NormalDamage()                                                 // 노멀 대미지
    {        
        if (isDeadLock)
        {
            //PV.RPC(nameof(Wording_H), RpcTarget.AllBuffered);
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, damNormal);
        }
    }
    public void Respawn()                                                      // 리스폰 메서드
    {
        PV.RPC(nameof(RespawnPlayer), RpcTarget.AllBuffered);
    }

    public IEnumerator PlayerDead()  ///////////////////////////////////////////죽음 메서드//////////////////////////////////////////////////////////////////
    {
        StartCoroutine(ShowDeadEffect());
        yield return new WaitForSeconds(0.001f);
        isDeadLock = false;                                                  // 중복죽음방지
        Nickname.gameObject.SetActive(false);                                // 플레이어 닉네임
        thirtyScreen.gameObject.SetActive(false);

        hand_Left.enabled = false;
        hand_Right.enabled = false;

        for (int i = 1; i < playerColls.Count; i++)
        {
            playerColls[i].enabled = false;
        }

        curHP = 0;
        HP.gameObject.SetActive(false);                                      // 플레이어 HP
        HP.value = 0;
        HP.maxValue = inItHP;
        myHp.value = 0;
        myHp.maxValue = inItHP;

        at_L_Rend.material = DeadMat_Hand;
        at_R_Rend.material = DeadMat_Hand;

        if (DataManager.DM.currentMap == Map.TOY)
        {
            head_Rend.material = DeadMat_Head;                                 // 아바타 머리 머티리얼
            body_Rend.material = DeadMat_Body;                                 // 아바타 몸통 머티리얼
            body_Rend_belt.material = DeadMat_Body;

        }

        if (DataManager.DM.currentMap == Map.WESTERN)
        {
            head_Rend.material = DeadMat_Head;                                 // 아바타 머리 머티리얼
            body_Rend.material = DeadMat_Body;                                 // 아바타 몸통 머티리얼
            body_Rend_belt.material = DeadMat_Body;

        }

    }
    public IEnumerator PlayerRespawn() /////////////////////////////////////////리스폰 메서드/////////////////////////////////////////////////////////////////////
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

        effects[2].SetActive(true);                   // 실드 효과 On        
        yield return new WaitForSeconds(1.7f);           // 2초간격       
        effects[2].SetActive(false);                  // 실드 효과 Off
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
    public IEnumerator ShowDamageScreen()                                      // 피격 스크린 보여주기
    {
        AudioManager.AM.PlaySE(damage);
        damageScreen.color = new Color(1, 0, 0, 0.7f);
        yield return new WaitForSeconds(0.45f);
        damageScreen.color = Color.clear;
    }
    public IEnumerator ShowDeadEffect()                                        // 죽음 효과 보여주기
    {
        effects[0].SetActive(true);
        yield return new WaitForSeconds(3f);
        effects[0].SetActive(false);
    }
    public IEnumerator ShowRespawnEffect()                                     // 부활 효과 보여주기
    {
        effects[1].SetActive(true);
        yield return new WaitForSeconds(2);
        effects[1].SetActive(false);
    }
    public IEnumerator ShowCri()                                               // 크리티컬 효과 보여주기
    {
        wording_Cr.SetActive(true);
        yield return new WaitForSeconds(1);
        wording_Cr.SetActive(false);
    }
    public IEnumerator ShowHit()                                               // 타격 효과 보여주기
    {
        wording_Hit.SetActive(true);
        yield return new WaitForSeconds(1);
        wording_Hit.SetActive(false);
    }
    public IEnumerator DamagedDelay()                                          // 데미지시 이중피격 방지(딜레이)
    {
        while (delayTime >= 0)
        {
            delayTime -= Time.deltaTime;
            //Debug.Log("콜라이더 OFF");
            playerColls[1].enabled = false;                                      // 머리 콜라이더
            playerColls[2].enabled = false;                                      // 몸통 콜라이더           

            isDamaged = true;
        }
        yield return new WaitForSeconds(0.16f);
        playerColls[1].enabled = true;                                      // 머리 콜라이더
        playerColls[2].enabled = true;                                      // 몸통 콜라이더

        isDamaged = false;
        //Debug.Log("콜라이더 ON");
        if (HP.value <= 0)
        {
            playerColls[1].enabled = false;                                      // 머리 콜라이더
            playerColls[2].enabled = false;                                      // 몸통 콜라이더
        }
    }
    public IEnumerator MyHpOnOff()                                             // 데미지시 남은HP 보여주기
    {
        myHp.gameObject.SetActive(true);
        //yield return StartCoroutine(Fade(0,1));       // 페이드인
        yield return StartCoroutine(Fade(1, 0));       // 페이드아웃
        myHp.gameObject.SetActive(false);
    }
    public IEnumerator Fade(float start, float end)                            // 데미지 페이드 효과 보여주기
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            // fadeTime으로 나누어서 fadeTime 시간 동안
            // percent 값이 0 ~ 1로 증가하도록 함
            currentTime += Time.deltaTime;
            percent = currentTime / animTime;

            // 알파값을 시작부터 끝까지 fadeTime 시간 동안 변화
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

    private void OnTriggerEnter(Collider col)                                 // 리스폰 태그 시 메서드
    {
        if (col.CompareTag("RespawnBlue") && !isAlive && DataManager.DM.currentTeam == Team.BLUE)
        {
            PV.RPC(nameof(RespawnPlayer), RpcTarget.All);
            //Debug.Log("블루팀 리스폰");
        }

        if (col.CompareTag("RespawnRed") && !isAlive && DataManager.DM.currentTeam == Team.RED)
        {
            PV.RPC(nameof(RespawnPlayer), RpcTarget.All);
            //Debug.Log("레드팀 리스폰");
        }

        if (col.CompareTag("Warning"))
        {
            warningScreen.SetActive(true);
            //Debug.Log("경계선 경고!!!!!!!!");
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Warning"))
        {
            warningScreen.SetActive(false);
            //Debug.Log("경계선을 벗어남");
        }
    }

    [PunRPC]
    public void Damaged(int pow)                                                  // 플레이어 데미지 받았을 때
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
                //Debug.Log("남은 HP : " + HP.value.ToString() + "%");

                if (HP.value <= 30)
                {
                    thirtyScreen.gameObject.SetActive(true);
                }

                if (HP.value <= 0)
                {
                    isAlive = false;
                    deadScreen.gameObject.SetActive(true);
                    if (DataManager.DM.currentMap == Map.TOY)                                                  // 토이
                    {
                        PV.RPC(nameof(PlayerDeadT), RpcTarget.AllViaServer, team);
                    }
                    else if (DataManager.DM.currentMap == Map.WESTERN)                                         // 웨스턴
                    {
                        PV.RPC(nameof(PlayerDeadW), RpcTarget.AllViaServer, team);
                    }

                    PV.RPC(nameof(DeadPlayer), RpcTarget.AllBuffered);
                   // Debug.Log("킬 성공");
                }
            }
        }
    }

    [PunRPC]
    public void PlayerDeadT(int team)                                           // 첫번째 게임에서 사망 시
    {
        if (team == 0)                    // 레드팀이 블루팀을 죽였을 때  == 블루팀 사망
        {
            GunShootManager.GSM.score_RedKill++;
            //Debug.Log("레드에게 블루가 죽었음");
        }
        else                              // 블루팀이 레드팀을 죽였을 때  == 레드팀 사망
        {
            GunShootManager.GSM.score_BlueKill++;
            //Debug.Log("블루에게 레드가 죽었음");
        }
    }

    [PunRPC]
    public void PlayerDeadW(int team)                                           // 두번째 게임에서 사망 시
    {
        if (team == 0)                   // 레드팀이 블루팀을 죽였을 때  == 블루팀 사망
        {
            WesternManager.WM.score_RedKill++;
            //Debug.Log("레드에게 블루가 죽었음");
        }
        else                             // 블루팀이 레드팀을 죽였을 때  == 레드팀 사망
        {
            WesternManager.WM.score_BlueKill++;
            //Debug.Log("블루에게 레드가 죽었음");
        }
    }

    [PunRPC]
    public void DeadPlayer()                                                    // 플레이어 죽었을때
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
    public void RespawnPlayer()                                                 // 플레이어 리스폰할 때
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
    public void Wording_C()                                                     // 크리티컬 텍스트 보여주기
    {
        StartCoroutine(ShowCri());
    }
    [PunRPC]
    public void Wording_H()                                                     // 타격 텍스트 보여주기
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
    public void BodyShot()// 몸을 맞았을 때 몸을 맞춘 플레이어를 기준으로 효과음 들림
    {
        if (!PV.IsMine)
        {
            AudioManager.AM.PlaySE("Hit");
        }
    }*/
}
