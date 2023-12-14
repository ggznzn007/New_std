using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallonCtrl : MonoBehaviour
{
    public float speed;
    public float disSpeed;
    public Transform[] endPoint;
    public Transform currentEndPoint;
    int rand;

    private void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.up;        
        endPoint = GameObject.Find("EndPoints").GetComponentsInChildren<Transform>();
        rand = RanddomInt();
        currentEndPoint = endPoint[rand];
    }

    public int RanddomInt()
    {
        int rand = Random.Range(0, endPoint.Length);
        return rand;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentEndPoint.position, speed / disSpeed); // 위로 이동
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Finish"))        // 풍선이 최종지점에 닿으면 풍선 터뜨리기
        {
            Destroy(gameObject, 0.05f);
            Manager_Ballon.MB.ballCount--;
        }
    }

}
