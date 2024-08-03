using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
public class DayTime : MonoBehaviour
{
   // public GameObject nightView, dayView, chatting, panel;   
    private void Awake()
    {
      DayTimePlayerSpawn();        
    }
   
    public void ClickQuit()
    {
        LoadingUIController.Instance.LoadScene("Main");
        ((PlayGamesPlatform)Social.Active).SignOut();
    }

    public void DayTimePlayerSpawn()
    {
        GameObject player1 = Instantiate(Resources.Load("InPlayer"), new Vector3(-6, 14, 0), Quaternion.identity) as GameObject;
        player1.transform.localScale = new Vector3(0.3f, 0.3f, 1);
    }

    public void ClickBack()
    {
        LoadingUIController.Instance.LoadScene("Main");
    }

    public void SetActiveTrue()
    {
        this.gameObject.SetActive(true);
    }

    public void SetActiveFalse()
    {
        this.gameObject.SetActive(false);
    }        
}
