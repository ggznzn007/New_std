using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static UnityEngine.Debug;
using TMPro;
using UnityEngine.XR;
using System.Linq;
using SimpleFileBrowser;
using UnityEngine.Networking;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Rendering;

public class FootPrint : MonoBehaviour
{
    public static FootPrint FPM;
    [SerializeField] private Camera myCam;
    [SerializeField] GameObject frontdeadLine;
    [SerializeField] GameObject hiddendeadLine;
    [SerializeField] GameObject backdeadLine;
    [SerializeField] GameObject footPrint;
    [SerializeField] GameObject foot;
    [SerializeField] GameObject backGround;
    [SerializeField] GameObject startBackGround;
    [SerializeField] GameObject setCanvas;
    [SerializeField] private Slider sizeSlider;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider distanceSlider;
    [SerializeField] private Slider timeSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider exSlider;
    [SerializeField] private TextMeshProUGUI sizeText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI bgmText;
    [SerializeField] private TextMeshProUGUI exText;
    [SerializeField] private AudioSource BGM_Speaker;
    [SerializeField] private AudioSource[] EX_Speaker;

    [SerializeField] TMP_Text secText;
    [SerializeField] TMP_Text countdownText;
    public GameObject startPannel;
    public GameObject failPannel;
    public GameObject successPannel;
    public float curSecond;

    private Vector3 m_Offset;
    private float m_ZCoord;
    private GameObject myFoot;
    private GameObject myFootReal;
    public bool gamePaused;
    public bool isStart;
    public bool isReady;
    public int lineCnt;
    private byte[] bg_data1;
    private byte[] bg_data2;

    private void Awake()
    {
        SetResolution();
        LoadBackGround();
    }

    void Start()
    {
        FPM = this;
        lineCnt = 0;
        LoadTime();
        isStart = false;
        isReady = false;
        failPannel.SetActive(false);
        successPannel.SetActive(false);
        hiddendeadLine.GetComponent<BoxCollider>().enabled = false;
        frontdeadLine.GetComponent<BoxCollider>().enabled = false;
        backdeadLine.GetComponent<BoxCollider>().enabled = false;
    }

    public void SetResolution()
    {
        int setWidth = 3200;   // 화면 가로
        int setHeight = 1080;  // 화면 세로
        Screen.SetResolution(setWidth, setHeight, true); // true: 풀스크린, false:창모드
    }

    void Update()
    {
        LoadAndUpdateValue();
        SettingUI();
        if (isStart)
        {
            if (gamePaused) return;
            curSecond -= Time.deltaTime;
            if (curSecond <= 0)
            {
                curSecond = 0;
                FailWalk();
            }
            int sec = (int)curSecond;
            int minutes = sec / 60; // 분 계산
            int seconds = sec % 60; // 초 계산
            // string formattedTime = string.Format("{0:00}m:{1:00}s", minutes, seconds);
            string formattedTime = string.Format("{00:00}:{01:00}", minutes, seconds);
            secText.text = formattedTime;
        }
        FootVisual();
        MoveObj();
        Zoominout();
    }

    public void BacktoStart()                                                // 재시작
    {
        isStart = false;
        isReady = false;
        curSecond = timeSlider.value;
        DeadLineMove.DLM.gameObject.transform.position = new Vector3(0, 2, -57);
        HiddenLine.HLM.gameObject.transform.position = new Vector3(0, 2, -30.9f);
        DeadLineMove2.DLM2.gameObject.transform.position = new Vector3(0, 0, distanceSlider.value);
        hiddendeadLine.GetComponent<BoxCollider>().enabled = false;
        frontdeadLine.GetComponent<BoxCollider>().enabled = false;
        backdeadLine.GetComponent<BoxCollider>().enabled = false;
        startPannel.SetActive(true);
        //   Destroy(footPrint);
    }

    public void StartWalk()                                                  // 게임 시작
    {
        startPannel.SetActive(false);
        isReady = true;
        StartCoroutine(CountDown());
    }

    public void FinishWalk()                                                 // 게임 끝(성공)
    {
        StartCoroutine(GameFinish());
    }

