using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;

public enum Team
{
    BLUE,RED
}

public class DataManager : MonoBehaviourPun
{
    public static DataManager DM;

    private void Awake()
    {
        if(DM == null)DM=this;
        else if (DM !=null) return;
        DontDestroyOnLoad(gameObject);
    }

    public Team currentTeam;

    public bool isSelected;

    public int startingNum;
}
