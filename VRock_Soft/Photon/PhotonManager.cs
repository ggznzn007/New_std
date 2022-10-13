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
public class PhotonManager : MonoBehaviourPunCallbacks
{
    //public static PhotonManager PUN2;
   // private readonly string version = "1.0"; // ���� ���� �Է� == ���� ������ �������� �������
    [SerializeField] GameObject lobbyPlayer;
    [SerializeField] GameObject selectUI;

    #region ����Ƽ �Լ�
    private void Awake()
    {
       /* PN.AutomaticallySyncScene = true;
        PN.GameVersion = version;
        Debug.Log($"������ ���Ƚ�� �ʴ� : {PN.SendRate}");                            // ���� ������ ��� Ƚ�� ����. �ʴ� 30ȸ     
        PN.ConnectUsingSettings();*/
       
    }

    public void SelectRed()
    {
        PN.LoadLevel("LobbyScene");
    }
    private void Start()
    {
        
    }

    void Update()
    {

       
    }
       
    #endregion

    #region ���� �ݹ� �Լ�

    public override void OnConnected()
    {
        Debug.Log("OnConnect ȣ��Ϸ�. ���� ��밡��");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("������ ������ ���� ����");
        Debug.Log($"{PN.InLobby}");
        PN.JoinLobby();                                           // �κ� ����     
    }

    public override void OnJoinedLobby()
    {
        selectUI.SetActive(true);
    }

    

    #endregion




}
