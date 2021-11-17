using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    float moveX, moveY;
    [Header("Speed Control")]
    [SerializeField] [Range(1f, 300f)] float moveSpeed = 150f;
    [Header("Control Area")]
    [SerializeField] Vector2 minPos, maxPos;

     void FixedUpdate()
    {      
        //ĳ���� ������
        moveX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        moveY = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;

        transform.position = new Vector2(transform.position.x + moveX, transform.position.y + moveY);      
        
        //Mathf.Clamp(���簪, �ִ밪, �ּҰ�);  ���簪�� �ִ밪������ ��ȯ���ְ� �ּҰ����� ������ �� �ּҰ������� ��ȯ�մϴ�.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPos.x, maxPos.x),
                                         Mathf.Clamp(transform.position.y, minPos.y, maxPos.y),
                                         Mathf.Clamp(transform.position.z, transform.position.z, transform.position.z));      
    }
}
