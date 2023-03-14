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
using UnityEditor;
public class IceBlock : XRGrabInteractable
{
    public static IceBlock IB;
    public GameObject myMesh;
    public GameObject iceEX;
    private PhotonView PV;
    
    void Start()
    {
        IB = this;
        PV = GetComponent<PhotonView>();
        myMesh.SetActive(true);
        iceEX.SetActive(false);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Icicle")|| collision.collider.CompareTag("FloorBox"))
        {
            PV.RPC(nameof(Disappear), RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void Disappear()
    {       
        StartCoroutine(EXOnOff());
    }

    public IEnumerator EXOnOff()
    {
        myMesh.SetActive(false);
        iceEX.SetActive(true);
        //PN.Instantiate(DestroyEX.name, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    } 
}
