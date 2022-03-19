using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marine : Unit
{
    void Start()
    {
        Debug.Log("Marine 생성 !!!");
    }

    public override void Move()
    {
        Debug.Log("Marine 이동 !!!");
    }
}
