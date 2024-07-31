using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea : MonoBehaviour
{
    public static CameraArea instance;

    [SerializeField] float smoothTimeX, smoothTimeY;

    [Header("Now Position")]
    [SerializeField] Vector2 velocity;
    [SerializeField] GameObject player;
    [Header("Control Area")]
    [SerializeField] Vector2 minPos, maxPos;
   
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (!player)
        {
            this.gameObject.SetActive(false);
        }
    }    

    private void FixedUpdate()
    {
        CameraMoving();
    }

    public void CameraMoving() // 캐릭터의 위에 따라 카메라가 이동하도록 하는 메서드
    {
        // Mathf.SmoothDamp는 천천히 값을 증가시키는 메서드이다.
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);
        // 카메라 이동
        transform.position = new Vector3(posX, posY, transform.position.z);
        //Mathf.Clamp(현재 값, 최대값, 최소값);  현재 값이 최대값까지만 반환해주고 최소값보다 작으면 그 최소값까지만 반환합니다.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPos.x, maxPos.x),
        Mathf.Clamp(transform.position.y, minPos.y, maxPos.y),
        Mathf.Clamp(transform.position.z, transform.position.z, transform.position.z));
    }
}

