using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private	StageData	stageData;

	private	Movement2D	movement2D;
	private	Weapon		weapon;

	private void Awake()
	{
		movement2D	= GetComponent<Movement2D>();
		weapon		= GetComponent<Weapon>();
	}

	private void Update()
	{
		// 방향 키 or wasd키를 눌러 이동방향 설정
		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");

		movement2D.MoveDirection = new Vector3(x, y, 0);

		// 공격 주기를 계산하고, 공격이 가능하면 공격
		weapon.WeaponAction();
	}

	private void LateUpdate()
	{
		// 플레이어 캐릭터가 스테이지 범위 바깥으로 나가지 못하도록 제어
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
										 Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y));
	}
}

