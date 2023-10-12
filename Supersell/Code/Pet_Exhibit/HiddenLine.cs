using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenLine : MonoBehaviour
{
    public static HiddenLine HLM;
    public float speed;
    public float disSpeed;
    public Vector3 startPoint;
    public Vector3 endPoint;

    private void Start()
    {
        HLM = this;
        startPoint = transform.position;
    }

    private void FixedUpdate()
    {
        if (FootPrint.FPM.isStart && !FootPrint.FPM.gamePaused)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, endPoint, speed / disSpeed);
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            FootPrint.FPM.FailWalk();
        }
    }
}
