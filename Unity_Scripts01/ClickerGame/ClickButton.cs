using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButton : MonoBehaviour
{
    public DataController dataController;

    public void OnClick()
    {
        int goldPerClick = dataController.GetGoldPerClick();

        dataController.AddGold(goldPerClick);
    }
}
