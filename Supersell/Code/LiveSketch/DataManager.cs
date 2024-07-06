using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager DM;
    public int activeToco = 0;

    void Start()
    {
        DM = this;
        DontDestroyOnLoad(DM);
    }
}
