using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSetter : MonoBehaviour
{
    public Text text;
    
    void Start()
    {
        text.font = UI_Setting.Instance.defaultFont;
        text.fontSize = UI_Setting.Instance.defaultFontSize;
        text.color = UI_Setting.Instance.defaultFontColor;
    }
}
