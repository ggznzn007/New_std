using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI goldDisplayer;
    public TextMeshProUGUI goldPerClickDisplayer;
    public TextMeshProUGUI goldPerSecDisplayer;

    private void Update()
    {
        goldDisplayer.text = " �� �ڻ�: " + DataController.Instance.GetGoldText(DataController.Instance.Gold);
        goldPerClickDisplayer.text = " Ŭ���� �߰��ݾ�: " +
            DataController.Instance.GetGoldText(DataController.Instance.GoldPerClick);
        goldPerSecDisplayer.text = " �ʴ� �߰��ݾ�: " +
            DataController.Instance.GetGoldText(DataController.Instance.GetGoldPerSec());
    }
}
