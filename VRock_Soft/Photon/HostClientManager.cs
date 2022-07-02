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
public class HostClientManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PN.SetMasterClient(PN.MasterClient);
    }
}
