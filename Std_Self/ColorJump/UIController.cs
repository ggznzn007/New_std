using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("게임메인")][SerializeField] private GameObject mainPanel;

    [Header("게임중")]
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private TextMeshProUGUI textScore;

    [Header("게임오버")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI textHighScore;

    [Header("볼륨옵션")]
    [SerializeField] private GameObject volumePanel;

    public void GameStart()
    {
        mainPanel.SetActive(false);
        inGamePanel.SetActive(true);
    }

    public void UpdateScore(int score)
    {
        if(score<10)
        {
            textScore.text = score.ToString("D2");
        }

        else
        {
            textScore.text = score.ToString();
        }
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        textHighScore.text = $"최고기록 : {PlayerPrefs.GetInt("HighScore")}";
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameController.GC.isPlaying = !GameController.GC.isPlaying;
        }

        if(!GameController.GC.isPlaying)
        {
            volumePanel.SetActive(true);            
        }

        else
        {
            volumePanel.SetActive(false);            
        }
    }
}
