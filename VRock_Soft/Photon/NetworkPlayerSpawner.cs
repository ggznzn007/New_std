using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnPlayerPrefab;
    /*[SerializeField] Transform[] spawnPoints;
    public int GetIndex
    {
        get
        {
            for (int i = 0; i < PN.PlayerList.Length; i++)
            {
                if (PN.PlayerList[i] == PN.LocalPlayer)
                    return i;
            }
            return -1;
        }
    }
*/
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        spawnPlayerPrefab = PN.Instantiate("Player", transform.position, transform.rotation);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PN.Destroy(spawnPlayerPrefab);
    }
}
