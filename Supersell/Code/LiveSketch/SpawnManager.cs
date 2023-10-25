using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleFileBrowser;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEditor;
using Unity.VisualScripting.Antlr3.Runtime;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine.U2D.Animation;
using System;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] spawnPlayer;
    public Transform[] spawnPoints;
    public Sprite sprite;

    //private byte[] img_Data;
    private string[] files;
    private string path = "SpawningPool";
    private string[] imgName = { "Bird" , "Monkey" };
    //private string imgName2 =  "Monkey" ;
    private string imgType = ".png";
    private GameObject imgPlayer;
    private int imgIndex1;
    private int imgIndex2;


    void Start()
    {
        Application.runInBackground = true;
        imgIndex1 = 1;
      //  imgIndex2 = 1;

        //files = System.IO.Directory.GetFiles(path);
    }


    void FixedUpdate()
    {      
        //if (!File.Exists(path + "/" + imgName[i] + imgIndex[i].ToString() + imgType)) { return; }
        if (File.Exists(path + "/" + imgName[0] + imgIndex1.ToString() + imgType))
        {
            //SpawnObject();
            StartCoroutine(DelayImportObject());
        }
       /*  if(File.Exists(path + "/" + imgName[1] + imgIndex2.ToString() + imgType))
        {
            StartCoroutine(DelayImportObject2());
        }*/
        else
        {
            return;
        }

    }

    public void SpawnObject()
    {
        if (!File.Exists(path + "/" + imgName[0] + imgIndex1.ToString() + imgType)) { return; }

        byte[] img_Data = File.ReadAllBytes(path + "/" + imgName[0] + imgIndex1.ToString() + imgType);  // �ش��������� �̹��� ������ �޾ƿ���

        Texture2D texture2 = new(3200, 1080, TextureFormat.ARGB32, false);       // �� �ؽ�ó �غ�
        texture2.LoadImage(img_Data);                                            // �� �ؽ�ó�� �̹��� ������ �ޱ�
        //Rect rect = new Rect(0, 0, texture2.width, texture2.height);

        int rand = UnityEngine.Random.Range(0, spawnPoints.Length);
        // �̹��� ��������Ʈ�� ������� �ؽ�ó ���
        // Sprite sprite = Sprite.Create(texture2, new Rect(0, 0, texture2.width, texture2.height), new Vector2(0.5f, 0.5f));
       // sprite = Sprite.Create(texture2, new Rect(0, 0, texture2.width, texture2.height), new Vector2(0.5f, 0.5f));

        // ������ �÷��̾ �ӽ� �÷��̾�(�̹���)�� ����
        imgPlayer = Instantiate(spawnPlayer[0], spawnPoints[rand].transform.position, spawnPoints[rand].transform.rotation);

        SpriteRenderer sp = imgPlayer.GetComponent<SpriteRenderer>();
        sp.sprite = sprite;
        //sp.sprite = Sprite.Create(texture2, rect, new Vector2(0.5f, 0.5f));
        

        // �÷��̾�(�̹���)�� ����� ������ �ε�
        //SpriteSkin skin = imgPlayer.GetComponent<SpriteSkin>();
        AssetDatabase.Refresh();
        Debug.Log(imgName[0]);
        imgIndex1++;
    }
   /* public void SpawnObject2()
    {
        if (!File.Exists(path + "/" + imgName[1] + imgIndex2.ToString() + imgType)) { return; }

        byte[] img_Data = File.ReadAllBytes(path + "/" + imgName[1] + imgIndex2.ToString() + imgType);  // �ش��������� �̹��� ������ �޾ƿ���

        Texture2D texture2 = new(3200, 1080, TextureFormat.ARGB32, false);       // �� �ؽ�ó �غ�
        texture2.LoadImage(img_Data);                                            // �� �ؽ�ó�� �̹��� ������ �ޱ�

        int rand = UnityEngine.Random.Range(0, spawnPoints.Length);
        // �̹��� ��������Ʈ�� ������� �ؽ�ó ���
        // Sprite sprite = Sprite.Create(texture2, new Rect(0, 0, texture2.width, texture2.height), new Vector2(0.5f, 0.5f));
        sprite = Sprite.Create(texture2, new Rect(0, 0, texture2.width, texture2.height), new Vector2(0.5f, 0.5f));

        // ������ �÷��̾ �ӽ� �÷��̾�(�̹���)�� ����
        imgPlayer = Instantiate(spawnPlayer[1], spawnPoints[rand].transform.position, Quaternion.identity);


        // �÷��̾�(�̹���)�� ����� ������ �ε�
        SpriteRenderer sp = imgPlayer.GetComponent<SpriteRenderer>();
        SpriteSkin skin = imgPlayer.GetComponent<SpriteSkin>();
        sp.sprite = sprite;
        AssetDatabase.Refresh();
        Debug.Log(imgName[1]);
        imgIndex2++;
    }*/

    IEnumerator DelayImportObject()
    {
        yield return new WaitForSeconds(2);
        SpawnObject();
    }
  /*  IEnumerator DelayImportObject2()
    {
        yield return new WaitForSeconds(2);
        SpawnObject2();
    }
*/



    /* public void OpenBrowser()                                            // ��� �ҷ����� �޼���
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
             img_Data = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);  // �ش��������� �̹��� ������ �޾ƿ���
             Texture2D texture2 = new(3200, 1080, TextureFormat.ARGB32, false);       // �� �ؽ�ó �غ�
             texture2.LoadImage(img_Data);                                            // �� �ؽ�ó�� �̹��� ������ �ޱ�
             // �̹��� ��������Ʈ�� ������� �ؽ�ó ���
             Sprite sprite = Sprite.Create(texture2, new Rect(0, 0, texture2.width, texture2.height), new Vector2(0.5f, 0.5f));
             // ������ �÷��̾ �ӽ� �÷��̾�(�̹���)�� ����
             imgPlayer = Instantiate(spawnPlayer, spawnPlayer.transform.position, Quaternion.identity);
             // �÷��̾�(�̹���)�� ����� ������ �ε�
             SpriteRenderer sp = imgPlayer.GetComponent<SpriteRenderer>();
             sp.sprite = sprite;
             // sp.material.mainTexture = texture2;
             *//* Renderer test = imgPlayer.GetComponent<Renderer>();
              test.material.mainTexture = texture2;*//*




             // string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
             // FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
         }
     }*/
}
