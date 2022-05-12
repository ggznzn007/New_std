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
        goldDisplayer.text = " ¸Ó´Ï: " + DataController.Instance.gold+"¿ø";
        goldPerClickDisplayer.text = " Å¬¸¯´ç È¹µæ¸Ó´Ï: " + DataController.Instance.goldPerClick + "¿ø";
        goldPerSecDisplayer.text = " ÃÊ´ç È¹µæ¸Ó´Ï: " + DataController.Instance.GetGoldPerSec() + "¿ø";
    }

    
}
