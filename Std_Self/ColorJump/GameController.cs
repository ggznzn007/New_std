using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController GC;
    [SerializeField] private Transform wallPrefab;            // �� ������
    [SerializeField] private Transform leftWalls;             // ���� ������ �θ� Ʈ������
    [SerializeField] private Transform rightWalls;            // ������ ������ �θ� Ʈ������
    [SerializeField] private int currentLevel = 1;            // ���� ����(������ ���� �� ���� ����)
    private int maxLevel = 7;
    private int currentScore = 0;
    [SerializeField] private List<Color32> colors;            // ���� ������
    [SerializeField] private Player_Color player;             // �÷��̾� ������Ʈ ����
    [SerializeField] private UIController uiController;

    private readonly float wallMaxScaleY = 20;                // �� �ִ� Yũ��

    // ������ ���� �� ���� [����-1] = �� ����
    private readonly int[] wallCountByLevel = new int[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
    // �������� �ʿ��� ���� [����-1] = �ʿ� ����
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

        // ���� �� ���� Ȯ��
        int currentWallCount = leftWalls.childCount;

        // �߰��� �ʿ��� ������ŭ �� ���� (���� ���� ������ ���� ������ �ʿ��� �� �������� ������ �� �߰� ����)
        if (currentWallCount < numberOfWalls)
        {
            for (int i = 0; i < numberOfWalls - currentWallCount; ++i)
            {
                // ���� �� ����
                Instantiate(wallPrefab, leftWalls);
                // ������ �� ����
                Instantiate(wallPrefab, rightWalls);

            }
        }

        for (int i = 0; i < numberOfWalls; ++i)
        {
            // �� ũ�� ���� y
            Vector3 scale = new(0.5f, 1, 1);
            scale.y = wallMaxScaleY / numberOfWalls;
            // �� ��ġ ���� y
            Vector3 position = Vector3.zero;
            position.y = scale.y * (numberOfWalls / 2 - i) - (numberOfWalls % 2 == 0 ? scale.y / 2 : 0);

            // ���� �� ��ġ/ũ�� ���� 
            SetTransform(leftWalls.GetChild(i), position, scale);
            // ������ �� ��ġ/ũ�� ���� 
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

        // ���� ���ð����� ��� ���󿡼� wallCountByLevel[currentLevel-1] ������ŭ ������ ���� ����
        int[] indexs = Utils.RandomNumberics(colors.Count, wallCountByLevel[currentLevel - 1]);
        for (int i = 0; i < indexs.Length; ++i)
        {
            tempColors.Add(colors[indexs[i]]);
        }

        int colorCount = tempColors.Count;

        // ���� �� ���� ����
        int[] leftWallIndexs = Utils.RandomNumberics(colorCount, colorCount);
        for (int i = 0; i < leftWalls.childCount; ++i)
        {
            leftWalls.GetChild(i).GetComponent<SpriteRenderer>().color = tempColors[leftWallIndexs[i]];
        }

        // ������ �� ���� ����
        int[] rightWallIndexs = Utils.RandomNumberics(colorCount, colorCount);
        for (int i = 0; i < rightWalls.childCount; ++i)
        {
            rightWalls.GetChild(i).GetComponent<SpriteRenderer>().color = tempColors[rightWallIndexs[i]];
        }

        // ������ ���� �迭 �� ������ ������ ������ �÷��̾��� �������� ����
        int index = Random.Range(0, tempColors.Count);
        player.GetComponent<SpriteRenderer>().color = tempColors[index];
    }

    /// �÷��̾ ���� �浹���� �� ó��
    public void CollosionWithWall()
    {
        // ���� ���� ����
        currentScore++;
        uiController.UpdateScore(currentScore);

        // ���� ���� ���� �����ϰ�, ���� ������ ���� ������ �ʿ��� �������� ������
        if (currentLevel < maxLevel && needLevelUpScore[currentLevel] < currentScore)
        {
            // ���� ���� ����
            currentLevel++;
            // �� �߰�
            SpawnWalls();
        }

        // ��, �÷��̾� ���� ����
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
