using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class Tile : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Sprite[] textures;

    private TextMeshProUGUI textNumeric;
    private Board board;
    private Vector3 correctPosition;

    public bool IsCorrected { private set; get; } = false;

    private int numeric;

    public int Numeric
    {
        set
        {
            numeric = value;
            textNumeric.text = numeric.ToString();
        }
        get => numeric;
    }

    public void Setup(Board board, int hideNumeric, int numeric)
    {
        this.board = board;
        textNumeric = GetComponentInChildren<TextMeshProUGUI>();

        int rand = Random.Range(0, textures.Length);// 타일색을 랜덤으로
        Sprite select = textures[rand];             // 랜덤텍스처를 스프라이트로 대입
        this.GetComponent<Image>().sprite = select; // 생성되는 타일의 스프라이트에 스프라이트로 대입

        Numeric = numeric;
        if(Numeric==hideNumeric)
        {
            GetComponent<UnityEngine.UI.Image>().enabled = false;
            textNumeric.enabled = false; // 타일의 위치를 바꾸기 위해 한칸은 비어있어야 하므로 텍스트 UI는 블라인드설정
        }
    }

    public void SetCorrectPosition()
    {
        correctPosition = GetComponent<RectTransform>().localPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        board.IsMoveTile(this);
    }

    public void OnMoveTo(Vector3 end)
    {
        StartCoroutine("MoveTo",end);
    }

    private IEnumerator MoveTo(Vector3 end)
    {
        float current = 0;
        float percent = 0;
        float moveTime = 0.1f;
        Vector3 start = GetComponent<RectTransform>().localPosition;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTime;

            GetComponent<RectTransform>().localPosition = Vector3.Lerp(start, end, percent);

            yield return null;
        }
        IsCorrected = correctPosition == GetComponent<RectTransform>().localPosition ? true : false;
        // 처음 숫자를 배치했을때 correctPosition을 설정하고, 
        // 이 값과 현재위치가 같으면 퍼즐이 제 위치에 있다고 판단 = true가 됨
        board.IsGameOver();
    }
}
