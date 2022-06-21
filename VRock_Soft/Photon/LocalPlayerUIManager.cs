using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PhotonNetwork;
using Random = UnityEngine.Random;
using TMPro;

public class LocalPlayerUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject GoHome_Button;
    void Start()
    {
        GoHome_Button.GetComponent<Button>().onClick.AddListener(VirtualWorldManager.Instance.LeaveRoomAndLoadRoomScene);
    }

    
}
