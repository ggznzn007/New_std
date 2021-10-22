using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;

    public GameObject battleButton;
    public GameObject cancelButton;
    public Text connectionInfoText;

    private void Awake()
    {
        lobby = this;// ΩÃ±€≈Ê
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        battleButton.SetActive(true);
        connectionInfoText.text = "Player has Connected to Master Server";
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnBattleButtonClicked()
    {
        battleButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "Tried to join a random game but failed. There must be no open games available";
        CreateRoom();
    }

    void CreateRoom()
    {
        int randomRoomName = Random.Range(0, 1000);
        // PhotonNetwork.CreateRoom(null, new RoomOptions());
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiPlayerSetting.multiplayerSetting.maxPlayers};
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "Tried to create a new room but failed. There must already be a room with the same name";
        CreateRoom();

    }

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "We are now in a room";

    }

    public void OnCancelButtonClicked()
    {
        cancelButton.SetActive(false);
        battleButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
