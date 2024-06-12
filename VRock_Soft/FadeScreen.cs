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
    private float fadeTime; // 값이 10이면 1초 값이 클수록 빠름
    [SerializeField]
    private AnimationCurve fadeCurve;
    private Image image; // 페이드 바탕이미지
    private FadeState fadeState;

    private void Awake()
    {
        //FadeSingleton();
        image = GetComponent<Image>();


        // FadeIn 알파값이 1 ~ 0으로 화면이 점점 밝아짐
        // StartCoroutine(Fade(1,0));

        // FadeOut 알파값이 0 ~ 1로 화면이 점점 어두워짐
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
            // fadeTime으로 나누어서 fadeTime 시간 동안
            // percent 값이 0 ~ 1로 증가하도록 함
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            // 알파값을 시작부터 끝까지 fadeTime 시간 동안 변화
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
            // 코루틴 내부에서 코루틴 함수를 호출하면 해당 코루틴 함수가 종료되어야 다음 문장 실행함
            yield return StartCoroutine(Fade(1, 0));

            yield return StartCoroutine(Fade(0, 1));

            // 1회만 재생하는 상태일 때 break;
            if(fadeState == FadeState.FadeInOut)
            {
                break;
            }
        }
    }
}
