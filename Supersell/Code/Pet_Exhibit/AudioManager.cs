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

    [Header("����� �ҽ�����")]
    [SerializeField] Sound[] bgm = null;

    [Header("ȿ���� �ҽ�����")]
    [SerializeField] Sound[] soundE = null;    

    [Header("����� ����Ŀ")]
    [SerializeField] AudioSource bgmSpeaker = null;

    [Header("ȿ���� ����Ŀ")]
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
                Debug.Log("��� ȿ��������Ŀ�� ������Դϴ�.");
                return;
            }
        }
        Debug.Log("��ϵ� ȿ������ �����ϴ�.");
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
                Debug.Log("��� ȿ��������Ŀ�� ������Դϴ�.");
                return;
            }
        }
        Debug.Log("��ϵ� ȿ������ �����ϴ�.");
    }

   

   

    public void PlayeBGM(int set)
    {        
        bgmSpeaker.clip = bgm[set].clip;
        bgmSpeaker.Play();
    }
}
