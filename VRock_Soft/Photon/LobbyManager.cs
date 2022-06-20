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

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField PlayerName_InputName;
    [SerializeField] GameObject connectUI;   
    [SerializeField] GameObject selectRoomUI;   
    [SerializeField] GameObject roomManager;        
    private string gameVersion = "1.0"; // ���� ����
   
    #region Unity Methods   
    void Start()
    {
        this.gameObject.SetActive(true);
        connectUI.SetActive(true);
        roomManager.SetActive(false);
        selectRoomUI.SetActive(false);
      
    }        
    void Update()
    {
        // Return == Ű���� ���� 
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0))
        {
            Connect();
        }
    }

    #endregion

    #region UI Callback Methods
    public void ConnectAnonymously() // �͸����� ����
    {
        // ���ӿ� �ʿ��� ����(���� ����) ����
        PN.GameVersion = gameVersion;
        // ������ ������ ������ ������ ���� ���� �õ�
        PN.ConnectUsingSettings();
        if (PN.ConnectUsingSettings())
        {
            this.gameObject.SetActive(false);
            connectUI.SetActive(false);
            roomManager.SetActive(true);
            selectRoomUI.SetActive(true);
        }

    }

    public void ConnectToPhotonServer()
    {
        if (PlayerName_InputName != null)
        {
            PN.NickName = PlayerName_InputName.text;
            PN.ConnectUsingSettings();
        }
    }

    #endregion

    #region Photon Callback Methods
    public void Connect()
    {
        //joinButton.interactable = false;
        if (PN.IsConnected)
        {
            // �� ���� ����
            print("�濡 ������...");
            PN.JoinRandomRoom();
        }
        else
        {
            // ������ ������ �������� �ƴ϶��, ������ ������ ���� �õ�
            print("������ ������� �ʾҽ��ϴ�.\n���� ��õ� ��...");
            // ������ �������� ������ �õ�
            PN.ConnectUsingSettings();
        }

    }
    public override void OnConnected()
    {
        base.OnConnected();
        print("�������ӿ� �����߽��ϴ�!!!");
    }

    public override void OnConnectedToMaster()
    {
        print("������ ����� �����Դϴ�.\n ������ �÷��̾�� : " +PN.NickName+"�Դϴ�.");
        this.gameObject.SetActive(false);
        connectUI.SetActive(false);
        roomManager.SetActive(true);
        selectRoomUI.SetActive(true);
        
    }
    // ������ ���� ���� ���н� �ڵ� ����
    public override void OnDisconnected(DisconnectCause cause)
    {
        // �� ���� ��ư�� ��Ȱ��ȭ
        //joinButton.interactable = false;
        // ���� ���� ǥ��
       print("�������ӿ� �����߽��ϴ�.\n���� ��õ� ��...");

        // ������ �������� ������ �õ�
        PN.ConnectUsingSettings();
    }
    // (�� ���� ����)���� �� ������ ������ ��� �ڵ� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ���� ���� ǥ��
        print("�� ���� ����, ���ο� �� ����...");
        // �ִ� �ο� ���� ������ ����� ����
        PN.CreateRoom(null, new RoomOptions { MaxPlayers = 6 });
    }
    // �뿡 ���� �Ϸ�� ��� �ڵ� ����
    public override void OnJoinedRoom()
    {
        // ���� ���� ǥ��
        print("�� ���� ����");
        // ��� �� �����ڵ��� Main ���� �ε��ϰ� ��

        
        //PhotonNetwork.LoadLevel("ChatMain");
    }

    #endregion
}
