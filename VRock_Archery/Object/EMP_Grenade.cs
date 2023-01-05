using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using static AudioManager;

public class EMP_Grenade : MonoBehaviourPunCallbacks
{
    public static EMP_Grenade EG;
    
    PhotonView PV;

    private void Awake()
    {
        EG = this;
        PV = GetComponent<PhotonView>();
    }
   
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    

}
