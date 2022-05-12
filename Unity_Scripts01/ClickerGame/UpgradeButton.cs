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
        upgradeDisplayer.text = upgradeName + "\n\n���: " + currentCost + "��" + "\n����: " + level +
            "\nŬ���� ȹ��Ӵ��߰�:\n " + goldByUpgrade+"��";
    }
}
