using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLineMove2 : MonoBehaviour
{
    public static DeadLineMove2 DLM2;
    public float speed;
    public float disSpeed;
    public Vector3 startPoint;
    public Vector3 endPoint;

    private void Start()
    {
        DLM2 = this;
        startPoint = transform.position;
    }

    private void FixedUpdate()
    {
        if (FootPrint.FPM.isStart && !FootPrint.FPM.gamePaused)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, endPoint, speed / disSpeed);
        }
    }
}
