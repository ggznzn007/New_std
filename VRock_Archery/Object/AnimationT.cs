using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.XR;
using Unity.XR.PXR;
public class AnimationT : MonoBehaviourPunCallbacks
{
    public AudioSource aniAudio;
    public AudioClip aniClip;
    public void Emp_Blue()
    {
        if(PN.IsMasterClient)
        {
           // GunShootManager.GSM.Emp_Blue();
        }
    }

    public void Emp_Red()
    {
        if (PN.IsMasterClient)
        {
           // GunShootManager.GSM.Emp_Red();
        }
    }

    private void Start()
    {
        aniAudio = GetComponent<AudioSource>();        
    }

    public void MoveAudio()
    {
        aniAudio.PlayOneShot(aniClip);
    }
}
