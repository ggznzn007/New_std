using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.SceneManagement;
public class TimeManager : MonoBehaviourPunCallbacks
{
    bool startTimer = false;
    double timerIncrementValue;
    double startTime;
    [SerializeField] double timer = 20;
    ExitGames.Client.Photon.Hashtable CustomeValue;
   
    void Start()
    {
        if (PN.IsMasterClient)
        {
            CustomeValue = new ExitGames.Client.Photon.Hashtable();
            startTime = PN.ServerTimestamp;
            startTimer = true;
            CustomeValue.Add("StartTime", startTime);
           // PN.SetCustomProperties(CustomeValue);
        }
        else
        {
          //  startTime = double.Parse(PN.CustomProperties["StartTime"].ToString());
            startTimer = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
