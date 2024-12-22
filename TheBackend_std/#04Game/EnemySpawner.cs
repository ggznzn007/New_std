using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField]
	private	GameController	gameController;
	[SerializeField]
	private	StageData		stageData;			// �� ������ ���� �������� ũ�� ����
	[SerializeField]
	private	GameObject		enemyPrefab;		// �����ؼ� ������ �� ĳ���� ������
	[SerializeField]
	private	float			spawnCycleTime;     // ���� �ֱ�

	private void Awake()
	{
		StartCoroutine(nameof(Process));
	}

	private IEnumerator Process()
	{
		int		enemyCount = 5;		// �ѹ��� �����ϴ� �� ����
		float	distance = 1.2f;	// �����Ǵ� �� ������ �Ÿ�
		float	firstX = -2.4f;		// ù ��° ���� ���� ��ġ (���� ��)

		while ( true )
		{
			for ( int i = 0; i < enemyCount; ++ i )
			{
				Vector3 position = new Vector3(firstX + distance * i, stageData.LimitMax.y + 1, 0);
				GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
				enemy.GetComponent<Enemy>().Setup(gameController);
			}
			// spawnCycleTime �ð����� ���
			yield return new WaitForSeconds(spawnCycleTime);
		}
	}
}

