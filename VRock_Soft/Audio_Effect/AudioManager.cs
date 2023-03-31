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
   
    [Header("����� �ҽ�����")]
    [SerializeField] Sound[] bgm = null;

    [Header("ȿ���� �ҽ�����")]
    [SerializeField] Sound[] soundE = null;

    [Header("��ź�� �ҽ�����")]
    [SerializeField] Sound[] soundX = null;

    [Header("������ �ҽ�����")]
    [SerializeField] Sound[] soundB = null;

    [Header("����� ����Ŀ")]
    [SerializeField] AudioSource bgmSpeaker = null;    

    [Header("ȿ���� ����Ŀ")]
    [SerializeField] AudioSource[] seSpeaker = null;

    [Header("��ź�� ����Ŀ")]
    [SerializeField] AudioSource[] bombSpeaker = null;

    [Header("������ ����Ŀ")]
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
                Debug.Log("��� ��ź������Ŀ�� ������Դϴ�.");
                return;
            }
        }
        Debug.Log("��ϵ� ��ź���� �����ϴ�.");
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
                Debug.Log("��� ����������Ŀ�� ������Դϴ�.");
                return;
            }
        }
        Debug.Log("��ϵ� �������� �����ϴ�.");
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
                Debug.Log("��� ȿ��������Ŀ�� ������Դϴ�.");
                return;
            }
        }
        Debug.Log("��ϵ� ȿ������ �����ϴ�.");
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
                Debug.Log("��� ȿ��������Ŀ�� ������Դϴ�.");
                return;
            }
        }
        Debug.Log("��ϵ� ȿ������ �����ϴ�.");
    }

    public void PlayeRandomBGM()
    {
        int rand = Random.Range(0,bgm.Length);
        bgmSpeaker.clip = bgm[rand].clip;
        bgmSpeaker.Play();
    }
   
    
}
