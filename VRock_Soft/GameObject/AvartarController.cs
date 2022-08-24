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
    public Image HP;                                                         // 플레이어 HP
    public Text Nickname;                                                    // 플레이어 닉네임
    private PhotonView PV;                                                   // 포톤뷰
    public MeshRenderer head_Rend;                                           // 아바타 머리     렌더러
    public MeshRenderer body_Rend;                                           // 아바타 몸      
    public MeshRenderer hair_Rend;                                           // 아바타 머리색   
    public MeshRenderer eye_L_Rend;                                          // 아바타 왼쪽눈   
    public MeshRenderer eye_R_Rend;                                          // 아바타 오른쪽눈  
    public SkinnedMeshRenderer glove_R_Rend;                                 // 아바타 장갑     
    public SkinnedMeshRenderer hand_R_Rend;                                  // 아바타 오른손   
    public Material[] head;                                                  // 아바타 머리     머티리얼
    public Material[] body;                                                  // 아바타 몸       
    public Material hair;                                                    // 아바타 머리색
    public Material eye_L;                                                   // 아바타 왼쪽눈
    public Material eye_R;                                                   // 아바타 오른쪽눈
    public Material glove_R;                                                 // 아바타 장갑
    public Material hand_R;                                                  // 아바타 오른손    
    public Material DeadRend;                                                // 플레이어 죽음   머티리얼    
    public List<Collider> playerColls;                                       // 플레이어 콜라이더
    public bool isDead;                                                      // 플레이어 죽음 판단여부

    private void Awake()
    {
        ATC = this;
        PV = GetComponent<PhotonView>();             
    }

    void Start()
    {        
        Nickname.text = PV.IsMine ? PN.NickName : PV.Owner.NickName;         // 플레이어 포톤뷰가 자신이면 닉네임을 아니면 오너 닉네임
        Nickname.color = PV.IsMine ? Color.white : Color.red;                // 플레이어 포톤뷰가 자신이면 흰색 아니면 빨간색

        HP.fillAmount = 1f;                                                  // 플레이어 HP 초기화
        isDead = false;                                                      // 플레이어 죽음 초기화

        head_Rend.materials = head;                                      
        body_Rend.materials = body;
        hair_Rend.material = hair;
        eye_L_Rend.material = eye_L;
        eye_R_Rend.material = eye_R;
        glove_R_Rend.material = glove_R;
        hand_R_Rend.material = hand_R;

        // DeadRend = GetComponentInChildren<Material>();
        /* head = GetComponentInChildren<MeshRenderer>().materials;
         body = GetComponentInChildren<MeshRenderer>().materials;
         hair = GetComponentInChildren<MeshRenderer>().material;
         eye_L = GetComponentInChildren<MeshRenderer>().material;
         eye_R = GetComponentInChildren<MeshRenderer>().material;
         glove_R = GetComponentInChildren<SkinnedMeshRenderer>().material;
         hand_R = GetComponentInChildren<SkinnedMeshRenderer>().material;*/
    }


    void Update()
    {
        if (!PV.IsMine) return;        
    }    

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Respawn")&& isDead)
        {
            PlayerRespawn();
            PV.RPC("RespawnPlayer", RpcTarget.AllBuffered);
            Debug.Log("리스폰");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet")&& !isDead)
        {
            HitPlayer();
            Debug.Log("총알에 맞음");
        }

    }
  
    public void HitPlayer()
    {
        HP.fillAmount -= 0.5f;
        if (HP.fillAmount <= 0)
        {           
            PV.RPC("DeadPlayer", RpcTarget.AllBuffered);
           if(PV.IsMine)
            {
                GunManager.gunManager.DestroyGun_Delay();
            }
            Debug.Log("킬 성공");
        }
    }
    public void PlayerDead()
    {
        isDead = true;
        Nickname.gameObject.SetActive(false);
        HP.gameObject.SetActive(false);
        HP.fillAmount = 0f;

        head_Rend.material = DeadRend;                 
        body_Rend.material = DeadRend;       
        hair_Rend.material = DeadRend;
        eye_L_Rend.material = DeadRend;
        eye_R_Rend.material = DeadRend;
        glove_R_Rend.material = DeadRend;
        hand_R_Rend.material = DeadRend;
    }

    public void PlayerRespawn()
    {
        isDead = false;        
        HP.gameObject.SetActive(true);
        Nickname.gameObject.SetActive(true);
        HP.fillAmount = 1f;

        head_Rend.materials = head;
        body_Rend.materials = body;
        hair_Rend.material = hair;
        eye_L_Rend.material = eye_L;
        eye_R_Rend.material = eye_R;
        glove_R_Rend.material = glove_R;
        hand_R_Rend.material = hand_R;
    }
    

    [PunRPC]
    public void DeadPlayer()
    {       
        PlayerDead();
    }

    [PunRPC]
    public void RespawnPlayer()
    {        
        PlayerRespawn();
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(HP.fillAmount);

        }
        else
        {
            HP.fillAmount = (float)stream.ReceiveNext();

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
