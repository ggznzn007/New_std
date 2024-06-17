using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneMoveManager : MonoBehaviour
{
    public GameObject BG;
    public GameObject Login;
    public GameObject SelectFairyScene;
    public GameObject Welcome;
    public GameObject Plant;
    public GameObject Plantlist;
    public GameObject rose;
    public GameObject InputField;
    public GameObject BtnNextobj;
    public GameObject WelcomeBox;
    public GameObject Oxygen;
    public GameObject Tutorial;
    public GameObject savebutton;
    public GameObject startbutton;
    public GameObject black;
    public GameObject ErrorPopup;
    public InputField tutorialAnswer;
    public Text DialougeText;
    public Text TutorialText;
    public TextMeshProUGUI FairyText;
    public DisableTrackedVisualsLogin login;
    private string fullText;
    private string currentText = "";
    private string changeText;

    int n;

    int oxygenCnt;

    private void Awake()
    {
        oxygenCnt = GameManager.inst.oxygenCnt;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("tutorial") == 1)
        {
            black.SetActive(true);
            SceneManager.LoadScene(1);
        }
    }

    public void MoveScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void SetLoginCanvas(GameObject screen)
    {
        black.SetActive(false);
        BG.SetActive(false);
        Login.SetActive(false);
        SelectFairyScene.SetActive(false);
        Welcome.SetActive(false);
        Plant.SetActive(false);
        WelcomeBox.SetActive(false);
        Oxygen.SetActive(false);
        Tutorial.SetActive(false);
        if (screen == Welcome)
        {
            PlaceObjectsOnPlaneLogin.PlaceObjectsOnLogin.isTouch = true;            
            login.AppearPoint();
            DialougeText.text = "Touch to summon your own Fairy!";            
            //BtnNextobj.SetActive(true);//유니티 실행할때 켜놓을 것
        }

        screen.SetActive(true);
    }

    public void BtnNext()
    {
        switch (n)
        {            
            case 0:
                //요정생성
                Tutorial.SetActive(false);
                BtnNextobj.SetActive(false);
                InputField.SetActive(false);
                DialougeText.text = "";
                GetComponent<LoginAudioManager>().PlayAudio(1);
                changeText = "I have a present for you. In order to receive your present you would have to answer my question. Shall we start?";
                FairyTalk(changeText);
                break;
            case 1:
                //질문하고 답하기
                BtnNextobj.SetActive(false);
                DialougeText.text = "How are you today?";
                InputField.SetActive(true);
                if(tutorialAnswer.text.Length > 2)
                {
                    savebutton.SetActive(true);
                }
                break;
            case 2:
                //식물을 심으러 갈래? voice plant 재생하기 + 식물 보여주기
                DialougeText.text = "";
                BtnNextobj.SetActive(false);
                InputField.SetActive(false);
/*                Oxygen.SetActive(true);
                oxygenCnt = 10;
                GameManager.inst.oxygenCnt += oxygenCnt;*/
                changeText = "Nicely done! This is a special gift for you! Would you like to plant it in your garden?";
                FairyTalk(changeText);
                GetComponent<LoginAudioManager>().PlayAudio(2);
                rose.SetActive(true);
                break;
            case 3:
                //바닥재인식, 정원 심기, 식물 심기, 식물 누르면 팝업창 노출                
                PlaceObjectsOnPlaneLogin.PlaceObjectsOnLogin.DestroyFairy();
                BtnNextobj.SetActive(false);
                DialougeText.text = "Touch to summon your garden!";
                //dialougetxt.text = "정원을 나타내세요";
                rose.SetActive(false);
                login.AppearPoint();//포인트 생성
                login.CreateGarden();//정원생성;        

                break;
            case 4:
                //text 원하는 곳을 눌러 식물을 심으세요~!
                BtnNextobj.SetActive(false);
                DialougeText.text = "Touch the button below to select a flower and plant it on the arrow.";
                SetLoginCanvas(Plant);//식물 심기
                break;
            case 5:
                Plant.SetActive(false);
                DialougeText.text = "Touch the flower to check your records.";
                BtnNextobj.SetActive(false);
                break;
            case 6:
                BtnNextobj.SetActive(false);
                DialougeText.text = "End of tutorial!";
                changeText = "Well done! This is the end of the tutorial. Now let's begin decorating your own garden.";
                FairyTalk(changeText);
                GetComponent<LoginAudioManager>().PlayAudio(3);
                PlayerPrefs.SetInt("tutorial", 1);
                break;
            case 7:
                MoveScene("Main_pss");
                break;
        }
        n++;
    }

    public void BtnPlant()
    {
        Plantlist.SetActive(true);
    }

    public void TutorialTalk()
    {
        StartCoroutine(coTutorialTalk());
    }

    IEnumerator coTutorialTalk()
    {
        changeText = $"Welcome {PlayerPrefs.GetString("User")}! \n Touch the START BUTTON to continue!";
        for (int i = 0; i <= changeText.Length; i++)
        {
            currentText = changeText.Substring(0, i);
            TutorialText.text = currentText;
            yield return new WaitForSeconds(0.1f);
        }
        startbutton.SetActive(true);        
    }

    public void TutorialSaveInfo()
    {
        if (tutorialAnswer.text.Length > 2)
        {
            savebutton.SetActive(false);
            PlayerPrefs.SetString("TutorialAnswer", tutorialAnswer.text);
            Debug.Log(PlayerPrefs.GetString("TutorialAnswer"));
            BtnNextobj.SetActive(true);
        }
        else
        {
            ErrorPopup.SetActive(true);
        }
    }

    public void FairyTalk(string change)
    {
        WelcomeBox.SetActive(true);
        StartCoroutine(coFairyTalk(change));
    }

    IEnumerator coFairyTalk(string change)
    {
        FairyText.text = change;
        for (int i = 0; i <= change.Length; i++)
        {
            currentText = change.Substring(0, i);
            FairyText.text = currentText;
            yield return new WaitForSeconds(0.01f);
        }
        BtnNextobj.SetActive(true);
    }

    public void FairyTalkStart()
    {
        DialougeText.text = "";
        BtnNextobj.SetActive(false);
        WelcomeBox.SetActive(true);
        changeText = "Hi! Are you ready to decorate your garden with me?";
        FairyTalk(changeText);
    }
}
