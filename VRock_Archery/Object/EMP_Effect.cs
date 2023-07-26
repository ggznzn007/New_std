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

public class EMP_Effect : MonoBehaviourPun
{
    public Collider coll;    
    //public PhotonView PV;

    private void Awake()
    {
        coll = GetComponent<Collider>();   
        //PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        StartCoroutine(CollOnOff());
    }

    public IEnumerator CollOnOff()
    {
        coll.enabled = true;
        //yield return new WaitForSeconds(0.005f);
        yield return new WaitForSeconds(0.03f);
        coll.enabled = false;        
    }   
    /*[System.Obsolete]
    private IEnumerator Start()
    {
        StartCoroutine(CollOnOff());
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().duration);
        Destroy(gameObject);
    }*/
}
