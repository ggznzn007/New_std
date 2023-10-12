using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    //public float volume;   
    public AudioClip clip;
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
   

    private void Start()
    {
        AM = this;
        PlayeBGM(0);
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

    public void StopSE(string soundName)
    {
        for (int i = 0; i < soundE.Length; i++)
        {
            if (soundName == soundE[i].name)
            {
                for (int j = 0; j < seSpeaker.Length; j++)
                {
                    if (seSpeaker[j].isPlaying)
                    {
                        seSpeaker[j].clip = soundE[i].clip;
                        seSpeaker[j].Stop();
                        return;
                    }
                }
                Debug.Log("모든 효과음스피커가 사용중입니다.");
                return;
            }
        }
        Debug.Log("등록된 효과음이 없습니다.");
    }

   

   

    public void PlayeBGM(int set)
    {        
        bgmSpeaker.clip = bgm[set].clip;
        bgmSpeaker.Play();
    }
}
