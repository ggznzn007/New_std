using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    //public float volume;   
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager AM;

    [Header("배경음 소스파일")]
    [SerializeField] Sound[] bgm = null;

    [Header("효과음 소스파일")]
    [SerializeField] Sound[] soundE = null;

    [Header("배경음 스피커")]
    [SerializeField] AudioSource bgmSpeaker = null;

    [Header("효과음 스피커")]
    [SerializeField] AudioSource[] seSpeaker = null;

    [SerializeField] private Slider bgmVolSlider;
    [SerializeField] private Slider seVolSlider;

    private void Start()
    {
        AM = this;
        PlayeRandomBGM();
        GetBGMVolume();
        GetSEVolume();
    }
    public void PlayeRandomBGM()
    {
        int rand = Random.Range(0, bgm.Length);
        bgmSpeaker.clip = bgm[rand].clip;
        bgmSpeaker.Play();
    }

    public void PlaySE(string soundName)
    {
        for (int i = 0; i < soundE.Length; i++)
        {
            if (soundName == soundE[i].name)
            {
                for (int j = 0; j < seSpeaker.Length; j++)
                {
                    if (!seSpeaker[j].isPlaying)
                    {
                        seSpeaker[j].clip = soundE[i].clip;
                        seSpeaker[j].PlayOneShot(seSpeaker[j].clip);
                        return;
                    }
                }
                Debug.Log("모든 효과음스피커가 사용중입니다.");
                return;
            }
        }
        Debug.Log("등록된 효과음이 없습니다.");
    }

    public void SetSEVolume(float volume)
    {
        for (int i = 0; i < seSpeaker.Length; i++)
        {
            seSpeaker[i].volume = volume;
            seVolSlider.value = seSpeaker[i].volume;
            PlayerPrefs.SetFloat("SE_Vol", seVolSlider.value);
        }
    }
    public void SetBGMVolume(float volume)
    {
        bgmVolSlider.value = volume;
        bgmSpeaker.volume = bgmVolSlider.value;
        PlayerPrefs.SetFloat("BGM_Vol", bgmVolSlider.value);
    }
  
    private void GetBGMVolume()
    {
        float bgm_Vol = PlayerPrefs.GetFloat("BGM_Vol");
        bgmVolSlider.value = bgm_Vol;
    }

    private void GetSEVolume()
    {
        float se_Vol = PlayerPrefs.GetFloat("SE_Vol");
        for (int i = 0; i < seSpeaker.Length; i++)
        {
            seSpeaker[i].volume = se_Vol;
            seVolSlider.value = seSpeaker[i].volume;
        }
    }
}
