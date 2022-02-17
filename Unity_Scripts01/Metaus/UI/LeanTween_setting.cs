using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTween_setting : MonoBehaviour
{
    
   private void Start()
    {
        this.transform.LeanScale(new Vector2(.9f,.9f), 0.6f).setLoopPingPong();
    }

   
    
    public void Open()
    {
        //transform.LeanScale(Vector2.one, 1f);
    }

    public void Close()
    {
        transform.LeanScale(Vector2.zero, 1f).setEaseInBack();
    }

    
}
