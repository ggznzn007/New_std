using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionText : MonoBehaviour
{
    //GameObject obj;
    //GameObject obj2;

    Text question;
    Text answer;
    public string showquestion;
    private string showanswer1, showanswer2, showanswer3;
    public int ran;
    int cnt = 1;
    public Entity_TypicalQuestions item;
    public TextMeshProUGUI getQuestion;
    public TextMeshProUGUI getAnswer;

    public void Click()
    {
        /*        Debug.Log(item.sheets[0].list.Count);
                int max = item.sheets[0].list.Count-1;
                Debug.Log(max);
                int ran = Random.Range(0, max);*/

        int cnt = item.sheets[0].list.Count;
        int ran = Random.Range(0, cnt);

        showquestion = item.sheets[0].list[ran].Question;
        showanswer1 = item.sheets[0].list[ran].Answer1;
        showanswer2 = item.sheets[0].list[ran].Answer2;
        showanswer3 = item.sheets[0].list[ran].Answer3;

        QuestionsText();
        AnswersText();
        /*        item.sheets[0].list.RemoveAt(ran);*/

    }
    void QuestionsText()
    {
        StartCoroutine(CoQuestionText());
    }

    IEnumerator CoQuestionText()
    {
        yield return new WaitForSeconds(0.01f);
        //GetComponent<TextMeshProUGUI>().text= "Hey " + PlayerPrefs.GetString("User") + "!\n" + showquestion;
        getQuestion.text = "Hey " + PlayerPrefs.GetString("User") + "!\n" + showquestion;
    }
    void AnswersText()
    {
        StartCoroutine(CoAnswersText());
    }

    IEnumerator CoAnswersText()
    {
        getAnswer.text = "";
        yield return new WaitForSeconds(5f);
        getAnswer.text = showanswer1;
        yield return new WaitForSeconds(5f);
        getAnswer.text = showanswer2;
        yield return new WaitForSeconds(5f);
        getAnswer.text = showanswer3;
        yield return new WaitForSeconds(5f);
    }
}
