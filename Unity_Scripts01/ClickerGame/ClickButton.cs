using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButton : MonoBehaviour
{
    public Animator anim;
    public void OnMouseDown()
    {
       DataController.Instance.gold+= DataController.Instance.goldPerClick;

        anim.SetTrigger("OnClick");  
    }
}
