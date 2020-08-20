using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

public class csLegacyAnimation : MonoBehaviour
{
    Animation ani;
    void Start()
    {
        ani = GetComponent<Animation>();
    }

    private void OnMouseExit()
    {
        doCharge();
    }

    private void OnMouseDown()
    {
        doVictory();
    }

    public void doWalk()
    {
        StartCoroutine(coWalk());
    }
    public void doAttack()
    {
        StartCoroutine(coAttack());
    }
    public void doFastWalk()
    {
        StartCoroutine(coFastWalk());
    }

    public void doVictory()
    {
        StartCoroutine(coVictory());
    }

    public void doCharge()
    {
        StartCoroutine(coCharge());
    }

    IEnumerator coWalk()
    {
        ani.Play("walk");
        yield return new WaitForSeconds(1.2f);
        ani.Play("idle");
    }
    /*
     * Play : 무조건 애니메이션 실행
     * CrossFade : 이전 애니와 현재 애니를 0.2초간 겹쳐서
     *            자연스럽게 전환
     */
    IEnumerator coAttack()
    {
        ani.CrossFade("attack", 0.2f);
        yield return new WaitForSeconds(1.167f);
        ani.CrossFade("idle", 0.2f);
    }
    IEnumerator coFastWalk()
    {
        ani.Stop();
        ani["walk"].speed = 5f;
        ani["walk"].wrapMode = WrapMode.Loop;
        ani.CrossFade("walk", 0.2f);
        yield return new WaitForSeconds(5.0f);
        ani.CrossFade("idle", 0.2f);
        ani["walk"].speed = 1f;
    }

    IEnumerator coVictory()
    {
        ani.CrossFade("victory", 0.2f);
        yield return new WaitForSeconds(2.7f);
        ani.CrossFade("idle", 0.2f);
    }

    IEnumerator coCharge()
    {
        ani.CrossFade("charge", 0.2f);
        yield return new WaitForSeconds(0.733f);
        ani.CrossFade("idle", 0.2f);
    }
}
