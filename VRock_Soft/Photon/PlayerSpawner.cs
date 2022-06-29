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
public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject player;
    int[] nums = { 1, 1, 2, 2, 3, 3 };
    private void Start()
    {

    }

    public override void OnJoinedRoom()
    {       
        if (PN.IsConnectedAndReady)
        {
            switch (PN.CurrentRoom.PlayerCount)
            {
                case 1:
                    PN.NickName = "VRock �����" + nums[0] + "�� Player";
                    break;
                case 2:
                    PN.NickName = "VRock ������" + nums[1] + "�� Player";
                    break;
                case 3:
                    PN.NickName = "VRock �����" + nums[2] + "�� Player";
                    break;
                case 4:
                    PN.NickName = "VRock ������" + nums[3] + "�� Player";
                    break;
                case 5:
                    PN.NickName = "VRock �����" + nums[4] + "�� Player";
                    break;
                case 6:
                    PN.NickName = "VRock ������" + nums[5] + "�� Player";
                    break;
            }
        }
        StartCoroutine(nameof(CreatePlayer));


    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PN.DestroyAll(player);

    }

    IEnumerator CreatePlayer()
    {
        yield return new WaitUntil(() => PN.IsConnected);
        if (!PN.IsConnected) { PN.ConnectUsingSettings(); }
                
        switch (PN.CurrentRoom.PlayerCount)
        {
            case 1:
            case 3:
            case 5:
                Transform[] BlueTeamSpots = GameObject.Find("BlueTeamSpots").GetComponentsInChildren<Transform>();
                int blueSpawnspot = Random.Range(0, BlueTeamSpots.Length);
                PN.Instantiate("BlueTeamPlayer", BlueTeamSpots[blueSpawnspot].position, BlueTeamSpots[blueSpawnspot].rotation, 0);
                //GameObject.Find("BlueTeamPlayer(Clone)/Avatar/Body").GetComponent<MeshRenderer>().materials[0].color = Color.blue;
                Debug.Log($"{PN.NickName} ���������� �����Ϸ�");

                break;
            case 2:
            case 4:
            case 6:
                Transform[] RedTeamSpots = GameObject.Find("RedTeamSpots").GetComponentsInChildren<Transform>();
                int redSpawnspot = Random.Range(0, RedTeamSpots.Length);
                PN.Instantiate("RedTeamPlayer", RedTeamSpots[redSpawnspot].position, RedTeamSpots[redSpawnspot].rotation, 0);
                //GameObject.Find("RedTeamPlayer(Clone)/Avatar/Body").GetComponent<MeshRenderer>().materials[0].color = Color.red;
                Debug.Log($"{PN.NickName} ���������� �����Ϸ�");

                break;

        }
    }

}

