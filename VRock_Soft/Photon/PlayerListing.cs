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
using Antilatency;
using Antilatency.TrackingAlignment;
using Antilatency.DeviceNetwork;
using Antilatency.Alt;
using Antilatency.SDK;
public class PlayerListing : MonoBehaviour
{

    public Player Player { get; private set; }
   //public RoomInfo RoomInfo { get; private set; }

    public void SetPlayerInfo(Player player)
    {
        Player = player;
        
        Debug.Log($"{player.NickName }");
    }
}
