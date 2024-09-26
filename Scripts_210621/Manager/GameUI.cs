using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
     
    [Header("Ammo")]
    public Text score;
     
    int currentScore = 0;

    //Instance
    public static GameUI inst;

    void Awake () { inst = this; }
       
    IEnumerator Count(float target, float current)
    {
        float duration = 0.5f; // 카운팅에 걸리는 시간 설정. 
        float offset = (currentScore - current) / duration;

        while (current < currentScore)
        {
            current += offset * Time.deltaTime;
            score.text = ((int)current).ToString("n0");
            yield return null;
        }
        current = currentScore;

        if (current != currentScore)
        {
            score.text = ((int)current).ToString("n0");
        }
    }

    public void ScoreUp(int score)
    {
        currentScore += score;
        //StartCoroutine(Count(currentScore, currentScore - Player.inst.money));
    }
}
