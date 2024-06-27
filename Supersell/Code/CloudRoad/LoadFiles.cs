using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SimpleFileBrowser;
using UnityEngine.XR;
using Unity.VisualScripting;

public class LoadFiles : MonoBehaviour
{
    public GameObject Backgroud;

    public void ClickLoadImg()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
        FileBrowser.SetDefaultFilter(".jpg");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);
        if(Backgroud != null )
        {
            StartCoroutine(LoadIMG());
        }               
    }

    IEnumerator LoadIMG()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");
        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {     
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);
            Texture2D texture2 = new(600, 600, TextureFormat.ARGB32, false);
            texture2.LoadImage(bytes);
            Backgroud = GameObject.Find("Cube");
            Renderer test = Backgroud.GetComponent<Renderer>();
            test.material.mainTexture = texture2;

            string savePath = Application.persistentDataPath + "/Image";           
        }
    }
}
