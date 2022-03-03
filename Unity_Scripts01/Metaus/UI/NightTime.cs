using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class NightTime : MonoBehaviour
{
   
    private void Awake()
    {
        NightTimePlayerSpawn();
    }
    public void ClickQuit()
    {

        LoadingUIController.Instance.LoadScene("Main");
        ((PlayGamesPlatform)Social.Active).SignOut();

    }
    public void NightTimePlayerSpawn()
    {
        GameObject player2 = Instantiate(Resources.Load("InPlayer2"), new Vector3(-6, 14, 0), Quaternion.identity) as GameObject;
        player2.transform.localScale = new Vector3(0.3f, 0.3f, 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {            
            LoadingUIController.Instance.LoadScene("Main");
        }
    }

    public void ClickBack()
    {
        LoadingUIController.Instance.LoadScene("Main");
    }

    
}
