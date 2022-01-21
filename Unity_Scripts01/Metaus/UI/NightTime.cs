using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightTime : MonoBehaviour
{
    public GameObject nightView, dayView;   
    public bool isDay = false;

    private void Awake()
    {
       if(!isDay)
        {
            NightTimePlayerSpawn();
        }       
    }

    public void NightTimePlayerSpawn()
    {        
        GameObject player2 = Instantiate(Resources.Load("InPlayer"), new Vector3(-3, 12, 0), Quaternion.identity) as GameObject;
        player2.transform.localScale = new Vector3(0.3f, 0.3f, 1);
    }

    public void ClickBack()
    {
        LoadingSceneCtrl.LoadScene("Main");
    }

    // 채팅창 활성화
    public void ClickChat()
    {
        // chatting.SetActive(true);
    }

    // 채팅차 비활성화
    public void ClickQuit()
    {
        // chatting.SetActive(false);
    }
   
    public void ChangingNight()
    {
        isDay = false;
        nightView.gameObject.SetActive(true);
        dayView.gameObject.SetActive(false);
    }

    public void ChangingDay()
    {
        isDay = true;
        nightView.gameObject.SetActive(false);
        dayView.gameObject.SetActive(true);
    }
}
