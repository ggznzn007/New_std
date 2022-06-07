using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;

public class HeroineButton : MonoBehaviour
{
    public TextMeshProUGUI itemDisplayer;

    public CanvasGroup canvasGroup;

    public Slider slider;

    public string itemName;

    public BigInteger level = 1;

    [HideInInspector]
    public BigInteger currentCost = 9000;

    public BigInteger startCurrentCost = 9000;

    [HideInInspector]
    public BigInteger goldPerSec;

    public BigInteger startGoldPerSec = 9000;

    public float costPow;

    public float upgradePow;

    [HideInInspector]
    public bool isPurchased = false;

    public Button heroineButton;
    private void Start()
    {
        DataController.Instance.LoadHeroineButton(this);

        StartCoroutine("AddGoldLoop");
        UpdateUI();
    }

    public void HeroineSE()
    {
        SoundController.instance.Playsound(SoundController.instance.heroineClick);
    }
    public void PurchaseItem()
    {
        if(DataController.Instance.Gold >=currentCost)
        {
            isPurchased = true;
            DataController.Instance.Gold -=currentCost;
            level++;
            UpdateItem();
            UpdateUI();
            DataController.Instance.SaveHeroineButton(this);
        }
    }

    IEnumerator AddGoldLoop()
    {
        while(true)
        {
            if(isPurchased)
            {
                DataController.Instance.Gold+=goldPerSec;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    public void UpdateItem()
    {
        goldPerSec = (goldPerSec) + startGoldPerSec * (BigInteger)Mathf.Pow(upgradePow, (int)level);
        currentCost = currentCost/2 + startCurrentCost * (BigInteger)Mathf.Pow(costPow, (int)level);
        
    }

    public void UpdateUI()
    {
        itemDisplayer.text = itemName + " Level " + level + "\n\n구매금액: "  
            + DataController.Instance.GetGoldText(currentCost)+
            "\n초당 추가금액: " + DataController.Instance.GetGoldText(goldPerSec);

        slider.minValue = 0;
        slider.maxValue =(long)currentCost;

        slider.value = (float)DataController.Instance.Gold;

        if(isPurchased)
        {
           canvasGroup.alpha = 1.0f;
        }
        else
        {
           canvasGroup.alpha = 0.6f;
        }              
        
    }
   /* public string GetCurrentCostText2(BigInteger data) // 골드 표현 형식을 소수점 까지 표시하는 메서드
    {
        int placeN = 4; // 세자리 단위로 끊어서 표현
        BigInteger value = currentCost; // 빅인티저에 골드를 대입
        List<BigInteger> numList = new List<BigInteger>();
        BigInteger p = (int)Mathf.Pow(10, placeN);

        do
        {
            numList.Add((int)(value % p));
            value /= p;
        }
        while (value >= 1);

        BigInteger num = numList.Count < 2 ? numList[0] : numList[numList.Count - 1] * p + numList[numList.Count - 2];
        float f = ((int)num / (float)p);
        return f.ToString("N0") + GetUnitText2(numList.Count - 3);
    }

    private string GetUnitText2(int index)
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
    }*/
   
    public void PurchaseActiveHero()
    {
        if (DataController.Instance.Gold >= currentCost)
        {
            heroineButton.interactable = true;
            slider.interactable = true;
        }
        else
        {
            heroineButton.interactable = false;
            slider.interactable = false;
        }
    }
    private void Update()
    {
        UpdateUI();
        //PurchaseActiveHero();
    }
}
