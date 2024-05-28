using UnityEngine;

public class LevelSystem : MonoBehaviour
{
	private readonly int increaseExperience = 25;	// ���� �÷��� �� ȹ�� ����ġ

	public void Process()
	{
		int currentLevel = BackendGameData.Instance.UserGameData.level;

		// ������ �ѹ� �÷����� ������ ����ġ ȹ��
		BackendGameData.Instance.UserGameData.experience += increaseExperience;

		// ���� ����ġ�� �ִ� ����ġ���� ũ�ų� ����, ���� ������ �ִ� �������� ���� ��
		if ( BackendGameData.Instance.UserGameData.experience >= BackendChartData.levelChart[currentLevel-1].maxExperience &&
			 BackendChartData.levelChart.Count > currentLevel )
		{
			// ������ ���� ����
			BackendGameData.Instance.UserGameData.gold += BackendChartData.levelChart[currentLevel-1].rewardGold;
			// ����ġ�� 0���� �ʱ�ȭ
			BackendGameData.Instance.UserGameData.experience = 0;
			// ���� 1 ����
			BackendGameData.Instance.UserGameData.level ++;
		}

		// ���� ���� ������Ʈ
		BackendGameData.Instance.GameDataUpdate();

		Debug.Log($"���� ���� : {BackendGameData.Instance.UserGameData.level}," +
				  $"����ġ : {BackendGameData.Instance.UserGameData.experience}/" +
				  $"{BackendChartData.levelChart[currentLevel-1].maxExperience}");
	}
}

