using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// android device only
public class AndroidUtils : MonoBehaviour
{
#if UNITY_ANDROID

    static public AndroidUtils instance;

    AndroidJavaObject currentActivity;
    AndroidJavaClass UnityPlayer;
    AndroidJavaObject context;
    AndroidJavaObject toast;


    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        UnityPlayer = 
        	new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        
        currentActivity = UnityPlayer
        	.GetStatic<AndroidJavaObject>("currentActivity");
        context = currentActivity
        	.Call<AndroidJavaObject>("getApplicationContext");
        	
        DontDestroyOnLoad(this.gameObject);
    }

    public void ShowToast(string message)
    {
        currentActivity.Call
        (
	        "runOnUiThread", 
	        new AndroidJavaRunnable(() =>
	        {
	            AndroidJavaClass Toast 
	            = new AndroidJavaClass("android.widget.Toast");
	            
	            AndroidJavaObject javaString 
	            = new AndroidJavaObject("java.lang.String", message);
	            
	            toast = Toast.CallStatic<AndroidJavaObject>
	            (
	            	"makeText", 
	            	context, 
	            	javaString, 
	            	Toast.GetStatic<int>("LENGTH_SHORT")
	            );
	            
	            toast.Call("show");
	        })
	     );
    }

    public void CancelToast()
    {
        currentActivity.Call("runOnUiThread", 
        	new AndroidJavaRunnable(() =>
	        {
	            if (toast != null) toast.Call("cancel");
	        }));
    }

    public Sprite TakeHiResShot()
    {
        string date = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string myFilename = "myScreenshot_" + date + ".png";
        // debugText.text = ": " + myFilename;
        string myDefaultLocation = Application.persistentDataPath + "/" + myFilename;
        //EXAMPLE OF DIRECTLY ACCESSING THE Camera FOLDER OF THE GALLERY
        //string myFolderLocation = "/storage/emulated/0/DCIM/Camera/";
        //EXAMPLE OF BACKING INTO THE Camera FOLDER OF THE GALLERY
        //string myFolderLocation = Application.persistentDataPath + "/../../../../DCIM/Camera/";
        //EXAMPLE OF DIRECTLY ACCESSING A CUSTOM FOLDER OF THE GALLERY
        string myFolderLocation = "/storage/emulated/0/DCIM/DRAW_ME/";
        string myScreenshotLocation = myFolderLocation + myFilename;
        //ENSURE THAT FOLDER LOCATION EXISTS
        if (!System.IO.Directory.Exists(myFolderLocation))
        {
            System.IO.Directory.CreateDirectory(myFolderLocation);
        }


        //TAKE THE SCREENSHOT AND AUTOMATICALLY SAVE IT TO THE DEFAULT LOCATION.

        //  캔버스 포함 전체 스크린샷!!
        //  Application.CaptureScreenshot(myScreenshotLocation);
        //
        //
        //

        //  캔버스 제외 카메라에 보이는 부분만 스크린 샷!!
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        Camera.main.targetTexture = rt;
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
        Camera.main.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();

        Sprite TempImage = Sprite.Create(screenShot, rec, new Vector2(0, 0), .01f);

        Camera.main.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(myScreenshotLocation, bytes);



        // 안드로이드 갤러리, 사진첩 업데이트 부분
        // 요거 안하면 "내파일" 에서는 보이지만 갤러리 및 사진첩 어플에서는 보이지 않는 문제가 생김!!!  

        //MOVE THE SCREENSHOT WHERE WE WANT IT TO BE STORED
        //  System.IO.File.Move(myDefaultLocation, myScreenshotLocation);
        //REFRESHING THE ANDROID PHONE PHOTO GALLERY IS BEGUN
        AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");
        // "android.intent.action.MEDIA_SCANNER_SCAN_FILE" <--- 요거 햇갈림.. 원래 찾은건 "android.intent.action.MEDIA_MOUNTED" 요렇게 하라고 나와있는데 안되서 저렇게 하니 됨.
        AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", new object[2] { "android.intent.action.MEDIA_SCANNER_SCAN_FILE", classUri.CallStatic<AndroidJavaObject>("parse", "file://" + myScreenshotLocation) });
        objActivity.Call("sendBroadcast", objIntent);
        // debugText.text = "Complete! - " + myScreenshotLocation;
        //REFRESHING THE ANDROID PHONE PHOTO GALLERY IS COMPLETE
        //AUTO LAUNCH/VIEW THE SCREENSHOT IN THE PHOTO GALLERY!!!
        // Application.OpenURL(myScreenshotLocation);
        //AFTERWARDS IF YOU MANUALLY GO TO YOUR PHOTO GALLERY,
        //YOU WILL SEE THE FOLDER WE CREATED CALLED "myFolder"
        // count++;

        return TempImage;
    }
#else
    void Awake()
    {
        Destroy(gameObject);
    }
#endif

}
