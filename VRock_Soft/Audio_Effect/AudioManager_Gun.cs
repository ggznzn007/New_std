using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Gun : MonoBehaviour
{

    [Header("배경음 재생기")]
    public AudioSource bgmPlayer;

    [Header("배경음 오디오소스")]
    public AudioClip[] bgm;

    private void Update()
    {
        if (!bgmPlayer.isPlaying)
        {
            RandomPlay();
        }
    }

    public void RandomPlay()
    {
        bgmPlayer.clip = bgm[Random.Range(0, bgm.Length)];
        bgmPlayer.Play();
    }
}
