using UnityEngine;

public class Weapon : MonoBehaviour
{
	[SerializeField]
	private	GameObject	projectilePrefab;		// 공격할 때 생성하는 발사체 프리팹
	[SerializeField]
	private	float		attackRate = 0.1f;		// 공격 속도

	private	float		lastAttackTime = 0;     // 마지막 공격시간 체크

	public void WeaponAction()
	{
		// 마지막 공격으로부터 attackRate 시간만큼 지나야 공격 가능
		if ( Time.time - lastAttackTime > attackRate )
		{
			// 현재 플레이어 위치(transform.position)에 발사체 오브젝트를 생성
			Instantiate(projectilePrefab, transform.position, Quaternion.identity);

			// 공격주기가 되어야 공격할 수 있도록 하기 위해 현재 공격시간 저장
			lastAttackTime = Time.time;
		}
	}
}

