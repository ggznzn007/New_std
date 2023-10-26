using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleFileBrowser;
//using UnityEngine.UIElements;
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


    private byte[] img_Data;
    private string[] files;
    private string path = "SpawningPool";
    private string[] imgName = { "Bird", "Monkey" };
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
       /* DirectoryInfo directory = new DirectoryInfo(path);
        foreach (FileInfo file in directory.GetFiles())
        {

        }*/
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
        /*  if(File.Exists(path + "/" + imgName[1] + imgIndex2.ToString() + imgType))
         {
             StartCoroutine(DelayImportObject2());
         }*//*
         else
         {
             return;
         }*/

    }

    public Texture2D TextureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            newText.LoadImage(img_Data);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }

    public void SpawnObject()
    {
        if (!File.Exists(path + "/" + imgName[0] + imgIndex1.ToString() + imgType)) { return; }
        int rand = UnityEngine.Random.Range(0, spawnPoints.Length);
        
         img_Data = File.ReadAllBytes(path + "/" + imgName[0] + imgIndex1.ToString() + imgType);  // �ش��������� �̹��� ������ �޾ƿ���        
        //byte[] img_Data = File.ReadAllBytes(path + "/" + imgName[0] + imgIndex1.ToString() + imgType);  // �ش��������� �̹��� ������ �޾ƿ���        

        Texture2D texture2 = new(3200, 1080, TextureFormat.ARGB32, false);       // �� �ؽ�ó �غ�        

        texture2.LoadImage(img_Data);                                           // �� �ؽ�ó�� �̹��� ������ �ޱ�

        Rect rect = new Rect(0, 0, texture2.width, texture2.height);

        // �̹��� ��������Ʈ�� ������� �ؽ�ó ���
        // Sprite sprite = Sprite.Create(texture2, new Rect(0, 0, texture2.width, texture2.height), new Vector2(0.5f, 0.5f));
        // sprite = Sprite.Create(texture2, new Rect(0, 0, texture2.width, texture2.height), new Vector2(0.5f, 0.5f));

        // ������ �÷��̾ �ӽ� �÷��̾�(�̹���)�� ����
        GameObject imgPlayer = Instantiate(spawnPlayer[0], spawnPoints[rand].transform.position, Quaternion.identity);
        //imgPlayer.GetComponent<SpriteRenderer>().material.mainTexture = texture2;        
        //imgPlayer.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture2, rect, new Vector2(0.5f, 0.5f),100);
        //imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        TextureFromSprite(imgPlayer.GetComponent<SpriteRenderer>().sprite);
        imgPlayer.GetComponent<SpriteSkin>();

        //SpriteRenderer sp = imgPlayer.GetComponent<SpriteRenderer>();
        // sp.sprite = Sprite.Create(texture2, rect, new Vector2(0.5f, 0.5f));
        //sp.sprite.texture.LoadImage(img_Data);
        // sp.sprite.name = "Temp"+ imgIndex1.ToString();       
        //sp.sprite = sprite;        

        // �÷��̾�(�̹���)�� ����� ������ �ε�
        //AssetDatabase.Refresh();
        Debug.Log(path + "/" + imgName[0] + imgIndex1.ToString() + imgType);
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
        //yield return new WaitForSeconds(0.1f);
        //imgPlayer = null;
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
