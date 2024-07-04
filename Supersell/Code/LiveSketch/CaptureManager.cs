using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CaptureManager : MonoBehaviour
{
    public RenderTexture DrawTexture;   //PNGÀúÀåÇÒ Å¸°Ù ·»´õ ÅØ½ºÃÄ    
    public string fileName = "ScreenShot.png";    
    public int capInt;
    public string num;

    private void Start()
    {
        PlayerPrefs.GetInt("preInt", capInt);
        PlayerPrefs.GetString("preString", num);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            // RenderTextureSave();
            capInt++;
            CaptureToPng();            
        }

        PlayerPrefs.SetInt("preInt", capInt);
        PlayerPrefs.SetString("preString", num);
    }

    void RenderTextureSave()
    {
        RenderTexture.active = DrawTexture;
        var texture2D = new Texture2D(DrawTexture.width, DrawTexture.height);        
        texture2D.ReadPixels(new Rect(0, 0, DrawTexture.width, DrawTexture.height), 0, 0);
        texture2D.Apply();
        var data = texture2D.EncodeToPNG();
        File.WriteAllBytes("Capture/Image.png", data);
        Debug.Log("ÂûÄ¬");       
    }

    void CaptureToPng()
    {                
        num = capInt.ToString();
        string filePath = Path.Combine(Application.dataPath, num+"_"+fileName);
        ScreenCapture.CaptureScreenshot(filePath);
        Debug.Log(num+fileName);
        Debug.Log("ÂûÄ¬");        
    }
}
