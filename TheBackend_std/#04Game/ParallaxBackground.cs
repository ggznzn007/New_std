using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
	[SerializeField]
	private	Transform	target;							// 현재 배경과 이어지는 배경
	[SerializeField]
	private	float		scrollAmount = 14.08f;			// 두 배경 사이의 y 거리
	[SerializeField]
	private	float		moveSpeed = 3;					// 배경의 이동 속도
	[SerializeField]
	private	Vector3		moveDirection = Vector3.down;	// 배경의 이동 방향

	private void Update()
	{
		// 배경이 moveDirection 방향으로 moveSpeed의 속도로 이동
		transform.position += moveDirection * moveSpeed * Time.deltaTime;

		// 배경이 설정된 범위를 벗어나면 위치 재설정
		if ( transform.position.y <= -scrollAmount )
		{
			transform.position = target.position - moveDirection * scrollAmount;
		}
	}
}

