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


    /*private void OnApplicationPause(bool pause)
    {
        UpdateLastPlayDate();
    }*/

    /*   public string GetGoldText(BigInteger data) // 골드 표현 형식을 소수점 까지 표시하는 메서드
       {
           int placeN = 2; // 네자리 단위로 끊어서 표현
           BigInteger value = data; // 빅인티저에 골드를 대입
           List<BigInteger> numList = new List<BigInteger>();
           BigInteger p = (long)Mathf.Pow(10, placeN);         

          do
           {
               numList.Add((long)(value % p));            
               value /= p;
           }
           while (value >= 1);

           BigInteger num = numList.Count < 2 ? numList[0] : numList[numList.Count-1] * p + numList[numList.Count-2];
           float f = (int)num / (float)p;        
          return f.ToString() + GetUnitText(numList.Count - 1);
       }*/

    /*    public string GetGoldText(BigInteger value)
        {
            List<BigInteger> numlist = new List<BigInteger>();
            BigInteger val = (BigInteger)Mathf.Pow(10,4);
            BigInteger val2 = value/val;

            if (value >= 1)
            {
                numlist.Add(item: val2);

            }

            BigInteger idx = numlist.Count-1;
            //if (idx < 0) { return ""; }
            BigInteger returnString = (idx / 26) + 1;
            string recallString = "";
            for (int i = 0; i < returnString; i++)
            {
                recallString += (char)(97 + idx % 26);
            }

            return value.ToString("N0") + recallString;
        }

        public string GetUnitText(BigInteger index)
        {
            BigInteger idx = index-1;
            if (idx < 0) { return ""; }
            BigInteger recallCount = (idx / 26) + 1;
            string recallString = "";
            for (int i = 0; i < recallCount; i++)
            {
                recallString += (char)(97 + idx % 26);
            }
            return recallString;
        }*/

    /*private string[] goldStrings = new string[]
    {
        "","만","억","조","경","해","자","양","가","구","간","정","재","극","항하사","아승기","나유타","불가사의","무량대수"
    };*/

    private string[] goldStrings = new string[]
    {
        "","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
    };
    public string GetGoldText(BigInteger data) // 골드 표현 형식을 소수점 까지 표시하는 메서드
    {
        BigInteger value = data; // 빅인티저에 골드를 대입
        List<BigInteger> numList = new List<BigInteger>();
        BigInteger p = (BigInteger)Mathf.Pow(10, 4);

        do
        {
            numList.Add((int)(value % p));
            value /= p;
        }
        while (value >= 1);
        string returnString = "";
        for (int i = 0; i < numList.Count; i++)
        {
            returnString = numList[i] + goldStrings[i];
            if(i>goldStrings.Length)
            {
                goldStrings[i] += goldStrings[i];
            }
        }
        return returnString;
    }


    public BigInteger Gold
    {
        get
        {
            if (!PlayerPrefs.HasKey("Gold"))
            {
                return 1;
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
            if (!(PlayerPrefs.HasKey("GoldPerClick")))
            {
                return 1;
            }
            string tmpPerClick = PlayerPrefs.GetString("GoldPerClick");
            return BigInteger.Parse(tmpPerClick);
        }
        set
        {
            PlayerPrefs.SetString("GoldPerClick", value.ToString());
        }
    }


    DateTime GetLastPlayDate()
    {
        if (!PlayerPrefs.HasKey("Time"))
        {
            return DateTime.Now;
        }

        string timeBinaryInString = PlayerPrefs.GetString("Time");        
        BigInteger timeBinaryInBigInteger = Convert.ToInt64(timeBinaryInString);
        return DateTime.FromBinary((long)timeBinaryInBigInteger);
        

    }
    public void OnApplicationQuit()
    {
        UpdateLastPlayDate();
        Debug.Log("UpdateLastPlayDate Callback Complete");
    }
    public void UpdateLastPlayDate()
    {
        PlayerPrefs.SetString("Time", DateTime.Now.ToBinary().ToString());
    }

    public BigInteger timeAfterLastPlay
    {

        get
        {
            DateTime currentTime = DateTime.Now;
            DateTime lastPlayDate = GetLastPlayDate();

            return (BigInteger)currentTime.Subtract(lastPlayDate).TotalSeconds; // 시간차를 구하는 프로퍼티
        }
    }

    private void Awake()
    {
        heroineButtons = FindObjectsOfType<HeroineButton>();
    }


    void Start()
    {
        Gold += GetGoldPerSec() * timeAfterLastPlay;
        InvokeRepeating("UpdateLastPlayDate", 0f, 3f);
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

        string tmpUplevel = PlayerPrefs.GetString(key + "_level", upgradeButton.level.ToString());
        upgradeButton.level = BigInteger.Parse(tmpUplevel);
        string tmpGoldByUp = PlayerPrefs.GetString(key + "_goldByUpgrade", upgradeButton.goldByUpgrade.ToString());
        upgradeButton.goldByUpgrade = BigInteger.Parse(tmpGoldByUp);
        string tmpCurrent = PlayerPrefs.GetString(key + "_cost", upgradeButton.currentCost.ToString());
        upgradeButton.currentCost = BigInteger.Parse(tmpCurrent);
        //upgradeButton.level = PlayerPrefs.GetInt(key + "_level", 1);
        /*upgradeButton.goldByUpgrade = PlayerPrefs.GetInt(key + "_goldByUpgrade",
            (int)upgradeButton.startGoldByUpgrade);*/
        //upgradeButton.currentCost = PlayerPrefs.GetInt(key + "_cost", (int)upgradeButton.startCurrentCost);
    }

    public void SaveUpgradeButton(UpgradeButton upgradeButton)
    {
        string key = upgradeButton.upgradeName;

        PlayerPrefs.SetString(key + "_level", upgradeButton.level.ToString());        
        PlayerPrefs.SetString(key + "_goldByUpgrade", upgradeButton.goldByUpgrade.ToString());
        PlayerPrefs.SetString(key + "_cost", upgradeButton.currentCost.ToString());
        /* PlayerPrefs.SetInt(key + "_level", (int)upgradeButton.level);
         PlayerPrefs.SetInt(key + "_goldByUpgrade", (int)upgradeButton.goldByUpgrade);
         PlayerPrefs.SetInt(key + "_cost", (int)upgradeButton.currentCost);*/
    }

    public void LoadHeroineButton(HeroineButton HeroineButton)
    {
        string key = HeroineButton.itemName;

        string tmpUplevel = PlayerPrefs.GetString(key + "_level", HeroineButton.level.ToString());
        HeroineButton.level = BigInteger.Parse(tmpUplevel);
        string tmpCurrent = PlayerPrefs.GetString(key + "_cost", HeroineButton.startCurrentCost.ToString());
        HeroineButton.currentCost = BigInteger.Parse(tmpCurrent);
        string tmpGoldPerS = PlayerPrefs.GetString(key + "_goldPerSec", HeroineButton.goldPerSec.ToString());
        HeroineButton.goldPerSec = BigInteger.Parse(tmpGoldPerS);


        /*HeroineButton.level = PlayerPrefs.GetInt(key + "_level");
        HeroineButton.currentCost = PlayerPrefs.GetInt(key + "_cost", (int)HeroineButton.startCurrentCost);
        HeroineButton.goldPerSec = PlayerPrefs.GetInt(key + "_goldPerSec");*/

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

        PlayerPrefs.SetString(key + "_level", HeroineButton.level.ToString());
        PlayerPrefs.SetString(key + "_cost", HeroineButton.currentCost.ToString());
        PlayerPrefs.SetString(key + "_goldPerSec", HeroineButton.goldPerSec.ToString());
        /* PlayerPrefs.SetInt(key + "_level", (int)HeroineButton.level);
         PlayerPrefs.SetInt(key + "_cost", (int)HeroineButton.currentCost);
         PlayerPrefs.SetInt(key + "_goldPerSec", (int)HeroineButton.goldPerSec);*/

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
        BigInteger goldPerSec = 1;
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
