using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleEffect : MonoBehaviour                  // �ѱ� ȿ�� 
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
