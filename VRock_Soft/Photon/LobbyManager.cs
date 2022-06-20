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
    private string gameVersion = "1.0"; // 게임 버전
   
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
        // Return == 키보드 엔터 
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0))
        {
            Connect();
        }
    }

    #endregion

    #region UI Callback Methods
    public void ConnectAnonymously() // 익명으로 연결
    {
        // 접속에 필요한 정보(게임 버전) 설정
        PN.GameVersion = gameVersion;
        // 설정한 정보를 가지고 마스터 서버 접속 시도
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
            // 룸 접속 실행
            print("방에 접속중...");
            PN.JoinRandomRoom();
        }
        else
        {
            // 마스터 서버에 접속중이 아니라면, 마스터 서버에 접속 시도
            print("서버와 연결되지 않았습니다.\n접속 재시도 중...");
            // 마스터 서버로의 재접속 시도
            PN.ConnectUsingSettings();
        }

    }
    public override void OnConnected()
    {
        base.OnConnected();
        print("서버접속에 성공했습니다!!!");
    }

    public override void OnConnectedToMaster()
    {
        print("서버와 연결된 상태입니다.\n 접속한 플레이어는 : " +PN.NickName+"입니다.");
        this.gameObject.SetActive(false);
        connectUI.SetActive(false);
        roomManager.SetActive(true);
        selectRoomUI.SetActive(true);
        
    }
    // 마스터 서버 접속 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        // 룸 접속 버튼을 비활성화
        //joinButton.interactable = false;
        // 접속 정보 표시
       print("서버접속에 실패했습니다.\n접속 재시도 중...");

        // 마스터 서버로의 재접속 시도
        PN.ConnectUsingSettings();
    }
    // (빈 방이 없어)랜덤 룸 참가에 실패한 경우 자동 실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 접속 상태 표시
        print("빈 방이 없음, 새로운 방 생성...");
        // 최대 인원 수용 가능한 빈방을 생성
        PN.CreateRoom(null, new RoomOptions { MaxPlayers = 6 });
    }
    // 룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom()
    {
        // 접속 상태 표시
        print("방 참가 성공");
        // 모든 룸 참가자들이 Main 씬을 로드하게 함

        
        //PhotonNetwork.LoadLevel("ChatMain");
    }

    #endregion
}
