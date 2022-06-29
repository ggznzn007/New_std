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

public class NetworkedPlayer : MonoBehaviourPunCallbacks
{
    public GameObject LocalXRRigGameObject;
    public GameObject AvatarHead;
    public GameObject AvatarBody;   
    
    private PhotonView PV;
    
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        if(PV.IsMine)
        {
            LocalXRRigGameObject.SetActive(true);
            SetLayerRecursively(AvatarHead, 8);
            SetLayerRecursively(AvatarHead, 9);
        }
        else
        {
            LocalXRRigGameObject.SetActive(false);
            SetLayerRecursively(AvatarHead, 0);
            SetLayerRecursively(AvatarHead, 0);
        }
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
