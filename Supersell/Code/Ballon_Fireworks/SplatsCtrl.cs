using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatsCtrl : MonoBehaviour                        // ������ ȿ�� ��Ʈ��
{
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sortingOrder = Manager_Ballon.MB.sortIndex;
    }
}
