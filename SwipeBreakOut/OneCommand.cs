using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class OneCommand : MonoBehaviour
{
    #region 태그에 따른 함수호출
    void Awake() { if (CompareTag("GameManager")) Awake_GM(); }

    void Update() { if (CompareTag("GameManager")) Update_GM(); }

    void FixedUpdate() { if (CompareTag("GameManager")) FixedUpdate_GM(); }

    void Start() { if (CompareTag("Ball")) Start_BALL(); }

    void OnCollisionEnter2D(Collision2D col) { if (CompareTag("Ball")) StartCoroutine(OnCollisionEnter2D_BALL(col)); }

    void OnTriggerEnter2D(Collider2D col) { if (CompareTag("Ball")) StartCoroutine(OnTriggerEnter2D_BALL(col)); }
    #endregion



    #region GameManager.Cs
    [Header("GameManagerValue")]
    public float groundY = -55.489f;
    public GameObject P_Ball, P_GreenOrb, P_Block, P_ParticleBlue, P_ParticleGreen, P_ParticleRed;
    public GameObject BallPreview, Arrow, GameOverPanel, BallCountTextObj, BallPlusTextObj;
    public Transform GreenBallGroup, BlockGroup, BallGroup;
    public LineRenderer MouseLR, BallLR;
    public Text BestScoreText, ScoreText, BallCountText, BallPlusText, FinalScoreText, NewRecordText;
    public Color[] blockColor;
    public Color greenColor;
    public AudioSource S_GameOver, S_GreenOrb, S_Plus;
    public AudioSource[] S_Block;
    public Quaternion QI = Quaternion.identity;
    public bool shotTrigger, shotable;
    public Vector3 veryFirstPos;

    Vector3 firstPos, secondPos, gap;
    int score, timerCount, launchIndex;
    bool timerStart, isDie, isNewRecord, isBlockMoving;
    float timeDelay;



    #region 시작
    void Awake_GM()
    {
        // 9:16 고정해상도 카메라
        Camera camera = Camera.main;
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16); // (가로 / 세로)
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;


        // 시작
        BlockGenerator();
        BestScoreText.text = "최고기록 : " + PlayerPrefs.GetInt("BestScore").ToString();
    }



    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);



    public void VeryFirstPosSet(Vector3 pos) { if (veryFirstPos == Vector3.zero) veryFirstPos = pos; }
    #endregion



    #region 블럭
    void BlockGenerator()
    {
        // 점수 
        ScoreText.text = "현재점수 : " + (++score).ToString();
        if (PlayerPrefs.GetInt("BestScore", 0) < score)
        {
            PlayerPrefs.SetInt("BestScore", score);
            BestScoreText.text = "최고기록 : " + PlayerPrefs.GetInt("BestScore").ToString();
            BestScoreText.color = greenColor;
            isNewRecord = true;
        }


        // 점수에 따른 블럭복사개수 정하기
        int count;
        int randBlock = Random.Range(0, 24);
        if (score <= 10) count = randBlock < 16 ? 1 : 2;
        else if (score <= 20) count = randBlock < 8 ? 1 : (randBlock < 16 ? 2 : 3);
        else if (score <= 40) count = randBlock < 9 ? 2 : (randBlock < 18 ? 3 : 4);
        else count = randBlock < 8 ? 2 : (randBlock < 16 ? 3 : (randBlock < 20 ? 4 : 5));


        // 스폰좌표에 블럭, 초록구 생성
        List<Vector3> SpawnList = new List<Vector3>();
        for (int i = 0; i < 6; i++) SpawnList.Add(new Vector3(-46.7f + i * 18.68f, 51.2f, 0));

        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, SpawnList.Count);

            Transform TR = Instantiate(P_Block, SpawnList[rand], QI).transform;
            TR.SetParent(BlockGroup);
            TR.GetChild(0).GetComponentInChildren<Text>().text = score.ToString();

            SpawnList.RemoveAt(rand);
        }
        Instantiate(P_GreenOrb, SpawnList[Random.Range(0, SpawnList.Count)], QI).transform.SetParent(BlockGroup);


        // 블럭 내리기
        isBlockMoving = true;
        for (int i = 0; i < BlockGroup.childCount; i++) StartCoroutine(BlockMoveDown(BlockGroup.GetChild(i)));
    }



    IEnumerator BlockMoveDown(Transform TR)
    {
        yield return new WaitForSeconds(0.2f);
        Vector3 targetPos = TR.position + new Vector3(0, -12.8f, 0);
        BlockColorChange();


        // 막줄이면 게임오버 트리거, 콜라이더 비활성화
        if (targetPos.y < -50)
        {
            if (TR.CompareTag("Block")) isDie = true;
            for (int i = 0; i < BallGroup.childCount; i++)
                BallGroup.GetChild(i).GetComponent<CircleCollider2D>().enabled = false;
        }


        // 0.3초간 블럭 이동
        float TT = 1.5f;
        while (true)
        {
            yield return null; TT -= Time.deltaTime * 1.5f;
            TR.position = Vector3.MoveTowards(TR.position, targetPos + new Vector3(0, -6, 0), TT);
            if (TR.position == targetPos + new Vector3(0, -6, 0)) break;
        }
        TT = 0.9f;
        while (true)
        {
            yield return null; TT -= Time.deltaTime;
            TR.position = Vector3.MoveTowards(TR.position, targetPos, TT);
            if (TR.position == targetPos) break;
        }
        isBlockMoving = false;


        // 이동되고 난 후 막줄이면 블럭이면 게임오버, 초록구면 파괴
        if (targetPos.y < -50)
        {
            if (TR.CompareTag("Block"))
            {
                for (int i = 0; i < BallGroup.childCount; i++)
                    Destroy(BallGroup.GetChild(i).gameObject);
                Destroy(Instantiate(P_ParticleBlue, veryFirstPos, QI), 1);

                BallCountTextObj.SetActive(false);
                BallPlusTextObj.SetActive(false);
                BestScoreText.gameObject.SetActive(false);
                ScoreText.gameObject.SetActive(false);

                GameOverPanel.SetActive(true);
                FinalScoreText.text = "최종점수 : " + score.ToString();
                if (isNewRecord) NewRecordText.gameObject.SetActive(true);

                Camera.main.GetComponent<Animator>().SetTrigger("shake");
                S_GameOver.Play();
            }
            else
            {
                Destroy(TR.gameObject);
                Destroy(Instantiate(P_ParticleGreen, TR.position, QI), 1);

                for (int i = 0; i < BallGroup.childCount; i++)
                    BallGroup.GetChild(i).GetComponent<CircleCollider2D>().enabled = true;
            }
        }
    }



    public void BlockColorChange()
    {
        // 블럭텍스트 / 스코어를 7등분해서 색을 칠함
        for (int i = 0; i < BlockGroup.childCount; i++)
        {
            if (BlockGroup.GetChild(i).CompareTag("Block"))
            {
                float per = int.Parse(BlockGroup.GetChild(i).GetChild(0).GetComponentInChildren<Text>().text) / (float)score;
                Color curColor;
                if (per <= 0.1428f) curColor = blockColor[6];
                else if (per <= 0.2856f) curColor = blockColor[5];
                else if (per <= 0.4284f) curColor = blockColor[4];
                else if (per <= 0.5172f) curColor = blockColor[3];
                else if (per <= 0.714f) curColor = blockColor[2];
                else if (per <= 0.8568f) curColor = blockColor[1];
                else curColor = blockColor[0];
                BlockGroup.GetChild(i).GetComponent<SpriteRenderer>().color = curColor;
            }
        }
    }
    #endregion



    void Update_GM()
    {
        if (isDie) return;


        // 마우스 첫번째 좌표
        if (Input.GetMouseButtonDown(0) && firstPos == Vector3.zero)
            firstPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);


        // 모든 움직임이 끝나면 쏠 수 있음
        shotable = true;
        for (int i = 0; i < BallGroup.childCount; i++)
            if (BallGroup.GetChild(i).GetComponent<OneCommand>().isMoving) shotable = false;
        if (isBlockMoving) shotable = false;


        if (!shotable) return;


        // 모든 공이 바닥에 부딪히면 한 번 실행
        if (shotTrigger && shotable)
        {
            shotTrigger = false;
            BlockGenerator();
            timeDelay = 0;

            StartCoroutine(BallCountTextShow(GreenBallGroup.childCount));
            for (int i = 0; i < GreenBallGroup.childCount; i++) StartCoroutine(GreenBallMove(GreenBallGroup.GetChild(i)));
        }


        timeDelay += Time.deltaTime;
        if (timeDelay < 0.1f) return; // 0.1초 딜레이로 너무 빠르게 손 떼면 라인이 남는 버그 제거


        bool isMouse = Input.GetMouseButton(0);
        if (isMouse)
        {
            // 차이값
            secondPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
            if ((secondPos - firstPos).magnitude < 1) return;
            gap = (secondPos - firstPos).normalized;
            gap = new Vector3(gap.y >= 0 ? gap.x : gap.x >= 0 ? 1 : -1, Mathf.Clamp(gap.y, 0.2f, 1), 0);





            // 화살표, 공 미리보기
            Arrow.transform.position = veryFirstPos;
            Arrow.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(gap.y, gap.x) * Mathf.Rad2Deg);
            BallPreview.transform.position =
                Physics2D.CircleCast(new Vector2(Mathf.Clamp(veryFirstPos.x, -54, 54), groundY), 1.7f, gap, 10000, 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Block")).centroid; // 1.8



            RaycastHit2D hit = Physics2D.Raycast(veryFirstPos, gap, 10000, 1 << LayerMask.NameToLayer("Wall"));

            // 라인
            MouseLR.SetPosition(0, firstPos);
            MouseLR.SetPosition(1, secondPos);
            BallLR.SetPosition(0, veryFirstPos);
            BallLR.SetPosition(1, (Vector3)hit.point - gap * 1.5f);
        }
        BallPreview.SetActive(isMouse);
        Arrow.SetActive(isMouse);


        if (Input.GetMouseButtonUp(0))
        {
            if ((secondPos - firstPos).magnitude < 1) return;

            // 라인 초기화
            MouseLR.SetPosition(0, Vector3.zero);
            MouseLR.SetPosition(1, Vector3.zero);
            BallLR.SetPosition(0, Vector3.zero);
            BallLR.SetPosition(1, Vector3.zero);

            timerStart = true;
            veryFirstPos = Vector3.zero;
            firstPos = Vector3.zero;
        }
    }



    IEnumerator BallCountTextShow(int greenBallCount)
    {
        // 초록공 합쳐지기 전후 공 개수 보여주기
        BallCountTextObj.transform.position = new Vector3(Mathf.Clamp(veryFirstPos.x, -49.9f, 49.9f), -65, 0);
        BallCountText.text = "x" + BallGroup.childCount.ToString();

        yield return new WaitForSeconds(0.17f);

        if (BallGroup.childCount > score) Destroy(BallGroup.GetChild(BallGroup.childCount - 1).gameObject);
        BallCountText.text = "x" + BallGroup.childCount.ToString();


        // 바닥에 떨어진 초록공 +로 표시하기
        if (greenBallCount != 0)
        {
            BallPlusTextObj.SetActive(true);
            BallPlusTextObj.transform.position = new Vector3(Mathf.Clamp(veryFirstPos.x, -49.9f, 49.9f), -47, 0);
            BallPlusText.text = "+" + greenBallCount.ToString();
            S_Plus.Play();

            yield return new WaitForSeconds(0.5f);

            BallPlusTextObj.SetActive(false);
        }
    }



    IEnumerator GreenBallMove(Transform TR)
    {
        // 바닥에 떨어진 초록공 최초좌표로 0.17초간 이동
        Instantiate(P_Ball, veryFirstPos, QI).transform.SetParent(BallGroup);
        float speed = (TR.position - veryFirstPos).magnitude / 12f;
        while (true)
        {
            yield return null;
            TR.position = Vector3.MoveTowards(TR.position, veryFirstPos, speed);
            if (TR.position == veryFirstPos) { Destroy(TR.gameObject); yield break; }
        }
    }



    void FixedUpdate_GM()
    {
        // 0.06초 간격으로 공 발사
        if (timerStart && ++timerCount == 3)
        {
            timerCount = 0;
            BallGroup.GetChild(launchIndex++).GetComponent<OneCommand>().Launch(gap);
            BallCountText.text = "x" + (BallGroup.childCount - launchIndex).ToString();
            if (launchIndex == BallGroup.childCount)
            {
                timerStart = false;
                launchIndex = 0;
                BallCountText.text = "";
            }
        }
    }
    #endregion



    #region BallScript.Cs
    [Header("BallScriptValue")]
    public GameObject GreenBall;
    public Rigidbody2D RB;
    public bool isMoving;

    OneCommand GM;



    void Start_BALL() => GM = GameObject.FindWithTag("GameManager").GetComponent<OneCommand>();



    public void Launch(Vector3 pos)
    {
        GM.shotTrigger = true;
        isMoving = true;
        RB.AddForce(pos * 7000);
    }



    IEnumerator OnCollisionEnter2D_BALL(Collision2D col)
    {
        GameObject Col = col.gameObject;
        Physics2D.IgnoreLayerCollision(2, 2);


        // 가로로 움직일경우 아래로 내림
        Vector2 pos = RB.velocity.normalized;
        if (pos.magnitude != 0 && pos.y < 0.15f && pos.y > -0.15f)
        {
            RB.velocity = Vector2.zero;
            RB.AddForce(new Vector2(pos.x > 0 ? 1 : -1, -0.2f).normalized * 7000);
        }


        // 바닥충돌시 최초좌표로 이동
        if (Col.CompareTag("Ground"))
        {
            RB.velocity = Vector2.zero;
            transform.position = new Vector2(col.contacts[0].point.x, GM.groundY);
            GM.VeryFirstPosSet(transform.position);

            while (true)
            {
                yield return null;
                transform.position = Vector3.MoveTowards(transform.position, GM.veryFirstPos, 4);
                if (transform.position == GM.veryFirstPos) { isMoving = false; yield break; }
            }
        }


        // 블럭충돌시 블럭숫자 1씩 줄이다 0이되면 부숨
        if (Col.CompareTag("Block"))
        {
            Text BlockText = col.transform.GetChild(0).GetComponentInChildren<Text>();
            int blockValue = int.Parse(BlockText.text) - 1;
            GM.BlockColorChange();

            for (int i = 0; i < GM.S_Block.Length; i++)
            {
                if (GM.S_Block[i].isPlaying) continue;
                else { GM.S_Block[i].Play(); break; }
            }


            if (blockValue > 0)
            {
                BlockText.text = blockValue.ToString();
                Col.GetComponent<Animator>().SetTrigger("shock");
            }
            else
            {
                Destroy(Col);
                Destroy(Instantiate(GM.P_ParticleRed, col.transform.position, Quaternion.identity), 1);
            }
        }
    }



    IEnumerator OnTriggerEnter2D_BALL(Collider2D col)
    {
        // 초록구 충돌시 초록공 생성해서 아래로 떨어짐
        if (col.gameObject.CompareTag("GreenOrb"))
        {
            Destroy(col.gameObject);
            Destroy(Instantiate(GM.P_ParticleGreen, col.transform.position, GM.QI), 1);

            GM.S_GreenOrb.Play();
            Transform TR = Instantiate(GreenBall, col.transform.position, GM.QI).transform;
            TR.SetParent(GameObject.Find("GreenBallGroup").transform);
            Vector3 targetPos = new Vector3(TR.position.x, GM.groundY, 0);
            while (true)
            {
                yield return null;
                TR.position = Vector3.MoveTowards(TR.position, targetPos, 2.5f);
                if (TR.position == targetPos) yield break;
            }
        }
    }
    #endregion
}
