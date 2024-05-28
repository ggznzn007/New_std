using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public static class BackendChartData
{
	// ������ ������ �ʿ� ����ġ�� ���� ����
	public	static	List<LevelChartData>	levelChart;

	static BackendChartData()
	{
		levelChart = new List<LevelChartData>();
	}

	public static void LoadAllChart()
	{
		LoadLevelChart();
	}

	public static void LoadLevelChart()
	{
		Backend.Chart.GetChartContents(Constants.LEVEL_CHART, callback =>
		{
			if ( callback.IsSuccess() )
			{
				// JSON ������ �Ľ� ����
				try
				{
					LitJson.JsonData jsonData = callback.FlattenRows();

					// �޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
					if ( jsonData.Count <= 0 )
					{
						Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
					}
					else
					{
						for ( int i = 0; i < jsonData.Count; ++ i )
						{
							LevelChartData newChart	= new LevelChartData();
							newChart.level			= int.Parse(jsonData[i]["level"].ToString());
							newChart.maxExperience	= int.Parse(jsonData[i]["maxExperience"].ToString());
							newChart.rewardGold		= int.Parse(jsonData[i]["rewardGold"].ToString());

							levelChart.Add(newChart);

							Debug.Log($"Level : {newChart.level}, Max Exp : {newChart.maxExperience}," +
									  $"Reward Gold : {newChart.rewardGold}");
						}
					}
				}
				// JSON ������ �Ľ� ����
				catch ( System.Exception e )
				{
					// try-catch ���� ���
					Debug.LogError(e);
				}
			}
			else
			{
				Debug.LogError($"{Constants.LEVEL_CHART}�� ��Ʈ �ҷ����⿡ ���� �߻� : {callback}");
			}
		});
	}
}

[System.Serializable]
public class LevelChartData
{
	public	int		level;
	public	int		maxExperience;
	public	int		rewardGold;
}

