using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField]
	private	GameController	gameController;
	[SerializeField]
	private	StageData		stageData;			// 적 생성을 위한 스테이지 크기 정보
	[SerializeField]
	private	GameObject		enemyPrefab;		// 복제해서 생성할 적 캐릭터 프리팹
	[SerializeField]
	private	float			spawnCycleTime;     // 생성 주기

	private void Awake()
	{
		StartCoroutine(nameof(Process));
	}

	private IEnumerator Process()
	{
		int		enemyCount = 5;		// 한번에 생성하는 적 숫자
		float	distance = 1.2f;	// 생성되는 적 사이의 거리
		float	firstX = -2.4f;		// 첫 번째 적의 생성 위치 (왼쪽 끝)

		while ( true )
		{
			for ( int i = 0; i < enemyCount; ++ i )
			{
				Vector3 position = new Vector3(firstX + distance * i, stageData.LimitMax.y + 1, 0);
				GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
				enemy.GetComponent<Enemy>().Setup(gameController);
			}
			// spawnCycleTime 시간동안 대기
			yield return new WaitForSeconds(spawnCycleTime);
		}
	}
}

