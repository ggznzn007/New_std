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

public class Manager : MonoBehaviour                     // 불꽃놀이 매니저
{
    [Header("메인카메라")][SerializeField] private Camera myCam;                    // 카메라
    [Header("보이지않는 손")][SerializeField] GameObject hiddenHand;                // 히든 손가락
    [Header("메인 배경")][SerializeField] GameObject backGround;                   // 배경
    [Header("세팅 판넬")][SerializeField] GameObject setCanvas;                    // 세팅 캔버스
    [Header("불꽃크기 조절 슬라이더")][SerializeField] private Slider sizeSlider;    // 사이즈 슬라이더
    [Header("불꽃 파티클")][SerializeField] GameObject[] fireParticles;            // 불꽃놀이 파티클

    List<GameObject> randList = new List<GameObject>();
    private Vector3 m_Offset;
    private float m_ZCoord;
    private GameObject myHand;
    private bool gamePaused;
    private GameObject myFire;
    private bool isClick;
    private int exCount;
    private int[] rCount;

    private void Start()
    {       
        LoadValue();
        exCount = 0;
    }

    private void Update()
    {
        TouchThat();
        MoveObj();
        Zoominout();
        SettingUI();
    }

    public void SetResolution()                     // 해상도 세팅 메서드 Resolution setting method
    {
        int setWidth = 3200;   // 화면 가로
        int setHeight = 1080;  // 화면 세로

        Screen.SetResolution(setWidth, setHeight, true); // true: 풀스크린, false:창모드
    }

   /* GameObject[] ChooseSet(int numRequired)
    {
        GameObject[] result = new GameObject[numRequired];

        int numToChoose = numRequired;

        for (int numLeft = fireParticles.Length; numLeft > 0; numLeft--)
        {

            float prob = (float)numToChoose / (float)numLeft;

            if (Random.value <= prob)
            {
                numToChoose--;
                result[numToChoose] = fireParticles[numLeft - 1];

                if (numToChoose == 0)
                {
                    break;
                }
            }
        }
        return result;
    }*/

    void TouchThat()                                                        // 화면 터치 시 폭죽 메서드 Touch the Screen method
    {
        if (gamePaused) return;
        
        if (Input.GetMouseButtonDown(0))                                     // 마우스 클릭시 한번만 호출
        {           
            int random = Random.Range(0,fireParticles.Length);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Vector3 hitPos = hit.point;
                Instantiate(fireParticles[random], hitPos, fireParticles[random].transform.rotation);
                // myHand = Instantiate(fireParticles[rand], hitPos, fireParticles[rand].transform.rotation);
                // m_ZCoord = Camera.main.WorldToScreenPoint(myHand.transform.position).z;
                //  m_Offset = myHand.transform.position - GetMouseWorldPosition();
            }
        }

       /* if (Input.GetMouseButton(0))                                         // 마우스 클릭중(드래그) 계속 호출
        {
            //myHand.transform.position = GetMouseWorldPosition() + m_Offset;
            int rand = Random.Range(0, fireParticles.Length);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (exCount >= 30) return;
                Vector3 hitPos = hit.point;
                Instantiate(fireParticles[rand], hitPos, fireParticles[rand].transform.rotation);
                exCount++;
                // myHand = Instantiate(fireParticles[rand], hitPos, fireParticles[rand].transform.rotation);
                // m_ZCoord = Camera.main.WorldToScreenPoint(myHand.transform.position).z;
                //  m_Offset = myHand.transform.position - GetMouseWorldPosition();
            }
        }*/

        if (Input.GetMouseButtonUp(0))                                       // 마우스 클릭종료 시 한번만 호출
        {           
            exCount = 0;
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

    public void SizeSetting()
    {
        float sizeVal = sizeSlider.value;
        PlayerPrefs.SetFloat("size", sizeVal);
        for (int i = 0; i < fireParticles.Length; i++)
        {
            fireParticles[i].transform.localScale = new Vector3(sizeVal, sizeVal, sizeVal);
        }
    }

    public void LoadValue()
    {
        float sizeVal = PlayerPrefs.GetFloat("size");
        sizeSlider.value = sizeVal;
    }

    IEnumerator DelayDestroy()                                             // 가상의 봉을 딜레이 후 삭제
    {
        yield return new WaitForSeconds(3f);
        Destroy(myHand);
    }

    void SettingUI()                                                // 세팅 UI 메서드 Setting UI method
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

    void MoveObj()                                         // 카메라 움직임 메서드 Camera movement method
    {
        if (gamePaused) return;
        float keyH = Input.GetAxis("Horizontal");
        float keyV = Input.GetAxis("Vertical");
        keyH = keyH * Utils.move_Speed * Time.deltaTime;
        keyV = keyV * Utils.move_Speed * Time.deltaTime;
        myCam.transform.Translate(Vector3.right * keyH);
        myCam.transform.Translate(Vector3.up * keyV);

        if (myCam.transform.position.x <= Utils.minPosX)
        {
            myCam.transform.position = new Vector3(Utils.minPosX, myCam.transform.position.y, myCam.transform.position.z);
        }
        if (myCam.transform.position.x >= Utils.maxPosX)
        {
            myCam.transform.position = new Vector3(Utils.maxPosX, myCam.transform.position.y, myCam.transform.position.z);
        }

        if (myCam.transform.position.z <= Utils.minPosZ)
        {
            myCam.transform.position = new Vector3(myCam.transform.position.x, myCam.transform.position.y, Utils.minPosZ);
        }
        if (myCam.transform.position.z >= Utils.maxPosZ)
        {
            myCam.transform.position = new Vector3(myCam.transform.position.x, myCam.transform.position.y, Utils.maxPosZ);
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
        Camera.main.fieldOfView += scroollWheel * Time.deltaTime * Utils.scroll_Speed;

        if (Camera.main.fieldOfView >= Utils.maxZoom)
        {
            Camera.main.fieldOfView = Utils.maxZoom;
        }
        if (Camera.main.fieldOfView <= Utils.minZoom)
        {
            Camera.main.fieldOfView = Utils.minZoom;
        }

        /* Vector3 cameraDirection = this.transform.localRotation * Vector3.back;           // 카메라 트랜스폼 자체이동코드

         this.transform.position += scrollSpeed * scroollWheel * Time.deltaTime * cameraDirection; */
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

            // string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            // FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
        }
    }
}
