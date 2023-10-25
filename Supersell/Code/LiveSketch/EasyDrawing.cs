using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyDrawing : MonoBehaviour
{
    public Camera d_Cam;
    public GameObject brush;

    LineRenderer curLine;

    Vector2 lastPos;

    private void Update()
    {
        Drawing();
    }

    void Drawing()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreateBrush();
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector2 mousePos = d_Cam.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos != lastPos)
            {
                AddAPoint(mousePos);
                lastPos = mousePos;
            }
        }
        else
        {
            curLine = null;
        }
    }

    void CreateBrush()
    {
        GameObject brushInstance = Instantiate(brush);
        curLine = brushInstance.GetComponent<LineRenderer>();

        Vector2 mousePos = d_Cam.ScreenToWorldPoint(Input.mousePosition);

        curLine.SetPosition(0, mousePos);
        curLine.SetPosition(1, mousePos);
    }

    void AddAPoint(Vector2 pointPos)
    {
        curLine.positionCount++;
        int positionIndex = curLine.positionCount - 1;
        curLine.SetPosition(positionIndex, pointPos);
    }
}
