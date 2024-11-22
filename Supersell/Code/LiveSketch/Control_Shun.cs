using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Control_Shun : PlayerControl
{
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sk = GetComponent<SpriteSkin>();
        waitTime = Setting.startWaitTime;
    }

    private void Start()
    {      
        anim = GetComponent<Animator>();
    }
}
