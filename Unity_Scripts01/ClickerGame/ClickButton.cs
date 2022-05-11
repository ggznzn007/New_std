using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButton : MonoBehaviour
{
    public void OnMouseDown()
    {
       DataController.Instance.gold+= DataController.Instance.goldPerClick;
    }
}
