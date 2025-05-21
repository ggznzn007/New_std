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

    public void Playsound(AudioClip clip) // �ش� �Ҹ� ��� �޼ҵ�
    {
        sound.PlayOneShot(clip);
    }
}
