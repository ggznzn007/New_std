using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadingUIController : MonoBehaviour
{
    private static LoadingUIController instance; // �̱���
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
        int rand = UnityEngine.Random.Range(0, sprites.Length);
        int randTxt = UnityEngine.Random.Range(0, gameTips.Length);
        backGround.sprite = sprites[rand];
        gameTip.sprite = gameTips[randTxt];
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
    private string tip1_Text = "���� �Ǹ� ���� �̺�Ʈ�� ������!!!";
    private string tip2_Text = "ģ����� ������ȭ�� �����ؿ�.";
    private string tip3_Text = "�� ������ ������ �̵� ���̿���. . .";

    public Text loadingInfo;
    
    [SerializeField] Image backGround, gameTip;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Sprite[] gameTips;

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

            yield return new WaitForSeconds(0.1f);
        }
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
                timer += Time.unscaledDeltaTime * 0.4f;

               /*backGround.sprite = backGround.sprite;
                if (progressBar.fillAmount <= progressBar.fillAmount)
                {
                    // gameTip.text = tip2_Text;
                    //backGround.sprite = sprites[0];
                }
                else if (progressBar.fillAmount <= 0.8f)
                {
                    // gameTip.text = tip1_Text;
                    //backGround.sprite = sprites[1];
                }
                else if (progressBar.fillAmount <= 1.2f)
                {
                    //gameTip.text = tip3_Text;
                    //backGround.sprite = sprites[2];
                }
                else
                {
                    backGround.sprite = backGround.sprite;
                }*/


                progressBar.fillAmount = Mathf.Lerp(0.01f, 1f, timer);

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
            timer += Time.unscaledDeltaTime * 1.5f;

            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
            canvasGroup.transform.LeanScale(Vector2.zero, timer).setEaseShake();
        }

        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }
}
