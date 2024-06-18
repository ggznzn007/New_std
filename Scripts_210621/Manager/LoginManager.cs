using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoginManager : MonoBehaviour
{
    private bool checkok = false;
    public SceneMoveManager scenemovemanager;
    public GameObject Tutorial;
    public GameObject SelectFairyScene;
    public GameObject ErrorPopup;
    public TMP_InputField nickName;
    public TMP_InputField gardenName;
    public TMP_InputField age;
    public TextMeshProUGUI ErrorInfo; //에러 메세지
    public TextMeshProUGUI PopupAnswer; //팝업 답
    public TextMeshProUGUI PlantDate;  //심는 날짜
    public TextMeshProUGUI Weather;    //날씨
    public TextMeshProUGUI Location;  //위치
    public TextMeshProUGUI RecordDate; //일기 기록    

    private void Update()
    {
        PopupAnswer.text = PlayerPrefs.GetString("TutorialAnswer"); //튜토리얼 답변 출력
        PlantDate.text = PlayerPrefs.GetString("Date");  //오늘 날짜 출력
        RecordDate.text = PlayerPrefs.GetString("Date");

        Weather.text = "Temp" + PlayerPrefs.GetString("temp") + "(°C), " + PlayerPrefs.GetString("Weather") ;
        Location.text = "Country :  " + PlayerPrefs.GetString("Country") +"   City : " + PlayerPrefs.GetString("City");
        if (age.text.Length > 2)
        {
            ErrorPopup.SetActive(true);
            ErrorInfo.text = "Please enter your age properly.";
            age.text = "";
        }

        if (gardenName.text.Length > 12)
        {
            ErrorPopup.SetActive(true);
            ErrorInfo.text = "Please enter your Garden Name between 5 and 12 characters.";
            gardenName.text = "";
        }

        if (nickName.text.Length > 12)
        {
            ErrorPopup.SetActive(true);
            ErrorInfo.text = "Please enter your User Name between 5 and 12 characters.";
            nickName.text = "";
        }
    }

    public void enrollNickName()//별명 등록
    {
        if (nickName.text.Length < 2)
        {
            Debug.Log("id를 2자이상 입력해주세요");
        }
        else
        {
            Debug.Log("Id 등록 완료");
        }
    }

    public void enrollPassWord()//비밀번호 등록
    {
        if (gardenName.text.Length < 2)
        {
            Debug.Log("id를 2자이상 입력해주세요");
        }
        else
        {
            Debug.Log("Id 등록 완료");
        }
    }

    public void SaveInfo() //저장 버튼을 눌렀을 때 호출되는 함수
    {
        PlayerPrefs.SetString("User", nickName.text);
        PlayerPrefs.SetString("Garden", gardenName.text);
        PlayerPrefs.SetString("Age", age.text);
        if (PlayerPrefs.GetString("User").Length < 5 || PlayerPrefs.GetString("User").Length > 12)
        {
            ErrorPopup.SetActive(true);
            ErrorInfo.text = "Please enter your User Name between 5 and 12 characters.";
        }
        else if (PlayerPrefs.GetString("Garden").Length < 5 || PlayerPrefs.GetString("Garden").Length > 12)
        {
            ErrorPopup.SetActive(true);
            ErrorInfo.text = "Please enter your Garden Name between 5 and 12 characters.";
        }
        else if (PlayerPrefs.GetString("Age").Length == 0 || PlayerPrefs.GetString("Age").Length > 2)
        {
            ErrorPopup.SetActive(true);
            ErrorInfo.text = "Please enter your age properly.";
        }
        else
        {
            checkok = true;
        }

        if (checkok)
        {
            ErrorInfo.text = "Please enter more than 2 characters.";
            scenemovemanager.SetLoginCanvas(SelectFairyScene);

            scenemovemanager.TutorialTalk();
        }
    }    

    public void ButtonDestroy()
    {
        ErrorPopup.SetActive(false);
    }
}
