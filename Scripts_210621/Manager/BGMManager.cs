using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;
}
public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    [Header("Set Sounds Sources")]
    [SerializeField] Sound[] bgmSounds;//배경음
    [SerializeField] Sound[] sfxSounds;//버튼효과음

    [Header("BGM Player")]
    [SerializeField] AudioSource bgmPlayer;

    [Header("Sound Player")]
    [SerializeField] AudioSource[] sfxPlayer;

    void Start()
    {
        instance = this;
        PlayRandomBGM();

    }

    public void PlayRandomBGM()
    {
        int random = Random.Range(0, 5);
        bgmPlayer.clip = bgmSounds[random].clip;
        bgmPlayer.Play();
    }

    public void PlaySE(string _soundName)
    {
        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if (_soundName == sfxSounds[i].soundName)
            {
                for (int x = 0; x < sfxPlayer.Length; x++)
                {
                    if (!sfxPlayer[x].isPlaying)
                    {
                        sfxPlayer[x].clip = sfxSounds[i].clip;
                        sfxPlayer[x].Play();
                        break;
                    }
                }
                break;
            }
        }
        return;
    }
}
