using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PhotonNetwork;
using Random = UnityEngine.Random;
using TMPro;
public class VirtualWorldManager : MonoBehaviourPunCallbacks
{
    public static VirtualWorldManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
    }

    public void LeaveRoomAndLoadRoomScene()
    {
        PN.LeaveRoom();
    }

    #region Photon Callback Methods
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print(newPlayer.NickName + "님이 접속하였습니다." + "접속자 수:" + PN.CurrentRoom.PlayerCount);
    }

    public override void OnLeftRoom()
    {
        PN.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PN.LoadLevel("LobbyScene");
    }
    #endregion

    public void BackToLobby()
    {
        PN.LeaveRoom();
        PN.Disconnect();
        PN.LoadLevel("LobbyScene");
    }
}
