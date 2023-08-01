using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // 1.색상을 배열로 등록해두고 사용
    // 2.Random.ColorHSV 메소드 사용하여 임의의 색상 사용

    [SerializeField]
    private Color[] colorPalette;  // 색상목록

    [SerializeField]
    private float difficultyModifier; // 색상이 다른 정도 (높을수록 다르다)

    [SerializeField] [Range(2, 6)]
    public int blockCount = 2;

    [SerializeField]
    private BlockSpawner blockSpawner;

    private List<Block> blockList = new List<Block>();

    [SerializeField]
    private GameObject gameoverPanel;

    private Color currentColor;
    private Color otherOneColor;                          // 다른 하나적용색상
    private int otherBlockIndex;                          // 색상이 다른 하나의 블록 인덱스
    public int stageIndex;
    private int score;
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI scoreText;    

    private void Awake()
    {        
        Initialzing();

        SetColors();
    }   

    private void Initialzing()
    {        
        gameoverPanel.SetActive(false);
        blockList = blockSpawner.SpawnBlocks(blockCount);
        stageIndex = 1;
        score = 1;
        stageText.text = "Stage " + stageIndex.ToString();
        scoreText.text = "Score " + score.ToString();
        for (int i = 0; i < blockList.Count; ++i)
        {
            blockList[i].Setup(this);            
        }       
    }   

    private void SetColors()
    {
        // 블록 색이 바뀔때마다 정답 블록의 색상이 다른 블록들과 비슷한색상으로 보이도록 감소
        difficultyModifier *= 0.95f;

        // 기본 블록 색상
        Color currentColor = colorPalette[Random.Range(0,colorPalette.Length)];

        // 정답 블록 색상
        float diff = (1.0f / 255.0f) * difficultyModifier;
        otherOneColor = new Color(currentColor.r-diff,currentColor.g-diff,currentColor.b-diff);

        // 정답 블록 순번
        otherBlockIndex = Random.Range(0,blockList.Count);
        print(otherBlockIndex);

        // 하나의 정답과 나머지 블록들의 색상 설정
        for (int i = 0; i < blockList.Count; i++)
        {
            if(i==otherBlockIndex)
            {
                blockList[i].Color = otherOneColor;
            }    
            else
            {
                blockList[i].Color = currentColor;
            }           
        }
    }

    public void CheckBlock(Color color)
    {
        // 색상이 다른 하나의 블럭과 매개변수 color의 색상이 같으면
        // 플레이어가 선택한 블록이 정답 블럭 = 정답
        if (blockList[otherBlockIndex].Color == color)
        {           
            // 색상 재 선택
            SetColors();
            stageIndex++;
            score = score+stageIndex;
            stageText.text = "Stage " + stageIndex.ToString();
            scoreText.text = "Score " + score.ToString();
            print("색상 일치 ! - 점수 획득 처리");            
        }
        else
        {
            print("실패...");
            gameoverPanel.SetActive(true);
            //UnityEditor.EditorApplication.ExitPlaymode();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
