using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebat : Unit
{
    void Start()
    {
        Debug.Log("Firebat 생성 !!!");
    }

    public override void Move()
    {
        Debug.Log("Firebat 이동 !!!");
    }
}
