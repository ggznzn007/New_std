using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("���Ӹ���")][SerializeField] private GameObject mainPanel;

    [Header("������")]
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private TextMeshProUGUI textScore;

    [Header("���ӿ���")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI textHighScore;

    [Header("�����ɼ�")]
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
        textHighScore.text = $"�ְ��� : {PlayerPrefs.GetInt("HighScore")}";
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
