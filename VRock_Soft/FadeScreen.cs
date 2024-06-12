using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FadeState { FadeIn =0 , FadeOut, FadeInOut,FadeLoop}

public class FadeScreen : MonoBehaviour
{
    public static FadeScreen fade;

    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime; // ���� 10�̸� 1�� ���� Ŭ���� ����
    [SerializeField]
    private AnimationCurve fadeCurve;
    private Image image; // ���̵� �����̹���
    private FadeState fadeState;

    private void Awake()
    {
        //FadeSingleton();
        image = GetComponent<Image>();


        // FadeIn ���İ��� 1 ~ 0���� ȭ���� ���� �����
        // StartCoroutine(Fade(1,0));

        // FadeOut ���İ��� 0 ~ 1�� ȭ���� ���� ��ο���
        // StartCoroutine(Fade(0, 1));

        OnFade(FadeState.FadeIn);
    }

    public void OnFade(FadeState state)
    {
        fadeState = state;

        switch(fadeState)
        {
            case FadeState.FadeIn:
                StartCoroutine(Fade(1, 0));
                break;
            case FadeState.FadeOut:
                StartCoroutine(Fade(0, 1));
                break;
            case FadeState.FadeInOut:
            case FadeState.FadeLoop:
                StartCoroutine(FadeInOut());
                break;
                
        }
    }

    public void FadeSingleton()
    {
        if (fade == null)
        {
            fade = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (fade != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while(percent < 1)
        {
            // fadeTime���� ����� fadeTime �ð� ����
            // percent ���� 0 ~ 1�� �����ϵ��� ��
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            // ���İ��� ���ۺ��� ������ fadeTime �ð� ���� ��ȭ
            Color color = image.color;
           // color.a = Mathf.Lerp(start, end, percent);
            color.a = Mathf.Lerp(start, end,fadeCurve.Evaluate(percent));
            image.color = color;

            yield return null;
        }
    }

    private IEnumerator FadeInOut()
    {
        while(true)
        {
            // �ڷ�ƾ ���ο��� �ڷ�ƾ �Լ��� ȣ���ϸ� �ش� �ڷ�ƾ �Լ��� ����Ǿ�� ���� ���� ������
            yield return StartCoroutine(Fade(1, 0));

            yield return StartCoroutine(Fade(0, 1));

            // 1ȸ�� ����ϴ� ������ �� break;
            if(fadeState == FadeState.FadeInOut)
            {
                break;
            }
        }
    }
}
