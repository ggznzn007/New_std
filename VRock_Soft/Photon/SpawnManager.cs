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

public class SpawnManager : MonoBehaviourPunCallbacks                                  // LobbyScene_Real ��ũ��Ʈ
{
    //public static SpawnManager SpawnPlayer;
    [SerializeField] GameObject BlueTeam;
    [SerializeField] GameObject RedTeam;
    private GameObject player;



    private void Awake()
    {
        /*if (SpawnPlayer != null && SpawnPlayer != this)
        {
            Destroy(this.gameObject);
        }
        SpawnPlayer = this;*/


    }

    private void Start()
    {
        if (PN.IsConnectedAndReady && StartManager.NetWorkMgr.isRed)
        {
            SpawnRedPlayer();
            PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        }
        else
        {
            SpawnBluePlayer();
            PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        }

    }

    private void Update()
    {
       /* if (!PN.InRoom)
        {
            LeaveRoomAndLoadRoomScene();
        }*/
    }

    public void SpawnRedPlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        }

        RedTeam = PN.Instantiate(RedTeam.name, Vector3.zero, Quaternion.identity, 0);
        player = RedTeam;
        /* string nickAlt = PN.NickName;
         Debug.Log($"{nickAlt} ���������� �����Ϸ�");*/

        foreach (var player in PN.CurrentRoom.Players)
        {
            player.Value.NickName = PN.LocalPlayer.NickName;
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}��"); // $ == String.Format() ���� 
        }
    }
    public void SpawnBluePlayer()
    {
        if (!PN.IsConnected)
        {
            PN.ConnectUsingSettings();
            PN.AutomaticallySyncScene = true;                                           // ���� ���� �����鿡�� �ڵ����� �� ����ȭ 
        }

        BlueTeam = PN.Instantiate(BlueTeam.name, Vector3.zero, Quaternion.identity, 0);
        player = BlueTeam;
        /*string nickAlt = PN.NickName;
        Debug.Log($"{nickAlt} ���������� �����Ϸ�");*/

        foreach (var player in PN.CurrentRoom.Players)
        {
            player.Value.NickName = PN.LocalPlayer.NickName;
            Debug.Log($"UserID :  {player.Value.NickName}\n\t     ActorNumber : {player.Value.ActorNumber}��"); // $ == String.Format() ���� 
        }
    }
    public void GunShootingStart()
    {
        if (PN.IsMasterClient)
        {
            PN.LoadLevel("GunShooting");
        }

    }


   /* #region �泪���� �Լ�

    public void LeaveRoomAndLoadRoomScene()
    {
        PN.Disconnect();
        //DestroyImmediate(player);

    }


    public override void OnLeftRoom()
    {       
        DestroyImmediate(player);
        SceneManager.LoadScene("StartScene");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // PN.LoadLevel("StartScene");
        SceneManager.LoadScene("StartScene");
    }

    #endregion*/

}
