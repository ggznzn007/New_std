using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatsCtrl : MonoBehaviour                        // 페인팅 효과 컨트롤
{
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sortingOrder = Manager_Ballon.MB.sortIndex;
    }
}
