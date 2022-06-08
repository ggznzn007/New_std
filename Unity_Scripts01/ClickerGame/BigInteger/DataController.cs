using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;
using System.Text;
using TMPro;

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

    public string GetGoldText(BigInteger data) // 골드 표현 형식을 소수점 까지 표시하는 메서드
    {
        int placeN = 4; // 네자리 단위로 끊어서 표현
        BigInteger value = data; // 빅인티저에 골드를 대입
        List<BigInteger> numList = new List<BigInteger>();
        long p = (int)Mathf.Pow(10, placeN);       

        do
        {
            numList.Add((long)(value % p));            
            value /= p;
        }
        while (value >= 1);

        BigInteger num = numList.Count < 2 ? numList[0] : numList[numList.Count - 1] * p + numList[numList.Count - 2];        
        float f = (int)num / (float)p;
        return f.ToString("N2") + GetUnitText(numList.Count - 1);
    }

    public string GetUnitText(BigInteger index)
    {
        BigInteger idx = index - 1;
        if (idx < 0) { return ""; }
        BigInteger recallCount = (idx / 26) + 1;
        string recallString = "";
        for (int i = 0; i < recallCount; i++)
        {
            recallString += (char)(97 + idx % 26);
        }
        return recallString;
    }

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
*/

        /*  private string[] goldStrings = new string[]
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
          }*/

        /* public string GetGoldText(BigInteger value)// 노가다
         {

             BigInteger div = (BigInteger)Mathf.Pow(10, 4);

             if (value >= (BigInteger)10000000000000000000)
             {
                 // value = value / (BigInteger)10000000000000000;
                 value = (value / (div ^ 4));
                 return value.ToString() + "E";
             }

             else if (value >= (BigInteger)10000000000000000)
             {
                 // value = value / (BigInteger)10000000000000000;
                 value = (value / (div ^ 4));
                 return value.ToString() + "D";
             }
             else if (value >= (BigInteger)1000000000000)
             {
                 //value = value / (BigInteger)1000000000000;
                 value = (value / (div ^ 3));
                 return value.ToString() + "C";
             }
             else if (value >= (BigInteger)100000000)
             {
                 //value = value / (BigInteger)100000000;
                 value = (value / (div ^ 2));
                 return value.ToString() + "B";
             }
             else if (value >= (BigInteger)10000)
             {
                 //value = value / (BigInteger)10000;
                 value = (value / div);
                 return value.ToString() + "A";
             }
             else if (value < (BigInteger)10000)
             {
                 //value = (int)value / 10000;

                 return value.ToString() + "";
             }


             return null;
         }*/


    public BigInteger Gold
    {
        get
        {
            if (!PlayerPrefs.HasKey("Gold"))
            {
                return 10000;
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
                return 10000;
            }
            string tmpPerClick = PlayerPrefs.GetString("GoldPerClick");
            return BigInteger.Parse(tmpPerClick);
        }
        set
        {
            PlayerPrefs.SetString("GoldPerClick", value.ToString());
        }
    }
    
    public void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            UpdateLastPlayDate();
        }
        
    }
    public void OnApplicationQuit()
    {
        UpdateLastPlayDate();       
    }

    public void OnApplicationPause(bool pause)
    {
        if(!pause)
        {
            UpdateLastPlayDate();
        }
           
    }

    private void Awake()
    {
        heroineButtons = FindObjectsOfType<HeroineButton>();       
    }

    public void UpdateLastPlayDate()
    {
        PlayerPrefs.SetString("Time", DateTime.Now.ToBinary().ToString());        
    }
    DateTime GetLastPlayDate()
    {
        if (!PlayerPrefs.HasKey("Time"))
        {
            return DateTime.Now;
        }

        string timeString = PlayerPrefs.GetString("Time");
        long timeBigInteger = Convert.ToInt64(timeString);
        return DateTime.FromBinary(timeBigInteger);


    }

    public long timeAfterLastPlay
    {

        get
        {
            DateTime currentTime = DateTime.Now;
            DateTime lastPlayDate = GetLastPlayDate();

            return (long)currentTime.Subtract(lastPlayDate).TotalSeconds; // 시간차를 구하는 프로퍼티
        }
    }

    void Start()
    {
        Gold += GetGoldPerSec() * (BigInteger)timeAfterLastPlay;
        InvokeRepeating(nameof(UpdateLastPlayDate), 0f, 5f);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            GameQuit();

#else
    Application.Quit();

#endif
        }
    }
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
        BigInteger goldPerSec = 9000;
        for (int i = 0; i < heroineButtons.Length; i++)
        {
            if (heroineButtons[i].isPurchased == true)
            {
                goldPerSec += heroineButtons[i].goldPerSec;
            }
        }

        return goldPerSec;
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
