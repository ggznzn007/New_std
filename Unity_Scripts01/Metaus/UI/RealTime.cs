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
        string YearMonthAndDay = DateTime.Now.ToString(("yy년 MM월 dd일"));

        dayTxt.text = YearMonthAndDay;

        string dayTime = DateTime.Now.ToString("t");

        timeTxT.text = "현재시간 : " + dayTime;

    }

    
}
