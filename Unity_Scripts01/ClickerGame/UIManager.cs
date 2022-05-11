using System.Collections;
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
        goldDisplayer.text = "GOLD : " + DataController.GetInstance().GetGold();
        goldPerClickDisplayer.text = "GOLD PER CLICK : " + DataController.GetInstance().GetGoldPerClick();
        goldPerSecDisplayer.text = "GOLD PER SEC : " + DataController.GetInstance().GetGoldPerSec();
    }
}
