using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class AdminMove : MonoBehaviourPun
{
    public TMP_Text blueScore;
    public TMP_Text redScore;   
    public TMP_Text timerText;
    private readonly float moveSpeed = 5f;

    void Update()
    {
        GetScore();
        TimerTxt();
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dirXY = Vector3.right * h + Vector3.up * v;
        dirXY.Normalize();
        transform.position += dirXY * moveSpeed * Time.deltaTime;
    }

    public void GetScore()
    {
        if(DataManager.DM.currentMap==Map.TOY)
        {
            blueScore.text = GunShootManager.GSM.score_BlueKill.ToString();   // ����� ����
            redScore.text = GunShootManager.GSM.score_RedKill.ToString();     // ������ ����
        }
        else if(DataManager.DM.currentMap==Map.WESTERN)
        {
            blueScore.text = WesternManager.WM.score_BlueKill.ToString();
            redScore.text = WesternManager.WM.score_RedKill.ToString();
        }
        
    }

    public void TimerTxt()
    {
        if(DataManager.DM.currentMap==Map.TOY)
        {
           timerText.text =  GunShootManager.GSM.timerText.text;
        }
        else if(DataManager.DM.currentMap == Map.WESTERN)
        {
            timerText.text = WesternManager.WM.timerText.text;
        }
        
    }

    public void SetResolution()
    {
        int setWidth = 1920;
        int setHeight = 1080;

        Screen.SetResolution(setWidth, setHeight, false); // â���
       // Screen.SetResolution(setWidth, setHeight, true); // Ǯ��ũ�����
    }
    
}
