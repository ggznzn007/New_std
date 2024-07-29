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
        goldDisplayer.text = " 총 재산: " + DataController.Instance.GetCommaGold()+"원";
        goldPerClickDisplayer.text = " 클릭당 추가금액: " + DataController.Instance.GetCommaClick() + "원";
        goldPerSecDisplayer.text = " 초당 추가금액: " + DataController.Instance.GetCommaSec() + "원";
    }    
}
