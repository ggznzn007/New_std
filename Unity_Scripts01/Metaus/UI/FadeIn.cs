using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public float animTime = 2f;         // Fade 애니메이션 재생 시간 (단위:초).  

    private Image fadeImage;            // UGUI의 Image컴포넌트 참조 변수.  

    private float start = 1f;           // Mathf.Lerp 메소드의 첫번째 값.  
    private float end = 0f;             // Mathf.Lerp 메소드의 두번째 값.  
    private float time = 0f;            // Mathf.Lerp 메소드의 시간 값.  
    
    void Awake()
    {
        // Image 컴포넌트를 검색해서 참조 변수 값 설정.  
        fadeImage = GetComponent<Image>();
    }   

    void FixedUpdate()
    {
        // Fade 애니메이션 재생.  
        PlayFadeIn();        
    }

    // Fade 애니메이션 함수.  
    public void PlayFadeIn()
    {        
            // 경과 시간 계산.  
            // 2초(animTime)동안 재생될 수 있도록 animTime으로 나누기.  
            time += Time.deltaTime / animTime;

            // Image 컴포넌트의 색상 값 읽어오기.  
            Color color = fadeImage.color;
            // 알파 값 계산.  
            color.a = Mathf.Lerp(start, end, time);
            // 계산한 알파 값 다시 설정.  
            fadeImage.color = color;

        //Destroy(gameObject, 2.2f);
        Invoke("FadeSetActive", 2f);
    }

    public void FadeSetActive()
    {
        this.gameObject.SetActive(false);         
    }
}
