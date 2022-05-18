using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;

public class UpgradeButton : MonoBehaviour
{
    public TextMeshProUGUI upgradeDisplayer;

    public string upgradeName;

    [HideInInspector] // �ν����� �󿡼� ���� ����
    public BigInteger goldByUpgrade;
   
    public BigInteger startGoldByUpgrade = 1; // ���� ���� �� ���ʰ�

    [HideInInspector]
    public BigInteger currentCost = 1;

    public BigInteger startCurrentCost = 1;

    [HideInInspector]
    public BigInteger level = 1;

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

    
    public void PurchaseUpgrade()
    {
        if(DataController.Instance.Gold>=currentCost)
        {
            DataController.Instance.Gold -=currentCost;
            level++;
            DataController.Instance.GoldPerClick+=goldByUpgrade;

            UpdateUpgrade();
            UpdateUI();
            DataController.Instance.SaveUpgradeButton(this);
        }
    }

    public void UpdateUpgrade()
    {
        goldByUpgrade += startGoldByUpgrade * (BigInteger)Mathf.Pow(upgradePow, (int)level);
        currentCost += startCurrentCost * (BigInteger)Mathf.Pow(costPow, (int)level);
    }

    
    public void UpdateUI()
    {        
        upgradeDisplayer.text ="  \t"+upgradeName + " Level " + (int)level 
            + "\n\n���űݾ�: " + GetCurrentCostText1(currentCost) +
        "\nŬ���� �߰��ݾ�:\n " + DataController.Instance.GetGoldText(DataController.Instance.GoldPerClick);
    }

    public string GetCurrentCostText1(BigInteger data) // ��� ǥ�� ������ �Ҽ��� ���� ǥ���ϴ� �޼���
    {
        int placeN = 3; // ���ڸ� ������ ��� ǥ��
        BigInteger value = data; // ����Ƽ���� ��带 ����
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
        return f.ToString("N2") + GetUnitText1(numList.Count - 1);
    }

    private string GetUnitText1(int index)
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
}
