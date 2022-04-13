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
        int rand = Random.Range(0, textures.Length);
        Sprite select = textures[rand];
        this.GetComponent<Image>().sprite = select;
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
        Debug.Log("Click" + Numeric);

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
    }
}
