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
using static ObjectPooler;
public class PlayerNetworkSetup : MonoBehaviourPunCallbacks, IPunObservable
{
    public static PlayerNetworkSetup NetPlayer;
    public GameObject LocalXRRigGameObject;
    public GameObject AvatarHead;
    public GameObject AvatarBody;
   // public Rigidbody RB;
    public Image HP;
    public Text Nickname;
    private PhotonView PV;
    /* public GameObject AvatarHand_L;
     public GameObject AvatarHand_R;
 */

    public void Awake()
    {
       
        NetPlayer = this;
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            LocalXRRigGameObject.SetActive(true);
            SetLayerRecursively(go: AvatarHead, 8);
            SetLayerRecursively(go: AvatarBody, 9);
            /*SetLayerRecursively(go: AvatarHand_L, 10);
            SetLayerRecursively(go: AvatarHand_R, 10);*/
        }
        else
        {
            LocalXRRigGameObject.SetActive(false);
            SetLayerRecursively(go: AvatarHead, 0);
            SetLayerRecursively(go: AvatarBody, 0);
            /* SetLayerRecursively(go: AvatarHand_L, 0);
             SetLayerRecursively(go: AvatarHand_R, 0);*/
        }

    }

    private void Start()
    {
        if(PN.IsConnected)
        {
            Nickname.text = PV.IsMine ? PN.NickName : PV.Owner.NickName;
            Nickname.color = PV.IsMine ? Color.white : Color.red;
        }
           

    }


    private void Update()
    {
        if (!PV.IsMine) return;
    }

    void SetLayerRecursively(GameObject go, int layerNum)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNum;
        }
    }


    public void HitPlayer()
    {
        HP.fillAmount -= 0.1f;
        if (HP.fillAmount <= 0)
        {
            ReadySceneManager.readySceneManager.localPlayer.SetActive(true);
            ReadySceneManager.readySceneManager.mainBG.SetActive(false);
            ReadySceneManager.readySceneManager.startUI.SetActive(false);
            PV.RPC("DestroyPlayer", RpcTarget.AllBuffered);
            Debug.Log("적에게 명중");
        }
    }
    [PunRPC]
    public void DestroyPlayer()
    {
        Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(HP.fillAmount);
        }
        else
        {
            HP.fillAmount = (float)stream.ReceiveNext();
        }
    }
}
