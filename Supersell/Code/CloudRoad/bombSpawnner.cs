using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using SimpleFileBrowser;
using UnityEngine.Networking;
using static UnityEngine.Debug;

[System.Serializable]
public class Sound
{
    public string name;
    //public float volume;   
    public AudioClip clip;
}

public class bombSpawnner : MonoBehaviour
{
    [SerializeField] private GameObject _bombPrefab;

    [Header("배경음 소스파일")]
    [SerializeField] Sound[] bgm = null;

    [Header("배경음 스피커")]
    [SerializeField] AudioSource bgmSpeaker = null;

    [Header("효과음 스피커")]
    [SerializeField] AudioSource[] seSpeaker;

    public GameObject hand;
    public GameObject canvasObj;
    public GameObject[] flowers;
    bool gamePaused;
    public int position = 0;
    public int samplerate = 44100;
    public float frequency = 440;

    void Start()
    {
        PlayeRandomBGM();

        string[] files = System.IO.Directory.GetFiles("bg");
        foreach (string file in files)
        {
            //       //Do work on the files here
            //         Debug.Log(file);
            byte[] data = File.ReadAllBytes("bg/bg.jpg");
            Texture2D texture2 = new Texture2D(64, 64, TextureFormat.ARGB32, false);
            texture2.LoadImage(data);
            hand = GameObject.Find("Cube");
            Renderer test = hand.GetComponent<Renderer>();
            test.material.mainTexture = texture2;
            // test.setTexture("HEHE",texture2);
        }

        string[] files2 = System.IO.Directory.GetFiles("flower");

        foreach (string file in files2)
        {
            //       //Do work on the files here
            //         Debug.Log(file);
            byte[] data = File.ReadAllBytes(file);
            Texture2D texture2 = new Texture2D(600, 600, TextureFormat.ARGB32, false);
            texture2.LoadImage(data);
            string[] splitArray = file.Split(char.Parse("\\"));
            splitArray = splitArray[1].Split(char.Parse("."));
            flowers = GameObject.FindGameObjectsWithTag(splitArray[0]);
            Debug.Log(flowers);
            foreach (GameObject flower in flowers)
            {
                SpriteRenderer test = flower.GetComponent<SpriteRenderer>();
                Sprite tempS = Sprite.Create(texture2, new Rect(0, 0, texture2.width, texture2.height), Vector2.zero);
                test.sprite = tempS;
            }
            // test.setTexture("HEHE",texture2);
        }
    }

    public void PlayeRandomBGM()
    {
        int rand = Random.Range(0, bgm.Length);
        bgmSpeaker.clip = bgm[rand].clip;
        bgmSpeaker.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            gamePaused = !gamePaused;
        //Now we enable and disable the game object!
        if (gamePaused)
        {
            canvasObj.SetActive(true);
        }

        else
        {
            canvasObj.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            int xcount = Random.Range(0, 5);
            for (int i = 0; i < seSpeaker.Length; i++)
            {
                if (!seSpeaker[i].isPlaying)
                {
                    if (seSpeaker[i].isPlaying) return;
                    seSpeaker[xcount].Play();
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseScreenPosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (!gamePaused)
                    SpawnBombAtPos(hitInfo.point);
            }
        }
    }

    private void SpawnBombAtPos(Vector3 spawnPosition)
    {
        spawnPosition.y = 1;
        GameObject bomb = Instantiate(_bombPrefab, spawnPosition, Quaternion.identity);
    }

    public void LoadImages()
    {
        string[] files = System.IO.Directory.GetFiles("bg");
        foreach (string file in files)
        {
            //       //Do work on the files here
            //         Debug.Log(file);
            byte[] data = File.ReadAllBytes("bg/bg.jpg");
            Texture2D texture2 = new(64, 64, TextureFormat.ARGB32, false);
            texture2.LoadImage(data);
            hand = GameObject.Find("Cube");
            Renderer test = hand.GetComponent<Renderer>();
            test.material.mainTexture = texture2;
            // test.setTexture("HEHE",texture2);
        }
    }

    public void OpenBrowser()
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
            Texture2D texture2 = new(600, 600, TextureFormat.ARGB32, false);
            texture2.LoadImage(bytes);
            hand = GameObject.Find("Cube");
            Renderer test = hand.GetComponent<Renderer>();
            test.material.mainTexture = texture2;

            // string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            // FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
        }
    }

    // private IEnumerator cbs(){
    //      foreach (FileInfo f in info) 
    // {

    //         var go = new GameObject("MyGameObject");
    //         go.AddComponent<AudioSource>();
    //         AudioSource aud = go.GetComponent<AudioSource>();
    //         AudioClip clip1;
    //         // clip1 = LoadClip(f.FullName);
    //         WWW www = new WWW("file://"+f.FullName);
    //         yield return www;
    //         aud.clip = url.GetAudioClip(false,true);
    //         // clip1.play()
    //         // aud.clip = Resources.Load<AudioClip>(f.FullName);
    //         // aud.play();


    // }

    ///스타트 기존 코드
    ///   // string [] files = System.IO.Directory.GetFiles("testImg");
    // foreach (string file in files)
    //  {
    //       //Do work on the files here
    //       Debug.Log(file);
    //  }

    // var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "PREFAB_FLOWER1");
    // foreach (GameObject go in objects){

    //     GameObject child = go.transform.GetChild(0).gameObject;

    //     SpriteRenderer sr = child.GetComponent<SpriteRenderer>(); 
    //     // Sprite myFruit = Resources.Load("testImg/꽃.png", typeof(Sprite)) as Sprite;
    //     Sprite myFruit;
    //     byte[] data = File.ReadAllBytes("testImg/꽃.png");
    //     Texture2D texture2 = new Texture2D(64, 64, TextureFormat.ARGB32, false);
    //     texture2.LoadImage(data);
    //     myFruit = Sprite.Create(texture2, new Rect(0.0f, 0.0f, texture2.width, texture2.height), new Vector2(0.5f, 0.5f), 100.0f);
    //     sr.sprite=myFruit;

    // }


    // DirectoryInfo dir = new DirectoryInfo("Assets/effect");
    // FileInfo[] info = dir.GetFiles("*.*");
    // StartCoroutine(cbs);
    // foreach (FileInfo f in info) 
    // {

    //         var go = new GameObject("MyGameObject");
    //         go.AddComponent<AudioSource>();
    //         AudioSource aud = go.GetComponent<AudioSource>();
    //         AudioClip clip1;
    //         // clip1 = LoadClip(f.FullName);

    //         aud.clip = url.GetAudioClip(false,true);
    //         // clip1.play()
    //         // aud.clip = Resources.Load<AudioClip>(f.FullName);
    //         // aud.play();


    // }
}

