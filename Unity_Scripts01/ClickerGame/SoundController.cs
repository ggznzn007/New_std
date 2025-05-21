using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;

    AudioSource sound;

    [Header("AudioClip")]
    public AudioClip upgradeClick;
    public AudioClip playerHit;
    public AudioClip heroineClick;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        sound = GetComponent<AudioSource>();
    }

    public void Playsound(AudioClip clip) // 해당 소리 재생 메소드
    {
        sound.PlayOneShot(clip);
    }
}
