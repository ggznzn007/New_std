using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI goldDisplayer;
    public TextMeshProUGUI goldPerClickDisplayer;

    public DataController dataController;

    private void Update()
    {
        goldDisplayer.text = "GOLD : " + dataController.GetGold();
        goldPerClickDisplayer.text = "GOLD PER CLICK : " + dataController.GetGoldPerClick();
    }
}
