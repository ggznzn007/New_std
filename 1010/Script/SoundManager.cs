using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    AudioSource sound;

    [Header("AudioClip")]
    public AudioClip blockClick;
    public AudioClip blockDrop;
    public AudioClip blockClear;
    public AudioClip blockSet;
    public AudioClip gameOver;

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
