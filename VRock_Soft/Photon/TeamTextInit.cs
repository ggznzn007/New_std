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
public class TeamTextInit : MonoBehaviourPunCallbacks
{
    TextMeshPro teamTextInit;
    private string redTeamText = "·¹µåÆÀ";

    void Start()
    {
        teamTextInit = GetComponentInChildren<TextMeshPro>();
        

    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Cube")
        {
            teamTextInit.text = redTeamText;
            teamTextInit.color = Color.red;
            Debug.Log("ÅÂ±× ÆÀ±ÛÀÚ");
        }
    }
}
