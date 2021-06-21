using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LoginAudioManager : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip[] voiceAudio;
    
    public void PlayAudio(int n)
    {        
        AudioSource.clip = voiceAudio[n];
        AudioSource.Play();        
    }
}
