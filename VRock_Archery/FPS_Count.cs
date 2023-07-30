using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS_Count : MonoBehaviour
{
   // [Range(1, 100)]
    //public int font_Size;
   // [Range(0, 1)]
  //  public float Red, Green, Blue;

    public Text textFPS;
    float deltaTime = 0.0f;
   // GUIStyle style;
   // Rect rect;
    float msec;
    float fps;
    float worstFps = 100f;
    //string text;

    private void Awake()
    {
        /* int w = Screen.width, h = Screen.height;
         rect = new Rect(0, 0, w, h);
         style = new GUIStyle();
         style.alignment = TextAnchor.MiddleLeft;
         //style.fontSize = h * 2 / font_Size;
         style.fontSize = font_Size;
         style.normal.textColor = new Color(Red, Green, Blue, 1.0f);*/
       
        StartCoroutine(nameof(WorstReset));
    }

    private void Start()
    {
       // font_Size = font_Size == 0 ? 50 : font_Size;
        

    }
    private void FixedUpdate()
    {
        ShowFPS();
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;        
    }
    
    IEnumerator WorstReset() //�ڷ�ƾ���� 5�� �������� ���� ������ ��������.
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            worstFps = 100f;
        }
    }

    public void ShowFPS()
    {
        msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime;  // �ʴ� ������ 1�ʿ�
        if (fps < worstFps)         // ���ο� ���� FPS�� ���Դٸ� worstFps�� �ٲ���.
        {
            worstFps = fps;
        }
        textFPS.text =  fps.ToString("F1") + " ["+ worstFps.ToString("F1")+"]";
    }

   /* private void OnGUI()
    {       
        msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime;  // �ʴ� ������ 1�ʿ�
        if(fps<worstFps)         // ���ο� ���� FPS�� ���Դٸ� worstFps�� �ٲ���.
        {
            worstFps = fps;
        }
        //text = string.Format("{0:0.00} ms({1:0.}) FPS", msec, fps);
        //text = msec.ToString("F1") + "ms ����������(" + fps.ToString("F1") + ") => ����������:" + worstFps.ToString("F1");    
        text = "(" + fps.ToString("F1") + ")  " + worstFps.ToString("F1");    
        GUI.Label(rect, text, style);
    }*/

}
