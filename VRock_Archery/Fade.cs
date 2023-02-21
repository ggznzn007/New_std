using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private GameObject fadeInPanel;
    private Image image;
    

    private bool isAlpha = false;

    private void Awake()
    {
        fadeInPanel = this.gameObject;
        image = fadeInPanel.GetComponent<Image>();
    }

     void Update()
    {
       
        StartCoroutine(FadeInScreen());
        if(isAlpha)
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator FadeInScreen()
    {
        Color color = image.color;
        
        for (int i = 100; i >= 0; i--)
        {           
            color.a -= Time.deltaTime * 0.006f;
            image.color = color;
            if(image.color.a<=0)
            {
                isAlpha = true;
            }
        }
        yield return null;
    }

   
}
