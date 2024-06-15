using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyPlantPopUp : MonoBehaviour
{
    public Text UserName;
    public GameObject ErrorPopup;
    int oxygentCnt;  

    private void Awake()
    {
        oxygentCnt = GameManager.inst.oxygenCnt;
    }

    private void Start()
    {
        UserName.text = PlayerPrefs.GetString("User");
    }

    public void btnYes()
    {
        if ((oxygentCnt % 1000) >= 50)
        {
            GameManager.inst.oxygenCnt -= 50;
            DecoGarden.decoGarden.btnYes();
            Debug.Log("산다");
        }
        //else if ((oxygentCnt % 1000) < 50)
        //{
        //    Debug.Log("못산다");
        //    ErrorPopup.SetActive(true);
        //}
    }
}
