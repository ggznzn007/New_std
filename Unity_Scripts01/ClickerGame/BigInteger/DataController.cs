using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;
using System.Text;

public class DataController : MonoBehaviour
{
    private static DataController instance;

    public static DataController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataController>();
                if (instance == null)
                {
                    GameObject container = new GameObject("DataController");

                    instance = container.AddComponent<DataController>();
                }
            }

            return instance;
        }
    }

    private HeroineButton[] heroineButtons;

   
    DateTime GetLastPlayDate()
    {
        if (!PlayerPrefs.HasKey("Time"))
        {
            return DateTime.Now;
        }

        string timeBinaryInString = PlayerPrefs.GetString("Time");
        long timeBinaryInLong = Convert.ToInt64(timeBinaryInString);

        return DateTime.FromBinary(timeBinaryInLong);
    }


    void UpdateLastPlayDate()
    {
        PlayerPrefs.SetString("Time", DateTime.Now.ToBinary().ToString());
    }
    public void OnApplicationQuit()
    {
        Debug.Log("Callback");
        UpdateLastPlayDate();
    }

    private void OnApplicationPause(bool pause)
    {
        UpdateLastPlayDate();
    }

    public string GetGoldText(BigInteger data) // 골드 표현 형식을 소수점 까지 표시하는 메서드
    {
        int placeN = 4; // 네자리 단위로 끊어서 표현
        BigInteger value = data; // 빅인티저에 골드를 대입
        List<int> numList = new List<int>();
        int p = (int)Mathf.Pow(10, placeN);

        do
        {
            numList.Add((int)(value % p));
            value /= p;
        }
        while (value >= 1);

        int num = numList.Count < 2 ? numList[0] : numList[numList.Count -1] * p + numList[numList.Count - 2];
        //int num = numList.Count < 2 ? numList[0] : numList[numList.Count-1]*p;
        float f = (num / (float)p);
        return f.ToString("N0")+ GetUnitText(numList.Count-1);
    }

    private string GetUnitText(int index)
    {
        int idx = index - 1;
        if (idx < 0) { return ""; }
        int recallCount = (idx / 26) + 1;
        string recallString = "";
        for (int i = 0; i < recallCount; i++)
        {
            recallString += (char)(97 + idx % 26);
        }
        return recallString;
    }

   /* public string GetGoldText(BigInteger data) // 골드 표현 형식을 소수점 까지 표시하는 메서드
    {
        int placeN = 3; // 세자리 단위로 끊어서 표현
        BigInteger value = data; // 빅인티저에 골드를 대입
        List<int> numList = new List<int>();
        int p = (int)Mathf.Pow(10, placeN);

        do
        {
            numList.Add((int)(value % p));
            value /= p;
        }
        while (value >= 1);

        int num = numList.Count < 2 ? numList[0] : numList[numList.Count - 1] * p + numList[numList.Count - 2];
        float f = (num / (float)p);
        return f.ToString("N2") + GetUnitText(numList.Count - 1);
    }*/
    public BigInteger Gold
    {
        get
        {
            if (!PlayerPrefs.HasKey("Gold"))
            {
                return 0;
            }
            string tmpGold = PlayerPrefs.GetString("Gold");
            
            return BigInteger.Parse(tmpGold);
        }
        set
        {
            PlayerPrefs.SetString("Gold", value.ToString());
        }
    }

   

    public BigInteger GoldPerClick
    {
        get
        {
            return PlayerPrefs.GetInt("GoldPerClick", 10000);
        }
        set
        {
            PlayerPrefs.SetInt("GoldPerClick", (int)value);
        }
    }

    public int TimeAfterLastPlay
    {
        
        get
        {
            DateTime currentTime = DateTime.Now;
            DateTime lastPlayDate = GetLastPlayDate();

            return (int)currentTime.Subtract(lastPlayDate).TotalSeconds; // 시간차를 구하는 프로퍼티
        }
    }

    private void Awake()
    {
        heroineButtons = FindObjectsOfType<HeroineButton>();
    }


    private void Start()
    {
        Gold += GetGoldPerSec() * TimeAfterLastPlay;
        InvokeRepeating(nameof(UpdateLastPlayDate), 0f, 2f);   
    }
  /*private void Update()
      {
         
          if (Input.GetKeyDown(KeyCode.Escape))
          {            
  #if UNITY_EDITOR
              UnityEditor.EditorApplication.isPlaying = false;
              Application.Quit();
              // 안드로이드
  #else
  Application.Quit();

  #endif
          }
      }*/
    public void LoadUpgradeButton(UpgradeButton upgradeButton)
    {
        string key = upgradeButton.upgradeName;

        upgradeButton.level = PlayerPrefs.GetInt(key + "_level", 1);
        upgradeButton.goldByUpgrade = PlayerPrefs.GetInt(key + "_goldByUpgrade",
            (int)upgradeButton.startGoldByUpgrade);
        upgradeButton.currentCost = PlayerPrefs.GetInt(key + "_cost", (int)upgradeButton.startCurrentCost);        
    }

    public void SaveUpgradeButton(UpgradeButton upgradeButton)
    {
        string key = upgradeButton.upgradeName;

        PlayerPrefs.SetInt(key + "_level", (int)upgradeButton.level);
        PlayerPrefs.SetInt(key + "_goldByUpgrade", (int)upgradeButton.goldByUpgrade);
        PlayerPrefs.SetInt(key + "_cost", (int)upgradeButton.currentCost);       
    }

    public void LoadHeroineButton(HeroineButton HeroineButton)
    {
        string key = HeroineButton.itemName;

        HeroineButton.level = PlayerPrefs.GetInt(key + "_level");
        HeroineButton.currentCost = PlayerPrefs.GetInt(key + "_cost",(int)HeroineButton.startCurrentCost);
        HeroineButton.goldPerSec = PlayerPrefs.GetInt(key + "_goldPerSec");

        if (PlayerPrefs.GetInt(key + "_isPurchased") == 1)
        {
            HeroineButton.isPurchased = true;
        }
        else
        {
            HeroineButton.isPurchased = false;
        }
    }

    public void SaveHeroineButton(HeroineButton HeroineButton)
    {
        string key = HeroineButton.itemName;

        PlayerPrefs.SetInt(key + "_level",(int)HeroineButton.level);
        PlayerPrefs.SetInt(key + "_cost", (int)HeroineButton.currentCost);
        PlayerPrefs.SetInt(key + "_goldPerSec", (int)HeroineButton.goldPerSec);

        if (HeroineButton.isPurchased)
        {
            PlayerPrefs.SetInt(key + "_isPurchased", 1);
        }
        else
        {
            PlayerPrefs.SetInt(key + "_isPurchased", 0);
        }
    }

    public BigInteger GetGoldPerSec()
    {
        BigInteger goldPerSec = 0;
        for (int i = 0; i < heroineButtons.Length; i++)
        {
            if (heroineButtons[i].isPurchased == true)
            {
                goldPerSec += heroineButtons[i].goldPerSec;
            }
        }
        
        return goldPerSec;
    }

    
}
