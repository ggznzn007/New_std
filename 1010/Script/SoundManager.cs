using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // 싱글톤으로 소리제어
    AudioSource sound; 

    [Header("AudioClip")]
    public AudioClip blockClick;         // 블럭선택
    public AudioClip blockDrop;          // 블럭놓았을 때
    public AudioClip blockClear;         // 라인이 없어질 때
    public AudioClip blockSet;           // 블럭생성
    public AudioClip gameOver;           // 게임오버

    private void Awake()
    {
        if (instance == null)
            instance = this;

        sound = GetComponent<AudioSource>();
    }

    public void Playsound(AudioClip clip) // 해당 소리 재생 메소드
    {
        sound.PlayOneShot(clip); 
    }
}
