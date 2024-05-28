using UnityEngine;
using TMPro;

public class GameUIController : MonoBehaviour
{
	[SerializeField]
	private	GameController	gameController;

	[Header("InGame")]
	[SerializeField]
	private	TextMeshProUGUI	textScore;

	[Header("Game Over")]
	[SerializeField]
	private	GameObject		panelGameOver;
	[SerializeField]
	private	TextMeshProUGUI	textResultScore;
	
	[Header("Game Over UI Animation")]
	[SerializeField]
	private	ScaleEffect		effectGameOver;
	[SerializeField]
	private	CountingEffect	effectResultScore;

	private void Update()
	{
		textScore.text = $"SCORE {gameController.Score}";
	}

	public void OnGameOver()
	{
		// GameOver Panel UI Ȱ��ȭ
		panelGameOver.SetActive(true);
		// ȹ�� ���� ���
		textResultScore.text = gameController.Score.ToString();
		// "GAME OVER" �ؽ�Ʈ ũ�� ��� �ִϸ��̼�
		effectGameOver.Play(200, 100);
		// 0 -> gameController.Score���� ������ ī�����ϴ� �ִϸ��̼�
		effectResultScore.Play(0, gameController.Score);
	}

	public void BtnClickGoToLobby()
	{
		Utils.LoadScene(SceneNames.Lobby);
	}
}

