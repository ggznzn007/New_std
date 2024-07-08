using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{
    public SpriteRenderer mySpriteRenderer { get; set; }

    public int CompareTo(Obstacle other)
    {
        if (mySpriteRenderer.sortingOrder > other.mySpriteRenderer.sortingOrder)
        {
            return 1;
        }
        else if (mySpriteRenderer.sortingOrder < other.mySpriteRenderer.sortingOrder)
        {
            return -1;
        }
        return 0;
    }

    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
}
