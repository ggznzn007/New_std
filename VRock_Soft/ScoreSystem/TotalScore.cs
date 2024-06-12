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

public class TotalScore : MonoBehaviourPunCallbacks//, IPunObservable
{
    public static TotalScore TS;
    public TMP_Text blueScore;
    public TMP_Text redScore;
    public int score_Blue;
    public int score_Red;
    //private PhotonView PV;
    private void Awake()
    {
        TS = this;
       // PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        blueScore.text = score_Blue.ToString();
        redScore.text = score_Red.ToString();
    }

    public void AddScoreBlue(int scorePlus)
    {
        score_Blue += scorePlus;
    }

    public void AddScoreRed(int scorePlus)
    {
        score_Red += scorePlus;
    }

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(blueScore);
            stream.SendNext(redScore);
        }
        else
        {
            blueScore.text = (string)stream.ReceiveNext();
            redScore.text = (string)stream.ReceiveNext();
        }
    }*/
}
