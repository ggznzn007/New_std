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
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Manager_Ballon : MonoBehaviour                 // 풍선 매니저
{
    public static Manager_Ballon MB;
    [Header("메인 카메라")]
    [SerializeField] private Camera myCam;
    [Header("보이지않는 손")]
    [SerializeField] GameObject hiddenHand;
    [Header("메인 배경")]
    [SerializeField] GameObject backGround;
    [Header("셋팅 판넬")]
    [SerializeField] GameObject settinPanel;
    [Header("게임 판넬")]
    [SerializeField] GameObject gameoverPanel;
    [Header("풍선 크기조절 슬라이더")]
    [SerializeField] private Slider sizeSlider;
    [Header("풍선 개수조절 슬라이더")]
    [SerializeField] private Slider countSlider;
    [Header("타겟 풍선 텍스트")]
    [SerializeField] private TextMeshProUGUI targetText;
    [Header("목표 수 텍스트")]
    [SerializeField] private TextMeshProUGUI goalText;
    [Header("풍선 터진 개수 텍스트")]
    [SerializeField] private TextMeshProUGUI exploCountText;
    [Header("풍선 크기 텍스트")]
    [SerializeField] private TextMeshProUGUI sizeText;
    [Header("풍선 개수 텍스트")]
    [SerializeField] private TextMeshProUGUI countText;
    [Header("게임 정보 텍스트")]
    [SerializeField] private TextMeshProUGUI gameInfoText;
    [Header("풍선 생성 지점")]
    [SerializeField] private Transform[] spawnPoints;
    [Header("풍선 생성 범위")]
    [SerializeField] private BoxCollider area;
    [Header("풍선들")]
    [SerializeField] GameObject[] Ballons;
    [Header("풍선 현재 개수")]
    public int ballCount;
    [Header("풍선 총 개수")]
    public int cntSet;
    [Header("풍선 정렬 순서 숫자")]
    public int sortIndex = 0;
    [Header("터진 풍선 숫자")]
    public int explodedBallon;
    [Header("목표 풍선 숫자")]
    public int goalCount;

    public string[] ballonName =
    {
        "Black","Blue","Green","LightGreen","Orange","Pink","Red","Sky","White","Yellow"
    };

    public string targetName;

    private Vector3 m_Offset;
    private float m_ZCoord;
    private GameObject myHand;
    private bool gamePaused;
    private bool gameOver;

    private void Awake()
    {
        // SetResolution();                            // 해상도 초기화 메서드 Initialize Resolution Call
    }

    private void Start()
    {
        MB = this;
        area = GetComponent<BoxCollider>();         // 풍선 생성범위 초기화 Initialize balloon spawn range
        area.enabled = false;                       // 콜라이더 기능 끄기  Turning off the collider function        
        ballCount = 0;                              // 풍선 갯수 초기화    Initialize balloon count
        SetInitValue();
        InvokeRepeating(nameof(Spawn), 0.1f, 0.2f); // 시작 시 풍선 생성함수 일정간격으로 재생   Create Balloon at Start
    }

    private void Update()
    {
        LoadSizeValue();                            // 풍선 설정값 업데이트 메서드 Update balloon settings Method Call
        TouchThat();                                // 풍선 터치 시 메서드 Balloon Touch Method Call
        MoveObj();                                  // 카메라 움직임 메서드 Camera movement method Call
        Zoominout();                                // 카메라 줌인아웃 메서드 Camera zoom-in method Call
        SettingUI();                                // 세팅UI 메서드        Setting UI Method Call 
        PaintDeleteDirect();
        GameDone();
    }

    public void SetInitValue()
    {
        explodedBallon = 0;
        gameOver = false;
        int rand = Random.Range(0, ballonName.Length);
        int randGoal = Random.Range(Utils.goalMin, Utils.goalMax);
        targetName = ballonName[rand];
        goalCount = randGoal;
    }

    public void SetResolution()                     // 해상도 세팅 메서드 Resolution setting method
    {
        int setWidth = 3200;   // 화면 가로
        int setHeight = 1080;  // 화면 세로

        Screen.SetResolution(setWidth, setHeight, true); // true: 풀스크린, false:창모드
    }

    public void PaintDelete()                       // 페인팅효과 삭제 메서드 Delete painting effects method
    {
        GameObject[] painted = GameObject.FindGameObjectsWithTag("Effects");
        foreach (GameObject p in painted)
        {
            if (p != null)
            {
                Destroy(p);
                sortIndex = 0;
            }
        }
    }

    public void PaintDeleteDirect()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PaintDelete();
        }
    }

    public Vector3 GetRandomPosition()              // 풍선 생성 범위 메서드 Balloon Generation Range Method
    {
        Vector3 basePosition = transform.position;
        Vector3 size = area.size;

        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);
        float posZ = basePosition.z + Random.Range(-size.z / 2f, size.z / 2f);

        Vector3 spawnPos = new(posX, posY, posZ);
        return spawnPos;
    }

    public void Spawn()                              // 풍선 생성 메서드 Spawn Ballon method
    {
        if (ballCount >= cntSet) return;
        ballCount++;

        int selection = Random.Range(0, Ballons.Length);
        selection = selection switch
        {
            0 => 1,
            1 => 2,
            2 => 3,
            3 => 4,
            4 => 5,
            5 => 6,
            6 => 7,
            7 => 8,
            8 => 9,
            9 => 0,
            _ => 1,
        }; ;
        int selecPos = Random.Range(0, spawnPoints.Length);
        GameObject selectedPrefab = Ballons[selection];
        Debug.Log(Ballons[selection].name);
        Transform spawnPosition = spawnPoints[selecPos];
        Instantiate(selectedPrefab, spawnPosition.position, selectedPrefab.transform.rotation);
    }

    public void AddSpawn()                           // 추가 풍선 생성 메서드 Add Spawn Ballon method
    {
        int selection = Random.Range(0, Ballons.Length);
        GameObject selectedPrefab = Ballons[selection];
        Vector3 spawnPosition = GetRandomPosition();
        GameObject ballon = Instantiate(selectedPrefab, spawnPosition, selectedPrefab.transform.rotation);
        cntSet++;
    }

    public void SubBallon()
    {
        cntSet--;
    }

    void TouchThat()                                                       // 풍선 터치 시 메서드 Touch the Ballon method
    {
        if (Input.GetKey(KeyCode.Space))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif   
        }                                // 프로그램 종료
        if (gamePaused || gameOver) return;

        if (Input.GetMouseButtonDown(0))                                     // 마우스 클릭시 한번만 호출
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Vector3 hitPos = hit.point;
                myHand = Instantiate(hiddenHand, hitPos, hiddenHand.transform.rotation);
                m_ZCoord = Camera.main.WorldToScreenPoint(myHand.transform.position).z;
                m_Offset = myHand.transform.position - GetMouseWorldPosition();
            }
        }

        if (Input.GetMouseButton(0))                                         // 마우스 클릭중(드래그) 계속 호출
        {
            myHand.transform.position = GetMouseWorldPosition() + m_Offset;
        }

        if (Input.GetMouseButtonUp(0))                                       // 마우스 클릭종료 시 한번만 호출
        {
            StartCoroutine(DelayDestroy());
        }
    }


    Vector3 GetMouseWorldPosition()                                        // 마우스 위치값 메서드 Mouse Location Value method
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = m_ZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    public void GameDone()
    {
        if (explodedBallon == goalCount)
        {
            gameOver = true;
            gameoverPanel.SetActive(true);
            gameInfoText.text = "Great! Awesome!";
        }
        else if (explodedBallon < 0)
        {
            gameOver = true;
            gameoverPanel.SetActive(true);
            gameInfoText.text = "Game Over Try Again!";
        }
        else
        {
            gameOver = false;
            gameoverPanel.SetActive(false);
        }
    }

    public void SizeSetting()                                             // 풍선 크기 설정 메서드 Balloon size setting method
    {
        float sizeVal = sizeSlider.value;
        PlayerPrefs.SetFloat("size", sizeVal);
        sizeText.text = sizeVal.ToString("F0");
        PlayerPrefs.SetString("sizeText", sizeText.text);
        for (int i = 0; i < Ballons.Length; i++)
        {
            Ballons[i].transform.localScale = new Vector3(sizeVal, sizeVal, sizeVal);
        }
    }

    public void CountSetting()                                          // 풍선 개수 설정 메서드 Ballon Count setting method
    {
        cntSet = (int)countSlider.value;
        PlayerPrefs.SetFloat("ballCount", cntSet);
        countText.text = cntSet.ToString("F0");
        PlayerPrefs.SetString("countText", countText.text);
    }

    public void LoadSizeValue()                                        // 풍선 세팅값 불러오는 메서드 Load Settings method
    {
        float sizeVal = PlayerPrefs.GetFloat("size");
        string sizeTxt = PlayerPrefs.GetString("sizeText");
        sizeSlider.value = sizeVal;
        sizeText.text = sizeTxt;
        string countTxt = PlayerPrefs.GetString("countText");
        countText.text = countTxt;
        float ballonCnt = PlayerPrefs.GetFloat("ballCount");
        cntSet = (int)ballonCnt;
        countSlider.value = ballonCnt;
        targetText.text = targetName;
        goalText.text = goalCount.ToString();
        exploCountText.text = explodedBallon.ToString();
    }

    IEnumerator DelayDestroy()                                             // 가상의 손을 딜레이 후 삭제
    {
        yield return null;
        Destroy(myHand);
    }

    void SettingUI()                                     // 세팅 UI 메서드 Setting UI method
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            gamePaused = !gamePaused;

        if (gamePaused)
        {
            settinPanel.SetActive(true);
        }
        else
        {
            settinPanel.SetActive(false);
        }
    }


    void MoveObj()                                         // 카메라 움직임 메서드 Camera movement method
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

    void Zoominout()                                            // 카메라 줌인 메서드 Camera zoom-in method
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
    }

    public void OpenBrowser()                                      // 배경설정을 위한 파일불러오기 메서드 File import method for setting background
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
        FileBrowser.SetDefaultFilter(".jpg");
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

            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);
            Texture2D texture2 = new(1920, 1080, TextureFormat.ARGB32, false);
            texture2.LoadImage(bytes);
            backGround.GetComponent<Renderer>().material.mainTexture = texture2;
            //backGround = GameObject.Find("Background");
            // Renderer test = backGround.GetComponent<Renderer>();
            // test.material.mainTexture = texture2;

            // string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            // FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif   
    }
}
