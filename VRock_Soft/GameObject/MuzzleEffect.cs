using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleEffect : MonoBehaviour                  // ÃÑ±¸ È¿°ú 
{
    Transform tr;

    private void Awake()
    {
        tr = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        
    }
}
