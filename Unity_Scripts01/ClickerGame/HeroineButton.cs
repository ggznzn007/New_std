using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroineButton : MonoBehaviour
{
    public Color upgradableColor = Color.blue;

    public Color notupgradableColor = Color.red;

    public Image colorImage;

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
    public void PurchaseItem()
    {
        if(DataController.Instance.gold >=currentCost)
        {
            isPurchased = true;
            DataController.Instance.gold -=currentCost;
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
                DataController.Instance.gold+=goldPerSec;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    public void UpdateItem()
    {
        goldPerSec = (int)(goldPerSec) + startGoldPerSec * (int)Mathf.Pow(upgradePow, level);
        currentCost = (int)(currentCost*0.5f) + startCurrentCost * (int)Mathf.Pow(costPow, level);
        
    }

    public void UpdateUI()
    {
        itemDisplayer.text = itemName + "\nLevel: " + level + "\nCost: " + currentCost +
            "\nGold Per Sec: " + goldPerSec;

        slider.minValue = 0;
        slider.maxValue = currentCost;

        slider.value = DataController.Instance.gold;

        if(isPurchased)
        {
            canvasGroup.alpha = 1.0f;
        }
        else
        {
            canvasGroup.alpha = 0.6f;
        }

        if(currentCost <= DataController.Instance.gold)
        {
            colorImage.color = upgradableColor;
        }
        else
        {
            colorImage.color = notupgradableColor; 
        }
    }

    private void Update()
    {
        UpdateUI();
    }
}
