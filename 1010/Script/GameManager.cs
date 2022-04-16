using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    const int SIZE = 10;
    public Color[] ShapeColors;
    public GameObject[] Cells;
    public int[,] Array = new int[SIZE, SIZE];
    public GameObject[] Shapes;
    public Transform[] BlockPos;
    public event System.Action EditorRepaint = () => { };

    // 점수관련 변수   
    int beforeScore;
    int newScore;
    public TextMeshProUGUI ScoreTextMeshPro;
    public TextMeshProUGUI BestScoreText;

    [SerializeField]
    private GameObject bgm;
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private GameObject backPanel;
    [SerializeField]
    private TextMeshProUGUI menupanelScore;
    [SerializeField]
    private TextMeshProUGUI backpanelScore;
    

    void Start()
    {
        if (PlayerPrefs.HasKey("beforeScore"))
        {
            beforeScore = PlayerPrefs.GetInt("beforeScore");
            BestScoreText.text = "Best : " + beforeScore.ToString();
        }
        PlayerPrefs.Save();
        StartCoroutine(SpawnBlocks());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bgm.GetComponent<AudioSource>().Pause();
            SoundManager.instance.Playsound(SoundManager.instance.blockClick);
            backPanel.SetActive(true);
            backpanelScore.text = "현재기록 : " + newScore.ToString();
        }
            /*#if UNITY_EDITOR            
                        UnityEditor.EditorApplication.isPlaying = false;
                        // 안드로이드
            #else
            Application.Quit();

            #endif
                    }*/
        }
    IEnumerator SpawnBlocks()
    {
        SoundManager.instance.Playsound(SoundManager.instance.blockSet);
        for (int i = 0; i < BlockPos.Length; i++)
        {
            yield return new WaitForSeconds(0.08f);            
            Transform CurShape = Instantiate(Shapes[Random.Range(0, Shapes.Length)],
            //Transform CurShape = Instantiate(Shapes[0],
            //Transform CurShape = Instantiate(Shapes[8],
            BlockPos[i].position + new Vector3(10, 0, 0), Quaternion.identity).transform;            
            CurShape.SetParent(BlockPos[i]);
            CurShape.DOMove(BlockPos[i].position, 0.4f);
        }
        yield return null;
        if (!AvailCheck()) Die();
    }


    GameObject GetCell(int x, int y)
    {
        return Cells[y * SIZE + x];
    }

    bool InRange(int x, int y)
    {
        if (x < 0 || y < 0 || x >= SIZE || y >= SIZE) return false;
        return true;
    }

    bool Possible(int x, int y)
    {
        if (Array[x, y] != 0) return false;
        return true;
    }


    public void BlockInput(CellScript cellScript, int colorIndex, Vector3 lastPos, Vector3[] ShapePos)
    {

        for (int i = 0; i < ShapePos.Length; i++)
        {
            Vector3 SumPos = ShapePos[i] + lastPos;
            if (!InRange((int)SumPos.x, (int)SumPos.y)) return;
            if (!Possible((int)SumPos.x, (int)SumPos.y)) return;
        }

        for (int i = 0; i < ShapePos.Length; i++)
        {
            Vector3 SumPos = ShapePos[i] + lastPos;
            Array[(int)SumPos.x, (int)SumPos.y] = colorIndex;
            GetCell((int)SumPos.x, (int)SumPos.y).GetComponent<SpriteRenderer>().color = ShapeColors[colorIndex];
        }

        cellScript.ForceDestroy();
        LineLogic(ShapePos.Length);
        Invoke("EndTurn", 0.07f);
        EditorRepaint();
    }

    void LineLogic(int shapePosLength)
    {
        // 가로세로
        int oneLine = 0;
        for (int i = 0; i < SIZE; i++)
        {
            int horizontalCount = 0;
            int verticalCount = 0;
            for (int j = 0; j < SIZE; j++)
            {
                if (Array[j, i] != 0) ++horizontalCount;
                if (Array[i, j] != 0) ++verticalCount;
            }

            if (horizontalCount == 10)
            {
                ++oneLine;
                for (int j = 0; j < SIZE; j++) Array[j, i] = -1;
            }
            if (verticalCount == 10)
            {
                ++oneLine;
                for (int j = 0; j < SIZE; j++) Array[i, j] = -1;
            }
        }

        // 파괴
        for (int i = 0; i < SIZE; i++)
            for (int j = 0; j < SIZE; j++)
                if (Array[i, j] == -1)
                {
                    Array[i, j] = 0;
                    GameObject CurCell = GetCell(i, j);
                    var doScale = CurCell.transform.DOScale(Vector3.zero, 0.2f);
                    doScale.OnComplete(() =>
                    {
                        CurCell.GetComponent<SpriteRenderer>().color = ShapeColors[0];
                        CurCell.transform.localScale = new Vector3(0.28f, 0.28f, 1);

                    });
                    SoundManager.instance.Playsound(SoundManager.instance.blockClear);
                }

        // 점수		
        newScore += oneLine * 12 + shapePosLength;

        ScoreTextMeshPro.text = newScore.ToString();
        if (newScore > beforeScore)
        {
            PlayerPrefs.SetInt("beforeScore", newScore);
        }
        
    }


    bool Putable(Vector3[] ShapePos)
    {
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                int count = 0;
                for (int k = 0; k < ShapePos.Length; k++)
                {
                    Vector3 CurShapePos = ShapePos[k] + new Vector3(i, j, 0);
                    if (!InRange((int)CurShapePos.x, (int)CurShapePos.y)) break;
                    if (!Possible((int)CurShapePos.x, (int)CurShapePos.y)) break;
                    ++count;
                }
                if (count == ShapePos.Length) return true;
            }
        }
        return false;
    }


    bool AvailCheck()
    {
        int count = 0;
        for (int i = 0; i < BlockPos.Length; i++)
        {
            if (BlockPos[i].childCount != 0)
            {
                count++;
                if (Putable(BlockPos[i].GetComponentInChildren<CellScript>().ShapePos)) return true; // 가능한 블럭
            }
        }
        return count == 0;
    }


    void EndTurn()
    {
        ScoreTextMeshPro.text = newScore.ToString();

        if (!AvailCheck()) { Die(); return; }

        int count = 0;
        for (int i = 0; i < BlockPos.Length; i++)
            if (BlockPos[i].childCount != 0) ++count;

        if (count == 0) StartCoroutine(SpawnBlocks());
    }


    void Die()
    {
        SoundManager.instance.Playsound(SoundManager.instance.gameOver);
        bgm.GetComponent<AudioSource>().Pause();
        print("죽음");
        menuPanel.SetActive(true);
        menupanelScore.text = "현재기록 : " + newScore.ToString();
    }

    public  void BackToGame()
    {
        SoundManager.instance.Playsound(SoundManager.instance.blockClick);
        backPanel.SetActive(false);
        bgm.GetComponent<AudioSource>().Play();
    }
    public void ReStart()
    {
        SoundManager.instance.Playsound(SoundManager.instance.blockClick);
        SceneManager.LoadScene("SampleScene");       
    }

    public void QuitGame()
    {        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        // 안드로이드
#else
Application.Quit();

#endif
    }
}
