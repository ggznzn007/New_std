using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution : MonoBehaviour
{
    public SpriteRenderer sr;

    private void Awake()
    {
        float spriteX = sr.sprite.bounds.size.x;
        float spriteY = sr.sprite.bounds.size.y;

        float screenY = Camera.main.orthographicSize * 2;
        float screenX = screenY / Screen.height * Screen.width;

        transform.localScale = new Vector2(Mathf.Ceil(screenX/spriteX), Mathf.Ceil(screenY/spriteY));
    }
}
