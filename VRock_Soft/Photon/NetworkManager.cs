using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PhotonNetwork;
using Random = UnityEngine.Random;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    /*public Transform spawnPoint;
    public GameObject player;*/
    [SerializeField] Vector3 playerPosition;
    [SerializeField] Quaternion playerRotation;
    [SerializeField] Transform[] spawnPoints;
    private void Start()
     {
         ConnectToServer();
         
    }

    void ConnectToServer() => PN.ConnectUsingSettings(); // ���� ������ ����    

     public override void OnConnectedToMaster() // ���� ������ ����Ǿ��� �� 
     {
         base.OnConnectedToMaster();
         print("���� ���� �Ϸ�");
         RoomOptions roomOptions = new RoomOptions(); // �� �Ҵ�
         roomOptions.MaxPlayers = 6;                  // �� �ο� ���� 
         roomOptions.IsVisible = true;                // �� ���־� ����
         roomOptions.IsOpen = true;                   // �� ���� ����

         PN.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default); // �κ񿡼� �� ����
         print("�游��� ���� �Ϸ�");
        // �� �����ϴµ� ���� ������ �����ϰ� ����

    }

     public override void OnJoinedRoom() // ��ȿ� ���� ��
     {
        //PN.Instantiate("NormalPlayer", new Vector3(-11.48f,4.03f,36.1f), Quaternion.identity, 0);  
        //PN.Instantiate("NormalPlayer", transform.position, Quaternion.identity,0);  // �÷��̾� ����
        CreatePlayer();
     }


    void CreatePlayer()
    {
       
        /*spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();

        Vector3 position = spawnPoints[PN.CurrentRoom.PlayerCount].position;
        Quaternion rotation = spawnPoints[PN.CurrentRoom.PlayerCount].rotation;*/

        GameObject playerClone = PN.Instantiate("NewPlayer", spawnPoints[Random.Range(0,6)].position, Quaternion.identity, 0);
        print("�÷��̾ ���������� ���������Ǿ����ϴ�.");
    }

    [ContextMenu("���� ���� ����")]
    void Info()
    {
        if(PN.InRoom)
        {
            print("���� �� �̸�: " + PN.CurrentRoom.Name);
            print("���� �� �ο� ��: " + PN.CurrentRoom.PlayerCount);
            print("���� �� MAX�ο�: " + PN.CurrentRoom.MaxPlayers);

            string playerStr = "�濡 �ִ� �÷��̾� ���";
            for (int i = 0; i < PN.PlayerList.Length; i++)
            {
                playerStr += PN.PlayerList[i].NickName + ",";
                print(playerStr);
            }

        }
        else
        {
            print("������ �ο� ��: " + PN.CountOfPlayers);
            print("�κ� �ִ� ����: " + PN.InLobby);
            print("������ �ο� ��: " + PN.CountOfPlayers);
            print("���� ���Ῡ��: " + PN.IsConnected);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        
    }

   

    /*  public override void OnPlayerEnteredRoom(Player newPlayer)
      {
          base.OnPlayerEnteredRoom(newPlayer);
      }*/

    /* string _room = "Tutorial";

     // Use this for initialization
     void Start()
     {

         Debug.Log("Network Controller Start");

         PN.ConnectUsingSettings();
     }

   public override void OnJoinedLobby()
     {
         Debug.Log("Joined Lobby");

         PN.JoinRandomRoom();

         //RoomOptions roomOptions = new RoomOptions() {};
         //PhotonNetwork.JoinOrCreateRoom(_room, roomOptions, TypedLobby.Default);
     }


     public override void OnJoinedRoom()
     {
         Debug.Log("Joined Room");

         PN.Instantiate("NormalPlayer", Vector3.one, Quaternion.identity, 0);
     }*/




}
