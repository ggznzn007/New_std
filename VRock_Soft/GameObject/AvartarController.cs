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
using Unity.XR.PXR;
using static Pico.Platform.Message;

public class AvartarController : MonoBehaviourPun, IPunObservable
{
    public static AvartarController ATC;                                     // 싱글턴

    [Header("플레이어 정보")]
    [SerializeField] TextMeshPro Nickname;                            // 플레이어 닉네임
    [SerializeField] PhotonView PV;                                   // 포톤뷰
    public Slider HP;                                                 // 플레이어 HP
    [SerializeField] int curHP = 100;
    [SerializeField] int inItHP = 100;
    [SerializeField] int actNumber = 0;
    [SerializeField] int attackPower = 15;
    [SerializeField] int attackPowerH = 30;
    [SerializeField] int grenadePower = 40;
    [SerializeField] XRDirectInteractor hand_Left;
    [SerializeField] XRDirectInteractor hand_Right;
    [SerializeField] GameObject at_hand_Left;
    [SerializeField] GameObject at_hand_Right;
    [SerializeField] GameObject FPS;
    [SerializeField] Camera myCam;

    [Header("플레이어 렌더러 묶음")]
    [SerializeField] MeshRenderer head_Rend;                                           // 아바타 머리     렌더러
    [SerializeField] MeshRenderer body_Rend;                                           // 아바타 몸          
    [SerializeField] MeshRenderer body_Rend_belt;                                           // 아바타 몸         
                                                                                            // [SerializeField] SkinnedMeshRenderer glove_L_Rend;                                 // 아바타 왼손   장갑     
                                                                                            // [SerializeField] SkinnedMeshRenderer glove_R_Rend;                                 // 아바타 오른손 장갑     
    //[SerializeField] MeshRenderer hand_L_Rend;                                  // 컨트롤러 왼손   
    [SerializeField] MeshRenderer at_L_Rend;                                  // 아바타 왼손   
    //[SerializeField] MeshRenderer hand_R_Rend;                                  // 컨트롤러 오른손   
    [SerializeField] MeshRenderer at_R_Rend;                                  // 아바타 오른손   

    [Header("플레이어 머티리얼 묶음")]
    [SerializeField] Material head_Mat;                                             // 아바타 머리     머티리얼    
    [SerializeField] Material body_Mat;                                             // 아바타 몸          
    [SerializeField] Material body_Mat_B;                                             // 웨스턴 아바타 몸        
    [SerializeField] Material hand_R;                                                  // 아바타 오른손    
                                                                                      // [SerializeField] Material glove_R;                                                 // 아바타 오른손 장갑 
    [SerializeField] Material DeadMat_Head;                                         // 머리 죽음   머티리얼
    [SerializeField] Material DeadMat_Body;                                         // 몸   죽음   머티리얼
    [SerializeField] Material DeadMat_Hand;                                            // 손   죽음   머티리얼

    [Header("플레이어 콜라이더")]
    public List<Collider> playerColls;                                       // 플레이어 콜라이더

    [Header("플레이어 죽음여부")]
    public bool isAlive;                                                      // 플레이어 죽음 판단여부

    [Header("플레이어 피격효과 이미지")]
    [SerializeField] Image damageScreen;
    [SerializeField] Image deadScreen;
    [SerializeField] Image threeScreen;

    [Header("플레이어 파티클 효과 묶음")]
    [SerializeField] GameObject[] effects;
    [SerializeField] bool isDeadLock;

