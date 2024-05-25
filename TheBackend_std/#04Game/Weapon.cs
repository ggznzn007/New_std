using UnityEngine;

public class Weapon : MonoBehaviour
{
	[SerializeField]
	private	GameObject	projectilePrefab;		// ������ �� �����ϴ� �߻�ü ������
	[SerializeField]
	private	float		attackRate = 0.1f;		// ���� �ӵ�

	private	float		lastAttackTime = 0;     // ������ ���ݽð� üũ

	public void WeaponAction()
	{
		// ������ �������κ��� attackRate �ð���ŭ ������ ���� ����
		if ( Time.time - lastAttackTime > attackRate )
		{
			// ���� �÷��̾� ��ġ(transform.position)�� �߻�ü ������Ʈ�� ����
			Instantiate(projectilePrefab, transform.position, Quaternion.identity);

			// �����ֱⰡ �Ǿ�� ������ �� �ֵ��� �ϱ� ���� ���� ���ݽð� ����
			lastAttackTime = Time.time;
		}
	}
}

