using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private HeroineButton[] HeroineButtons;

   public long gold
    {
        get
        {
            if(!PlayerPrefs.HasKey("Gold"))
            {
                return 0;
            }
            string tmpGold =  PlayerPrefs.GetString("Gold");
            return long.Parse(tmpGold);            
        }
        set
        {
            PlayerPrefs.SetString("Gold", value.ToString());
        }
    }

    public int goldPerClick
    {
        get
        {
            return PlayerPrefs.GetInt("GoldPerClick",1);
        }
        set
        {
            PlayerPrefs.SetInt("GoldPerClick", value);
        }
    }

    private void Awake()
    {
        HeroineButtons = FindObjectsOfType<HeroineButton>();
    }

    

    
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
        PlayerPrefs.SetInt(key + "_goldByUpgrade",upgradeButton.goldByUpgrade);
        PlayerPrefs.SetInt(key + "_cost", upgradeButton.currentCost);
    }

    public void LoadHeroineButton(HeroineButton HeroineButton)
    {
        string key = HeroineButton.itemName;

        HeroineButton.level = PlayerPrefs.GetInt(key + "_level");
        HeroineButton.currentCost = PlayerPrefs.GetInt(key + "_cost", HeroineButton.startCurrentCost);
        HeroineButton.goldPerSec = PlayerPrefs.GetInt(key + "_goldPerSec");

        if(PlayerPrefs.GetInt(key+"_isPurchased")==1)
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
        for (int i = 0; i < HeroineButtons.Length; i++)
        {
            goldPerSec += HeroineButtons[i].goldPerSec;

        }

        return goldPerSec;
    }
}
