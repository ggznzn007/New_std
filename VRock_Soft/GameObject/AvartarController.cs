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
    public static AvartarController ATC;
    public Image HP;
    public Text Nickname;
    private PhotonView PV;
    public Renderer head;
    public Renderer body;
    public Renderer hair;
    public Renderer eye_L;
    public Renderer eye_R;
    public Renderer glove_R;
    public Renderer hand_R;
    public Renderer[] CurRends;
    public Material DeadRend;
    public Material[] RespawnRends;
   
    /* public GameObject ConHand_L;
     public GameObject ConHand_R;
     public GameObject Hand_L;
     public GameObject Hand_R;
     public GameObject deathHead;
     public GameObject deathBody;
     public GameObject deathHand_L;
     public GameObject deathHand_R;*/

    public GameObject gunPrefab;
    /*  public GameObject Controll_L;
      public GameObject Controll_R;*/

    public List<Collider> playerColls;
    public bool isDead;
    private void Awake()
    {
        ATC = this;
        PV = GetComponent<PhotonView>();
        isDead = false;
        
       /* playerRends = GetComponentsInChildren<MeshRenderer>();
        PlayerRends = GetComponentsInChildren<Renderer>();
        DeadRends = GetComponentsInChildren<Material>();
        CurRends = GetComponentsInChildren<Renderer>();

        deathHead.SetActive(false);
        deathBody.SetActive(false);
        deathHand_L.SetActive(false);
        deathHand_R.SetActive(false);
        Hand_R.SetActive(true);
        Hand_L.SetActive(true);
        playerColls = new List<Collision>();
        materials = new List<Material>();
         boxRends = GetComponentsInChildren<MeshRenderer>();
        Controll_R.GetComponentsInChildren<SkinnedMeshRenderer>();*/
    }

    void Start()
    {
       // DeadRend = GetComponentInChildren<Material>();
        head = GetComponentInChildren<Renderer>();
        body = GetComponentInChildren<Renderer>();
        hair = GetComponentInChildren<Renderer>();
        eye_L = GetComponentInChildren<Renderer>();
        eye_R = GetComponentInChildren<Renderer>();
        glove_R = GetComponentInChildren<Renderer>();
        hand_R = GetComponentInChildren<Renderer>();

      //  RespawnRends = GetComponentsInChildren<Material>();
        if (PN.IsConnected)
        {
            Nickname.text = PV.IsMine ? PN.NickName : PV.Owner.NickName;
            Nickname.color = PV.IsMine ? Color.white : Color.red;
            HP.fillAmount = 1f;
           
        }
    }


    void Update()
    {
        if (!PV.IsMine) return;
    }

    public void HitPlayer()
    {
        HP.fillAmount -= 0.6f;
        if (HP.fillAmount <= 0)
        {
            isDead = true;
            Nickname.gameObject.SetActive(false);
            HP.gameObject.SetActive(false);
            //playerColl.enabled = false;            
            PV.RPC("DestroyPlayer", RpcTarget.AllBuffered);
            GunManager.gunManager.DestroyGun_Delay();
            Debug.Log("킬 성공");
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Respawn"))
        {
            PlayerRespawn();
            Debug.Log("리스폰");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            HitPlayer();
            Debug.Log("총알에 맞음");
        }

    }



    IEnumerator Damaged()
    {
        // SetPlayerInVisible(false);

        /*Controll_R.modelPrefab = deathHand_R.transform;
        Controll_L.modelPrefab = deathHand_L.transform;
        gunPrefab.SetActive(false);
        deathHead.SetActive(true);
        deathBody.SetActive(true);
        deathHand_L.SetActive(true);
        deathHand_R.SetActive(true);*/

        yield return new WaitForSeconds(5f);
        /* Controll_R.gameObject.SetActive(true);
         Controll_L.modelPrefab = Hand_L.transform;
         deathHead.SetActive(false);
         deathBody.SetActive(false);
         deathHand_L.SetActive(false);
         deathHand_R.SetActive(false);
         Hand_R.SetActive(true);
         Hand_L.SetActive(true);*/
        //  SetPlayerVisible(true);
        // SetPlayerInVisible(true);
        HP.gameObject.SetActive(true);
        Nickname.gameObject.SetActive(true);
        HP.fillAmount = 1f;
        gunPrefab.SetActive(true);

    }

    public void PlayerDead()
    {
        head.materials[0]= DeadRend;
        head.materials[1]= DeadRend;
        body.materials[0] = DeadRend;
        body.materials[1] = DeadRend;
        hair.material = DeadRend;
        eye_L.material = DeadRend;
        eye_R.material = DeadRend;
        glove_R.material = DeadRend;
        hand_R.material = DeadRend;
    }

    public void PlayerRespawn()
    {
        head.materials[0] = RespawnRends[0];
        head.materials[1] = RespawnRends[1];
        body.materials[0] = RespawnRends[2];
        body.materials[1] = RespawnRends[3];
        hair.material = RespawnRends[4]; ;
        eye_L.material = RespawnRends[5];
        eye_R.material = RespawnRends[5];
        glove_R.material = RespawnRends[6];
        hand_R.material = RespawnRends[7];

        HP.gameObject.SetActive(true);
        Nickname.gameObject.SetActive(true);
        HP.fillAmount = 1f;
        isDead = false;        
    }



    /*void SetPlayerInVisible(bool isVisible)
    {
        for (int i = 0; i < playerRends.Length; i++)
        {
           
           // playerColl.enabled = isVisible;
        }

    }*/




    [PunRPC]
    public void DestroyPlayer()
    {
        // Destroy(gameObject);
        // StartCoroutine(Damaged());
        PlayerDead();
    }

    [PunRPC]
    public void RespawnPlayer()
    {
        // Destroy(gameObject);
        // StartCoroutine(Damaged());
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
}
