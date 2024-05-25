using UnityEngine;

public class Movement2D : MonoBehaviour
{
	[SerializeField]
	private	float	moveSpeed = 0;					// 이동 속도
	[SerializeField]
	private	Vector3	moveDirection = Vector3.zero;	// 이동 방향
	
	public	Vector3	MoveDirection { set => moveDirection = value; }

	private void Update()
	{
		transform.position += moveDirection * moveSpeed * Time.deltaTime;
	}
}