    public void FailWalk()                                                   // 게임 실패
    {
        StartCoroutine(GameFail());
    }

    IEnumerator CountDown()                                                  // 카운트다운
    {
        int count = 3;
        AudioManager.AM.PlaySE("Button");
        AudioManager.AM.PlayeBGM(1);
        countdownText.gameObject.SetActive(true);
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSecondsRealtime(1.0f);
            count--;
        }
        countdownText.text = "START!";
        yield return new WaitForSeconds(1f);
        isStart = true;
        countdownText.gameObject.SetActive(false);
        hiddendeadLine.GetComponent<BoxCollider>().enabled = true;
        frontdeadLine.GetComponent<BoxCollider>().enabled = true;
        backdeadLine.GetComponent<BoxCollider>().enabled = true;
    }

    IEnumerator GameFinish()
    {
        isStart = false;
        isReady = false;
        AudioManager.AM.PlaySE("Success");
        successPannel.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        successPannel.SetActive(false);
        BacktoStart();
    }

    IEnumerator GameFail()
    {
        isStart = false;
        isReady = false;
        AudioManager.AM.PlaySE("Fail");
        yield return new WaitForSeconds(0.2f);
        failPannel.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        failPannel.SetActive(false);
        BacktoStart();
    }

    public void DistanceSetting()                                               // 두개의 bar간 거리 세팅
    {
        if (isStart) { return; }
        float distanceVal = distanceSlider.value;
        PlayerPrefs.SetFloat("distance", distanceVal);
        backdeadLine.transform.position = new Vector3(0, 2, distanceVal);
        distanceText.text = distanceVal.ToString("F0");
        PlayerPrefs.SetString("distanceTxt", distanceText.text);
    }

    public void TimeSetting()                                                   // 시간 세팅
    {
        if (isStart) { return; }
        curSecond = timeSlider.value;
        PlayerPrefs.SetFloat("time", curSecond);
        timeText.text = curSecond.ToString("F0") + " Sec";
        PlayerPrefs.SetString("timeTxt", timeText.text);
    }

    public void SizeSetting()                                                   // 발자국 크기 세팅
    {
        float sizeVal = sizeSlider.value;
        PlayerPrefs.SetFloat("size", sizeVal);
        footPrint.transform.localScale = new Vector3(sizeVal, sizeVal, 1f);
        sizeText.text = sizeVal.ToString("F1");// 소수점 1자리 표현
        PlayerPrefs.SetString("sizeTxt", sizeText.text);
    }

    public void SpeedSetting()                                                  // bar 속도 세팅
    {
        float speedVal = speedSlider.value;
        PlayerPrefs.SetFloat("speed", speedVal);
        DeadLineMove.DLM.speed = speedVal;
        DeadLineMove2.DLM2.speed = speedVal;
        HiddenLine.HLM.speed = speedVal;
        speedText.text = speedVal.ToString("F1");// 소수점 1자리 표현
        PlayerPrefs.SetString("speedTxt", speedText.text);
    }

    public void BGM_Vol_Setting()
    {
        float bgmVol = bgmSlider.value;
        PlayerPrefs.SetFloat("BGM_Vol", bgmVol);
        BGM_Speaker.volume = bgmVol;
        bgmText.text = bgmVol.ToString("F1");
        PlayerPrefs.SetString("bgmTxt", bgmText.text);
    }

    public void EX_Vol_Setting()
    {
        float exVol = exSlider.value;
        PlayerPrefs.SetFloat("EX_Vol", exVol);
        for (int i = 0; i < EX_Speaker.Length; i++)
        {
            EX_Speaker[i].volume = exVol;
        }
        exText.text = exVol.ToString("F1");
        PlayerPrefs.SetString("exTxt", exText.text);
    }

    public void LoadAndUpdateValue()                                            // 세팅 값 불러오기
    {
        float sizeVal = PlayerPrefs.GetFloat("size");
        float speedVal = PlayerPrefs.GetFloat("speed");
        float distanceVal = PlayerPrefs.GetFloat("distance");
        float bgmVolVal = PlayerPrefs.GetFloat("BGM_Vol");
        float exVolVal = PlayerPrefs.GetFloat("EX_Vol");
        string sizeTxt = PlayerPrefs.GetString("sizeTxt");
        string speedTxt = PlayerPrefs.GetString("speedTxt");
        string distanceTxt = PlayerPrefs.GetString("distanceTxt");
        string bgmTxt = PlayerPrefs.GetString("bgmTxt");
        string exTxt = PlayerPrefs.GetString("exTxt");
        sizeSlider.value = sizeVal;
        speedSlider.value = speedVal;
        distanceSlider.value = distanceVal;
        bgmSlider.value = bgmVolVal;
        exSlider.value = exVolVal;
        sizeText.text = sizeTxt;
        speedText.text = speedTxt;
        distanceText.text = distanceTxt;
        bgmText.text = bgmTxt;
        exText.text = exTxt;
    }

    public void LoadTime()                                                    // 세팅 시간 불러오기
    {
        curSecond = PlayerPrefs.GetFloat("time");
        timeSlider.value = curSecond;
        string timeTxt = PlayerPrefs.GetString("timeTxt");
        timeText.text = timeTxt;
    }

    void FootVisual()                                                            // 발자국 메서드
    {
        if (gamePaused) { return; }
        if (!gamePaused)
        {
            if (Input.GetMouseButtonDown(0))                                     // 마우스 클릭시 한번만 호출
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                {
                    Vector3 hitPos = hit.point;
                    if (isReady)
                    {
                        myFoot = Instantiate(footPrint, hitPos, footPrint.transform.rotation);
                        myFootReal = Instantiate(foot, hitPos, foot.transform.rotation);
                        m_ZCoord = Camera.main.WorldToScreenPoint(myFoot.transform.position).z;
                        m_Offset = myFoot.transform.position - GetMouseWorldPosition();
                    }
                }
            }

            if (Input.GetMouseButton(0))                                         // 마우스 클릭중(드래그) 계속 호출
            {
                if (isReady)
                {
                    myFoot.transform.position = GetMouseWorldPosition() + m_Offset;
                    myFootReal.transform.position = GetMouseWorldPosition() + m_Offset;
                }
            }

            if (Input.GetMouseButtonUp(0))                                       // 마우스 클릭종료 시 한번만 호출
            {
                Destroy(myFoot, 0.5f);
                Destroy(myFootReal, 0.5f);
                // StartCoroutine(DelayDestroy());
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif   
        }                                // 프로그램 종료
    }

    Vector3 GetMouseWorldPosition()                                        // 마우스 위치값 함수
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = m_ZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

  /*  IEnumerator DelayDestroy()                                             // 가상의 봉을 딜레이 후 삭제
    {
        yield return null;
        Destroy(myFoot);
        Destroy(myFootReal);
    }*/

    void SettingUI()                                                       // 세팅 UI 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            gamePaused = !gamePaused;

        if (gamePaused)
        {
            setCanvas.SetActive(true);
        }
        else
        {
            setCanvas.SetActive(false);
        }
    }

    void MoveObj()                                                        // 카메라 무빙
    {
        if (gamePaused) return;
        float keyH = Input.GetAxis("Horizontal");
        float keyV = Input.GetAxis("Vertical");
        keyH = keyH * Utils.move_Speed * Time.deltaTime;
        keyV = keyV * Utils.move_Speed * Time.deltaTime;
        myCam.transform.Translate(Vector3.right * keyH);
        myCam.transform.Translate(Vector3.up * keyV);

        if (myCam.transform.position.x <= Utils.minOrth_x)
        {
            myCam.transform.position = new Vector3(Utils.minOrth_x, myCam.transform.position.y, myCam.transform.position.z);
        }
        if (myCam.transform.position.x >= Utils.maxOrth_x)
        {
            myCam.transform.position = new Vector3(Utils.maxOrth_x, myCam.transform.position.y, myCam.transform.position.z);
        }

        if (myCam.transform.position.z <= Utils.minOrth_z)
        {
            myCam.transform.position = new Vector3(myCam.transform.position.x, myCam.transform.position.y, Utils.minOrth_z);
        }
        if (myCam.transform.position.z >= Utils.maxOrth_z)
        {
            myCam.transform.position = new Vector3(myCam.transform.position.x, myCam.transform.position.y, Utils.maxOrth_z);
        }

        /*  float mouseX = Input.GetAxis("Mouse X");                        회전코드
          float mouseY = Input.GetAxis("Mouse Y");
          transform.Rotate(Vector3.up * speed_rota * mouseX);
          transform.Rotate(Vector3.left * speed_rota * mouseY);*/
    }

    void Zoominout()                                                     // 카메라 줌인아웃
    {
        if (gamePaused) return;
        float scroollWheel = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize += scroollWheel * Time.deltaTime * Utils.scroll_Speed;

        if (Camera.main.orthographicSize >= Utils.maxOrth)
        {
            Camera.main.orthographicSize = Utils.maxOrth;
        }
        if (Camera.main.orthographicSize <= Utils.minOrth)
        {
            Camera.main.orthographicSize = Utils.minOrth;
        }

        /* Vector3 cameraDirection = this.transform.localRotation * Vector3.back;           // 카메라 트랜스폼 자체이동코드

         this.transform.position += scrollSpeed * scroollWheel * Time.deltaTime * cameraDirection; */
    }

    public void LoadBackGround()
    {
        if (Directory.Exists("BackGround") == false)
        {
            Directory.CreateDirectory("BackGround");
        }

        string[] files = Directory.GetFiles("BackGround");//(System.IO)생략
        foreach (string file in files)
        {
            bg_data1 = File.ReadAllBytes("BackGround/BG_Main.png");
            Texture2D imageTexture = new(3200, 1080, TextureFormat.RGB24, false);
            imageTexture.LoadImage(bg_data1);
            backGround = GameObject.Find("Background");
            Renderer rd = backGround.GetComponent<Renderer>();
            rd.material.mainTexture = imageTexture;
        }

        string[] files2 = Directory.GetFiles("BackGround");
        foreach (string file2 in files)
        {
            //byte[] bytes = File.ReadAllBytes("BackGround/BG_Start.png");
            bg_data2 = File.ReadAllBytes("BackGround/BG_Start.png");
            Texture2D texture2 = new(3200, 1080, TextureFormat.ARGB32, false);
            texture2.LoadImage(bg_data2);
            startBackGround = GameObject.Find("StartImage");
            RawImage test = startBackGround.GetComponent<RawImage>();
            test.texture = texture2;
        }
    }

    public void OpenBrowser_BG_Main()                                            // 배경 불러오기 메서드
    {
        FileBrowser
            .SetFilters(true, new FileBrowser
            .Filter("Images", ".jpg", ".png", "json")
            , new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
        FileBrowser.SetDefaultFilter(".png");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);
        StartCoroutine(ShowLoadDialogCoroutine());
    }


    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");
        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log(FileBrowser.Result[i]);

            bg_data1 = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

            Texture2D texture2 = new(3200, 1080, TextureFormat.ARGB32, false);
            texture2.LoadImage(bg_data1);
            backGround = GameObject.Find("Background");
            Renderer test = backGround.GetComponent<Renderer>();
            test.material.mainTexture = texture2;

            // string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            // FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
        }
    }

    public void OpenBrowser_BG_Start()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
        FileBrowser.SetDefaultFilter(".jpg");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);
        StartCoroutine(ShowLoadDialogCoroutine_1());
    }

    IEnumerator ShowLoadDialogCoroutine_1()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");
        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log(FileBrowser.Result[i]);

            bg_data2 = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);
            Texture2D texture2 = new(3200, 1080, TextureFormat.ARGB32, false);
            texture2.LoadImage(bg_data2);
            startBackGround = GameObject.Find("StartImage");
            RawImage test = startBackGround.GetComponent<RawImage>();
            test.texture = texture2;

            //string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            // FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
        }
    }

    public void ReStart()                                                // 재시작 
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()                                               // 게임 종료
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 프로그램 종료
#endif   
    }
}
