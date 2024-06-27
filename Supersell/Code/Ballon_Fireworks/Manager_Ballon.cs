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


public class Manager_Ballon : MonoBehaviour                 // ǳ�� �Ŵ���
{
    public static Manager_Ballon MB;
    [Header("���� ī�޶�")]
    [SerializeField] private Camera myCam;
    [Header("�������ʴ� ��")]
    [SerializeField] GameObject hiddenHand;
    [Header("���� ���")]
    [SerializeField] GameObject backGround;
    [Header("���� �ǳ�")]
    [SerializeField] GameObject settinPanel;
    [Header("���� �ǳ�")]
    [SerializeField] GameObject gameoverPanel;
    [Header("ǳ�� ũ������ �����̴�")]
    [SerializeField] private Slider sizeSlider;
    [Header("ǳ�� �������� �����̴�")]
    [SerializeField] private Slider countSlider;
    [Header("Ÿ�� ǳ�� �ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI targetText;
    [Header("��ǥ �� �ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI goalText;
    [Header("ǳ�� ���� ���� �ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI exploCountText;
    [Header("ǳ�� ũ�� �ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI sizeText;
    [Header("ǳ�� ���� �ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI countText;
    [Header("���� ���� �ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI gameInfoText;
    [Header("ǳ�� ���� ����")]
    [SerializeField] private Transform[] spawnPoints;
    [Header("ǳ�� ���� ����")]
    [SerializeField] private BoxCollider area;
    [Header("ǳ����")]
    [SerializeField] GameObject[] Ballons;
    [Header("ǳ�� ���� ����")]
    public int ballCount;
    [Header("ǳ�� �� ����")]
    public int cntSet;
    [Header("ǳ�� ���� ���� ����")]
    public int sortIndex = 0;
    [Header("���� ǳ�� ����")]
    public int explodedBallon;
    [Header("��ǥ ǳ�� ����")]
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
        // SetResolution();                            // �ػ� �ʱ�ȭ �޼��� Initialize Resolution Call
    }

    private void Start()
    {
        MB = this;
        area = GetComponent<BoxCollider>();         // ǳ�� �������� �ʱ�ȭ Initialize balloon spawn range
        area.enabled = false;                       // �ݶ��̴� ��� ����  Turning off the collider function        
        ballCount = 0;                              // ǳ�� ���� �ʱ�ȭ    Initialize balloon count
        SetInitValue();
        InvokeRepeating(nameof(Spawn), 0.1f, 0.2f); // ���� �� ǳ�� �����Լ� ������������ ���   Create Balloon at Start
    }

    private void Update()
    {
        LoadSizeValue();                            // ǳ�� ������ ������Ʈ �޼��� Update balloon settings Method Call
        TouchThat();                                // ǳ�� ��ġ �� �޼��� Balloon Touch Method Call
        MoveObj();                                  // ī�޶� ������ �޼��� Camera movement method Call
        Zoominout();                                // ī�޶� ���ξƿ� �޼��� Camera zoom-in method Call
        SettingUI();                                // ����UI �޼���        Setting UI Method Call 
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

    public void SetResolution()                     // �ػ� ���� �޼��� Resolution setting method
    {
        int setWidth = 3200;   // ȭ�� ����
        int setHeight = 1080;  // ȭ�� ����

        Screen.SetResolution(setWidth, setHeight, true); // true: Ǯ��ũ��, false:â���
    }

    public void PaintDelete()                       // ������ȿ�� ���� �޼��� Delete painting effects method
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

    public Vector3 GetRandomPosition()              // ǳ�� ���� ���� �޼��� Balloon Generation Range Method
    {
        Vector3 basePosition = transform.position;
        Vector3 size = area.size;

        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);
        float posZ = basePosition.z + Random.Range(-size.z / 2f, size.z / 2f);

        Vector3 spawnPos = new(posX, posY, posZ);
        return spawnPos;
    }

    public void Spawn()                              // ǳ�� ���� �޼��� Spawn Ballon method
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

    public void AddSpawn()                           // �߰� ǳ�� ���� �޼��� Add Spawn Ballon method
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

    void TouchThat()                                                       // ǳ�� ��ġ �� �޼��� Touch the Ballon method
    {
        if (Input.GetKey(KeyCode.Space))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
#endif   
        }                                // ���α׷� ����
        if (gamePaused || gameOver) return;

        if (Input.GetMouseButtonDown(0))                                     // ���콺 Ŭ���� �ѹ��� ȣ��
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

        if (Input.GetMouseButton(0))                                         // ���콺 Ŭ����(�巡��) ��� ȣ��
        {
            myHand.transform.position = GetMouseWorldPosition() + m_Offset;
        }

        if (Input.GetMouseButtonUp(0))                                       // ���콺 Ŭ������ �� �ѹ��� ȣ��
        {
            StartCoroutine(DelayDestroy());
        }
    }


    Vector3 GetMouseWorldPosition()                                        // ���콺 ��ġ�� �޼��� Mouse Location Value method
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

    public void SizeSetting()                                             // ǳ�� ũ�� ���� �޼��� Balloon size setting method
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

    public void CountSetting()                                          // ǳ�� ���� ���� �޼��� Ballon Count setting method
    {
        cntSet = (int)countSlider.value;
        PlayerPrefs.SetFloat("ballCount", cntSet);
        countText.text = cntSet.ToString("F0");
        PlayerPrefs.SetString("countText", countText.text);
    }

    public void LoadSizeValue()                                        // ǳ�� ���ð� �ҷ����� �޼��� Load Settings method
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

    IEnumerator DelayDestroy()                                             // ������ ���� ������ �� ����
    {
        yield return null;
        Destroy(myHand);
    }

    void SettingUI()                                     // ���� UI �޼��� Setting UI method
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


    void MoveObj()                                         // ī�޶� ������ �޼��� Camera movement method
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

    void Zoominout()                                            // ī�޶� ���� �޼��� Camera zoom-in method
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

    public void OpenBrowser()                                      // ��漳���� ���� ���Ϻҷ����� �޼��� File import method for setting background
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
        Application.Quit(); // ���ø����̼� ����
#endif   
    }
}
