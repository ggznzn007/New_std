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

public class AnimationW : MonoBehaviourPunCallbacks
{
    public AudioSource aniAudio;
    public AudioClip aniClip; 

    void RunAudio()
    {
        aniAudio.PlayOneShot(aniClip);        
    }

    private void Start()
    {
       aniAudio = GetComponent<AudioSource>();
        //anim = GetComponent<Animation>();
    }

}
