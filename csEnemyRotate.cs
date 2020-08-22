using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csEnemyRotate : MonoBehaviour
{
    float accTime = 0f;
    bool isRotate = false;
    public float speed = 1000f;
    public float maxTime = 1f;
    void Update()
    {
        // 1초 후에 회전 중지
        if (accTime > maxTime)
            isRotate = false;
        // isRotate가 true면 회전을 하되 진행시간을 누적하라
        if (isRotate)
        {
            accTime += Time.deltaTime;
            transform.Rotate(Vector3.up * speed * Time.deltaTime);
        }
    }
    /// <summary>
    /// Enemy를 클릭했을 때 외부에서 호출하는 메서드
    /// 진행시간 0
    /// 회전시작 true
    /// </summary>
    public void RotateByHit()
    {
        accTime = 0f;
        isRotate = true;
    }
}
