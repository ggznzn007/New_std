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

    [Header("폭탄음 소스파일")]
    [SerializeField] Sound[] soundX = null;

    [Header("배경음 스피커")]
    [SerializeField] AudioSource bgmSpeaker = null;    

    [Header("효과음 스피커")]
    [SerializeField] AudioSource[] seSpeaker = null;

    [Header("폭탄음 스피커")]
    [SerializeField] AudioSource[] bombSpeaker = null;

  
    private void Start()
    {
        AM = this;
        PlayeRandomBGM();
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
    public void PlaySX(string soundName)
    {
        for (int i = 0; i < soundX.Length; i++)
        {
            if (soundName == soundX[i].name)
            {
                for (int j = 0; j < bombSpeaker.Length; j++)
                {
                    if (!bombSpeaker[j].isPlaying)
                    {
                        bombSpeaker[j].clip = soundX[i].clip;
                        bombSpeaker[j].PlayOneShot(bombSpeaker[j].clip);
                        return;
                    }
                }
                Debug.Log("모든 효과음스피커가 사용중입니다.");
                return;
            }
        }
        Debug.Log("등록된 효과음이 없습니다.");
    }

    public void PlayeRandomBGM()
    {
        int rand = Random.Range(0,bgm.Length);
        bgmSpeaker.clip = bgm[rand].clip;
        bgmSpeaker.Play();
    }
   
    
}
