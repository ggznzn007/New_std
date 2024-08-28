using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Slider timeoutSlider;
    [SerializeField] private TextMeshProUGUI timeoutText;
    [SerializeField] private TextMeshProUGUI gamoverText;
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private float timeLimit = 60;

    private float currentTime;
    private int totalMatches = 10;
    private int matchesFound = 0;

    private List<Card> allCards;
    private Card flippedCard;
    private bool isFlipping = false;
    private bool isGameOver = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {        
        Board board = FindObjectOfType<Board>();
        allCards = board.GetCards();
        currentTime = timeLimit;
        StartCoroutine(FlipAllCardRoutine());
    }

    void SetCurrentTimeText()
    {
        int timeSec = Mathf.CeilToInt(currentTime);
        timeoutText.SetText(timeSec.ToString());
    }

    IEnumerator FlipAllCardRoutine()
    {
        isFlipping = true;
        yield return new WaitForSeconds(0.5f);
        FlipAllCards();
        yield return new WaitForSeconds(2);
        FlipAllCards();
        yield return new WaitForSeconds(0.5f);
        isFlipping = false;
        yield return StartCoroutine(CountDownTimerRoutine());
    }

    IEnumerator CountDownTimerRoutine()
    {
        while (currentTime > 0&&!isGameOver)
        {
            currentTime -= Time.deltaTime;
            timeoutSlider.value = currentTime / timeLimit;
            SetCurrentTimeText();
            yield return null;
        }
        GameOver(false);
    }

    void FlipAllCards()
    {
        foreach (Card card in allCards)
        {
            card.FlipCard();
        }
    }

    public void CardClicked(Card card)
    {
        if (isFlipping || isGameOver) { return; }
        card.FlipCard();

        if (flippedCard == null)
        {
            flippedCard = card;
        }
        else
        {
            StartCoroutine(CheckMatchRoutine(flippedCard, card));
        }
    }

    IEnumerator CheckMatchRoutine(Card card1, Card card2)
    {
        isFlipping = true;

        if (card1.cardID == card2.cardID)
        {
            card1.SetMatched();
            card2.SetMatched();
            matchesFound++;

            if (matchesFound == totalMatches)
            {
                GameOver(true);
            }
        }
        else
        {
            yield return new WaitForSeconds(1);
            card1.FlipCard();
            card2.FlipCard();
            yield return new WaitForSeconds(0.4f);
        }

        isFlipping = false;
        flippedCard = null;
    }

    void GameOver(bool success)
    {
        if (!isGameOver)
        {
            isGameOver = true;

            StopCoroutine(nameof(CountDownTimerRoutine));

            if (success)
            {
                gamoverText.SetText("Great!!!");
            }
            else
            {
                gamoverText.SetText("Game Over");
            }

            Invoke(nameof(ShowGameOverPanel), 2f);
        }
    }

    void ShowGameOverPanel()
    {
        gameoverPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
