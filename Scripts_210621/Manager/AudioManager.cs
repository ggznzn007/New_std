using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Enemy")]
    public AudioClip[] enemyImpactSFX;
    public AudioClip[] enemyDeathSFX;

    [Header("Weapon")]
    public AudioClip dryFire;

    [Header("Player")]
    public AudioClip[] playerImpactSFX;

    [Header("UI")]
    public AudioClip buttonHover;
    public AudioClip buttonClick;

    [Header("Prefabs")]
    public GameObject audioSourcePrefab;        //Prefab of audio source to be spawned in for temp sounds like explosions, impacts.

    [Header("Global Audio Sources")]
    public AudioSource uiAudioSource;

    //Instance
    public static AudioManager inst;
    void Awake () { inst = this; }

    void Start ()
    {
        if(PlayerPrefs.HasKey("Volume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("Volume");
        }
        else
        {
            PlayerPrefs.SetFloat("Volume", AudioListener.volume);
        }
    }

    public void Play (AudioSource source, AudioClip clip)
    {
        source.pitch = Random.Range(0.9f, 1.1f);
        source.PlayOneShot(clip);
    }

    public void Play (AudioSource source, AudioClip clip, bool changePitch)
    {
        if(changePitch)
            source.pitch = Random.Range(0.9f, 1.1f);
        else
            source.pitch = 1.0f;

        source.PlayOneShot(clip);
    }
     
}
