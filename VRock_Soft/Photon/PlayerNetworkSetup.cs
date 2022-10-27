using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Linq;
using System.IO;
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
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class PlayerNetworkSetup : MonoBehaviourPunCallbacks//, IPunObservable
{
   // public static PlayerNetworkSetup NetPlayer;
    public GameObject LocalXRRigGameObject;
    public GameObject AvatarHead;
    public GameObject AvatarBody; 
    public GameObject AvatarHand_L; 
    public GameObject AvatarHand_R;

    
    public void Awake()
    {      
       // NetPlayer = this;
    }

    private void Start()
    {        
        if (photonView.IsMine)                                 // 로컬 플레이어 
        {
            LocalXRRigGameObject.SetActive(true);
            SetLayerRecursively(go: AvatarHead, 8);
            SetLayerRecursively(go: AvatarBody, 9);
            SetLayerRecursively(go: AvatarHand_L, 10);
            SetLayerRecursively(go: AvatarHand_R, 10);
        }

        else                                                   // 리모트 플레이어
        {
            LocalXRRigGameObject.SetActive(false);
            SetLayerRecursively(go: AvatarHead, 0);
            SetLayerRecursively(go: AvatarBody, 0);
            SetLayerRecursively(go: AvatarHand_L, 0);
            SetLayerRecursively(go: AvatarHand_R, 0);
        }
    }
    private void Update()
    {
        if (!photonView.IsMine) return;      
    }

    void SetLayerRecursively(GameObject go, int layerNum)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNum;
        }
    }   

    
}
