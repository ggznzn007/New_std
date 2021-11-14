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
        //캐릭터 움직임
        moveX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        moveY = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;

        transform.position = new Vector2(transform.position.x + moveX, transform.position.y + moveY);      
        
        //Mathf.Clamp(현재값, 최대값, 최소값);  현재값이 최대값까지만 반환해주고 최소값보다 작으면 그 최소값까지만 반환합니다.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPos.x, maxPos.x),
                                         Mathf.Clamp(transform.position.y, minPos.y, maxPos.y),
                                         Mathf.Clamp(transform.position.z, transform.position.z, transform.position.z));      
    }
}
