using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController GC;
    [SerializeField] private Transform wallPrefab;            // 벽 프리팹
    [SerializeField] private Transform leftWalls;             // 왼쪽 벽들의 부모 트랜스폼
    [SerializeField] private Transform rightWalls;            // 오른쪽 벽들의 부모 트랜스폼
    [SerializeField] private int currentLevel = 1;            // 현재 레벨(레벨에 따라 벽 개수 변경)
    private int maxLevel = 7;
    private int currentScore = 0;
    [SerializeField] private List<Color32> colors;            // 벽의 색상목록
    [SerializeField] private Player_Color player;             // 플레이어 컴포넌트 정보
    [SerializeField] private UIController uiController;

    private readonly float wallMaxScaleY = 20;                // 벽 최대 Y크기

    // 레벨에 따른 벽 개수 [레벨-1] = 벽 개수
    private readonly int[] wallCountByLevel = new int[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
    // 레벨업에 필요한 점수 [레벨-1] = 필요 점수
    private readonly int[] needLevelUpScore = new int[8] { 1, 2, 3, 5, 8, 13, 21, 34 };

    public bool isPlaying = false;

    private void Awake()
    {
        GC = this;
        SpawnWalls();
        SetColors();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5);
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                player.GameStart();
                uiController.GameStart();

                yield break;
            }

            yield return null;
        }
    }

    private void SpawnWalls()
    {
        int numberOfWalls = wallCountByLevel[currentLevel - 1];

        // 현재 벽 개수 확인
        int currentWallCount = leftWalls.childCount;

        // 추가로 필요한 개수만큼 벽 생성 (현재 벽의 개수가 현재 레벨에 필요한 벽 개수보다 적으면 벽 추가 생성)
        if (currentWallCount < numberOfWalls)
        {
            for (int i = 0; i < numberOfWalls - currentWallCount; ++i)
            {
                // 왼쪽 벽 생성
                Instantiate(wallPrefab, leftWalls);
                // 오른쪽 벽 생성
                Instantiate(wallPrefab, rightWalls);

            }
        }

        for (int i = 0; i < numberOfWalls; ++i)
        {
            // 벽 크기 설정 y
            Vector3 scale = new(0.5f, 1, 1);
            scale.y = wallMaxScaleY / numberOfWalls;
            // 벽 위치 설정 y
            Vector3 position = Vector3.zero;
            position.y = scale.y * (numberOfWalls / 2 - i) - (numberOfWalls % 2 == 0 ? scale.y / 2 : 0);

            // 왼쪽 벽 위치/크기 설정 
            SetTransform(leftWalls.GetChild(i), position, scale);
            // 오른쪽 벽 위치/크기 설정 
            SetTransform(rightWalls.GetChild(i), position, scale);
        }
    }

    private void SetTransform(Transform t, Vector3 position, Vector3 scale)
    {
        t.localPosition = position;
        t.localScale = scale;
    }

    private void SetColors()
    {
        var tempColors = new List<Color32>();

        // 현재 선택가능한 모든 색상에서 wallCountByLevel[currentLevel-1] 개수만큼 임의의 색상 추출
        int[] indexs = Utils.RandomNumberics(colors.Count, wallCountByLevel[currentLevel - 1]);
        for (int i = 0; i < indexs.Length; ++i)
        {
            tempColors.Add(colors[indexs[i]]);
        }

        int colorCount = tempColors.Count;

        // 왼쪽 벽 색상 설정
        int[] leftWallIndexs = Utils.RandomNumberics(colorCount, colorCount);
        for (int i = 0; i < leftWalls.childCount; ++i)
        {
            leftWalls.GetChild(i).GetComponent<SpriteRenderer>().color = tempColors[leftWallIndexs[i]];
        }

        // 오른쪽 벽 색상 설정
        int[] rightWallIndexs = Utils.RandomNumberics(colorCount, colorCount);
        for (int i = 0; i < rightWalls.childCount; ++i)
        {
            rightWalls.GetChild(i).GetComponent<SpriteRenderer>().color = tempColors[rightWallIndexs[i]];
        }

        // 임의의 색상 배열 중 임의의 색상을 선택해 플레이어의 색상으로 설정
        int index = Random.Range(0, tempColors.Count);
        player.GetComponent<SpriteRenderer>().color = tempColors[index];
    }

    /// 플레이어가 벽과 충돌했을 때 처리
    public void CollosionWithWall()
    {
        // 현재 점수 증가
        currentScore++;
        uiController.UpdateScore(currentScore);

        // 아직 레벨 업이 가능하고, 현재 점수가 다음 레벨에 필요한 점수보다 높으면
        if (currentLevel < maxLevel && needLevelUpScore[currentLevel] < currentScore)
        {
            // 현재 레벨 증가
            currentLevel++;
            // 벽 추가
            SpawnWalls();
        }

        // 벽, 플레이어 색상 설정
        SetColors();
    }

    public void GameOver()
    {
        StartCoroutine(nameof(GameOverProcess));
    }

    private IEnumerator GameOverProcess()
    {
        if (currentScore > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
        }

        uiController.GameOver();

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }

            yield return null;
        }
    }
}
