using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cardRenderer;
    [SerializeField] private Sprite animalSprite;
    [SerializeField] private Sprite backSprite;

    private bool isFlipped = false;
    private bool isFlipping = false;
    private bool isMatched = false;
    public int cardID;

    public void SetCardID(int id)
    {
        cardID = id;
    }

    public void SetMatched()
    {
        isMatched = true;
    }

    public void SetAnimalSprite(Sprite sprite)
    {
        animalSprite = sprite;
    }

    public void FlipCard()
    {
        isFlipping = true;

        Vector3 originScale = transform.localScale;
        Vector3 targetScale = new Vector3(0, originScale.y, originScale.z);
        transform.DOScale(targetScale, 0.2f).OnComplete(() =>
        {
            isFlipped = !isFlipped;
            if (isFlipped)
            {
                cardRenderer.sprite = animalSprite;
            }
            else
            {
                cardRenderer.sprite = backSprite;
            }

            transform.DOScale(originScale, 0.2f).OnComplete(() => { isFlipping = false; });
        });
    }

    private void OnMouseDown()
    {
        if (!isFlipping&&!isMatched&&!isFlipped)
        {
            GameManager.instance.CardClicked(this);
        }

    }
}
