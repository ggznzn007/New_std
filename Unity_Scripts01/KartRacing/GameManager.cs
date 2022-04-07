using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player")]
    public Car player;

    public float baseSpeed;
    public int lap;
    public bool check;

    [Header("RacingCars")]
    public Car[] car;
    public Transform[] target;
    public Controller controllPad;
    public Transform cam;

    [Header("MenuText")]
    public GameObject startMenu;
    public GameObject selectMenu;
    public GameObject ui;
    public GameObject finishMenu;

    [Header("Text")]
    public TextMeshProUGUI bestLapTimeText;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI curTimeText;
    public TextMeshProUGUI curSpeedText;
    public TextMeshProUGUI[] lapTimeText;

    float curTime;
    float bestLapTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        SpeedSet();
        BestLapTimeSet();


    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            // 안드로이드
#else
Application.Quit();

#endif
        }
    }

    public void GameStart()
    {
        StartCoroutine("StartCount");

    }

    void BestLapTimeSet()
    {
        bestLapTime = PlayerPrefs.GetFloat("BestLap");
        bestLapTimeText.text = string.Format("Best {0:00}:{1:00.00}", (int)(bestLapTime / 60 % 60), bestLapTime % 60);

        if (bestLapTime == 0)
        {
            bestLapTimeText.text = "Best  - ";
        }
    }

    public void LapTime()
    {
        if (lap == 3)
        {
            SE_Manager.instance.Playsound(SE_Manager.instance.goal);
            cam.parent = null;
            StopCoroutine("Timer");
            finishMenu.SetActive(true);

            player.player = false; // 플레이어 선택해제
            player.StartAI(); // AI플레이어 돌아감 
            controllPad.gameObject.SetActive(false); // 게임조작키 해제
            player.transform.GetChild(3).gameObject.SetActive(false); // 주행 사운드 해제

            if (curTime < bestLapTime | bestLapTime == 0)
            {
                bestLapTimeText.gameObject.SetActive(false);
                bestLapTimeText.text = string.Format("Best {0:00}:{1:00.00}", (int)(curTime / 60 % 60), curTime % 60);
                bestLapTimeText.gameObject.SetActive(true);

                PlayerPrefs.SetFloat("BestLap", curTime);
            }
        }

        lapTimeText[lap - 1].gameObject.SetActive(false);
        lapTimeText[lap - 1].text = string.Format("{0:00}:{1:00.00}", (int)(curTime / 60 % 60), curTime % 60);
        lapTimeText[lap - 1].gameObject.SetActive(true);
    }

    IEnumerator StartCount()
    {
        selectMenu.SetActive(false);
        ui.SetActive(true);

        SE_Manager.instance.Playsound(SE_Manager.instance.count[3]);
        countText.text = "3";
        countText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        SE_Manager.instance.Playsound(SE_Manager.instance.count[2]);
        countText.gameObject.SetActive(false);
        countText.text = "2";
        countText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        SE_Manager.instance.Playsound(SE_Manager.instance.count[1]);
        countText.gameObject.SetActive(false);
        countText.text = "1";
        countText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        SE_Manager.instance.Playsound(SE_Manager.instance.count[0]);
        countText.gameObject.SetActive(false);
        countText.text = "Go!";
        countText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        countText.gameObject.SetActive(false);

        controllPad.gameObject.SetActive(true);
        player.player = true;
        check = true;

        controllPad.StartController();
        for (int i = 0; i < car.Length; i++)
        {
            car[i].StartAI();
        }

        StartCoroutine("Timer");
    }

    IEnumerator Timer()
    {
        while (true)
        {
            curTime += Time.deltaTime;

            curTimeText.text = string.Format("{0:00}:{1:00.00}", (int)(curTime / 60 % 60), curTime % 60);
            yield return null;
        }
    }
    void SpeedSet()
    {
        for (int i = 0; i < car.Length; i++)
        {
            car[i].carSpeed = Random.Range(baseSpeed, baseSpeed + 0.7f);
        }
    }

    public void StartBtn()
    {
        SE_Manager.instance.Playsound(SE_Manager.instance.btn);

        startMenu.SetActive(false);
        selectMenu.SetActive(true);
    }

    public void ReStartBtn()
    {
        SE_Manager.instance.Playsound(SE_Manager.instance.btn);
        SceneManager.LoadScene("SampleScene");
    }
}
