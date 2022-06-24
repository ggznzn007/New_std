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

public class SpawnManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject RedTeamPlayer;
    [SerializeField] GameObject BlueTeamPlayer;
    [SerializeField] GameObject host;
    
    void Start()
    {
        if(PN.IsConnectedAndReady)
        {
            if (PN.IsMasterClient)
            {
                
                Transform hostSpot = GameObject.Find("HostSpot").GetComponent<Transform>();
                host = PN.Instantiate("Host Player", hostSpot.position, hostSpot.rotation, 0);
                Debug.Log("호스트 접속완료");
            }
            else
            {
                Transform[] RedTeamSpots = GameObject.Find("RedTeamSpots").GetComponentsInChildren<Transform>();
                Transform[] BlueTeamSpots = GameObject.Find("BlueTeamSpots").GetComponentsInChildren<Transform>();
                int red = Random.Range(0, RedTeamSpots.Length);
                int Blue = Random.Range(0, BlueTeamSpots.Length);
                int num = Random.Range(1, 7);
                if (num % 2 == 1) // 홀수 레드
                {
                    RedTeamPlayer = PN.Instantiate("RedTeamPlayer", RedTeamSpots[red].position, RedTeamSpots[red].rotation, 0);
                    Debug.Log("레드팀 플레이어 접속완료");
                }
                else  // 짝수 블루
                {
                    BlueTeamPlayer = PN.Instantiate("BlueTeamPlayer", BlueTeamSpots[Blue].position, BlueTeamSpots[Blue].rotation, 0);
                    Debug.Log("블루팀 플레이어 접속완료");
                }
            }
        }
       /* int num = Random.Range(1, 8);
        PN.NickName = "플레이어 " + num; // 플레이어 이름
        if (PN.IsConnectedAndReady)
        {
            Transform hostSpot = GameObject.Find("HostSpot").GetComponent<Transform>();
            Transform[] RedTeamSpots = GameObject.Find("RedTeamSpots").GetComponentsInChildren<Transform>();
            Transform[] BlueTeamSpots = GameObject.Find("BlueTeamSpots").GetComponentsInChildren<Transform>();
            int red = Random.Range(0, RedTeamSpots.Length);
            int Blue = Random.Range(0, BlueTeamSpots.Length);
            
           if(num % 2 == 1)
            {
                PN.Instantiate(RedTeamPlayer.name, RedTeamSpots[red].position, RedTeamSpots[red].rotation, 0);
            }
           else
            {
                PN.Instantiate(BlueTeamPlayer.name, BlueTeamSpots[Blue].position, BlueTeamSpots[Blue].rotation, 0);
            }

            PN.Instantiate(host.name, hostSpot.position, hostSpot.rotation, 0);
        }*/
    }    
    void Update()
    {
        
    }
}