    private float delayTime = 1f;
    public bool isDamaged = false;    
    public int team;
    //private int showCount = 0;

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
        //PN.UseRpcMonoBehaviourCache = true;
    }

    void Update()
    {
        if (!PV.IsMine) return;
        Nick_HP_Pos();
        Show_Frame();
        UnShow_Frame();
        GameOverInteract();
        //PV.RefreshRpcMonoBehaviourCache();
    }

    public void GameOverInteract()
    {
        if(DataManager.DM.gameOver)
        {
            hand_Left.interactionLayers = 0;                                     
            hand_Right.interactionLayers = 0;
            for (int i = 3; i < playerColls.Count; i++)
            {
                playerColls[i].enabled = false;
            }
        }       
    }

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
        Nickname.transform.position = myCam.transform.position + new Vector3(0, 0.55f, 0);
        // HP.transform.forward = -myCam.transform.forward;
        Nickname.transform.forward = -myCam.transform.forward;
    }
    public void Initialize()                                                  // 플레이어 초기화 메서드
    {
        // 플레이어 HP & 닉네임 초기화
        Nickname.text = PV.IsMine ? PN.NickName : PV.Owner.NickName;         // 플레이어 포톤뷰가 자신이면 닉네임을 아니면 오너 닉네임
        Nickname.color = PV.IsMine ? Color.white : Color.red;                // 플레이어 포톤뷰가 자신이면 흰색 아니면 빨간색
        actNumber = PV.Owner.ActorNumber;
        isAlive = true;                                                      // 플레이어 죽음 초기화
        curHP = inItHP;                                                      // 플레이어 HP 초기화
        HP.value = inItHP;
        HP.maxValue = inItHP;                                                  // 실제로 보여지는 HP양 초기화      
        
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

    public IEnumerator PlayerDead()  ///////////////////////////////////////////죽음 메서드//////////////////////////////////////////////////////////////////
    {        
        StartCoroutine(ShowDeadEffect());       
        yield return new WaitForSeconds(0.001f);
        isDeadLock = false;                                                  // 중복죽음방지        
        Nickname.gameObject.SetActive(false);                                // 플레이어 닉네임
        threeScreen.gameObject.SetActive(false);      
      
        hand_Left.interactionLayers = 0;                                     // 인터렉션 레이어를 바꾸는 방법으로 소유권 이전
        hand_Right.interactionLayers = 0;                                    // 레이어 넘버 0 = 디폴트 ,6 = 인터렉터블, 12 = 쉴드

        playerColls[0].enabled = true;                                       // 리스폰 감지 콜라이더
        playerColls[1].enabled = false;                                      // 머리 콜라이더
        playerColls[2].enabled = false;                                      // 몸통 콜라이더
        playerColls[3].enabled = false;                                      // 아이템스폰박스 오른쪽
        playerColls[4].enabled = false;                                      // 아이템스폰박스 오른쪽        

        curHP = 0;
        HP.gameObject.SetActive(false);                                      // 플레이어 HP
        HP.value = 0;
        HP.maxValue = inItHP;

       // hand_L_Rend.material = DeadMat_Hand;                                 // 아바타 왼손 머티리얼 
      //  hand_R_Rend.material = DeadMat_Hand;                                 // 아바타 오른손 머티리얼 
        at_L_Rend.material = DeadMat_Hand;
        at_R_Rend.material = DeadMat_Hand;

        if (DataManager.DM.currentMap == Map.TOY)
        {
            head_Rend.material = DeadMat_Head;                                 // 아바타 머리 머티리얼
            body_Rend.material = DeadMat_Body;                                 // 아바타 몸통 머티리얼
        }

        if (DataManager.DM.currentMap == Map.WESTERN)
        {
            head_Rend.material = DeadMat_Head;                                 // 아바타 머리 머티리얼
            body_Rend.material = DeadMat_Body;                                 // 아바타 몸통 머티리얼
            body_Rend_belt.material = DeadMat_Body;            
        }
        //glove_L_Rend.material = DeadMat_Hand;                                // 아바타 왼손 장갑 머티리얼
        //  glove_R_Rend.material = DeadMat_Hand;                                // 아바타 오른손 장갑 머티리얼
    }

    public IEnumerator PlayerRespawn() /////////////////////////////////////////리스폰 메서드/////////////////////////////////////////////////////////////////////
    {
        deadScreen.gameObject.SetActive(false);
        Nickname.gameObject.SetActive(true);       

        playerColls[0].enabled = false;
        playerColls[1].enabled = true;
        playerColls[2].enabled = true;
        playerColls[3].enabled = true;
        playerColls[4].enabled = true;

        hand_Left.interactionLayers = 6 | 12;                          // 인터렉션 레이어를 바꾸는 방법으로 소유권 이전
        hand_Right.interactionLayers = 6 | 12;                         // 레이어 넘버 0 = 디폴트 ,6 = 인터렉터블, 12 = 쉴드


       // hand_L_Rend.material = hand_R;                                 // 아바타 왼손 머티리얼 
       // hand_R_Rend.material = hand_R;                                 // 아바타 오른손 머티리얼 
        at_L_Rend.material = hand_R;
        at_R_Rend.material = hand_R;

        if (DataManager.DM.currentMap == Map.TOY)
        {
            head_Rend.material = head_Mat;
            body_Rend.material = body_Mat;
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

        effects[2].SetActive(true);                   // 실드 효과 On
        yield return new WaitForSeconds(3.5f);           // 4초간격
        effects[2].SetActive(false);                  // 실드 효과 Off        
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
              Debug.Log("킬 성공" + hitter);
          }
      }*/

    private void OnTriggerEnter(Collider col)                                 // 리스폰 태그 시 메서드
    {
        if (col.CompareTag("RespawnBlue"))
        {
            if(!isAlive && DataManager.DM.currentTeam == Team.BLUE)
            {
                PV.RPC(nameof(RespawnPlayer), RpcTarget.AllBuffered);
                Debug.Log("블루팀 리스폰");
            }           
        }

         if (col.CompareTag("RespawnRed"))
        {
            if(!isAlive && DataManager.DM.currentTeam == Team.RED)
            {
                PV.RPC(nameof(RespawnPlayer), RpcTarget.AllBuffered);                
                Debug.Log("레드팀 리스폰");
            }         
        }
    }

    public void GrenadeDamage()                                              // 크리티컬(헤드샷) 데미지
    {
        StartCoroutine(ShowDamageScreen());
        if (isDeadLock)
        {
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, grenadePower);
        }
    }
    public void CriticalDamage()                                              // 크리티컬(헤드샷) 데미지
    {
        StartCoroutine(ShowDamageScreen());
        if (isDeadLock)
        {
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, attackPowerH);
            //PV.RPC(nameof(HeadShot), RpcTarget.AllBuffered);
        }
    }
    public void NormalDamage()                                                // 일반 데미지
    {
        StartCoroutine(ShowDamageScreen());
        if (isDeadLock)
        {
            PV.RPC(nameof(Damaged), RpcTarget.AllBuffered, attackPower);
            PV.RPC(nameof(BodyShot), RpcTarget.AllBuffered);
        }
    }
    public void Respawn()                                                     // 리스폰 메서드
    {
        PV.RPC(nameof(RespawnPlayer), RpcTarget.AllBuffered);
    }

    public IEnumerator ShowDamageScreen()                                      // 피격 스크린 보여주기
    {
        damageScreen.color = new Color(1, 0, 0, Random.Range(0.65f, 0.75f));
        yield return new WaitForSeconds(0.3f);
        damageScreen.color = Color.clear;
    }

    public IEnumerator ShowDeadEffect()                                       // 죽음 효과 보여주기
    {
        effects[0].SetActive(true);
        yield return new WaitForSeconds(3f);
        effects[0].SetActive(false);
    }

    public IEnumerator ShowRespawnEffect()                                     // 부활 효과 보여주기
    {
        effects[1].SetActive(true);
        yield return new WaitForSeconds(3f);
        effects[1].SetActive(false);
    }

    public IEnumerator DamagedDelay()                                          // 데미지시 이중피격 방지
    {
        while (delayTime >= 0)
        {
            delayTime -= Time.deltaTime;
            Debug.Log("콜라이더 OFF");
            playerColls[1].enabled = false;                                      // 머리 콜라이더
            playerColls[2].enabled = false;                                      // 몸통 콜라이더           

            isDamaged = true;
        }
        yield return new WaitForSeconds(0.17f);
        playerColls[1].enabled = true;                                      // 머리 콜라이더
        playerColls[2].enabled = true;                                      // 몸통 콜라이더

        isDamaged = false;
        Debug.Log("콜라이더 ON");
        if (HP.value <= 0)
        {
            playerColls[1].enabled = false;                                      // 머리 콜라이더
            playerColls[2].enabled = false;                                      // 몸통 콜라이더
        }
    }

    [PunRPC]
    public void Damaged(int pow)                                                  // 플레이어 데미지 받았을 때
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
                Debug.Log("남은 HP : " + HP.value.ToString() + "%");

                if (HP.value <= 30)
                {
                    threeScreen.gameObject.SetActive(true);
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
                    Debug.Log("킬 성공");
                }
            }
        }
    }

    [PunRPC]
    public void PlayerDeadT(int team)
    {
        if (team == 0)                    // 레드팀이 블루팀을 죽였을 때  == 블루팀 사망
        {
            GunShootManager.GSM.score_RedKill++;
            Debug.Log("레드에게 블루가 죽었음");
        }
        else                              // 블루팀이 레드팀을 죽였을 때  == 레드팀 사망
        {
            GunShootManager.GSM.score_BlueKill++;
            Debug.Log("블루에게 레드가 죽었음");
        }
    }

    [PunRPC]
    public void PlayerDeadW(int team)
    {
        if (team == 0)                   // 레드팀이 블루팀을 죽였을 때  == 블루팀 사망
        {
            WesternManager.WM.score_RedKill++;
            Debug.Log("레드에게 블루가 죽었음");
        }
        else                             // 블루팀이 레드팀을 죽였을 때  == 레드팀 사망
        {
            WesternManager.WM.score_BlueKill++;
            Debug.Log("블루에게 레드가 죽었음");
        }
    }

   /* [PunRPC]
    public void HeadShot() // 머리를 맞았을 때 머리를 맞춘 플레이어를 기준으로 효과음 들림
    {
        if (PV.IsMine)
        {
            AudioManager.AM.PlaySE("HeadShot");
        }
    }*/

    [PunRPC]
    public void BodyShot() // 몸을 맞았을 때 몸을 맞춘 플레이어를 기준으로 효과음 들림
    {
        if (!PV.IsMine)
        {
            AudioManager.AM.PlaySE("Hit");
        }
    }

    [PunRPC]
    public void DeadPlayer()                                                     // 플레이어 죽었을때
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
    public void RespawnPlayer()                                                 //  플레이어 리스폰할 때
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
