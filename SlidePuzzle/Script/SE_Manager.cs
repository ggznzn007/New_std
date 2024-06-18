using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE_Manager : MonoBehaviour
{
    public static SE_Manager instance;
    AudioSource sound;

    [Header("AudioClip")]
    public AudioClip shuffle;
    public AudioClip btn;
    public AudioClip gameClear;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        sound = GetComponent<AudioSource>();
    }

    public void Playsound(AudioClip clip)
    {
        sound.PlayOneShot(clip); 
    }
}
