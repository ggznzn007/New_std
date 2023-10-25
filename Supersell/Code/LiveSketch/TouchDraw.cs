using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDraw : MonoBehaviour
{
    Coroutine drawing;
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartLine();
            Debug.Log("클릭시작");
        }
        if(Input.GetMouseButtonUp(0))
        {
            FinishLine();
            Debug.Log("클릭끝");
        }
    }

    void StartLine()
    {
        if(drawing != null)
        {
            StopCoroutine(drawing);
        }
        drawing = StartCoroutine(DrawLine());
    }

    void FinishLine()
    {
        StopCoroutine(drawing);
    }

    IEnumerator DrawLine()
    {
        GameObject newDraw = Instantiate(Resources.Load("Line")as GameObject, new Vector3(0,0,0),Quaternion.identity);
        LineRenderer line = newDraw.GetComponent<LineRenderer>();
        line.positionCount = 0;

        while(true)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            line.positionCount++;
            line.SetPosition(line.positionCount-1, position);
            yield return null;
        }
    }
}
