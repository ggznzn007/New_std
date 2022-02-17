using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swipe_UI : MonoBehaviour
{
    [SerializeField]
    private Scrollbar scrollbar; // �����̵� ��
    [SerializeField]
    private Color changedCircleColor; // ���ϴ� ���� ����
    [SerializeField]
    private Transform[] circleContents;       // ���� �������� ��Ÿ���� ��

    private float scroll_Pos = 0; // ���� ������ ��ġ
    private float[] pos; // ���ϴ� ������ ��ġ
    private float distance;          // �� ������ ���� �Ÿ�
    private float circleContentScale = 1.2f; // ���ϱ� �� ���� ũ�⿡ ���ϴ� ��
    private float circleContentScaleChanged = 1.8f; // ���� �Ŀ� ���� ũ�⿡ ���ϴ� ��

    private bool runIt = false; // �������� ������ �ƴ��� �Ǵ�
    private float time;
    private Button takeTheBtn; // �ش��ư
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

    private void GecisiDuzenle(float distance, float[] pos, Button btn) // ���� ������ �ش��������� �����̵� 
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

    public void WhichBtnClicked(Button btn) // � ���� �������� �Ǵ�
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
    private void UpdateSwipeUI() // �޴� ���������ؼ� ������ �̵�
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
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.01f);// 0.1f => 0.01f����
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_Pos < pos[i] + (distance * 0.5f) && scroll_Pos > pos[i] - (distance * 0.5f))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.01f);// 0.1f => 0.01f����
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.75f, 0.75f), 0.05f);// 0.1f => 0.05f����
                    }
                }
            }
        }
    }

    private void UpdateCircleContent() // ��������ȭ�� ���� �ϴ� ���� ���� �� ���� ����
    {
        // ������ �Ʒ��� ��ġ�� ��ư ũ��, ���� ���� (���� �������� ��ư�� ����)
        for (int i = 0; i < pos.Length; i++)
        {
            circleContents[i].localScale = Vector2.one * circleContentScale;
            circleContents[i].GetComponent<Image>().color = Color.white;

            // ������ ������ �Ѿ�� ���� ������ �� �ٲ�
            if (scrollbar.value < pos[i] + (distance / 2f) &&
                scrollbar.value > pos[i] - (distance / 2f))//2 => 2.5f ����
            {
                circleContents[i].localScale = Vector2.one * circleContentScaleChanged;
                circleContents[i].GetComponent<Image>().color = changedCircleColor;
            }
        }
    }
}
