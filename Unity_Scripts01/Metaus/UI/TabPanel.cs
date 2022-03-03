using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabPanel : MonoBehaviour
{
   public List<TabButton> tabButtons;
   public List<GameObject> contentsPanels;
    int seleted = 0;
    private void Start()
    {
        TabClick(seleted);
    }
    public void TabClick(int id)
    {
        for (int i = 0; i < contentsPanels.Count; i++)
        {
            if(i==id)
            {
                contentsPanels[i].SetActive(true);
                tabButtons[i].Seleted();
            }
            else
            {
                contentsPanels[i].SetActive(false);
                tabButtons[i].Unseleted();
            }
        }
    }
}
