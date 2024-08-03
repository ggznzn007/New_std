using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public float animTime = 2f;         // Fade �ִϸ��̼� ��� �ð� (����:��).  

    private Image fadeImage;            // UGUI�� Image������Ʈ ���� ����.  

    private float start = 1f;           // Mathf.Lerp �޼ҵ��� ù��° ��.  
    private float end = 0f;             // Mathf.Lerp �޼ҵ��� �ι�° ��.  
    private float time = 0f;            // Mathf.Lerp �޼ҵ��� �ð� ��.  
    
    void Awake()
    {
        // Image ������Ʈ�� �˻��ؼ� ���� ���� �� ����.  
        fadeImage = GetComponent<Image>();
    }   

    void FixedUpdate()
    {
        // Fade �ִϸ��̼� ���.  
        PlayFadeIn();        
    }

    // Fade �ִϸ��̼� �Լ�.  
    public void PlayFadeIn()
    {        
            // ��� �ð� ���.  
            // 2��(animTime)���� ����� �� �ֵ��� animTime���� ������.  
            time += Time.deltaTime / animTime;

            // Image ������Ʈ�� ���� �� �о����.  
            Color color = fadeImage.color;
            // ���� �� ���.  
            color.a = Mathf.Lerp(start, end, time);
            // ����� ���� �� �ٽ� ����.  
            fadeImage.color = color;

        //Destroy(gameObject, 2.2f);
        Invoke("FadeSetActive", 2f);
    }

    public void FadeSetActive()
    {
        this.gameObject.SetActive(false);         
    }
}
