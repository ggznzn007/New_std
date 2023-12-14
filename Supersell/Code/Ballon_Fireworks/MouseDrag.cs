using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MouseDrag : MonoBehaviour/// <summary> ȭ�鿡 ���콺 �巡�׷� �簢�� ���� ���� ǥ���ϱ� </summary>
{
    private Vector2 mPosCur;   // �ǽð�(���� ������) ���콺 ��ǥ
    private Vector2 mPosBegin; // �巡�� ���� ���� ���콺 ��ǥ
    private Vector2 mPosMin;   // Rect�� �ּ� ���� ��ǥ
    private Vector2 mPosMax;   // Rect�� �ִ� ���� ��ǥ
    private bool showSelection;

    private void Update()
    {
        showSelection = Input.GetMouseButton(0);
        if (!showSelection) return;

        mPosCur = Input.mousePosition;
        mPosCur.y = Screen.height - mPosCur.y; // Y ��ǥ(����) ����

        if (Input.GetMouseButtonDown(0))
        {
            mPosBegin = mPosCur;
        }

        mPosMin = Vector2.Min(mPosCur, mPosBegin);
        mPosMax = Vector2.Max(mPosCur, mPosBegin);
    }

    private void OnGUI()
    {
        if (!showSelection) return;
        Rect rect = new() { min = mPosMin, max = mPosMax };
        GUI.Box(rect, "");
    }
}

