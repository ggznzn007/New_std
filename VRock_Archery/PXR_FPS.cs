// Copyright © 2015-2021 Pico Technology Co., Ltd. All Rights Reserved.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.XR.PXR
{
    public class PXR_FPS : MonoBehaviour
    {
        private Text fpsText;

        private readonly float updateInterval = 1.0f; //  readonly 추가
        private float timeLeft = 0.0f;
        //private string strFps = null;

      /*  float worstFps = 100f; // 추가코드
        float deltaTime = 0.0f;        
        float msec;
        float fps;*/

        void Awake()
        {
            fpsText = GetComponent<Text>();
           // StartCoroutine(nameof(worstReset)); // 추가코드
        }

        void FixedUpdate()
        {
            if (fpsText != null)
            {
                ShowFPS();
            }
        }

        private void ShowFPS()
        {
            timeLeft -= Time.unscaledDeltaTime;

            if (timeLeft <= 0.0f)
            {
                float fps = PXR_Plugin.System.UPxr_GetConfigInt(ConfigType.RenderFPS);

                fpsText.text = fps.ToString("F0");

                timeLeft += updateInterval;
            }
        }

        /* private void ShowFps()  // 기존코드
         {

             timeLeft -= Time.unscaledDeltaTime;

             if (timeLeft <= 0.0)
             {
                 float fps = PXR_Plugin.System.UPxr_GetConfigInt(ConfigType.RenderFPS);

                 strFps = string.Format("FPS: {0:f1}", fps);
                 fpsText.text = strFps;

                 timeLeft += updateInterval;
             }
         }*/



        /*  private void ShowFpsLift()
          {
              msec = deltaTime * 1000.0f;
              fps = 1.0f / deltaTime;  // 초당 프레임 1초에
              //fps = PXR_Plugin.System.UPxr_GetConfigInt(ConfigType.RenderFPS);
              if (fps < worstFps)         // 새로운 최저 FPS가 나왔다면 worstFps로 바꿔줌.
              {
                  worstFps = fps;
                  fpsText.text = fps.ToString("F1") + "  [" + worstFps.ToString("F1") + "]";
              }
          }

          // 추가코드
          IEnumerator worstReset() //코루틴으로 5초 간격으로 최저 프레임 리셋해줌.
          {
              while (true)
              {
                  yield return new WaitForSeconds(5f);
                  worstFps = 100f;
              }
          }*/
    }
}