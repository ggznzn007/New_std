using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButton : MonoBehaviour
{
    public TextMeshProUGUI itemDisplayer;

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
        DataController.GetInstance().LoadItemButton(this);

        StartCoroutine("AddGoldLoop");
        UpdateUI();
    }
    public void PurchaseItem()
    {
        if(DataController.GetInstance().GetGold()>=currentCost)
        {
            isPurchased = true;
            DataController.GetInstance().SubGold(currentCost);
            level++;
            UpdateItem();
            UpdateUI();
            DataController.GetInstance().SaveItemButton(this);
        }
    }

    IEnumerator AddGoldLoop()
    {
        while(true)
        {
            if(isPurchased)
            {
                DataController.GetInstance().AddGold(goldPerSec);
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
        itemDisplayer.text = itemName + "\nLevel : " + level + "\nCost : " + currentCost +
            "\nGold Per Sec : " + goldPerSec +"\nIsPurchased : " + isPurchased;
    }
}
