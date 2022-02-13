using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTween_setting : MonoBehaviour
{
    
    void Start()
    {
        //transform.localScale = Vector2.zero;
        //
        transform.LeanScale(new Vector2(.9f,.9f), 0.6f).setLoopPingPong();
        

    }

    private void Update()
    {
        //transform.LeanScale(Vector2.zero, 1f).setEaseInBack();
       // transform.LeanScale(Vector2.zero, 1f).setEaseInBounce();
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
