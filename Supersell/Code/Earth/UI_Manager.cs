using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public Slider widthSlider;   
    public Slider heightSlider;
    public GameObject ui_Rect;

    void Start()
    {

       // widthSlider.onValueChanged.AddListener(delegate { WidthValueChange(); }); 
    }

    
    public void WidthValueChange()
    {      
        //widthSlider.value = width;
    }
}
