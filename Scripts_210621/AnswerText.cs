using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class AnswerText : MonoBehaviour
{
    public TextMeshProUGUI RecordDate; //기록 날짜
    public TextMeshProUGUI PlantDate; //심는 날짜
    public TextMeshProUGUI Weather;   //날씨
    public TextMeshProUGUI Location;  //위치
    public TextMeshProUGUI Question;   //질문
    public TextMeshProUGUI Answer;     //답안
    public TextMeshProUGUI ErrorTitle; //에러팝 제목
    public TextMeshProUGUI ErrorText;  //에로팝 내용
    public GameObject Expected;        //기대
    public GameObject Angry;           //화남
    public GameObject Disgust;         //혐오
    public GameObject Happy;           //기쁨
    public GameObject Sad;             //슬픔
    public GameObject Scream;          //놀람
    public GameObject Trust;           //신뢰
    public GameObject Fearful;         //공포
    public APIManager apiManager;      //감정분석
    public InputField Conversation;
    public GameObject CompleteUI; //Complete 오브젝트 가져오기
    public GameObject ConversationUI; //Conversation 오브젝트 가져오기
    public InputField inputFieldRef;  //Conversation inputfield
    public QuestionText questionText;  //사용자가 답한 질문 가져오기
    public ArrayList answerlist = new ArrayList();   //유저 답변 리스트
    public ArrayList questionlist = new ArrayList(); //유저 질문 리스트
    public GameObject Great;
    public GameObject ErrorPopup;
    public APIManager aPIManager;
    public MenuUI menuUI;
    private string answer = "";
    private string question = "";
    public static int n = 0;

    void Start()
    {
        RecordDate.text = PlayerPrefs.GetString("Date"); 
        PlantDate.text = PlayerPrefs.GetString("Date");  //오늘 날짜 출력
        Weather.text = "Temp" + PlayerPrefs.GetString("temp") + "(°C), " + PlayerPrefs.GetString("Weather");             //날씨
        Location.text = "Country :  " + PlayerPrefs.GetString("Country") + "   City : " + PlayerPrefs.GetString("City"); //위치
    }

    private void Update()
    {

    }
    public void SaveUserAnswer()
    {
        PlayerPrefs.SetString("Question", questionText.showquestion);
        PlayerPrefs.SetString("Answer", Conversation.text);//Answer 저장

/*        string[] savequestions = new string[] { PlayerPrefs.GetString("Question") };
        string[] saveanswers = new string[] { PlayerPrefs.GetString("Answer") };
        SaveGame.Save<string[]>("questions.txt", savequestions);
        SaveGame.Save<string[]>("answers.txt", saveanswers);

        string[] loadquestions = SaveGame.Load<string[]>("questions.txt");
        string[] loadanswers = SaveGame.Load<string[]>("answers.txt");
        
        for(int i = 0; i < loadquestions.Length; i++)
            */
        
        
        if (PlayerPrefs.GetString("Answer").Length > 2)
        {
            answer = PlayerPrefs.GetString("Answer");
            aPIManager.sendText(answer);
            question = PlayerPrefs.GetString("Question");
            questionlist.Add(question);
            answerlist.Add(answer);
            inputFieldRef.text = "";
            GameManager.inst.oxygenCnt += 4000;
            menuUI.SetScreen(Great);            
        }
        else if (PlayerPrefs.GetString("Answer").Length <= 2)
        {
            ErrorTitle.text = "Error";
            ErrorText.text = "Please write more than two characters!";
            ErrorPopup.SetActive(true);
        }
    }
    public void NextQuestion()
    {
        CompleteUI.SetActive(true); //Complete 활성화
        ConversationUI.SetActive(false);
    }

    public void StartList()
    {
        Question.text = questionlist[0].ToString();
        Answer.text = answerlist[0].ToString();
        Happy.SetActive(false);
        Trust.SetActive(false);
        Fearful.SetActive(false);
        Expected.SetActive(false);
        Scream.SetActive(false);
        Sad.SetActive(false);
        Disgust.SetActive(false);
        Angry.SetActive(false);
        switch (aPIManager.emotionlist[0])
        {
            case "기쁨":
                Happy.SetActive(true);
                break;
            case "신뢰":
                Happy.SetActive(true);
                break;
            case "공포":
                Fearful.SetActive(true);
                break;
            case "기대":
                Expected.SetActive(true);
                break;
            case "놀라움":
                Scream.SetActive(true);
                break;
            case "슬픔":
                Sad.SetActive(true);
                break;
            case "혐오":
                Disgust.SetActive(true);
                break;
            case "분노":
                Angry.SetActive(true);
                break;
        }
    }

    public void NextList()
    {
        {
            try
            {
                if (n <= questionlist.Count)
                {
                    Question.text = questionlist[n + 1].ToString();
                    Answer.text = answerlist[n + 1].ToString();
                    Happy.SetActive(false);
                    Trust.SetActive(false);
                    Fearful.SetActive(false);
                    Expected.SetActive(false);
                    Scream.SetActive(false);
                    Sad.SetActive(false);
                    Disgust.SetActive(false);
                    Angry.SetActive(false);
                    switch (aPIManager.emotionlist[n + 1])
                    {
                        case "기쁨":
                            Happy.SetActive(true);
                            break;
                        case "신뢰":
                            Happy.SetActive(true);
                            break;
                        case "공포":
                            Fearful.SetActive(true);
                            break;
                        case "기대":
                            Expected.SetActive(true);
                            break;
                        case "놀라움":
                            Scream.SetActive(true);
                            break;
                        case "슬픔":
                            Sad.SetActive(true);
                            break;
                        case "혐오":
                            Disgust.SetActive(true);
                            break;
                        case "분노":
                            Angry.SetActive(true);
                            break;
                    }
                    n++;
                }
            }
            catch (System.ArgumentException)
            {
                ErrorTitle.text = "Last Record";
                ErrorText.text = "This is the end of your records.";
                ErrorPopup.SetActive(true);
            }
        }
    }

    public void PrevList()
    {
        try
        {
            if (n <= questionlist.Count)
            {
                Question.text = questionlist[n - 1].ToString();
                Answer.text = answerlist[n - 1].ToString();
                Happy.SetActive(false);
                Trust.SetActive(false);
                Fearful.SetActive(false);
                Expected.SetActive(false);
                Scream.SetActive(false);
                Sad.SetActive(false);
                Disgust.SetActive(false);
                Angry.SetActive(false);

                switch (aPIManager.emotionlist[n - 1])
                {
                    case "기쁨":
                        Happy.SetActive(true);
                        break;
                    case "신뢰":
                        Happy.SetActive(true);
                        break;
                    case "공포":
                        Fearful.SetActive(true);
                        break;
                    case "기대":
                        Expected.SetActive(true);
                        break;
                    case "놀라움":
                        Scream.SetActive(true);
                        break;
                    case "슬픔":
                        Sad.SetActive(true);
                        break;
                    case "혐오":
                        Disgust.SetActive(true);
                        break;
                    case "분노":
                        Angry.SetActive(true);
                        break;
                }
                n--;
            }
        }
        catch (System.ArgumentException)
        {
            ErrorTitle.text = "Last Record";
            ErrorText.text = "This is the end of your records.";
            ErrorPopup.SetActive(true);
        }
    }
}
 