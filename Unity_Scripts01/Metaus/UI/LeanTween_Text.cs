using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTween_Text : MonoBehaviour
{
    
    void Start()
    {
        this.transform.LeanScale(new Vector2(.9f, .9f), 0.5f).setLoopPingPong();
    }

   
}
