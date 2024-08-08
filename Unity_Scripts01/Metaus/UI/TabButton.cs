using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    Image backGround;
    public Sprite idleImg;
    public Sprite seletedImg;

    private void Awake()
    {
        backGround = GetComponent<Image>();
    }
    public void Seleted()
    {
        backGround.sprite = seletedImg;
    }
    public void Unseleted()
    {
        backGround.sprite = idleImg;
    }
}
