using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VolumManager : MonoBehaviourPunCallbacks
{
    public static VolumManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (v_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                v_instance = FindObjectOfType<VolumManager>();
            }
            // 싱글톤 오브젝트를 반환
            return v_instance;
        }
    }
    private static VolumManager v_instance; // 싱글톤이 할당될 static 변수

   
    
        

    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
    }

    public void Start()
    {
    }
    public void Update()
    {
        
      /*  AudioSource audio = GameObject.Find("VideoPlayer").GetComponentInChildren<AudioSource>();
       

        if (Input.GetKey(KeyCode.Comma))
        {
            
        }
        if (Input.GetKey(KeyCode.Period))
        {
            
        }*/





    }

   public void ToggleAudioVolume()
    {
        AudioListener speaker = GameObject.Find("Camera").GetComponentInChildren<AudioListener>();
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }
}
