using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("폭탄음 소스파일")]
    [SerializeField] Sound[] soundX = null;

    [Header("예비음 소스파일")]
    [SerializeField] Sound[] soundB = null;

    [Header("배경음 스피커")]
    [SerializeField] AudioSource bgmSpeaker = null;    

    [Header("효과음 스피커")]
    [SerializeField] AudioSource[] seSpeaker = null;

    [Header("폭탄음 스피커")]
    [SerializeField] AudioSource[] bombSpeaker = null;

    [Header("예비음 스피커")]
    [SerializeField] AudioSource[] beepSpeaker = null;

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
                        //seSpeaker[j].Play();
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
                        //bombSpeaker[j].Play();
                        bombSpeaker[j].PlayOneShot(bombSpeaker[j].clip);
                        return;
                    }
                }
                Debug.Log("모든 폭탄음스피커가 사용중입니다.");
                return;
            }
        }
        Debug.Log("등록된 폭탄음이 없습니다.");
    }

    public void PlaySB(string soundName)
    {
        for (int i = 0; i < soundB.Length; i++)
        {
            if (soundName == soundB[i].name)
            {
                for (int j = 0; j < beepSpeaker.Length; j++)
                {
                    if (!beepSpeaker[j].isPlaying)
                    {                        
                        beepSpeaker[j].clip = soundB[i].clip;
                        //beepSpeaker[j].Play();
                        beepSpeaker[j].PlayOneShot(beepSpeaker[j].clip);
                        return;
                    }
                }
                Debug.Log("모든 예비음스피커가 사용중입니다.");
                return;
            }
        }
        Debug.Log("등록된 예비음이 없습니다.");
    }
    public void StopSB(string soundName)
    {
        for (int i = 0; i < soundB.Length; i++)
        {
            if (soundName == soundB[i].name)
            {
                for (int j = 0; j < beepSpeaker.Length; j++)
                {
                    if (beepSpeaker[j].isPlaying)
                    {
                        beepSpeaker[j].clip = soundB[i].clip;
                        beepSpeaker[j].Stop();
                        return;
                    }
                }
                Debug.Log("모든 효과음스피커가 사용중입니다.");
                return;
            }
        }
        Debug.Log("등록된 효과음이 없습니다.");
    }

    public void StopSX(string soundName)
    {
        for (int i = 0; i < soundX.Length; i++)
        {
            if (soundName == soundX[i].name)
            {
                for (int j = 0; j < bombSpeaker.Length; j++)
                {
                    if (bombSpeaker[j].isPlaying)
                    {
                        bombSpeaker[j].clip = soundX[i].clip;
                        bombSpeaker[j].Stop();
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
