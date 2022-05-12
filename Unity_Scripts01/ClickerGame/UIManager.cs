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
        goldDisplayer.text = " �Ӵ�: " + DataController.Instance.gold+"��";
        goldPerClickDisplayer.text = " Ŭ���� ȹ��Ӵ�: " + DataController.Instance.goldPerClick + "��";
        goldPerSecDisplayer.text = " �ʴ� ȹ��Ӵ�: " + DataController.Instance.GetGoldPerSec() + "��";
    }

    
}
