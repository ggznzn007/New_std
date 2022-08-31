using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AM; 

    [Header("����� �����")]
    public AudioSource bgmPlayer;
        
    [Header("����� ������ҽ�")]
    public AudioClip[] bgm;
    

    [Header("ȿ���� �����")]
    public AudioSource[] effectPlayer;

    [Header("ȿ���� ������ҽ�")]
    public AudioClip[] effectClip;

    public enum Effect { PlayerDamaged, PlayerDead, ReSpawn,PlayerKill};

    int effectCursor;

    private void Awake()
    {
        AM = this;        
    }

    public void EffectPlay(Effect type)
    {
        switch(type)
        {
            case Effect.PlayerDamaged:
                effectPlayer[effectCursor].clip = effectClip[0];
                break;
            case Effect.PlayerDead:
                effectPlayer[effectCursor].clip = effectClip[1];
                break;
            case Effect.ReSpawn:
                effectPlayer[effectCursor].clip = effectClip[2];
                break;
            case Effect.PlayerKill:
                effectPlayer[effectCursor].clip = effectClip[3];
                break;
        }

        effectPlayer[effectCursor].Play();
        effectCursor = (effectCursor + 1) % effectPlayer.Length;
    }

    private void Update()
    {
        if(!bgmPlayer.isPlaying)
        {
            RandomPlay();
        } 
       
       /* if(gunBgmPlayer.isPlaying)
        {
            RandomPlayStop();
        }*/
    }

    public void RandomPlay()
    {
        bgmPlayer.clip = bgm[Random.Range(0, bgm.Length)];
        bgmPlayer.Play();
    }
   /* public void RandomPlayStop()
    {
       // bgmPlayer.clip = bgm[Random.Range(0, bgm.Length)];
        bgmPlayer.Stop();
    }

    public void NormalPlay()
    {
        gunBgmPlayer.clip = gunBgm;
        gunBgmPlayer.Play();
    }*/
}
