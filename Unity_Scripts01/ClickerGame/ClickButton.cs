using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickButton : MonoBehaviour 
{
    public Animator anim;   

    public void OnMouseDown()
    { 
        SoundController.instance.Playsound(SoundController.instance.playerHit);
        DataController.Instance.Gold += DataController.Instance.GoldPerClick;

        anim.SetTrigger("OnClick");        
    }   
}
