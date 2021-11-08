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
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (v_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                v_instance = FindObjectOfType<VolumManager>();
            }
            // �̱��� ������Ʈ�� ��ȯ
            return v_instance;
        }
    }
    private static VolumManager v_instance; // �̱����� �Ҵ�� static ����

   
    
        

    private void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� ������Ʈ�� �ִٸ�
        if (instance != this)
        {
            // �ڽ��� �ı�
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
