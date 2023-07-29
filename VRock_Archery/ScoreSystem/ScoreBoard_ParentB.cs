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

public class ScoreBoard_ParentB : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform holder;
    
    [SerializeField] GameObject listMember;     

    Dictionary<Player, ScoreBoard_Blue> members = new Dictionary<Player, ScoreBoard_Blue>();
    
    private void Start()
    {     
        foreach (Player player in PN.PlayerList)
        {
            AddMember(player);
        }
    }

    void AddMember(Player player)
    {
        ScoreBoard_Blue Listing = Instantiate(listMember, holder).GetComponent<ScoreBoard_Blue>();
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
