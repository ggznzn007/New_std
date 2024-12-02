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
        int setWidth = 3200;   // ȭ�� ����
        int setHeight = 1080;  // ȭ�� ����
        Screen.SetResolution(setWidth, setHeight, true); // true: Ǯ��ũ��, false:â���
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
            int minutes = sec / 60; // �� ���
            int seconds = sec % 60; // �� ���
            // string formattedTime = string.Format("{0:00}m:{1:00}s", minutes, seconds);
            string formattedTime = string.Format("{00:00}:{01:00}", minutes, seconds);
            secText.text = formattedTime;
        }
        FootVisual();
        MoveObj();
        Zoominout();
    }

    public void BacktoStart()                                                // �����
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

    public void StartWalk()                                                  // ���� ����
    {
        startPannel.SetActive(false);
        isReady = true;
        StartCoroutine(CountDown());
    }

    public void FinishWalk()                                                 // ���� ��(����)
    {
        StartCoroutine(GameFinish());
    }

    public void FailWalk()                                                   // ���� ����
    {
        StartCoroutine(GameFail());
    }

    IEnumerator CountDown()                                                  // ī��Ʈ�ٿ�
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

    public void DistanceSetting()                                               // �ΰ��� bar�� �Ÿ� ����
    {
        if (isStart) { return; }
        float distanceVal = distanceSlider.value;
        PlayerPrefs.SetFloat("distance", distanceVal);
        backdeadLine.transform.position = new Vector3(0, 2, distanceVal);
        distanceText.text = distanceVal.ToString("F0");
        PlayerPrefs.SetString("distanceTxt", distanceText.text);
    }

    public void TimeSetting()                                                   // �ð� ����
    {
        if (isStart) { return; }
        curSecond = timeSlider.value;
        PlayerPrefs.SetFloat("time", curSecond);
        timeText.text = curSecond.ToString("F0") + " Sec";
        PlayerPrefs.SetString("timeTxt", timeText.text);
    }

    public void SizeSetting()                                                   // ���ڱ� ũ�� ����
    {
        float sizeVal = sizeSlider.value;
        PlayerPrefs.SetFloat("size", sizeVal);
        footPrint.transform.localScale = new Vector3(sizeVal, sizeVal, 1f);
        sizeText.text = sizeVal.ToString("F1");// �Ҽ��� 1�ڸ� ǥ��
        PlayerPrefs.SetString("sizeTxt", sizeText.text);
    }

    public void SpeedSetting()                                                  // bar �ӵ� ����
    {
        float speedVal = speedSlider.value;
        PlayerPrefs.SetFloat("speed", speedVal);
        DeadLineMove.DLM.speed = speedVal;
        DeadLineMove2.DLM2.speed = speedVal;
        HiddenLine.HLM.speed = speedVal;
        speedText.text = speedVal.ToString("F1");// �Ҽ��� 1�ڸ� ǥ��
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

    public void LoadAndUpdateValue()                                            // ���� �� �ҷ�����
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

    public void LoadTime()                                                    // ���� �ð� �ҷ�����
    {
        curSecond = PlayerPrefs.GetFloat("time");
        timeSlider.value = curSecond;
        string timeTxt = PlayerPrefs.GetString("timeTxt");
        timeText.text = timeTxt;
    }

    void FootVisual()                                                            // ���ڱ� �޼���
    {
        if (gamePaused) { return; }
        if (!gamePaused)
        {
            if (Input.GetMouseButtonDown(0))                                     // ���콺 Ŭ���� �ѹ��� ȣ��
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

            if (Input.GetMouseButton(0))                                         // ���콺 Ŭ����(�巡��) ��� ȣ��
            {
                if (isReady)
                {
                    myFoot.transform.position = GetMouseWorldPosition() + m_Offset;
                    myFootReal.transform.position = GetMouseWorldPosition() + m_Offset;
                }
            }

            if (Input.GetMouseButtonUp(0))                                       // ���콺 Ŭ������ �� �ѹ��� ȣ��
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
        Application.Quit(); // ���ø����̼� ����
#endif   
        }                                // ���α׷� ����
    }

    Vector3 GetMouseWorldPosition()                                        // ���콺 ��ġ�� �Լ�
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = m_ZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

  /*  IEnumerator DelayDestroy()                                             // ������ ���� ������ �� ����
    {
        yield return null;
        Destroy(myFoot);
        Destroy(myFootReal);
    }*/

    void SettingUI()                                                       // ���� UI 
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

    void MoveObj()                                                        // ī�޶� ����
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

        /*  float mouseX = Input.GetAxis("Mouse X");                        ȸ���ڵ�
          float mouseY = Input.GetAxis("Mouse Y");
          transform.Rotate(Vector3.up * speed_rota * mouseX);
          transform.Rotate(Vector3.left * speed_rota * mouseY);*/
    }

    void Zoominout()                                                     // ī�޶� ���ξƿ�
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

        /* Vector3 cameraDirection = this.transform.localRotation * Vector3.back;           // ī�޶� Ʈ������ ��ü�̵��ڵ�

         this.transform.position += scrollSpeed * scroollWheel * Time.deltaTime * cameraDirection; */
    }

    public void LoadBackGround()
    {
        if (Directory.Exists("BackGround") == false)
        {
            Directory.CreateDirectory("BackGround");
        }

        string[] files = Directory.GetFiles("BackGround");//(System.IO)����
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

    public void OpenBrowser_BG_Main()                                            // ��� �ҷ����� �޼���
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

    public void ReStart()                                                // ����� 
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()                                               // ���� ����
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���α׷� ����
#endif   
    }
}
