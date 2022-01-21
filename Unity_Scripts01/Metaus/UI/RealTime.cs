using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class RealTime : MonoBehaviour
{
    
    public Text dayTxt;
    public Text timeTxT;
   
    void Update()
    {
        GetCurrentDate();
    }

    public void GetCurrentDate()
    {
        string YearMonthAndDay = DateTime.Now.ToString(("yy³â MM¿ù ddÀÏ"));

        dayTxt.text = YearMonthAndDay;

        string dayTime = DateTime.Now.ToString("t");

        timeTxT.text = dayTime;

    }

    
}
