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
		// ���� Ű or wasdŰ�� ���� �̵����� ����
		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");

		movement2D.MoveDirection = new Vector3(x, y, 0);

		// ���� �ֱ⸦ ����ϰ�, ������ �����ϸ� ����
		weapon.WeaponAction();
	}

	private void LateUpdate()
	{
		// �÷��̾� ĳ���Ͱ� �������� ���� �ٱ����� ������ ���ϵ��� ����
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
										 Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y));
	}
}

