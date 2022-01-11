using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    [SerializeField]
    private Scrollbar scrollbar;              // ��ũ�ѹ��� ��ġ�� �������� ���� ������ �˻�
    [SerializeField]
    private Transform[] circleContents;       // ���� �������� ��Ÿ���� ��
    [SerializeField]
    private float swipeTime = 0.2f;           // ������ ���������Ǵ� �ð�
    [SerializeField]
    private float swipeDistance = 60.0f;      //������ ���������Ǳ� ���� �ּҰŸ�

    private float[] scrollPageValues;         // �� ������ ��ġ�� [0.0 - 1.0]
    private float valueDistance = 0;          // �� ������ ���� �Ÿ�
    private int currentPage = 0;              // ���� ������
    private int maxPage = 0;                  // �ִ� ������
    private float startTouchX;                // ��ġ ���� ��ġ
    private float endTouchX;                  // ��ġ ���� ��ġ
    private bool isSwipeMode = false;         // ���� ���������ǰ� �ִ��� üũ
    private float circleContentScale = 1.6f;  // ���� �������� �� ũ��
   

    private void Awake()
    {
        // ��ũ�� �Ǵ� �������� �� value���� �����ϴ� �迭 �޸� �Ҵ�
        scrollPageValues = new float[transform.childCount];

        // ��ũ�� �Ǵ� ������ ������ �Ÿ�
        valueDistance = 1f / (scrollPageValues.Length - 1f);

        // ��ũ�� �Ǵ� �������� �� value ��ġ ���� [0 <= value <= 1]
        for (int i = 0; i < scrollPageValues.Length; ++i)
        {
            scrollPageValues[i] = valueDistance * i;
        }

        // �ִ� ������ ��
        maxPage = transform.childCount;
    }

    private void Start()
    {
        // ���� ������ �� 0�� �������� �� �� �ֵ��� ����
        SetScrollBarValue(0);
    }

    public void SetScrollBarValue(int index)
    {
        currentPage = index;
        scrollbar.value = scrollPageValues[index];
    }

    private void Update()
    {
        UpdateInput();
        // ������ �Ʒ��� ��ġ ��ư ����
        UpdateCircleContent();
    }

    private void UpdateInput()
    {
        // ���� �������� ���̸� ��ġ �Ұ�
        if (isSwipeMode) { return; }

#if UNITY_EDITOR
        // ���콺 ���� ��ư ������ �� 1ȸ
        if (Input.GetMouseButtonDown(0))
        {
            // ��ġ ���� ���� (�������� ���� ����)
            startTouchX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // ��ġ ���� ���� (�������� ���� ����)
            endTouchX = Input.mousePosition.x;

            UpdateSwipe();
        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // ��ġ ���� ���� (�������� ���� ����)
                startTouchX = Input.mousePosition.x;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // ��ġ ���� ���� (�������� ���� ����)
                endTouchX = Input.mousePosition.x;

                UpdateSwipe();
            }
        }
#endif
    }

    private void UpdateSwipe()
    {
        // �ʹ� ���� �Ÿ��� �������� �� �������� X
        if (Mathf.Abs(startTouchX - endTouchX) < swipeDistance)
        {
            // ���� �������� ���������ؼ� ���ư�
            StartCoroutine(OnSwipeOneStep(currentPage));
            return;
        }

        // �������� ����
        bool isLeft = startTouchX < endTouchX ? true : false;

        // �̵������� ������ �� 
        if (isLeft)
        {
            // ���� �������� ���� ���̸� ����
            if (currentPage == 0) { return; }

            // �������� �̵��� ���� ���� ������ 1����
            currentPage--;
        }

        // �̵������� �������� ��
        else
        {
            // ���� �������� ������ ���̸� ����
            if (currentPage == maxPage - 1) { return; }

            // ���������� �̵��� ���� ���� ������ 1����
            currentPage++;
        }

        // currentIndex��° �������� ���������ؼ� �̵�
        StartCoroutine(OnSwipeOneStep(currentPage));
    }

    // �������� ���� �ѱ�� ȿ�� ���
    private IEnumerator OnSwipeOneStep(int index)
    {
        float start = scrollbar.value;
        float current = 0;
        float percent = 0;

        isSwipeMode = true;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / swipeTime;

            scrollbar.value = Mathf.Lerp(start, scrollPageValues[index], percent);

            yield return null;
        }

        isSwipeMode = false;
    }

    private void UpdateCircleContent()
    {
        // ������ �Ʒ��� ��ġ�� ��ư ũ��, ���� ���� (���� �������� ��ư�� ����)
        for (int i = 0; i < scrollPageValues.Length; i++)
        {
            circleContents[i].localScale = Vector2.one;
            circleContents[i].GetComponent<Image>().color = Color.grey;

            // ������ ������ �Ѿ�� ���� ������ �� �ٲ�
            if (scrollbar.value < scrollPageValues[i] + (valueDistance / 2) &&
                scrollbar.value > scrollPageValues[i] - (valueDistance / 2))
            {
                circleContents[i].localScale = Vector2.one * circleContentScale;
                circleContents[i].GetComponent<Image>().color = Color.gray;
            }
        }
    }
}
