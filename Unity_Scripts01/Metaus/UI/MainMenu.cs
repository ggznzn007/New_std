using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();           
        }        
    }
    public void ClickEnter()
    {        
        StartCoroutine(GetLoadingUI());        
    }
   private IEnumerator GetLoadingUI()
    {
        this.transform.LeanScale(Vector2.zero, 3f).setEaseInBack();
        yield return new WaitForSeconds(0.5f);
        LoadingUIController.Instance.LoadScene("GPGSLogin2");
    }
    public void ClickQuit()
    {
        // 유니티 에디터
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

        // 안드로이드
#else
Application.Quit();

#endif
    }   
}
