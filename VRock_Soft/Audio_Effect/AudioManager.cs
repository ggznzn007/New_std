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
    [SerializeField] Sound[] bgm;

    [Header("ȿ���� �ҽ�����")]
    [SerializeField] Sound[] soundE;

    [Header("��ź�� �ҽ�����")]
    [SerializeField] Sound[] soundX;

    [Header("����� ����Ŀ")]
    [SerializeField] AudioSource bgmSpeaker;    

    [Header("ȿ���� ����Ŀ")]
    [SerializeField] AudioSource[] seSpeaker;

    [Header("��ź�� ����Ŀ")]
    [SerializeField] AudioSource[] bombSpeaker;

    private void Awake()
    {
        AM = this;        
    }

    private void Start()
    {
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
                        bombSpeaker[j].PlayOneShot(bombSpeaker[j].clip);
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
