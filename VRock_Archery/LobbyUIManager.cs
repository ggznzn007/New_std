using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIManager : MonoBehaviour
{

   /* [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject selectPanel;*/
    [SerializeField] GameObject generalPlayer;
    void Start()
    {
        /*gamePanel.SetActive(false);
        selectPanel.SetActive(true);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GeneralPlayer()
    {
       
    }

    public void TeamSelectedUI()
    {
        generalPlayer.SetActive(false);
        /*selectPanel.SetActive(false);
        gamePanel.SetActive(true);*/
    }
}
