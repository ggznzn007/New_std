using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    // 블럭관련 변수
    const int SIZE = 10;                                        // 블럭 크기   
    public Color[] ShapeColors;                                 // 블럭 색깔
    public GameObject[] Cells;                                  // 게임바탕의 셀
    public int[,] Array = new int[SIZE, SIZE];                  // 게임바탕 셀의 배열
    public GameObject[] Shapes;                                 // 생성되는 블럭의 배열    
    public Transform[] BlockPos;                                // 블럭의 위치
    public event System.Action EditorRepaint = () => { };       // 셀을 재배치하기 위한

    // 점수관련 변수   
    int beforeScore;                                            // 최고점수를 저장하기 위한 변수
    int newScore;                                               // 새로운점수를 저장하기 위한 변수
    public TextMeshProUGUI ScoreTextMeshPro;                    // 현재점수
    public TextMeshProUGUI BestScoreText;                       // 최고점수


    // 옵션 변수
    [SerializeField]
    private GameObject bgm;                                     // BGM 오브젝트
    [SerializeField]
    private GameObject menuPanel;                               // 메뉴창(게임오버 시)
    [SerializeField]
    private GameObject backPanel;                               // 메뉴창(뒤로가기 눌렀을 때)
    [SerializeField]
    private TextMeshProUGUI menupanelScore;                     // 메뉴창에 뜨는 점수(게임오버)
    [SerializeField]
    private TextMeshProUGUI backpanelScore;                     // 메뉴창에 뜨는 점수(뒤로가기)

    void Start()
    {
        StartCoroutine(BeforeScore());
        StartCoroutine(SpawnBlocks()); // 블럭 생성
    }

    private void Update()
    {
        StartCoroutine(BackPanel());
    }

    IEnumerator BackPanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // 뒤로가기
        {
            bgm.GetComponent<AudioSource>().Pause(); // BGM 잠깐 멈춤
            SoundManager.instance.Playsound(SoundManager.instance.blockClick); // 블럭선택소리 재생
            backPanel.SetActive(true); // 메뉴창(뒤로가기) 활성화
            if (newScore > beforeScore)
            {
                backpanelScore.text = "New Best : " + newScore.ToString();
            }
            else
            {
                backpanelScore.text = "현재기록 : " + newScore.ToString(); // 현재점수 표시
            }
        }
        yield return null;
    }

    IEnumerator BeforeScore()
    {
        if (PlayerPrefs.HasKey("beforeScore"))
        {
            beforeScore = PlayerPrefs.GetInt("beforeScore");     // 이전 턴에서 저장된 점수를 불러오기
            BestScoreText.text = "Best : " + beforeScore.ToString(); // 최고점수 표시
        }
        PlayerPrefs.Save(); // 점수 저장
        yield return null;
    }

    public GameObject[] ShuffleShapes(GameObject[] array) // 블럭배열 내에 블럭을 섞는 메소드
    {
        int rand1, rand2;
        GameObject temp;

        for (int i = 0; i < array.Length; i++)
        {
            rand1 = Random.Range(0, array.Length);
            rand2 = Random.Range(0, array.Length);

            temp = array[rand1];
            array[rand1] = array[rand2];
            array[rand2] = temp;
        }
        return array;
    }
    /*for (int i = 0; i < BlockPos.Length; i++)
        {
            
            yield return new WaitForSeconds(0.08f);
            //Transform CurShape = Instantiate(Shapes[Random.Range(0,Shapes.Length)],
            Transform CurShape = Instantiate(Shapes[Random.Range(0, Shapes.Length)],
            //Transform CurShape = Instantiate(Shapes[17],            
            BlockPos[i].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
            CurShape.SetParent(BlockPos[i]);
            CurShape.DOMove(BlockPos[i].position, 0.4f);
        }*/

    IEnumerator SpawnBlocks() // 블럭생성 메소드
    {
        SoundManager.instance.Playsound(SoundManager.instance.blockSet); // 블럭생성소리 재생    
        yield return new WaitForSeconds(0.09f);
        if (BlockPos[0])
        {
            StartCoroutine(SpawnFirstBlock());
        }
        if (BlockPos[1])
        {
            StartCoroutine(SpawnSecondBlock());
        }
        if (BlockPos[2])
        {
            StartCoroutine(SpawnThirdBlock());
        }
        yield return new WaitForSeconds(0.1f);
        ShuffleColor(ShapeColors); // 3개의 블럭을 놓은 후 놓는 블럭의 색을 변경
        yield return null;

        if (!AvailCheck()) Die(); // 게임오버시 생성 중단
    }

    public IEnumerator SpawnFirstBlock()
    {
        if (newScore >= 0 && newScore < 1500)
        {
            int rand = Random.Range(0, 4);
            Debug.Log("Level 1 FirstBlock블럭생성");
            Transform CurShape = Instantiate(Shapes[rand], BlockPos[0].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
            CurShape.SetParent(BlockPos[0]);
            CurShape.DOMove(BlockPos[0].position, 0.4f);
            yield return null;
        }
        else if (newScore >= 1500 && newScore < 3000)
        {
            int rand = Random.Range(1, 5);
            Debug.Log("Level 2 FirstBlock블럭생성");
            Transform CurShape = Instantiate(Shapes[rand], BlockPos[0].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
            CurShape.SetParent(BlockPos[0]);
            CurShape.DOMove(BlockPos[0].position, 0.4f);
            yield return null;
        }
        else if (newScore >= 3000 && newScore < 4500)
        {
            int rand = Random.Range(2, 6);
            Debug.Log("Level 3 FirstBlock블럭생성");
            Transform CurShape = Instantiate(Shapes[rand], BlockPos[0].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
            CurShape.SetParent(BlockPos[0]);
            CurShape.DOMove(BlockPos[0].position, 0.4f);
            yield return null;
        }
        else if (newScore >= 4500 && newScore < 6000)
        {
            int rand = Random.Range(3, 7);
            Debug.Log("Level 4 FirstBlock블럭생성");
            Transform CurShape = Instantiate(Shapes[rand], BlockPos[0].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
            CurShape.SetParent(BlockPos[0]);
            CurShape.DOMove(BlockPos[0].position, 0.4f);
            yield return null;
        }
        else if (newScore >= 6000)
        {
            int rand = Random.Range(0, 7);
            Debug.Log("Level 5 FirstBlock블럭생성");
            Transform CurShape = Instantiate(Shapes[rand], BlockPos[0].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
            CurShape.SetParent(BlockPos[0]);
            CurShape.DOMove(BlockPos[0].position, 0.4f);
            yield return null;
        }
    }

    public IEnumerator SpawnSecondBlock()
    {
        if (newScore >= 0 && newScore < 1500)
        {
            int rand = Random.Range(7, 11);
            Debug.Log("Level 1 SecondBlock블럭생성");
            Transform CurShape = Instantiate(Shapes[rand], BlockPos[1].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
            CurShape.SetParent(BlockPos[1]);
            CurShape.DOMove(BlockPos[1].position, 0.4f);
            yield return null;
        }
        else if (newScore >= 1500 && newScore < 3000)
        {
            int rand = Random.Range(8, 12);
            Debug.Log("Level 2 SecondBlock블럭생성");
            Transform CurShape = Instantiate(Shapes[rand], BlockPos[1].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
            CurShape.SetParent(BlockPos[1]);
            CurShape.DOMove(BlockPos[1].position, 0.4f);
            yield return null;
        }
        else if (newScore >= 3000 && newScore < 4500)
        {
            int rand = Random.Range(9, 13);
            Debug.Log("Level 3 SecondBlock블럭생성");
            Transform CurShape = Instantiate(Shapes[rand], BlockPos[1].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
            CurShape.SetParent(BlockPos[1]);
            CurShape.DOMove(BlockPos[1].position, 0.4f);
            yield return null;
        }
        else if (newScore >= 4500 && newScore < 6000)
        {
            int rand = Random.Range(10, 14);
            Debug.Log("Level 4 SecondBlock블럭생성");
            Transform CurShape = Instantiate(Shapes[rand], BlockPos[1].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
            CurShape.SetParent(BlockPos[1]);
            CurShape.DOMove(BlockPos[1].position, 0.4f);
            yield return null;
        }
        else if (newScore >= 6000)
        {
            int rand = Random.Range(7, 14);
            Debug.Log("Level 5 SecondBlock블럭생성");
            Transform CurShape = Instantiate(Shapes[rand], BlockPos[1].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
            CurShape.SetParent(BlockPos[1]);
            CurShape.DOMove(BlockPos[1].position, 0.4f);
            yield return null;
        }
    }

    public IEnumerator SpawnThirdBlock()
    {
        if (newScore >= 0 && newScore < 1500)
        {
            int rand = Random.Range(14, 18);
            Debug.Log("Level 1 ThirdBlock블럭생성");
            Transform CurShape = Instantiate(Shapes[rand], BlockPos[2].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
            CurShape.SetParent(BlockPos[2]);
            CurShape.DOMove(BlockPos[2].position, 0.4f);
            yield return null;
        }
        else if (newScore >= 1500 && newScore < 3000)
        {
            int rand = Random.Range(15, 19);
            Debug.Log("Level 2 ThirdBlock블럭생성");
            Transform CurShape = Instantiate(Shapes[rand], BlockPos[2].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
            CurShape.SetParent(BlockPos[2]);
            CurShape.DOMove(BlockPos[2].position, 0.4f);
            yield return null;
        }
        else if (newScore >= 3000 && newScore < 4500)
        {
            int rand = Random.Range(16, 20);
            Debug.Log("Level 3 ThirdBlock블럭생성");
            Transform CurShape = Instantiate(Shapes[rand], BlockPos[2].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
            CurShape.SetParent(BlockPos[2]);
            CurShape.DOMove(BlockPos[2].position, 0.4f);
            yield return null;
        }
        else if (newScore >= 4500 && newScore < 6000)
        {
            int rand = Random.Range(17, 21);
            Debug.Log("Level 4 ThirdBlock블럭생성");
            Transform CurShape = Instantiate(Shapes[rand], BlockPos[2].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
            CurShape.SetParent(BlockPos[2]);
            CurShape.DOMove(BlockPos[2].position, 0.4f);
            yield return null;
        }
        else if (newScore >= 6000)
        {
            int rand = Random.Range(14, 21);
            Debug.Log("Level 5 ThirdBlock블럭생성");
            Transform CurShape = Instantiate(Shapes[rand], BlockPos[2].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
            CurShape.SetParent(BlockPos[2]);
            CurShape.DOMove(BlockPos[2].position, 0.4f);
            yield return null;
        }
    }

    /*public IEnumerator SpawnSecondBlock()
    {
        Transform CurShape = Instantiate(Shapes[Random.Range(7, 14)],BlockPos[0].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
        CurShape.SetParent(BlockPos[1]);
        CurShape.DOMove(BlockPos[1].position, 0.4f);
        yield return null;
    }
   
    public IEnumerator SpawnThirdBlock()
    {
        Transform CurShape = Instantiate(Shapes[Random.Range(14, 17)],BlockPos[2].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
        CurShape.SetParent(BlockPos[2]);
        CurShape.DOMove(BlockPos[2].position, 0.4f);
        yield return null;
    }*/

    GameObject GetCell(int x, int y)
    {
        return Cells[y * SIZE + x]; // 배경이 되는 셀 불러오기
    }

    bool InRange(int x, int y) // 배경이 되는 셀 범위
    {
        if (x < 0 || y < 0 || x >= SIZE || y >= SIZE) return false;
        return true;
    }

    bool Possible(int x, int y) // 가능한 지 여부 판단
    {
        if (Array[x, y] != 0) return false;
        return true;
    }

    public Color[] ShuffleColor(Color[] array) // 놓여지는 블럭의 색상을 섞는 메소드
    {
        int rand1, rand2;
        Color temp;

        for (int i = 0; i < array.Length; i++)
        {
            rand1 = Random.Range(1, array.Length);
            rand2 = Random.Range(1, array.Length);

            temp = array[rand1];
            array[rand1] = array[rand2];
            array[rand2] = temp;
        }
        return array;
    }

    public void BlockInput(CellScript cellScript, int colorIndex, Vector3 lastPos, Vector3[] ShapePos)
    {

        // 블럭을 놓는 메소드
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


    void LineLogic(int shapePosLength) // 라인 꽉차면 없어지고 점수계산되는 메소드
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

        // 라인 파괴
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

        // 점수계산	
        newScore += oneLine switch // Switch case 사용
        {
            1 => (oneLine * 10) + (shapePosLength + 20),       // 1줄 클리어, 블럭수 + 1*10             10  보너스
            2 => (oneLine * 80) + (shapePosLength + 60),       // 2줄 클리어, 블럭수 + (1+2)*10         30  보너스
            3 => (oneLine * 160) + (shapePosLength + 90),      // 3줄 클리어, 블럭수 + (1+2+3)*10       60  보너스
            4 => (oneLine * 240) + (shapePosLength + 120),     // 4줄 클리어, 블럭수 + (1+2+3+4)*10     100 보너스
            5 => (oneLine * 320) + (shapePosLength + 150),     // 5줄 클리어, 블럭수 + (1+2+3+4+5)*10   150 보너스
            6 => (oneLine * 350) + (shapePosLength),           // 6줄 클리어, 블럭수 + (1+2+3+4+5+6)*10 210 보너스
            _ => shapePosLength, //Default => _ 로 표현 (C# 9.0에서 나온 간결한 기능 )
        };
        // 기존 버전 점수 배점
        /*newScore += oneLine switch // Switch case 사용
        {
            1 => oneLine * 10 + shapePosLength,       // 1줄 클리어, 블럭수 + 1*10             10  보너스
            2 => oneLine * 30 + shapePosLength ,      // 2줄 클리어, 블럭수 + (1+2)*10         30  보너스
            3 => oneLine * 60 + shapePosLength ,      // 3줄 클리어, 블럭수 + (1+2+3)*10       60  보너스
            4 => oneLine * 100 + shapePosLength,      // 4줄 클리어, 블럭수 + (1+2+3+4)*10     100 보너스
            5 => oneLine * 150 + shapePosLength,      // 5줄 클리어, 블럭수 + (1+2+3+4+5)*10   150 보너스
            6 => oneLine * 210 + shapePosLength,      // 6줄 클리어, 블럭수 + (1+2+3+4+5+6)*10 210 보너스
            _ => shapePosLength, //Default => _ 로 표현 (C# 9.0에서 나온 간결한 기능 )
        };*/
        //newScore += oneLine * 10 + shapePosLength;       
        ScoreTextMeshPro.text = newScore.ToString(); // 현재점수 표시
        if (newScore > beforeScore) // 이전점수보다 현재점수 크면 저장
        {
            PlayerPrefs.SetInt("beforeScore", newScore);
        }
    }

    bool Putable(Vector3[] ShapePos) // 블럭을 놓을 수 있는지 판단
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

    void EndTurn() // 화면 아래에 3개의 블럭이 다 사용되면 새로운 블럭생성
    {
        ScoreTextMeshPro.text = newScore.ToString();

        if (!AvailCheck()) { Die(); return; }

        int count = 0;
        for (int i = 0; i < BlockPos.Length; i++)
            if (BlockPos[i].childCount != 0) ++count;

        if (count == 0) StartCoroutine(SpawnBlocks());

    }

    void Die() // 게임오버 판단 메소드
    {
        SoundManager.instance.Playsound(SoundManager.instance.gameOver);
        bgm.GetComponent<AudioSource>().Pause(); // BGM 잠시멈춤        
        menuPanel.SetActive(true); // 메뉴창 활성화
        if (newScore > beforeScore)
        {
            menupanelScore.text = "New Best : " + newScore.ToString();
        }
        else
        {

            menupanelScore.text = "현재기록 : " + newScore.ToString();
        }
    }

    public void BackToGame() // 돌아가기 
    {
        SoundManager.instance.Playsound(SoundManager.instance.blockClick);
        backPanel.SetActive(false); // 메뉴창 비활성화
        bgm.GetComponent<AudioSource>().Play(); // BGM 다시재생
    }

    public void ReStart() // 다시하기
    {
        SoundManager.instance.Playsound(SoundManager.instance.blockClick);
        SceneManager.LoadScene("SampleScene");       // 게임 재시작
    }

    public void QuitGame() // 나가기
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        // 안드로이드
#else
Application.Quit();

#endif
    }
}
