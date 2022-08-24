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
    public Image HP;                                                         // �÷��̾� HP
    public Text Nickname;                                                    // �÷��̾� �г���
    private PhotonView PV;                                                   // �����
    public MeshRenderer head_Rend;                                           // �ƹ�Ÿ �Ӹ�     ������
    public MeshRenderer body_Rend;                                           // �ƹ�Ÿ ��      
    public MeshRenderer hair_Rend;                                           // �ƹ�Ÿ �Ӹ���   
    public MeshRenderer eye_L_Rend;                                          // �ƹ�Ÿ ���ʴ�   
    public MeshRenderer eye_R_Rend;                                          // �ƹ�Ÿ �����ʴ�  
    public SkinnedMeshRenderer glove_R_Rend;                                 // �ƹ�Ÿ �尩     
    public SkinnedMeshRenderer hand_R_Rend;                                  // �ƹ�Ÿ ������   
    public Material[] head;                                                  // �ƹ�Ÿ �Ӹ�     ��Ƽ����
    public Material[] body;                                                  // �ƹ�Ÿ ��       
    public Material hair;                                                    // �ƹ�Ÿ �Ӹ���
    public Material eye_L;                                                   // �ƹ�Ÿ ���ʴ�
    public Material eye_R;                                                   // �ƹ�Ÿ �����ʴ�
    public Material glove_R;                                                 // �ƹ�Ÿ �尩
    public Material hand_R;                                                  // �ƹ�Ÿ ������    
    public Material DeadRend;                                                // �÷��̾� ����   ��Ƽ����    
    public List<Collider> playerColls;                                       // �÷��̾� �ݶ��̴�
    public bool isDead;                                                      // �÷��̾� ���� �Ǵܿ���

    private void Awake()
    {
        ATC = this;
        PV = GetComponent<PhotonView>();             
    }

    void Start()
    {        
        Nickname.text = PV.IsMine ? PN.NickName : PV.Owner.NickName;         // �÷��̾� ����䰡 �ڽ��̸� �г����� �ƴϸ� ���� �г���
        Nickname.color = PV.IsMine ? Color.white : Color.red;                // �÷��̾� ����䰡 �ڽ��̸� ��� �ƴϸ� ������

        HP.fillAmount = 1f;                                                  // �÷��̾� HP �ʱ�ȭ
        isDead = false;                                                      // �÷��̾� ���� �ʱ�ȭ

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
            Debug.Log("������");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet")&& !isDead)
        {
            HitPlayer();
            Debug.Log("�Ѿ˿� ����");
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
            Debug.Log("ų ����");
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
