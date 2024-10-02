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

        int rand = Random.Range(0, textures.Length);// Ÿ�ϻ��� ��������
        Sprite select = textures[rand];             // �����ؽ�ó�� ��������Ʈ�� ����
        this.GetComponent<Image>().sprite = select; // �����Ǵ� Ÿ���� ��������Ʈ�� ��������Ʈ�� ����

        Numeric = numeric;
        if(Numeric==hideNumeric)
        {
            GetComponent<UnityEngine.UI.Image>().enabled = false;
            textNumeric.enabled = false; // Ÿ���� ��ġ�� �ٲٱ� ���� ��ĭ�� ����־�� �ϹǷ� �ؽ�Ʈ UI�� ����ε弳��
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
        // ó�� ���ڸ� ��ġ������ correctPosition�� �����ϰ�, 
        // �� ���� ������ġ�� ������ ������ �� ��ġ�� �ִٰ� �Ǵ� = true�� ��
        board.IsGameOver();
    }
}
