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
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class TutorialManager : MonoBehaviourPunCallbacks
{
    private GameObject spawnPlayer;
    
    public override void OnJoinedRoom()
    {
        if(NetworkManager.NM.isRed)
        {
            NetworkManager.NM.inGame = false;
            spawnPlayer =  PN.Instantiate("AltRed", Vector3.zero, Quaternion.identity);
            Debug.Log($"{PN.CurrentRoom.Name} 방에 {PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
        }
        else
        {
            NetworkManager.NM.inGame = false;
            spawnPlayer =  PN.Instantiate("AltBlue", Vector3.zero, Quaternion.identity);
            Debug.Log($"{PN.CurrentRoom.Name} 방에 {PN.LocalPlayer.NickName} 님이 입장하셨습니다.");
        }
    }

    [ContextMenu("포톤 서버 정보")]
    void Info()
    {
        if (PN.InRoom)
        {
            print("현재 방 이름: " + PN.CurrentRoom.Name);
            print("현재 방 인원 수: " + PN.CurrentRoom.PlayerCount);
            print("현재 방 MAX인원: " + PN.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록";
            for (int i = 0; i < PN.PlayerList.Length; i++)
            {
                playerStr += PN.PlayerList[i].NickName + ",";
                print(playerStr);
            }

        }
        else
        {
            print("접속한 인원 수: " + PN.CountOfPlayers);
            print("로비에 있는 여부: " + PN.InLobby);
            print("접속한 인원 수: " + PN.CountOfPlayers);
            print("서버 연결여부: " + PN.IsConnected);
        }
    }

    public override void OnLeftRoom()
    {
        if(PN.IsMasterClient)
        {
            PN.DestroyAll();
        }
        
        PN.Destroy(spawnPlayer);
       
        SceneManager.LoadScene(0);
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName}님 현재인원:{PN.CurrentRoom.PlayerCount}");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName}님 현재인원:{PN.CurrentRoom.PlayerCount}");
    }

}
