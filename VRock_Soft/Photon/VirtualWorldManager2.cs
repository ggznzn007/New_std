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

public class VirtualWorldManager2 : MonoBehaviourPunCallbacks
{
    public static VirtualWorldManager2 virtualWorldManager2;


    private void Awake()
    {

        if (virtualWorldManager2 != null && virtualWorldManager2 != this)
        {
            Destroy(this.gameObject);
        }
        virtualWorldManager2 = this;
    }

    private void Update()
    {
        if (!PN.IsConnected)
            LeaveRoomAndLoadRoomScene2();
    }
    public void LeaveRoomAndLoadRoomScene2()
    {
        if (PN.InRoom)
        {
            if (PN.IsMasterClient && PN.CurrentRoom.PlayerCount > 1)
            {
                MigrateMaster2();
            }
            else
            {
                //PN.Destroy(RedTeam);                
                // PN.Destroy(BlueTeam);
                
                PN.LeaveRoom();
            }
        }
    }

    #region Photon Callback Methods
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + "님이 접속하였습니다." + "접속자 수:" + PN.CurrentRoom.PlayerCount);
    }

    public override void OnLeftRoom()
    {
        // PN.LoadLevel("StartScene2");
        
        SceneManager.LoadScene("StartScene");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //PN.LoadLevel("StartScene2");
    }
    #endregion

    private void MigrateMaster2()
    {
        var dict = PN.CurrentRoom.Players;
        if (PN.SetMasterClient(dict[dict.Count - 1]))
        {
            
            PN.LeaveRoom();
        }
    }

}
