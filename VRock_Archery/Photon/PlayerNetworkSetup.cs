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

public class PlayerNetworkSetup : MonoBehaviourPunCallbacks
{   
    public GameObject LocalXRRigGameObject;
    public GameObject AvatarHead;
    public GameObject AvatarBody; 
    public GameObject AvatarHand_L; 
    public GameObject AvatarHand_R;
    public MeshRenderer at_L;
    public MeshRenderer at_R;
    //public MeshRenderer at_ShieldR;
    //public Collider at_ShieldColl;
   // private bool triggerBtnR;

    private void Start()
    {        
        if (photonView.IsMine)                                 // 로컬 플레이어 
        {
            LocalXRRigGameObject.SetActive(true);
            SetLayerRecursively(go: AvatarHead, 8);
            SetLayerRecursively(go: AvatarBody, 9);
            //SetLayerRecursively(go: AvatarHand_L, 0);
            //SetLayerRecursively(go: AvatarHand_R, 0);
        }
        else                                                   // 리모트 플레이어
        {
            LocalXRRigGameObject.SetActive(false);
            SetLayerRecursively(go: AvatarHead, 0);
            SetLayerRecursively(go: AvatarBody, 0);
            //SetLayerRecursively(go: AvatarHand_L, 0);
            //SetLayerRecursively(go: AvatarHand_R, 0);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        /*if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out triggerBtnR) && triggerBtnR)
        {
            at_ShieldR.forceRenderingOff = false;
            at_ShieldColl.enabled = true;
        }
        else
        {
            at_ShieldR.forceRenderingOff = true;
            at_ShieldColl.enabled = false;
        }*/
    }

    void SetLayerRecursively(GameObject go, int layerNum)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNum;
        }
    }

    public void Hide_L()
    {
        if (photonView.IsMine)
            photonView.RPC(nameof(HideL), RpcTarget.AllBuffered);
    }

    public void Show_L()
    {
        if (photonView.IsMine)
            photonView.RPC(nameof(ShowL), RpcTarget.AllBuffered);
    }

    public void Hide_R()
    {
        if (photonView.IsMine)
            photonView.RPC(nameof(HideR), RpcTarget.AllBuffered);
    }
    public void Show_R()
    {
        if (photonView.IsMine)
            photonView.RPC(nameof(ShowR), RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void HideL()
    {
        at_L.forceRenderingOff = true;
    }

    [PunRPC]
    public void ShowL()
    {
        at_L.forceRenderingOff = false;
    }

    [PunRPC]
    public void HideR()
    {
        at_R.forceRenderingOff = true;
    }

    [PunRPC]
    public void ShowR()
    {
        at_R.forceRenderingOff = false;
    }
}
