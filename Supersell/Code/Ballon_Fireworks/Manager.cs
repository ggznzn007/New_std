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

public class Manager : MonoBehaviour                     // �Ҳɳ��� �Ŵ���
{
    [Header("����ī�޶�")][SerializeField] private Camera myCam;                    // ī�޶�
    [Header("�������ʴ� ��")][SerializeField] GameObject hiddenHand;                // ���� �հ���
    [Header("���� ���")][SerializeField] GameObject backGround;                   // ���
    [Header("���� �ǳ�")][SerializeField] GameObject setCanvas;                    // ���� ĵ����
    [Header("�Ҳ�ũ�� ���� �����̴�")][SerializeField] private Slider sizeSlider;    // ������ �����̴�
    [Header("�Ҳ� ��ƼŬ")][SerializeField] GameObject[] fireParticles;            // �Ҳɳ��� ��ƼŬ

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

    public void SetResolution()                     // �ػ� ���� �޼��� Resolution setting method
    {
        int setWidth = 3200;   // ȭ�� ����
        int setHeight = 1080;  // ȭ�� ����

        Screen.SetResolution(setWidth, setHeight, true); // true: Ǯ��ũ��, false:â���
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

    void TouchThat()                                                        // ȭ�� ��ġ �� ���� �޼��� Touch the Screen method
    {
        if (gamePaused) return;
        
        if (Input.GetMouseButtonDown(0))                                     // ���콺 Ŭ���� �ѹ��� ȣ��
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

       /* if (Input.GetMouseButton(0))                                         // ���콺 Ŭ����(�巡��) ��� ȣ��
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

        if (Input.GetMouseButtonUp(0))                                       // ���콺 Ŭ������ �� �ѹ��� ȣ��
        {           
            exCount = 0;
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

    IEnumerator DelayDestroy()                                             // ������ ���� ������ �� ����
    {
        yield return new WaitForSeconds(3f);
        Destroy(myHand);
    }

    void SettingUI()                                                // ���� UI �޼��� Setting UI method
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

    void MoveObj()                                         // ī�޶� ������ �޼��� Camera movement method
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

        /*  float mouseX = Input.GetAxis("Mouse X");                        ȸ���ڵ�
          float mouseY = Input.GetAxis("Mouse Y");
          transform.Rotate(Vector3.up * speed_rota * mouseX);
          transform.Rotate(Vector3.left * speed_rota * mouseY);*/
    }

    void Zoominout()                                            // ī�޶� ���� �޼��� Camera zoom-in method
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

        /* Vector3 cameraDirection = this.transform.localRotation * Vector3.back;           // ī�޶� Ʈ������ ��ü�̵��ڵ�

         this.transform.position += scrollSpeed * scroollWheel * Time.deltaTime * cameraDirection; */
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

            // string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            // FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
        }
    }
}
