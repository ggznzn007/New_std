using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onGameOver;      // 게임오버 되었을 때 호출할 메소드 등록 및 실행
    [SerializeField]
    private DailyRankRegister dailyRank;

    private int score = 0;

    public bool IsGameOver { set; get; } = false;
    public int Score
    {
        set => score = Mathf.Max(0, value);
        get => score;
    }

    public void GameOver()
    {
        // 중복 처리 되지 않도록 bool 변수로 제어
        if (IsGameOver == true) return;

        IsGameOver = true;

        // 게임오버 되었을 때 호출할 메소드들을 실행
        onGameOver.Invoke();

        // 현재 점수 정보를 바탕으로 랭킹 데이터 갱신
        dailyRank.Process(score);

      /*  // 경험치 증가 및 레벨업 여부 검사 // 임시
        // (현재 레벨 시스템에 대한 설정이 없기 때문에 경험치의 최대치를 100으로 가정)
        // (게임을 한번 플레이할 때마다 경험치는 25씩 증가)
        BackendGameData.Instance.UserGameData.experience += 25;
        if (BackendGameData.Instance.UserGameData.experience >= 100)
        {
            BackendGameData.Instance.UserGameData.experience = 0;
            BackendGameData.Instance.UserGameData.level++;
        }

        // 게임 정보 업데이트
        BackendGameData.Instance.GameDataUpdate();*/
    }
}

