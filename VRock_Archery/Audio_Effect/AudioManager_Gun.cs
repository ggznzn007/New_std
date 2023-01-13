using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Gun : MonoBehaviour
{

    [Header("����� �����")]
    public AudioSource bgmPlayer;

    [Header("����� ������ҽ�")]
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
