using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectFairy : GameManager
{
    public GameObject Temp;
    public SceneMoveManager sm;
    public void Select(int n)
    {        
        PlayerPrefs.SetInt("Fairy", n);        
        sm.SetLoginCanvas(Temp);
    }
}
