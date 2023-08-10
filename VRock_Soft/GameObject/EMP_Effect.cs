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

public class EMP_Effect : MonoBehaviour
{
    public CapsuleCollider coll;
    
    private void Awake()
    {
        coll = GetComponent<CapsuleCollider>();        
    }
    
    private void Start()
    {
        StartCoroutine(CollOnOff());       
    }

    public IEnumerator CollOnOff()
    {
        coll.enabled = true;
        yield return new WaitForSeconds(0.004f);
        coll.enabled = false;        
    }

    /*[System.Obsolete]
    public IEnumerator DestroyEX()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().duration);
        Destroy(gameObject);
    }*/
}
