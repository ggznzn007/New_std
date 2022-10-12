using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AM;

    [Header("배경음 스피커")]
    public AudioSource bgmPlayer;

    [Header("배경음 오디오소스")]
    public AudioClip[] bgm;


    [Header("효과음 스피커")]
    public AudioSource[] effectPlayer;

    [Header("효과음 오디오소스")]
    public AudioClip[] effectClip;

    public enum Effect
    {
        PlayerDamaged,
        PlayerDead,
        ReSpawn,
        PlayerKill,
        BulletImpact,
        GAMESTART,
        START,
        Three,
        Two,
        One,
        GAMEOVER,
        END,
        PlayerHit,
        HeadShot
    };

    int effectCursor;

    private void Awake()
    {
        AM = this;
    }

    public void EffectPlay(Effect type)
    {
        switch (type)
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
            case Effect.BulletImpact:
                effectPlayer[effectCursor].clip = effectClip[4];
                break;
            case Effect.GAMESTART:
                effectPlayer[effectCursor].clip = effectClip[5];
                break;
            case Effect.START:
                effectPlayer[effectCursor].clip = effectClip[6];
                break;
            case Effect.Three:
                effectPlayer[effectCursor].clip = effectClip[7];
                break;
            case Effect.Two:
                effectPlayer[effectCursor].clip = effectClip[8];
                break;
            case Effect.One:
                effectPlayer[effectCursor].clip = effectClip[9];
                break;
            case Effect.GAMEOVER:
                effectPlayer[effectCursor].clip = effectClip[10];
                break;
            case Effect.END:
                effectPlayer[effectCursor].clip = effectClip[11];
                break;
            case Effect.PlayerHit:
                effectPlayer[effectCursor].clip = effectClip[12];
                break;
            case Effect.HeadShot:
                effectPlayer[effectCursor].clip = effectClip[13];
                break;
        }

        effectPlayer[effectCursor].Play();
        effectCursor = (effectCursor + 1) % effectPlayer.Length;
    }

    private void Update()
    {
        if (!bgmPlayer.isPlaying)
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
