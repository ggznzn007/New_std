using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdminMove : MonoBehaviour
{
    private float moveSpeed = 5f;
    public TMP_Text blueScore;
    public TMP_Text redScore;
    public TMP_Text timerText;

    private void Start()
    {
        //SetResolution(); // �ػ� ����
    }

    void Update()
    {
        getScore();
        timerTxt();
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
       Vector3 dirXY = Vector3.right * h+Vector3.up *v;
        dirXY.Normalize();
        transform.position += dirXY * moveSpeed*Time.deltaTime;
    }

    public void getScore()
    {
        if(DataManager.DM.currentMap==Map.TOY)
        {
            blueScore.text = GunShootManager.GSM.score_Blue.ToString();   // ����� ����
            redScore.text = GunShootManager.GSM.score_Red.ToString();     // ������ ����
        }
        else if(DataManager.DM.currentMap==Map.WESTERN)
        {
            blueScore.text = WesternManager.WM.score_Blue.ToString();
            redScore.text = WesternManager.WM.score_Red.ToString();
        }
        
    }

    public void timerTxt()
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
