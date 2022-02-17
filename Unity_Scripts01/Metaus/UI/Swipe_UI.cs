using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swipe_UI : MonoBehaviour
{
    [SerializeField]
    private Scrollbar scrollbar; // 슬라이드 바
    [SerializeField]
    private Color changedCircleColor; // 변하는 원의 색상
    [SerializeField]
    private Transform[] circleContents;       // 현재 페이지를 나타내는 원

    private float scroll_Pos = 0; // 현재 페이지 위치
    private float[] pos; // 변하는 페이지 위치
    private float distance;          // 각 페이지 사이 거리
    private float circleContentScale = 1.2f; // 변하기 전 원의 크기에 곱하는 값
    private float circleContentScaleChanged = 1.8f; // 변한 후에 원의 크기에 곱하는 값

    private bool runIt = false; // 스와이프 중인지 아닌지 판단
    private float time;
    private Button takeTheBtn; // 해당버튼
    int btnNumber;

    private void FixedUpdate()
    {
        if (runIt)
        {
            GecisiDuzenle(distance, pos, takeTheBtn);
            time += Time.deltaTime;

            if (time > 1f)
            {
                time = 0;
                runIt = false;
            }
        }
        UpdateSwipeUI();
        UpdateCircleContent();
    }

    private void GecisiDuzenle(float distance, float[] pos, Button btn) // 원을 누르면 해당페이지로 슬라이드 
    {
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_Pos < pos[i] + (distance / 2) && scroll_Pos > pos[i] - (distance / 2))
            {
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[btnNumber], 1f * Time.deltaTime);

            }
        }

        for (int i = 0; i < btn.transform.parent.transform.childCount; i++)
        {
            btn.transform.name = ".";
        }
    }

    public void WhichBtnClicked(Button btn) // 어떤 원을 누른건지 판단
    {
        btn.transform.name = "clicked";
        for (int i = 0; i < btn.transform.parent.transform.childCount; i++)
        {
            if (btn.transform.parent.transform.GetChild(i).transform.name == "clicked")
            {
                btnNumber = i;
                takeTheBtn = btn;
                time = 0;
                scroll_Pos = (pos[btnNumber]);
                runIt = true;
            }
        }


    }
    private void UpdateSwipeUI() // 메뉴 스와이프해서 페이지 이동
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

    private void UpdateCircleContent() // 페이지변화에 따라 하단 원의 형태 및 색상 변경
    {
        // 페이지 아래에 배치된 버튼 크기, 색상 제어 (현재 페이지의 버튼만 수정)
        for (int i = 0; i < pos.Length; i++)
        {
            circleContents[i].localScale = Vector2.one * circleContentScale;
            circleContents[i].GetComponent<Image>().color = Color.white;

            // 페이지 절반이 넘어가면 현재 페이지 원 바뀜
            if (scrollbar.value < pos[i] + (distance / 2f) &&
                scrollbar.value > pos[i] - (distance / 2f))//2 => 2.5f 수정
            {
                circleContents[i].localScale = Vector2.one * circleContentScaleChanged;
                circleContents[i].GetComponent<Image>().color = changedCircleColor;
            }
        }
    }
}
