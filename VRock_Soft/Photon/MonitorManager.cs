using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorManager : MonoBehaviour
{
    void Start()
    {
        if (Display.displays.Length > 1)
        {
            foreach (var item in Display.displays)
            {
                item.Activate();
            }
        }
    
    }
 
    void Update()
    {
      

    }
   
}
