using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataManager : Singleton<DataManager>
{
    #region ���� ����
    public bool isPlaying;
    public bool isPaused;
    public float SceneTime_Spring;
    public float SceneTime_Summer;
    public float SceneTime_Fall;
    public float SceneTime_Winter;
    public float SceneTime_Fish;
    public float SceneTime = 10;
    public float DelayTime = 5;

    public GameObject settingPanel;
    public TMP_InputField springInput;
    public TMP_InputField summerInput;
    public TMP_InputField fallInput;
    public TMP_InputField winterInput;
    public TMP_InputField fishesInput;
   // public Slider springSlider;
  //  public Slider summerSlider;
  //  public Slider fallSlider;
  //  public Slider winterSlider;
    //public Slider fishSlider;

    public TextMeshProUGUI springText;    
    public TextMeshProUGUI summerText;
    public TextMeshProUGUI fallText;
    public TextMeshProUGUI winterText;
    public TextMeshProUGUI fishText;
    #endregion

    #region ����Ƽ �޼��� ����
    private void Start()
    {
        SceneTime_Spring = 10;
        SceneTime_Summer = 10;
        SceneTime_Fall = 10;
        SceneTime_Winter = 10;
        SceneTime_Fish = 10;
        isPaused = false;
    }

    private void Update()
    {
        GetSpringTime();
        GetSummerTime();
        GetFallTime();
        GetWinterTime();
        GetFishTime();

        if (Input.GetKeyDown(KeyCode.Escape))                    // ESC������ �Ͻ����� ���·� ����Ī
        {
            isPaused = !isPaused;
        }

        if (isPaused)                                             // �Ͻ����� �� ���ø޴� Ȱ��ȭ ��Ȱ��ȭ 
        {
            settingPanel.SetActive(true);
        }
        else
        {
            settingPanel.SetActive(false);           
        }

      
       


        if (Input.GetKey(KeyCode.Space))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
#endif   
        }
    }
    #endregion

    #region �ð����� �ҷ����� �޼��� ����
    public void GetSpringTime()
    {
        SceneTime_Spring = PlayerPrefs.GetFloat("setSpring");
        string sText = PlayerPrefs.GetString("setSprtxt");
       // springSlider.value = SceneTime_Spring;
        springText.text = sText;        
    }

    public void GetSummerTime()
    {
        SceneTime_Summer = PlayerPrefs.GetFloat("setSummer");
        string suText = PlayerPrefs.GetString("setSumtxt");
       // summerSlider.value = SceneTime_Summer;
        summerText.text = suText;
    }

    public void GetFallTime()
    {
        SceneTime_Fall = PlayerPrefs.GetFloat("setFall");
        string fText = PlayerPrefs.GetString("setFalltxt");
      //  fallSlider.value = SceneTime_Fall;
        fallText.text = fText;
    }

    public void GetWinterTime()
    {
        SceneTime_Winter = PlayerPrefs.GetFloat("setWinter");
        string wText = PlayerPrefs.GetString("setWintxt");
      //  winterSlider.value = SceneTime_Winter;
        winterText.text = wText;
    }

    public void GetFishTime()
    {
        SceneTime_Fish = PlayerPrefs.GetFloat("setFish");
        string fiText = PlayerPrefs.GetString("setFishtxt");
      //  fishSlider.value = SceneTime_Fish;
        fishText.text = fiText;
    }
    #endregion

    #region �ð����� �����ϴ� �޼��� ����
    public void SetSpringTime()
    {
        SceneTime_Spring = int.Parse(springInput.text);
        PlayerPrefs.SetFloat("setSpring", SceneTime_Spring);
        int temp =(int)SceneTime_Spring;
        int min = temp / 60;
        int sec = temp % 60;
        string setTime = string.Format("{0:00}�� : {1:00}��",min,sec);        
        PlayerPrefs.SetString("setSprtxt", setTime);
    }

    public void SetSummerTime()
    {
        SceneTime_Summer = int.Parse(summerInput.text);
        PlayerPrefs.SetFloat("setSummer", SceneTime_Summer);
        int temp = (int)SceneTime_Summer;
        int min = temp / 60;
        int sec = temp % 60;
        string setTime = string.Format("{0:00}�� : {1:00}��", min, sec);
        PlayerPrefs.SetString("setSumtxt", setTime);
    }

    public void SetFallTime()
    {
        SceneTime_Fall = int.Parse(fallInput.text);
        PlayerPrefs.SetFloat("setFall", SceneTime_Fall);
        int temp = (int)SceneTime_Fall;
        int min = temp / 60;
        int sec = temp % 60;
        string setTime = string.Format("{0:00}�� : {1:00}��", min, sec);
        PlayerPrefs.SetString("setFalltxt", setTime);
    }

    public void SetWinterTime()
    {
        SceneTime_Winter = int.Parse(winterInput.text);
        PlayerPrefs.SetFloat("setWinter", SceneTime_Winter);
        int temp = (int)SceneTime_Winter;
        int min = temp / 60;
        int sec = temp % 60;
        string setTime = string.Format("{0:00}�� : {1:00}��", min, sec);
        PlayerPrefs.SetString("setWintxt", setTime);
    }

    public void SetFishTime()
    {
        SceneTime_Fish = int.Parse(fishesInput.text);
        PlayerPrefs.SetFloat("setFish", SceneTime_Fish);
        int temp = (int)SceneTime_Fish;
        int min = temp / 60;
        int sec = temp % 60;
        string setTime = string.Format("{0:00}�� : {1:00}��", min, sec);
        PlayerPrefs.SetString("setFishtxt", setTime);
    }
    #endregion

    public void GotoThatScene(int seasons)
    {
        SceneManager.LoadScene(seasons);
    }
}
