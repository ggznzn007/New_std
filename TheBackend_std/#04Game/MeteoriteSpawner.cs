using System.Collections;
using UnityEngine;

public class MeteoriteSpawner : MonoBehaviour
{
	[SerializeField]
	private	GameController	gameController;
	[SerializeField]
	private	StageData		stageData;				// ���/� ������ ���� �������� ũ�� ����
	[SerializeField]
	private	GameObject		alertLinePrefab;		// �����ؼ� ������ ��� ������
	[SerializeField]
	private	GameObject		meteoritePrefab;		// �����ؼ� ������ � ������
	[SerializeField]
	private	float			minSpawnCycleTime = 1;	// �ּ� ���� �ֱ�
	[SerializeField]
	private	float			maxSpawnCycleTime = 4;  // �ִ� ���� �ֱ�

	private void Awake()
	{
		StartCoroutine(nameof(Process));
	}

	private IEnumerator Process()
	{
		while ( true )
		{
			// ��� �ð� ���� (minSpawnCycleTime ~ maxSpawnCycleTime)
			float spawnCycleTime = Random.Range(minSpawnCycleTime, maxSpawnCycleTime);
			// spawnCycleTime �ð����� ���
			yield return new WaitForSeconds(spawnCycleTime);

			// ���/��� �����Ǵ� x ��ġ�� �������� ũ�� ���� ������ ������ ���� ����
			float x = Random.Range(stageData.LimitMin.x, stageData.LimitMax.x);

			// ��� ������Ʈ ����
			GameObject alertLineClone = Instantiate(alertLinePrefab, new Vector3(x, 0, 0), Quaternion.identity);
			
			// 1�� ��� ��
			yield return new WaitForSeconds(1);
			
			// ��� ������Ʈ ����
			Destroy(alertLineClone);

			// � ������Ʈ ���� (y ��ġ�� �������� ��� ��ġ + 1)
			GameObject meteorite = Instantiate(meteoritePrefab, new Vector3(x, stageData.LimitMax.y + 1, 0), Quaternion.identity);
			meteorite.GetComponent<Meteorite>().Setup(gameController);
		}
	}
}

