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
        goldDisplayer.text = " 총 자산: " + DataController.Instance.GetGoldText(DataController.Instance.Gold);
        goldPerClickDisplayer.text = " 클릭당 추가금액: " +
            DataController.Instance.GetGoldText(DataController.Instance.GoldPerClick);
        goldPerSecDisplayer.text = " 초당 추가금액: " +
            DataController.Instance.GetGoldText(DataController.Instance.GetGoldPerSec());
    }
}
