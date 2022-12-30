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
public enum Team    // �� ���� ������
{
   ADMIN,BLUE,RED
}

public enum Map    // �� ���� ������
{
    TUTORIAL_T,
    TOY,
    TUTORIAL_W,
    WESTERN
}

public class DataManager : MonoBehaviourPun // == PlayerNetwork
{
    public static DataManager DM;    

    public Team currentTeam;  // ���� ������

    public Map currentMap;    // ���� �ʱ���

    public int startingNum = 0;   // ó�� ������ �����ϱ����� ��

    public bool isSelected;   // ���� �����ߴ��� ����

    public bool inGame;       // ���� ������ ����    

    //public bool grabBomb;     // ��ź�� �������� ����

    //public bool grabGun;      // ���� �������� ����

    //public bool grabShield;      // ���и� �������� ����

    public string nickName;

    public int teamInt;       // �� ���� 0=�����, 1=������

    //public GameObject myGun = null;
    private void Awake()
    {
        if (DM == null) DM = this;
        else if (DM != null) return;
        DontDestroyOnLoad(gameObject);
    }
}
