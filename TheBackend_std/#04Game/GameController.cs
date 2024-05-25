using UnityEngine;

public class GameController : MonoBehaviour
{
	public	bool	IsGameOver { set; get; } = false;

	public void GameOver()
	{
		// 중복 처리 되지 않도록 bool 변수로 제어
		if ( IsGameOver == true ) return;

		IsGameOver = true;

		// 경험치 증가 및 레벨업 여부 검사
		// (현재 레벨 시스템에 대한 설정이 없기 때문에 경험치의 최대치를 100으로 가정)
		// (게임을 한번 플레이할 때마다 경험치는 25씩 증가)
		BackendGameData.Instance.UserGameData.experience += 25;
		if ( BackendGameData.Instance.UserGameData.experience >= 100 )
		{
			BackendGameData.Instance.UserGameData.experience = 0;
			BackendGameData.Instance.UserGameData.level ++;
		}

		// 게임 정보 업데이트
		BackendGameData.Instance.GameDataUpdate(AfterGameOver);
	}

	public void AfterGameOver()
	{
		// 로비 씬으로 이동
		Utils.LoadScene(SceneNames.Lobby);
	}
}

