using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class Timer_GS : MonoBehaviourPunCallbacks//,IPunObservable
{
    public TextMeshPro timerText;
    //public double LimitTime;
    //double realTimer;
    // public int min;
    //public double sec;
    //PhotonView PV;
    public bool count;
    public int limitedTime;

    /*public float min = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] / 60);
    public float sec = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] % 60);*/
    readonly ExitGames.Client.Photon.Hashtable setTime = new ExitGames.Client.Photon.Hashtable();


    private void Awake()
    {
        //realTimer = LimitTime;
        //PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        count = true;
    }

    public void Update()
    {
        limitedTime = (int)PN.CurrentRoom.CustomProperties["Time"];
        float min = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] / 60);
        float sec = Mathf.FloorToInt((int)PN.CurrentRoom.CustomProperties["Time"] % 60);
        timerText.text = string.Format("�����ð� {0:00}�� {1:00}��", min, sec);
        if (limitedTime < 60)
        {
            timerText.text = string.Format("�����ð� {0:0}��", sec);
        }
        if (PN.IsMasterClient)
        {
            if (count)
            {
                count = false;
                StartCoroutine(timer());
            }
        }
    }

    public IEnumerator timer()
    {
        yield return new WaitForSeconds(1);
        int nextTime = limitedTime -= 1;
        setTime["Time"] = nextTime;
        PN.CurrentRoom.SetCustomProperties(setTime);
        count = true;

        if (limitedTime == 0)
        {
            limitedTime = 0;
            timerText.text = string.Format("�����ð� 0��");
            // StartCoroutine(LoadNext());

            Application.Quit();           // PN.LeaveRoom();

            Debug.Log("Ÿ�ӿ���");
        }
    }

    IEnumerator LoadNext()
    {
        SceneManager.LoadScene(0);
        yield return new WaitForSeconds(1f);
        PN.Disconnect();
    }

    /////////////////////////////////// 1 ����
    /* realTimer -= Time.deltaTime;  // �����ð� ����

       // �����ð��� 60�� ���� Ŭ��
       if (realTimer >= 60f)
       {
           min = (int)realTimer / 60;  // 60���� ������ ����� ���� �д����� ����
           sec = realTimer % 60;       // 60���� ������ ����� �������� �ʴ����� ����
           timerText.text = "�����ð� : " + min + "�� " + (int)sec + "��";
       }

       // �����ð��� 60�� �̸��� ��
       if (realTimer < 60f)
       {
           // �д����� �ʿ�����Ƿ� �ʴ����� ������ ����
           timerText.text = "�����ð� : " + (int)realTimer + "��";
       }

       // �����ð��� 0���� �۾��� ��
       if (realTimer <= 0)
       {
           // 0���� ����
           timerText.text = "�����ð� : 0��";
           Debug.Log("Ÿ�ӿ���");
           realTimer = 0f;
           PN.Disconnect();
       }*/

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(timerText.text);
        }
        else if (stream.IsReading)
        {
            timerText.text = (string)stream.ReceiveNext();
        }
    }*/
}
