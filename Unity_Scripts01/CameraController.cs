using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] float smoothTimeX, smoothTimeY;   // 카메라 움직임을 부드럽게 하기위한 변수
	[Header("Now Position")]
	[SerializeField] Vector2 velocity;                 // 속도
	[SerializeField] GameObject player;                // 플레이어
	[Header("Control Area")] 
	[SerializeField] Vector2 minPos, maxPos;           // 카메라 이동제한 위치값
	

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	// 캐릭터의 위에 따라 카메라가 이동하도록 하는 메서드
	void FixedUpdate()
	{
		float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
		// Mathf.SmoothDamp는 천천히 값을 증가시키는 메서드이다.
		float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);
		// 카메로 이동
		transform.position = new Vector3(posX, posY, transform.position.z);
		//Mathf.Clamp(현재값, 최대값, 최소값);  현재값이 최대값까지만 반환해주고 최소값보다 작으면 그 최소값까지만 반환합니다.
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPos.x, maxPos.x),
		Mathf.Clamp(transform.position.y, minPos.y, maxPos.y),
		Mathf.Clamp(transform.position.z, transform.position.z, transform.position.z));		
	}
}
