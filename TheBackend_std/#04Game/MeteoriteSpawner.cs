using System.Collections;
using UnityEngine;

public class MeteoriteSpawner : MonoBehaviour
{
	[SerializeField]
	private	GameController	gameController;
	[SerializeField]
	private	StageData		stageData;				// 경고선/운석 생성을 위한 스테이지 크기 정보
	[SerializeField]
	private	GameObject		alertLinePrefab;		// 복제해서 생성할 경고선 프리팹
	[SerializeField]
	private	GameObject		meteoritePrefab;		// 복제해서 생성할 운석 프리팹
	[SerializeField]
	private	float			minSpawnCycleTime = 1;	// 최소 생성 주기
	[SerializeField]
	private	float			maxSpawnCycleTime = 4;  // 최대 생성 주기

	private void Awake()
	{
		StartCoroutine(nameof(Process));
	}

	private IEnumerator Process()
	{
		while ( true )
		{
			// 대기 시간 설정 (minSpawnCycleTime ~ maxSpawnCycleTime)
			float spawnCycleTime = Random.Range(minSpawnCycleTime, maxSpawnCycleTime);
			// spawnCycleTime 시간동안 대기
			yield return new WaitForSeconds(spawnCycleTime);

			// 경고선/운석이 생성되는 x 위치는 스테이지 크기 범위 내에서 임의의 값을 선택
			float x = Random.Range(stageData.LimitMin.x, stageData.LimitMax.x);

			// 경고선 오브젝트 생성
			GameObject alertLineClone = Instantiate(alertLinePrefab, new Vector3(x, 0, 0), Quaternion.identity);
			
			// 1초 대기 후
			yield return new WaitForSeconds(1);
			
			// 경고선 오브젝트 삭제
			Destroy(alertLineClone);

			// 운석 오브젝트 생성 (y 위치는 스테이지 상단 위치 + 1)
			GameObject meteorite = Instantiate(meteoritePrefab, new Vector3(x, stageData.LimitMax.y + 1, 0), Quaternion.identity);
			meteorite.GetComponent<Meteorite>().Setup(gameController);
		}
	}
}

