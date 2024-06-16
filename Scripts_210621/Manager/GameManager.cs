using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages general game states and times.
/// </summary>
public class GameManager : MonoBehaviour
{
    System.DateTime dateTime = System.DateTime.Now;
    //Instance
    public static GameManager inst;

    public int oxygenCnt;
    public GameObject ExitPanel;
    
    void Awake()
    {
        inst = this;
        oxygenCnt = PlayerPrefs.GetInt("TotalOxygen");
        Debug.Log("마지막 산소 구하기 : " + PlayerPrefs.GetInt("TotalOxygen"));
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
               //////////앱 실행시 시간차 구하기//////////
        string lastTime = PlayerPrefs.GetString("SaveQuitTime");
        System.DateTime lastDateTime = System.DateTime.Parse(lastTime);
        System.TimeSpan compareTime = (System.DateTime.Now - lastDateTime);
        PlayerPrefs.SetInt("GetOfflineTime", (int)compareTime.TotalSeconds);

        Debug.Log("종료 시간 : " + System.DateTime.Now.ToString());
        Debug.Log("실행 시간 : " + System.DateTime.Now.ToString());
        Debug.LogFormat("게임 종료 후, {0}초 지났습니다.", (int)compareTime.TotalSeconds);

        PlayerPrefs.SetString("Date", dateTime.ToString("yyyy-MM-dd")); //날짜 저장

        Debug.Log(PlayerPrefs.GetString("Date"));
        ////////////////////////////////////////////////
    }

    private void OnApplicationQuit()
    {
        //앱 종료시 시간 저장해놓기//
        PlayerPrefs.SetString("SaveQuitTime", System.DateTime.Now.ToString());
        ////////////////////////////
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))  // 백키가 눌렸을때
            {
                Time.timeScale = 0f; // 먼저 시간을 정지시킨다.
                ExitPanel.SetActive(true);
            }
        }
    }

    public void ExitYes()
    {
        Application.Quit();   // 앱을 종료
    }

    public void ExitNo()
    {
        Time.timeScale = 1f; // 먼저 시간을 다시 가도록 원복 
        ExitPanel.SetActive(false); // Exit 팝업창을 지운다.
    }
  
    //Loads the menu scene.
    void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void _ScreenCaputure()
    {
        AndroidUtils.instance.TakeHiResShot();

        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidUtils.instance.ShowToast("이미지로 저장되었습니다");
        }
        else
        {
            Debug.Log("이미지로 저장되었습니다");
        }
    }
}