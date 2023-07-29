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

public class ScoreBoard_Member : MonoBehaviourPunCallbacks
{
    public TMP_Text usernameText;
    public TMP_Text killsText;
    public TMP_Text deathsText;

    Player myplayer;

    public void InitText(Player player)
    {
        myplayer = player;
        usernameText.text = player.NickName;
    }

    private void Update()
    {
        int killsRef = (int)myplayer.CustomProperties["kills"];
        killsText.text = killsRef.ToString();
       /* int deathsRef = (int)myplayer.CustomProperties["deaths"];
        deathsText.text = deathsRef.ToString();*/
    }
}
