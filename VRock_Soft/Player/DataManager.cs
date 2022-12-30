using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using Antilatency;
using Antilatency.TrackingAlignment;
using Antilatency.DeviceNetwork;
using Antilatency.Alt;
using Antilatency.SDK;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;
public enum Team    // 팀 선택 데이터
{
   ADMIN,BLUE,RED
}

public enum Map    // 맵 선택 데이터
{
    TUTORIAL_T,
    TOY,
    TUTORIAL_W,
    WESTERN
}

public class DataManager : MonoBehaviourPun // == PlayerNetwork
{
    public static DataManager DM;    

    public Team currentTeam;  // 현재 팀구분

    public Map currentMap;    // 현재 맵구분

    public int startingNum = 0;   // 처음 시작을 구분하기위한 수

    public bool isSelected;   // 팀을 선택했는지 여부

    public bool inGame;       // 게임 중인지 여부    

    //public bool grabBomb;     // 폭탄을 집었는지 여부

    //public bool grabGun;      // 총을 집었는지 여부

    //public bool grabShield;      // 방패를 집었는지 여부

    public string nickName;

    public int teamInt;       // 팀 숫자 0=블루팀, 1=레드팀

    //public GameObject myGun = null;
    private void Awake()
    {
        if (DM == null) DM = this;
        else if (DM != null) return;
        DontDestroyOnLoad(gameObject);
    }
}
