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

public class ScoreBoard_ParentR : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform holder;
    
    [SerializeField] GameObject listMember;      

    Dictionary<Player, ScoreBoard_Red> members = new Dictionary<Player, ScoreBoard_Red>();
    

    private void Start()
    {
        if(DataManager.DM.currentTeam==Team.RED)
        {
            foreach (Player player in PN.PlayerList)
            {
                if (DataManager.DM.currentTeam != Team.RED) return;
                AddMember(player);
            }
        }
        
       
    }

    void AddMember(Player player)
    {
        ScoreBoard_Red Listing = Instantiate(listMember, holder).GetComponent<ScoreBoard_Red>();
        Listing.InitText(player);
        members[player] = Listing;

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
  
}
