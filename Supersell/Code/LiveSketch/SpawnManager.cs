using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleFileBrowser;
using UnityEngine.UI;
using UnityEditor;
using Unity.VisualScripting.Antlr3.Runtime;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine.U2D.Animation;
using UnityEngine.U2D;
using System;
using UnityEngine.Networking;
using Unity.Mathematics;
using System.Linq;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    #region 선언부
    public static SpawnManager SM;
    // public List<GameObject> spawnPlayer;
    // public Transform[] spawnPoints;   
    public int totalObject;
    public GameObject setCanvas;
    public Slider totalSlider;
    public TextMeshProUGUI totalNum;
    public GameObject deActivePart;
    // public Slider speedSlider;
    // public Slider objSpeedSlider;

    private byte[] img_Data;
    private string[] files;
    private string path = "SpawningPool";
    List<string> aniGON = new List<string>();
    private string[] aniName = { "Anteater","Black caiman","Chameleon", "Elephant", "Gorilla",
                                 "Jaguar", "Okapi", "Poison dart frog", "Sloth" ,"Toco toucan"};

    private string[] dinoName = { "Barosaurus", "Brachiosaurus","Camptosaurus","Heterodontosaurus","Huayangosaurus",
                                  "Lufengosaurus","Mamenchisaurus","Scelidosaurus","Shunosaurus","Tuojiangosaurus" };

    private string imgType = ".png";
    private GameObject imgPlayer;
    private int imgIndex_Ante;
    private int imgIndex_Blac;
    private int imgIndex_Cham;
    private int imgIndex_Elep;
    private int imgIndex_Gori;
    private int imgIndex_Jagu;
    private int imgIndex_Okap;
    private int imgIndex_Pois;
    private int imgIndex_Slot;
    private int imgIndex_Toco;
    private int imgIndex_Baro;
    private int imgIndex_Brac;
    private int imgIndex_Camp;
    private int imgIndex_Hete;
    private int imgIndex_Huay;
    private int imgIndex_Lufe;
    private int imgIndex_Mame;
    private int imgIndex_Scel;
    private int imgIndex_Shun;
    private int imgIndex_Tuoj;

    private bool gamePaused;
    #endregion

    #region 유니티 시스템 메서드 집합
    void Start()
    {
        SM = this;
        LoadValue();
        Application.runInBackground = true;
        imgIndex_Ante = 0;
        imgIndex_Blac = 0;
        imgIndex_Cham = 0;
        imgIndex_Elep = 0;
        imgIndex_Gori = 0;
        imgIndex_Jagu = 0;
        imgIndex_Okap = 0;
        imgIndex_Pois = 0;
        imgIndex_Slot = 0;
        imgIndex_Toco = 0;

        imgIndex_Baro = 0;
        imgIndex_Brac = 0;
        imgIndex_Camp = 0;
        imgIndex_Hete = 0;
        imgIndex_Huay = 0;
        imgIndex_Lufe = 0;
        imgIndex_Mame = 0;
        imgIndex_Scel = 0;
        imgIndex_Shun = 0;
        imgIndex_Tuoj = 0;
        //  speedSlider.onValueChanged.AddListener(ChangeAnimationSpeed);
        // objSpeedSlider.onValueChanged.AddListener(ChangeSpeed);
    }
    /*  void ChangeAnimationSpeed(float newSpeed)
      {
          // 슬라이더 값에 따라 애니메이션 속도 변경
         imgPlayer.GetComponent<Animator>().speed = newSpeed;

      }
      void ChangeSpeed(float newSpeed)
      {
          // 슬라이더 값에 따라 속도 변경       
         imgPlayer.GetComponent<Control_Ante>().speed = newSpeed;
      }*/
    void SettingUI()
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

    public void SetCount()
    {
        float total = totalSlider.value;
        PlayerPrefs.SetFloat("count", total);
        totalObject = (int)total;
        totalNum.text = total.ToString("F0");
        PlayerPrefs.SetString("countTxt", totalNum.text);
    }

    public void LoadValue()
    {
        float sizeVal = PlayerPrefs.GetFloat("count");
        string exTxt = PlayerPrefs.GetString("countTxt");
        totalSlider.value = sizeVal;
        totalNum.text = exTxt;
    }

    [Obsolete]
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(MakeWebRequestCoroutine());
        }
        ScanToImages();
        SettingUI();
        SetCount();
    }

    void ScanToImages()
    {
        if (File.Exists(path + "/" + aniName[0] + imgIndex_Ante.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Ante(0));
        }
        if (File.Exists(path + "/" + aniName[1] + imgIndex_Blac.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Blac(1));
        }
        if (File.Exists(path + "/" + aniName[2] + imgIndex_Cham.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Cham(2));
        }
        if (File.Exists(path + "/" + aniName[3] + imgIndex_Elep.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Elep(3));
        }
        if (File.Exists(path + "/" + aniName[4] + imgIndex_Gori.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Gori(4));
        }
        if (File.Exists(path + "/" + aniName[5] + imgIndex_Jagu.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Jagu(5));
        }
        if (File.Exists(path + "/" + aniName[6] + imgIndex_Okap.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Okap(6));
        }
        if (File.Exists(path + "/" + aniName[7] + imgIndex_Pois.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Pois(7));
        }
        if (File.Exists(path + "/" + aniName[8] + imgIndex_Slot.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Slot(8));
        }
        if (File.Exists(path + "/" + aniName[9] + imgIndex_Toco.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Toco(9));
        }
        if (File.Exists(path + "/" + dinoName[0] + imgIndex_Baro.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Baro(0));
        }
        if (File.Exists(path + "/" + dinoName[1] + imgIndex_Brac.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Brac(1));
        }
        if (File.Exists(path + "/" + dinoName[2] + imgIndex_Camp.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Camp(2));
        }
        if (File.Exists(path + "/" + dinoName[3] + imgIndex_Hete.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Hete(3));
        }
        if (File.Exists(path + "/" + dinoName[4] + imgIndex_Huay.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Huay(4));
        }
        if (File.Exists(path + "/" + dinoName[5] + imgIndex_Lufe.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Lufe(5));
        }
        if (File.Exists(path + "/" + dinoName[6] + imgIndex_Mame.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Mame(6));
        }
        if (File.Exists(path + "/" + dinoName[7] + imgIndex_Scel.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Scel(7));
        }
        if (File.Exists(path + "/" + dinoName[8] + imgIndex_Shun.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Shun(8));
        }
        if (File.Exists(path + "/" + dinoName[9] + imgIndex_Tuoj.ToString() + imgType))
        {
            StartCoroutine(DelayImport_Tuoj(9));
        }
        else
        {
            return;
        }
    }
    #endregion

    #region 동물 스폰 메서드 집합
    void DeActiveCon()
    {
        foreach (var item in aniGON)
        {
            Debug.Log(item.ToString());
        }
        if (aniGON.Count > totalObject)
        {
            GameObject test = GameObject.Find(aniGON[0]);
            GameObject part = Instantiate(deActivePart, test.transform.position, Quaternion.identity);
            test.LeanScale(new Vector3(0f, 0f, 0f), 1).setEaseLinear();
            Destroy(test, 1);
            Destroy(part, 1);

            aniGON.RemoveAt(0);
        }
    }

    public void SpawnObject_Ante(int index)
    {
        if (!File.Exists(path + "/" + aniName[index] + imgIndex_Ante.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Ante % 10;
        img_Data = File.ReadAllBytes(path + "/" + aniName[index] + imgIndex_Ante.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기          

        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Ante();
        imgPlayer.LeanScale(new Vector3(0.5f, 0.5f, 0.5f), 1).setEaseLinear();                                // 생성 효과       
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        Debug.Log(imgIndex_Ante);
        aniGON.Add("Anteater " + testIdx.ToString() + "(Clone)");
        DeActiveCon();
        imgIndex_Ante++;
    }
    public void SpawnObject_Blac(int index)
    {
        if (!File.Exists(path + "/" + aniName[index] + imgIndex_Blac.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Blac % 10;
        img_Data = File.ReadAllBytes(path + "/" + aniName[index] + imgIndex_Blac.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기          
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Blac();
        imgPlayer.LeanScale(new Vector3(0.7f, 0.7f, 0.7f), 1).setEaseLinear();                                // 생성 효과       
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        Debug.Log(imgIndex_Blac);
        aniGON.Add("Black caiman " + testIdx.ToString() + "(Clone)");
        DeActiveCon();
        imgIndex_Blac++;
    }
    public void SpawnObject_Cham(int index)
    {
        if (!File.Exists(path + "/" + aniName[index] + imgIndex_Cham.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Cham % 10;
        img_Data = File.ReadAllBytes(path + "/" + aniName[index] + imgIndex_Cham.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기          
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Cham();
        imgPlayer.LeanScale(new Vector3(0.5f, 0.5f, 0.5f), 1).setEaseLinear();                                // 생성 효과       
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        Debug.Log(imgIndex_Cham);
        aniGON.Add("Chameleon " + testIdx.ToString() + "(Clone)");
        DeActiveCon();
        imgIndex_Cham++;
    }
    public void SpawnObject_Elep(int index)
    {
        if (!File.Exists(path + "/" + aniName[index] + imgIndex_Elep.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Elep % 10;
        img_Data = File.ReadAllBytes(path + "/" + aniName[index] + imgIndex_Elep.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기          
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Elep();
        imgPlayer.LeanScale(new Vector3(0.9f, 0.9f, 0.9f), 1).setEaseLinear();                                // 생성 효과       
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        aniGON.Add("Elephant " + testIdx.ToString() + "(Clone)");
        Debug.Log(imgIndex_Elep);
        DeActiveCon();
        imgIndex_Elep++;
    }
    public void SpawnObject_Gori(int index)
    {
        if (!File.Exists(path + "/" + aniName[index] + imgIndex_Gori.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Gori % 10;
        img_Data = File.ReadAllBytes(path + "/" + aniName[index] + imgIndex_Gori.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기          
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Gori();
        imgPlayer.LeanScale(new Vector3(0.6f, 0.6f, 0.6f), 1).setEaseLinear();                                // 생성 효과       
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        aniGON.Add("Gorilla " + testIdx.ToString() + "(Clone)");
        Debug.Log(imgIndex_Gori);
        DeActiveCon();
        imgIndex_Gori++;
    }
    public void SpawnObject_Jagu(int index)
    {
        if (!File.Exists(path + "/" + aniName[index] + imgIndex_Jagu.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Jagu % 10;
        img_Data = File.ReadAllBytes(path + "/" + aniName[index] + imgIndex_Jagu.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기          
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Jagu();
        imgPlayer.LeanScale(new Vector3(0.5f, 0.5f, 0.5f), 1).setEaseLinear();                                // 생성 효과       
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        aniGON.Add("Jaguar " + testIdx.ToString() + "(Clone)");
        Debug.Log(imgIndex_Jagu);
        DeActiveCon();
        imgIndex_Jagu++;
    }
    public void SpawnObject_Okap(int index)
    {
        if (!File.Exists(path + "/" + aniName[index] + imgIndex_Okap.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Okap % 10;
        img_Data = File.ReadAllBytes(path + "/" + aniName[index] + imgIndex_Okap.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기          
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Okap();
        imgPlayer.LeanScale(new Vector3(0.5f, 0.5f, 0.5f), 1).setEaseLinear();                                // 생성 효과       
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        aniGON.Add("Okapi " + testIdx.ToString() + "(Clone)");
        Debug.Log(imgIndex_Okap);
        DeActiveCon();
        imgIndex_Okap++;
    }
    public void SpawnObject_Pois(int index)
    {
        if (!File.Exists(path + "/" + aniName[index] + imgIndex_Pois.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Pois % 10;
        img_Data = File.ReadAllBytes(path + "/" + aniName[index] + imgIndex_Pois.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기          
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Pois();
        imgPlayer.LeanScale(new Vector3(0.5f, 0.5f, 0.5f), 1).setEaseLinear();                                // 생성 효과       
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        Debug.Log(imgIndex_Pois);
        aniGON.Add("Poison dart frog " + testIdx.ToString() + "(Clone)");
        DeActiveCon();
        imgIndex_Pois++;
    }
    public void SpawnObject_Slot(int index)
    {
        if (!File.Exists(path + "/" + aniName[index] + imgIndex_Slot.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Slot % 10;
        img_Data = File.ReadAllBytes(path + "/" + aniName[index] + imgIndex_Slot.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기          
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Slot();
        imgPlayer.LeanScale(new Vector3(0.5f, 0.5f, 0.5f), 1).setEaseLinear();                                // 생성 효과       
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        aniGON.Add("Sloth " + testIdx.ToString() + "(Clone)");
        Debug.Log(imgIndex_Slot);
        DeActiveCon();
        imgIndex_Slot++;
    }
    public void SpawnObject_Toco(int index)
    {
        if (!File.Exists(path + "/" + aniName[index] + imgIndex_Toco.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Toco % 10;
        img_Data = File.ReadAllBytes(path + "/" + aniName[index] + imgIndex_Toco.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기          
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Toco();
        imgPlayer.LeanScale(new Vector3(0.3f, 0.3f, 0.3f), 1).setEaseLinear();                                // 생성 효과       
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        Debug.Log(imgIndex_Toco);
        aniGON.Add("Toco toucan " + testIdx.ToString() + "(Clone)");
        DeActiveCon();
        imgIndex_Toco++;
        /*//Texture2D texture2 = new(3200, 1080, TextureFormat.ARGB32, false,true);       // 빈 텍스처 준비              
          //texture2.LoadImage(img_Data);                                                 // 빈 텍스처에 이미지 데이터 받기
          // 이미지 스프라이트에 만들어진 텍스처 담기
          //Sprite sprite = Sprite.Create(texture2, new Rect(0, 0, texture2.width, texture2.height), new Vector2(0.5f, 0.5f));
          // sprite = Sprite.Create(texture2, new Rect(0, 0, texture2.width, texture2.height), new Vector2(0.5f, 0.5f));

          // 프리팹 플레이어를 임시 플레이어(이미지)에 저장
          //GameObject imgPlayer = Instantiate(spawnPlayer[0], spawnPoints[rand].transform.position, Quaternion.identity);
          //Texture2D texture2 = LoadImage(path + "/" + imgName[0] + imgIndex1.ToString() + imgType);
         // Sprite tempSprite = Sprite.Create(texture2, new Rect(0, 0, texture2.width, texture2.height), new Vector2(0.5f, 0.5f));
         // SpriteSkin skin = imgPlayer.GetComponent<SpriteSkin>();
          //imgPlayer.GetComponent<SpriteRenderer>().sprite = tempSprite;
          //skin.GetComponent<SpriteRenderer>().sprite = tempSprite;
          //skin.alwaysUpdate=true;



          //imgPlayer.GetComponent<SpriteRenderer>().material.mainTexture = texture2;
          // imgPlayer.GetComponent<SpriteRenderer>().sprite = Sprite.Create(Base64ToTexture2D(img_Data), rect, new Vector2(0.5f, 0.5f),100);

          //SpriteRenderer sp = imgPlayer.GetComponent<SpriteRenderer>();
         // sp.sprite.texture.LoadImage(img_Data);
         // imgPlayer.GetComponent<SpriteRenderer>().sprite = sp.sprite;          

          //sp.sprite.texture.LoadImage(img_Data);
          //sp.sprite = Sprite.Create(texture2, rect, new Vector2(0.5f, 0.5f));
          // sp.sprite.name = "Temp"+ imgIndex1.ToString();       
          //sp.sprite = sprite;     
          //AssetDatabase.Refresh();
          //File.WriteAllBytes(path + "/" + imgName[0] + imgIndex1.ToString() + imgType, img_Data);*/
    }

    IEnumerator DelayImport_Ante(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Ante(index);
    }
    IEnumerator DelayImport_Blac(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Blac(index);
    }
    IEnumerator DelayImport_Cham(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Cham(index);
    }
    IEnumerator DelayImport_Elep(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Elep(index);
    }
    IEnumerator DelayImport_Gori(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Gori(index);
    }
    IEnumerator DelayImport_Jagu(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Jagu(index);
    }
    IEnumerator DelayImport_Okap(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Okap(index);
    }
    IEnumerator DelayImport_Pois(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Pois(index);
    }
    IEnumerator DelayImport_Slot(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Slot(index);
    }
    IEnumerator DelayImport_Toco(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Toco(index);
    }

    #endregion

    #region 공룡 스폰 메서드 집합
    public void SpawnObject_Baro(int index)
    {
        if (!File.Exists(path + "/" + dinoName[index] + imgIndex_Baro.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Baro % 10;
        img_Data = File.ReadAllBytes(path + "/" + dinoName[index] + imgIndex_Baro.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Baro();
        imgPlayer.LeanScale(new Vector3(1, 1, 1), 1).setEaseLinear();
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        Debug.Log(imgIndex_Baro);
        aniGON.Add("Barosaurus " + testIdx.ToString() + "(Clone)");
        DeActiveCon();
        imgIndex_Baro++;
    }
    public void SpawnObject_Brac(int index)
    {
        if (!File.Exists(path + "/" + dinoName[index] + imgIndex_Brac.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Brac % 10;
        img_Data = File.ReadAllBytes(path + "/" + dinoName[index] + imgIndex_Brac.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기        
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Brac();
        imgPlayer.LeanScale(new Vector3(1, 1, 1), 1).setEaseLinear();                                     // 생성 효과
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        aniGON.Add("Brachinosaurus " + testIdx.ToString() + "(Clone)");
        Debug.Log(imgIndex_Brac);
        DeActiveCon();
        imgIndex_Brac++;
    }
    public void SpawnObject_Camp(int index)
    {
        if (!File.Exists(path + "/" + dinoName[index] + imgIndex_Camp.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Camp % 10;
        img_Data = File.ReadAllBytes(path + "/" + dinoName[index] + imgIndex_Camp.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Camp();
        imgPlayer.LeanScale(new Vector3(.8f, .8f, .8f), 1).setEaseLinear();                                     // 생성 효과
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        aniGON.Add("Camptosaurus " + testIdx.ToString() + "(Clone)");
        Debug.Log(imgIndex_Camp);
        DeActiveCon();
        imgIndex_Camp++;
    }
    public void SpawnObject_Hete(int index)
    {
        if (!File.Exists(path + "/" + dinoName[index] + imgIndex_Hete.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Hete % 10;
        img_Data = File.ReadAllBytes(path + "/" + dinoName[index] + imgIndex_Hete.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Hete();
        imgPlayer.LeanScale(new Vector3(.9f, .9f, .9f), 1).setEaseLinear();                                     // 생성 효과
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        aniGON.Add("Heterodontosaurus " + testIdx.ToString() + "(Clone)");
        Debug.Log(imgIndex_Hete);
        DeActiveCon();
        imgIndex_Hete++;
    }
    public void SpawnObject_Huay(int index)
    {
        if (!File.Exists(path + "/" + dinoName[index] + imgIndex_Huay.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Huay % 10;
        img_Data = File.ReadAllBytes(path + "/" + dinoName[index] + imgIndex_Huay.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기     
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Huay();
        imgPlayer.LeanScale(new Vector3(.8f, .8f, .8f), 1).setEaseLinear();                                     // 생성 효과
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        aniGON.Add("Huayangosaurus " + testIdx.ToString() + "(Clone)");
        Debug.Log(imgIndex_Huay);
        DeActiveCon();
        imgIndex_Huay++;
    }
    public void SpawnObject_Lufe(int index)
    {
        if (!File.Exists(path + "/" + dinoName[index] + imgIndex_Lufe.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Lufe % 10;
        img_Data = File.ReadAllBytes(path + "/" + dinoName[index] + imgIndex_Lufe.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기        
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Lufe();
        imgPlayer.LeanScale(new Vector3(.6f, .6f, .6f), 1).setEaseLinear();                                     // 생성 효과
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        aniGON.Add("Lufengosaurus " + testIdx.ToString() + "(Clone)");
        Debug.Log(imgIndex_Lufe);
        DeActiveCon();
        imgIndex_Lufe++;
    }
    public void SpawnObject_Mame(int index)
    {
        if (!File.Exists(path + "/" + dinoName[index] + imgIndex_Mame.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Mame % 10;
        img_Data = File.ReadAllBytes(path + "/" + dinoName[index] + imgIndex_Mame.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기        
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Mame();
        imgPlayer.LeanScale(new Vector3(.7f, .7f, .7f), 1).setEaseLinear();                                     // 생성 효과
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        aniGON.Add("Mamenchisaurus " + testIdx.ToString() + "(Clone)");
        Debug.Log(imgIndex_Mame);
        DeActiveCon();
        imgIndex_Mame++;
    }
    public void SpawnObject_Scel(int index)
    {
        if (!File.Exists(path + "/" + dinoName[index] + imgIndex_Scel.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Scel % 10;
        img_Data = File.ReadAllBytes(path + "/" + dinoName[index] + imgIndex_Scel.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기        
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Scel();
        imgPlayer.LeanScale(new Vector3(.7f, .7f, .7f), 1).setEaseLinear();                                     // 생성 효과
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        aniGON.Add("Scelidosaurus " + testIdx.ToString() + "(Clone)");
        Debug.Log(imgIndex_Scel);
        DeActiveCon();
        imgIndex_Scel++;
    }
    public void SpawnObject_Shun(int index)
    {
        if (!File.Exists(path + "/" + dinoName[index] + imgIndex_Shun.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Shun % 10;
        img_Data = File.ReadAllBytes(path + "/" + dinoName[index] + imgIndex_Shun.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기        
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Shun();
        imgPlayer.LeanScale(new Vector3(.7f, .7f, .7f), 1).setEaseLinear();                                     // 생성 효과
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        aniGON.Add("Shunosaurus " + testIdx.ToString() + "(Clone)");
        Debug.Log(imgIndex_Shun);
        DeActiveCon();
        imgIndex_Shun++;
    }
    public void SpawnObject_Tuoj(int index)
    {
        if (!File.Exists(path + "/" + dinoName[index] + imgIndex_Tuoj.ToString() + imgType)) { return; }
        int testIdx = imgIndex_Tuoj % 10;
        img_Data = File.ReadAllBytes(path + "/" + dinoName[index] + imgIndex_Tuoj.ToString() + imgType);  // 해당폴더에서 이미지 데이터 받아오기        
        GameObject imgPlayer = ObjectPool.OP.GetFromPool_Tuoj();
        imgPlayer.LeanScale(new Vector3(.7f, .7f, .7f), 1).setEaseLinear();                                     // 생성 효과
        imgPlayer.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(img_Data);
        aniGON.Add("Tuojiangosaurus " + testIdx.ToString() + "(Clone)");
        Debug.Log(imgIndex_Tuoj);
        DeActiveCon();
        imgIndex_Tuoj++;
    }

    IEnumerator DelayImport_Baro(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Baro(index);
    }
    IEnumerator DelayImport_Brac(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Brac(index);
    }
    IEnumerator DelayImport_Camp(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Camp(index);
    }
    IEnumerator DelayImport_Hete(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Hete(index);
    }
    IEnumerator DelayImport_Huay(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Huay(index);
    }
    IEnumerator DelayImport_Lufe(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Lufe(index);
    }
    IEnumerator DelayImport_Mame(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Mame(index);
    }
    IEnumerator DelayImport_Scel(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Scel(index);
    }
    IEnumerator DelayImport_Shun(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Shun(index);
    }
    IEnumerator DelayImport_Tuoj(int index)
    {
        yield return new WaitForSeconds(Setting.delayTime);
        SpawnObject_Tuoj(index);
    }

    [Obsolete]
    IEnumerator MakeWebRequestCoroutine()
    {
        UnityWebRequest request1 = UnityWebRequest.Get("http://192.168.0.106:15464/testScan");
        yield return request1.SendWebRequest();

        if (request1.isNetworkError || request1.isHttpError)
        {
            Debug.Log(request1.error);
            yield break;
        }

    }
    #endregion
}
