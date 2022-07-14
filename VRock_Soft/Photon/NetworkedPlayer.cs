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

public class NetworkedPlayer : MonoBehaviourPunCallbacks
{
   // public static NetworkedPlayer NetPlayer;
    public GameObject LocalXRRigGameObject;
    public GameObject AvatarHead;
    public GameObject AvatarBody;
    public GameObject AvatarHand_L;
    public GameObject AvatarHand_R;
           
    private PhotonView PV;
    

    public void Awake()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            LocalXRRigGameObject.SetActive(true);
            SetLayerRecursively(go: AvatarHead, 8);
            SetLayerRecursively(go: AvatarBody, 9);
            SetLayerRecursively(go: AvatarHand_L, 10);
            SetLayerRecursively(go: AvatarHand_R, 10);
        }
        else
        {
            LocalXRRigGameObject.SetActive(false);
            SetLayerRecursively(go: AvatarHead, 0);
            SetLayerRecursively(go: AvatarBody, 0);
            SetLayerRecursively(go: AvatarHand_L, 0);
            SetLayerRecursively(go: AvatarHand_R, 0);
        }

    }

    private void Start()
    {
        
    }
       

    private void Update()
    {
        if (!PV.IsMine)
        { return; }        
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
