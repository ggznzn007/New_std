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

public class DestroyParticle : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    [System.Obsolete]
    private IEnumerator Start()
    {
        PV = GetComponent<PhotonView>();
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().duration);
        Destroy(PV.gameObject);
    }   
}
