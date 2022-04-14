using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject resultPanel;
    [SerializeField]
    private TextMeshProUGUI textPlaytime;
    [SerializeField]
    private TextMeshProUGUI textMoveCount;
    [SerializeField]
    private Board board;

    public void OnResultPanel()
    {
        resultPanel.SetActive(true);

        textPlaytime.text = $"�÷��� Ÿ�� : {board.Playtime / 60:D2}:{board.Playtime % 60:D2}";
        textMoveCount.text = "Ÿ���̵� Ƚ�� : " + board.MoveCount;
    }

    public void OnClickRestart()
    {
        SE_Manager.instance.Playsound(SE_Manager.instance.btn);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
