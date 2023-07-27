using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonDestroy : MonoBehaviour
{    
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
