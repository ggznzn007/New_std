using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCapture : MonoBehaviour
{
    public void _ScreenCaputure()
    {
        AndroidUtils.instance.TakeHiResShot();

        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidUtils.instance.ShowToast("이미지로 저장되었습니다");
        }
        else
        {
            Debug.Log("이미지로 저장되었습니다");
        }
    }
}
