using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property_Ex : MonoBehaviour
{
    public int Age { get; private set; } = 35;

    private void Start()
    {
        print(Age);       
    }
}
