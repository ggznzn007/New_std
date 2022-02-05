using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingUIController : MonoBehaviour
{
    private static LoadingUIController instance; // ΩÃ±€≈Ê
    public static LoadingUIController Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<LoadingUIController>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }
    }

    private static LoadingUIController Create()
    {
        return Instantiate(Resources.Load<LoadingUIController>("LoadingUI"));
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Image progressBar;
    private string loadSceneName;
    private string info_Text = ". . . . .";
    public Text loadingInfo;
    [SerializeField] Image backGround;
    [SerializeField] Sprite[] sprites;


    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadSceneName = sceneName;
        StartCoroutine(InfoText());
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator InfoText()
    {
        for (int i = 0; i < info_Text.Length; i++)
        {
            loadingInfo.text = info_Text.Substring(0, i);
            
            //loadingInfo.text = info_Text.Substring(0, i);
            yield return new WaitForSeconds(0.1f);

        }
       // loadingInfo.text = "";
    }

    private IEnumerator LoadSceneProcess()
    {
        progressBar.fillAmount = 0f;

        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;

        float timer = 0f;
       
        while (!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime *0.2f;
               

                progressBar.fillAmount = Mathf.Lerp(0.1f, 1f, timer);

                if (progressBar.fillAmount >= 1f)
                {
                    
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 5f;
            
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }
}
