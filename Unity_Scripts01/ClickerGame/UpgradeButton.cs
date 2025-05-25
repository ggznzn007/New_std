using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    public TextMeshProUGUI upgradeDisplayer;

    public string upgradeName;

    [HideInInspector] // �ν����� �󿡼� ���� ����
    public int goldByUpgrade;

    public int startGoldByUpgrade = 1; // ���� ���� �� ���ʰ�

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

    public void UpgradeSE()
    {
        SoundController.instance.Playsound(SoundController.instance.upgradeClick);
    }

    public string GetCommaGold(int data)
    {
        return string.Format("{0:#,###}", data);
    }

    public void PurchaseUpgrade()
    {
        if (DataController.Instance.Gold >= currentCost)
        {
            DataController.Instance.Gold -= currentCost;
            level++;
            DataController.Instance.GoldPerClick += goldByUpgrade;

            UpdateUpgrade();
            UpdateUI();
            DataController.Instance.SaveUpgradeButton(this);
        }
    }

    public void UpdateUpgrade()
    {
        goldByUpgrade += startGoldByUpgrade * (int)Mathf.Pow(upgradePow, level);
        currentCost += startCurrentCost * (int)Mathf.Pow(costPow, level);
    }

    public void UpdateUI()
    {
        upgradeDisplayer.text = "  \t" + upgradeName + " Level " + level
            + "\n\n���űݾ�: " + GetCommaGold(currentCost) + "��"
            + "\nŬ���� �߰��ݾ�:\n " + GetCommaGold(goldByUpgrade) + "��";
    }
}
