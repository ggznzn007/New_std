using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    [SerializeField]
    private Scrollbar scrollbar;              // 스크롤바의 위치를 바탕으로 현재 페이지 검사
    [SerializeField]
    private Transform[] circleContents;       // 현재 페이지를 나타내는 원
    [SerializeField]
    private float swipeTime = 0.2f;           // 페이지 스와이프되는 시간
    [SerializeField]
    private float swipeDistance = 60.0f;      //페이지 스와이프되기 위한 최소거리

    private float[] scrollPageValues;         // 각 페이지 위치값 [0.0 - 1.0]
    private float valueDistance = 0;          // 각 페이지 사이 거리
    private int currentPage = 0;              // 현재 페이지
    private int maxPage = 0;                  // 최대 페이지
    private float startTouchX;                // 터치 시작 위치
    private float endTouchX;                  // 터치 종료 위치
    private bool isSwipeMode = false;         // 현재 스와이프되고 있는지 체크
    private float circleContentScale = 1.6f;  // 현재 페이지의 원 크기
   

    private void Awake()
    {
        // 스크롤 되는 페이지의 각 value값을 저장하는 배열 메모리 할당
        scrollPageValues = new float[transform.childCount];

        // 스크롤 되는 페이지 사이의 거리
        valueDistance = 1f / (scrollPageValues.Length - 1f);

        // 스크롤 되는 페이지의 각 value 위치 설정 [0 <= value <= 1]
        for (int i = 0; i < scrollPageValues.Length; ++i)
        {
            scrollPageValues[i] = valueDistance * i;
        }

        // 최대 페이지 수
        maxPage = transform.childCount;
    }

    private void Start()
    {
        // 최초 시작할 때 0번 페이지를 볼 수 있도록 설정
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
        // 페이지 아래에 배치 버튼 제어
        UpdateCircleContent();
    }

    private void UpdateInput()
    {
        // 현재 스와이프 중이면 터치 불가
        if (isSwipeMode) { return; }

#if UNITY_EDITOR
        // 마우스 왼쪽 버튼 눌렀을 때 1회
        if (Input.GetMouseButtonDown(0))
        {
            // 터치 시작 지점 (스와이프 방향 구분)
            startTouchX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // 터치 종료 지점 (스와이프 방향 구분)
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
                // 터치 시작 지점 (스와이프 방향 구분)
                startTouchX = Input.mousePosition.x;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // 터치 종료 지점 (스와이프 방향 구분)
                endTouchX = Input.mousePosition.x;

                UpdateSwipe();
            }
        }
#endif
    }

    private void UpdateSwipe()
    {
        // 너무 작은 거리를 움직였을 때 스와이프 X
        if (Mathf.Abs(startTouchX - endTouchX) < swipeDistance)
        {
            // 원래 페이지로 스와이프해서 돌아감
            StartCoroutine(OnSwipeOneStep(currentPage));
            return;
        }

        // 스와이프 방향
        bool isLeft = startTouchX < endTouchX ? true : false;

        // 이동방향이 왼쪽일 때 
        if (isLeft)
        {
            // 현재 페이지가 왼쪽 끝이면 종료
            if (currentPage == 0) { return; }

            // 왼쪽으로 이동을 위해 현재 페이지 1감소
            currentPage--;
        }

        // 이동방향이 오른쪽일 때
        else
        {
            // 현재 페이지가 오른쪽 끝이면 종료
            if (currentPage == maxPage - 1) { return; }

            // 오른쪽으로 이동을 위해 현재 페이지 1증가
            currentPage++;
        }

        // currentIndex번째 페이지로 스와이프해서 이동
        StartCoroutine(OnSwipeOneStep(currentPage));
    }

    // 스와이프 한장 넘기는 효과 재생
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
        // 페이지 아래에 배치된 버튼 크기, 색상 제어 (현재 페이지의 버튼만 수정)
        for (int i = 0; i < scrollPageValues.Length; i++)
        {
            circleContents[i].localScale = Vector2.one;
            circleContents[i].GetComponent<Image>().color = Color.grey;

            // 페이지 절반이 넘어가면 현재 페이지 원 바뀜
            if (scrollbar.value < scrollPageValues[i] + (valueDistance / 2) &&
                scrollbar.value > scrollPageValues[i] - (valueDistance / 2))
            {
                circleContents[i].localScale = Vector2.one * circleContentScale;
                circleContents[i].GetComponent<Image>().color = Color.gray;
            }
        }
    }
}
