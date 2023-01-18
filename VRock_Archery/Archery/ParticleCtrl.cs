using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ParticleCtrl : MonoBehaviourPun
{
    private ParticleSystem _particleSystem;
    //public PhotonView PV;

    private void Awake()
    {        
        _particleSystem = GetComponent<ParticleSystem>();
    }
    private void OnTriggerExit(Collider coll)
    {
        if(coll.CompareTag("RightHand")|| coll.CompareTag("LeftHand"))
        {
            _particleSystem.Stop();
        }
    }
}
