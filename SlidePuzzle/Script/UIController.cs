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

        textPlaytime.text = $"플레이 타임 : {board.Playtime / 60:D2}:{board.Playtime % 60:D2}";
        textMoveCount.text = "타일이동 횟수 : " + board.MoveCount;
    }

    public void OnClickRestart()
    {
        SE_Manager.instance.Playsound(SE_Manager.instance.btn);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
