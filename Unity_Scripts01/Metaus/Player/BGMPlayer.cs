using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;
}
public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer instance;

    [Header("Set Sounds Sources")]
    [SerializeField] Sound[] bgmSounds; // ����� ���    

    [Header("BGM Player")]
    [SerializeField] AudioSource bgmPlayer; // ����� �÷��̾�    

    public void Start()
    {
        instance = this;
        PlayRandomBGM();
    }

    public void PlayRandomBGM()
    {
        int random = Random.Range(0, 3);
        bgmPlayer.clip = bgmSounds[random].clip;
        bgmPlayer.Play();
    }       
}
