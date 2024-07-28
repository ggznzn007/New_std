using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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

    public string GetCommaGold()
    {
        return string.Format("{0:#,###}", Gold);
    }

    public string GetCommaClick()
    {
        return string.Format("{0:#,###}", GoldPerClick);
    }

    public string GetCommaSec()
    {
        return string.Format("{0:#,###}", GetGoldPerSec());
    }

    public long Gold
    {
        get
        {
            if (!PlayerPrefs.HasKey("Gold"))
            {
                return 0;
            }
            string tmpGold = PlayerPrefs.GetString("Gold");
            
            return long.Parse(tmpGold);
        }
        set
        {
            PlayerPrefs.SetString("Gold", value.ToString());
        }
    }   

    public int GoldPerClick
    {
        get
        {
            return PlayerPrefs.GetInt("GoldPerClick", 1);
        }
        set
        {
            PlayerPrefs.SetInt("GoldPerClick", value);
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
            upgradeButton.startGoldByUpgrade);
        upgradeButton.currentCost = PlayerPrefs.GetInt(key + "_cost", upgradeButton.startCurrentCost);
    }

    public void SaveUpgradeButton(UpgradeButton upgradeButton)
    {
        string key = upgradeButton.upgradeName;

        PlayerPrefs.SetInt(key + "_level", upgradeButton.level);
        PlayerPrefs.SetInt(key + "_goldByUpgrade", upgradeButton.goldByUpgrade);
        PlayerPrefs.SetInt(key + "_cost", upgradeButton.currentCost);
    }

    public void LoadHeroineButton(HeroineButton HeroineButton)
    {
        string key = HeroineButton.itemName;

        HeroineButton.level = PlayerPrefs.GetInt(key + "_level");
        HeroineButton.currentCost = PlayerPrefs.GetInt(key + "_cost", HeroineButton.startCurrentCost);
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

        PlayerPrefs.SetInt(key + "_level", HeroineButton.level);
        PlayerPrefs.SetInt(key + "_cost", HeroineButton.currentCost);
        PlayerPrefs.SetInt(key + "_goldPerSec", HeroineButton.goldPerSec);

        if (HeroineButton.isPurchased)
        {
            PlayerPrefs.SetInt(key + "_isPurchased", 1);
        }
        else
        {
            PlayerPrefs.SetInt(key + "_isPurchased", 0);
        }
    }

    public int GetGoldPerSec()
    {
        int goldPerSec = 0;
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
