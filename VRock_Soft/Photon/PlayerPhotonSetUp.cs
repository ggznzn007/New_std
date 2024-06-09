using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using TMPro;
public class PlayerPhotonSetUp : MonoBehaviourPunCallbacks
{
   /* public static GameObject LocalPlayerInstance;
    
    public GameObject local_XR_Player;

    public GameObject avatarHead;
    public GameObject avatarBody;

    private void Awake()
    {
        if(photonView.IsMine)
        {
            PlayerPhotonSetUp.LocalPlayerInstance = this.gameObject;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        if(photonView.IsMine)
        {
           
            // 로컬
            local_XR_Player.SetActive(true);

           SetLayerRecursively(avatarHead, 6);
          SetLayerRecursively(avatarBody, 7);
        }
        else
        {
            // 원격
            local_XR_Player.SetActive(false);

          SetLayerRecursively(avatarHead, 0);
          SetLayerRecursively(avatarBody, 0);
        }
        
    }
  
 
    void SetLayerRecursively(GameObject go, int layerNum)
    {
        if (go == null) return;
        foreach(Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNum;
        }
    }*/
}
