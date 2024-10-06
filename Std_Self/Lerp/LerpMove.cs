using System.Collections;
using UnityEngine;

public class LerpMove : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    [SerializeField] private float moveTime = 3;
    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private bool isRight = false;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(!isRight)
            {
                StartCoroutine(MoveToRight());
            }
            else
            {
                StartCoroutine (MoveToLeft());
            }            
        }
    }

    private IEnumerator MoveToRight()
    {
        transform.position = start.position;

        float current = 0;
        float percent = 0;

        while(percent<1)
        {
            current += Time.deltaTime;
            percent = current / moveTime;
            transform.position = Vector3.Lerp(start.position, end.position, moveCurve.Evaluate(percent));

            yield return null;
            if (transform.position == end.position)
            {
                isRight = true;
            }           
        }

        Debug.Log("Right Goal!");
    }

    private IEnumerator MoveToLeft()
    {
        transform.position = end.position;

        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTime;
            transform.position = Vector3.Lerp(end.position, start.position, moveCurve.Evaluate(percent));

            yield return null;
            if(transform.position==start.position)
            {
                isRight = false;
            }           
        }
        Debug.Log("Left Goal!");
    }
}
