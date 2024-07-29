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
        goldDisplayer.text = " �� ���: " + DataController.Instance.GetCommaGold()+"��";
        goldPerClickDisplayer.text = " Ŭ���� �߰��ݾ�: " + DataController.Instance.GetCommaClick() + "��";
        goldPerSecDisplayer.text = " �ʴ� �߰��ݾ�: " + DataController.Instance.GetCommaSec() + "��";
    }    
}
