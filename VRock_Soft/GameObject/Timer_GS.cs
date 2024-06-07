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
        timerText.text = string.Format("남은시간 {0:00}분 {1:00}초", min, sec);
        if (limitedTime < 60)
        {
            timerText.text = string.Format("남은시간 {0:0}초", sec);
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
            timerText.text = string.Format("남은시간 0초");
            // StartCoroutine(LoadNext());

            Application.Quit();           // PN.LeaveRoom();

            Debug.Log("타임오버");
        }
    }

    IEnumerator LoadNext()
    {
        SceneManager.LoadScene(0);
        yield return new WaitForSeconds(1f);
        PN.Disconnect();
    }

    /////////////////////////////////// 1 버전
    /* realTimer -= Time.deltaTime;  // 설정시간 감소

       // 설정시간이 60초 보다 클때
       if (realTimer >= 60f)
       {
           min = (int)realTimer / 60;  // 60으로 나눠서 생기는 몫을 분단위로 변경
           sec = realTimer % 60;       // 60으로 나워서 생기는 나머지를 초단위로 설정
           timerText.text = "남은시간 : " + min + "분 " + (int)sec + "초";
       }

       // 설정시간이 60초 미만일 때
       if (realTimer < 60f)
       {
           // 분단위는 필요없으므로 초단위만 남도록 설정
           timerText.text = "남은시간 : " + (int)realTimer + "초";
       }

       // 설정시간이 0보다 작아질 때
       if (realTimer <= 0)
       {
           // 0으로 고정
           timerText.text = "남은시간 : 0초";
           Debug.Log("타임오버");
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
