using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swipe_UI : MonoBehaviour
{
    [SerializeField]
    private Scrollbar scrollbar;
    [SerializeField]
    private Transform[] circleContents;       // 현재 페이지를 나타내는 원

    private float scroll_Pos = 0;
    private float[] pos;
    private float distance;          // 각 페이지 사이 거리
    private float circleContentScale = 1.9f;

    private void FixedUpdate()
    {
        UpdateSwipeUI();
        UpdateCircleContent();
    }

    private void UpdateSwipeUI()
    {
        
        pos = new float[transform.childCount];        
        distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scroll_Pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_Pos < pos[i] + (distance * 0.5f) && scroll_Pos > pos[i] - (distance * 0.5f))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.01f);// 0.1f => 0.01f조정
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_Pos < pos[i] + (distance * 0.5f) && scroll_Pos > pos[i] - (distance * 0.5f))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.01f);// 0.1f => 0.01f조정
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.75f, 0.75f), 0.05f);// 0.1f => 0.05f조정
                    }
                }
            }
        }
    }

    private void UpdateCircleContent()
    {
        // 페이지 아래에 배치된 버튼 크기, 색상 제어 (현재 페이지의 버튼만 수정)
        for (int i = 0; i < pos.Length; i++)
        {
            circleContents[i].localScale = Vector2.one;
            circleContents[i].GetComponent<Image>().color = Color.grey;

            // 페이지 절반이 넘어가면 현재 페이지 원 바뀜
            if (scrollbar.value < pos[i] + (distance / 2.5f) &&
                scrollbar.value > pos[i] - (distance / 2.5f))//2 => 2.5f 수정
            {
                circleContents[i].localScale = Vector2.one * circleContentScale;
                circleContents[i].GetComponent<Image>().color = Color.gray;
            }
        }
    }
}
