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
using System.Security.Cryptography;
using Unity.VisualScripting;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using System.Linq;

public class ScoreBoard : MonoBehaviourPunCallbacks
{
    public Transform holder_Blue;
    public Transform holder_Red;
    public GameObject listMember;
    //public List<PlayerStats> playerList;
    Dictionary<Player, ScoreBoard_Member> members = new Dictionary<Player, ScoreBoard_Member>();

  


    private void Start()
    {
        foreach (Player player in PN.PlayerList.ToArray())
        {
            AddMember(player);
            photonView.RPC("AddMember", RpcTarget.All, player);
        }
    }

    /* ScoreBoard_Member Listing = Instantiate(listMember, holder_Blue).GetComponent<ScoreBoard_Member>();
     Listing.InitText(player);
     members[player] = Listing;*/
    [PunRPC]
    void AddMember(Player player)
    {
        ScoreBoard_Member Listing = Instantiate(listMember).GetComponent<ScoreBoard_Member>();
        Listing.InitText(player);
        members[player] = Listing;

        if (DataManager.DM.currentTeam == Team.BLUE)
        {            
            Listing.transform.SetParent(holder_Blue.transform);
        }
        else
        {            
            Listing.transform.SetParent(holder_Red.transform);
        }



    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddMember(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveMember(otherPlayer);
    }


    void RemoveMember(Player player)
    {
        Destroy(members[player].gameObject);
        members.Remove(player);
    }

    /*public void SortRank()
    {
        for (int i = 0; i < players.Count; i++)
        {
            for (int j = i+1; j < players.Count; j++)
            {
                if (players[j].killsRef > players[i].killsRef)
                {
                    ScoreBoard_Blue tmp = players[i];
                    players[i] = players[j];
                    players[j] = tmp;
                }
            }
        }

    }*/
}
