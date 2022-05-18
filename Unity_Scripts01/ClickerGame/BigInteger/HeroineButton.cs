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

    public BigInteger level;

    [HideInInspector]
    public BigInteger currentCost = 1;

    public BigInteger startCurrentCost = 1;

    [HideInInspector]
    public BigInteger goldPerSec;

    public BigInteger startGoldPerSec = 1;

    public float costPow = 3.14f;

    public float upgradePow = 1.07f;

    [HideInInspector]
    public bool isPurchased = false;


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
        goldPerSec = (BigInteger)(goldPerSec) + startGoldPerSec * (BigInteger)Mathf.Pow(upgradePow, (int)level);
        currentCost = (BigInteger)currentCost/2 + startCurrentCost * (BigInteger)Mathf.Pow(costPow, (int)level);
        
    }

    public void UpdateUI()
    {
        itemDisplayer.text = itemName + " Level " + (int)level + "\n\n구매금액: "  + GetCurrentCostText2(currentCost)+
            "\n초당 추가금액: " + DataController.Instance.GetGoldText(goldPerSec);

        slider.minValue = 0;
        slider.maxValue =(float)currentCost;

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
    public string GetCurrentCostText2(BigInteger data) // 골드 표현 형식을 소수점 까지 표시하는 메서드
    {
        int placeN = 3; // 세자리 단위로 끊어서 표현
        BigInteger value = currentCost; // 빅인티저에 골드를 대입
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
        return f.ToString("N2") + GetUnitText2(numList.Count - 1);
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
    }
   
    private void Update()
    {
        UpdateUI();
    }
}
