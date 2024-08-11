using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffect : MonoBehaviour
{
    SpriteRenderer sprite;
    Vector2 direction;
    float moveSpeed = 0.02f;
    float sizeSpeed = 0.5f;
    float colorSpeed = 3f;
    float minSize = 0.1f;
    float maxSize = 0.6f;
    public Color[] colors;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = colors[Random.Range(0, colors.Length)];
        direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        float size = Random.Range(minSize, maxSize);
        transform.localScale = new Vector2(size, size);
    }

    void Update()
    {
        OnEffect();
    }

    public void OnEffect()
    {
        transform.Translate(direction * moveSpeed);
        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, Time.deltaTime * sizeSpeed);

        Color color = sprite.color;
        color.a = Mathf.Lerp(sprite.color.a, 0, Time.deltaTime * colorSpeed);
        sprite.color = color;
        if (sprite.color.a <= 0.01f)
        {
            Destroy(gameObject);
        }
    }
}
