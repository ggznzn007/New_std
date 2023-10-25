using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
   
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    
}
