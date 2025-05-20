using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroineButton : MonoBehaviour
{
    public TextMeshProUGUI itemDisplayer;

    public CanvasGroup canvasGroup;

    public Slider slider;

    public string itemName;

    public int level;

    [HideInInspector]
    public int currentCost = 1;

    public int startCurrentCost = 1;

    [HideInInspector]
    public int goldPerSec;

    public int startGoldPerSec = 1;

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
        if (DataController.Instance.Gold >= currentCost)
        {
            isPurchased = true;
            DataController.Instance.Gold -= currentCost;
            level++;
            UpdateItem();
            UpdateUI();
            DataController.Instance.SaveHeroineButton(this);
        }
    }

    IEnumerator AddGoldLoop()
    {
        while (true)
        {
            if (isPurchased)
            {
                DataController.Instance.Gold += goldPerSec;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    public void UpdateItem()
    {
        goldPerSec = (int)(goldPerSec) + startGoldPerSec * (int)Mathf.Pow(upgradePow, level);
        currentCost = (int)(currentCost * 0.6f) + startCurrentCost * (int)Mathf.Pow(costPow, level);
    }

    public void UpdateUI()
    {
        itemDisplayer.text = itemName + " Level " + level + "\n\n구매금액: " + GetCommaGold(currentCost) + "원" +
            "\n초당 추가금액: " + GetCommaGold(goldPerSec) + "원";

        slider.minValue = 0;
        slider.maxValue = currentCost;

        slider.value = DataController.Instance.Gold;

        if (isPurchased)
        {
            canvasGroup.alpha = 1.0f;
        }
        else
        {
            canvasGroup.alpha = 0.6f;
        }
    }

    public string GetCommaGold(int data)
    {
        return string.Format("{0:#,###}", data);
    }

    private void Update()
    {
        UpdateUI();
    }
}
