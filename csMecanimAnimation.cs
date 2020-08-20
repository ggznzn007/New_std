using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csMecanimAnimation : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

   
    public void doIdle()
    {
        anim.SetInteger("aniStep", 0);
    }
    public void doWalk()
    {
        anim.SetInteger("aniStep", 1);
    }
    public void doRun()
    {
        anim.SetInteger("aniStep", 2);
    }

    public void doJump()
    {
        StartCoroutine(coJump());
    }

    IEnumerator coJump()
    {
        anim.SetInteger("aniStep", 3);
        yield return new WaitForSeconds(0.8f);
        anim.SetInteger("aniStep", 0);
    }
}
