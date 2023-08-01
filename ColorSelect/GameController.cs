using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // 1.������ �迭�� ����صΰ� ���
    // 2.Random.ColorHSV �޼ҵ� ����Ͽ� ������ ���� ���

    [SerializeField]
    private Color[] colorPalette;  // ������

    [SerializeField]
    private float difficultyModifier; // ������ �ٸ� ���� (�������� �ٸ���)

    [SerializeField] [Range(2, 6)]
    public int blockCount = 2;

    [SerializeField]
    private BlockSpawner blockSpawner;

    private List<Block> blockList = new List<Block>();

    [SerializeField]
    private GameObject gameoverPanel;

    private Color currentColor;
    private Color otherOneColor;                          // �ٸ� �ϳ��������
    private int otherBlockIndex;                          // ������ �ٸ� �ϳ��� ��� �ε���
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
        // ��� ���� �ٲ𶧸��� ���� ����� ������ �ٸ� ��ϵ�� ����ѻ������� ���̵��� ����
        difficultyModifier *= 0.95f;

        // �⺻ ��� ����
        Color currentColor = colorPalette[Random.Range(0,colorPalette.Length)];

        // ���� ��� ����
        float diff = (1.0f / 255.0f) * difficultyModifier;
        otherOneColor = new Color(currentColor.r-diff,currentColor.g-diff,currentColor.b-diff);

        // ���� ��� ����
        otherBlockIndex = Random.Range(0,blockList.Count);
        print(otherBlockIndex);

        // �ϳ��� ����� ������ ��ϵ��� ���� ����
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
        // ������ �ٸ� �ϳ��� ���� �Ű����� color�� ������ ������
        // �÷��̾ ������ ����� ���� �� = ����
        if (blockList[otherBlockIndex].Color == color)
        {           
            // ���� �� ����
            SetColors();
            stageIndex++;
            score = score+stageIndex;
            stageText.text = "Stage " + stageIndex.ToString();
            scoreText.text = "Score " + score.ToString();
            print("���� ��ġ ! - ���� ȹ�� ó��");            
        }
        else
        {
            print("����...");
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
