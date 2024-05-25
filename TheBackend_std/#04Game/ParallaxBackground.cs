using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
	[SerializeField]
	private	Transform	target;							// ���� ���� �̾����� ���
	[SerializeField]
	private	float		scrollAmount = 14.08f;			// �� ��� ������ y �Ÿ�
	[SerializeField]
	private	float		moveSpeed = 3;					// ����� �̵� �ӵ�
	[SerializeField]
	private	Vector3		moveDirection = Vector3.down;	// ����� �̵� ����

	private void Update()
	{
		// ����� moveDirection �������� moveSpeed�� �ӵ��� �̵�
		transform.position += moveDirection * moveSpeed * Time.deltaTime;

		// ����� ������ ������ ����� ��ġ �缳��
		if ( transform.position.y <= -scrollAmount )
		{
			transform.position = target.position - moveDirection * scrollAmount;
		}
	}
}

