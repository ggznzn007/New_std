using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using TMPro;

public class LocalPlayerUIManager : MonoBehaviour
{
    [SerializeField]
     GameObject Back_Button;
    
    void Start()
    {
        Back_Button.GetComponent<Button>().onClick.AddListener(VirtualWorldManager.Instance.LeaveRoomAndLoadRoomScene);
        
    }

    
}
