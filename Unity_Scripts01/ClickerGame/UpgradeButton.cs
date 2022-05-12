using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    public TextMeshProUGUI upgradeDisplayer;

    public string upgradeName;

    [HideInInspector] // 인스펙터 상에서 값을 숨김
    public int goldByUpgrade;
   
    public int startGoldByUpgrade = 1; // 게임 시작 시 기초값

    [HideInInspector]
    public int currentCost = 1;

    public int startCurrentCost = 1;

    [HideInInspector]
    public int level = 1;

    public float upgradePow = 1.5f;

    public float costPow = 2.14f;

    private void Start()
    {
        DataController.Instance.LoadUpgradeButton(this);
        UpdateUI();
    }

    public void PurchaseUpgrade()
    {
        if(DataController.Instance.gold>=currentCost)
        {
            DataController.Instance.gold -=currentCost;
            level++;
            DataController.Instance.goldPerClick+=goldByUpgrade;

            UpdateUpgrade();
            UpdateUI();
            DataController.Instance.SaveUpgradeButton(this);
        }
    }

    public void UpdateUpgrade()
    {
        goldByUpgrade = startGoldByUpgrade * (int)Mathf.Pow(upgradePow, level);
        currentCost = startCurrentCost * (int)Mathf.Pow(costPow, level);
    }

    public void UpdateUI()
    {
        upgradeDisplayer.text = upgradeName + "\n\n비용: " + currentCost + "원" + "\n레벨: " + level +
            "\n클릭당 획득머니추가:\n " + goldByUpgrade+"원";
    }
}
