using BackEnd;
using UnityEngine;

public class DailyRankRegister : MonoBehaviour
{
    public void Process(int newScore)
    {
        //UpdateMyRankData(newScore);
        UpdateMyBestRankData(newScore);
    }

    private void UpdateMyRankData(int newScore)
    {
        string rowInDate = string.Empty;

        // ��ŷ �����͸� ������Ʈ�Ϸ��� ���� �����Ϳ��� ����ϴ� �������� inDate ���� �ʿ�
        Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where(), callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"������ ��ȸ �� ������ �߻��߽��ϴ� : {callback}");
                return;
            }

            Debug.Log($"������ ��ȸ�� �����߽��ϴ� : {callback}");

            if (callback.FlattenRows().Count > 0)
            {
                rowInDate = callback.FlattenRows()[0]["inDate"].ToString();
            }

            else
            {
                Debug.LogError("�����Ͱ� �������� �ʽ��ϴ�.");
                return;
            }

            Param param = new Param()
            {
                { "dailyBestScore", newScore }
            };

            Backend.URank.User.UpdateUserScore(Constants.DAILY_RANK_UUID, Constants.USER_DATA_TABLE, rowInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"��ŷ ��Ͽ� �����߽��ϴ� : {callback}");
                }
                else
                {
                    Debug.LogError($"��ŷ ��� �� ������ �߻��߽��ϴ� : {callback}");
                }
            });
        });
    }

    private void UpdateMyBestRankData(int newScore)
    {
        Backend.URank.User.GetMyRank(Constants.DAILY_RANK_UUID, callback =>
        {
            if (callback.IsSuccess())
            {
                // JSON ������ �Ľ� ����
                try
                {
                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    // �޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
                    if (rankDataJson.Count <= 0)
                    {
                        Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
                    }
                    else
                    {
                        // ��ŷ�� ����� ���� �÷����� "dailyBestScore"�� ����������
                        // ��ŷ�� �ҷ��� ���� �÷����� "score"�� ���ϵǾ� �ִ�.

                        // �߰��� ����� �׸��� �÷����� �״�� ���
                        int bestScore = int.Parse(rankDataJson[0]["score"].ToString());

                        // ���� ������ �ְ� �������� ������
                        if (newScore > bestScore)
                        {
                            // ���� ������ ���ο� �ְ� ������ �����ϰ�, ��ŷ�� ���
                            UpdateMyRankData(newScore);

                            Debug.Log($"�ְ� ���� ���� {bestScore} -> {newScore}");
                        }
                    }
                }
                // JSON ������ �Ľ� ����
                catch (System.Exception e)
                {
                    // try-catch ���� ���
                    Debug.LogError(e);
                }
            }
            else
            {
                // �ڽ��� ��ŷ ������ �������� ���� ���� ���� ������ ���ο� ��ŷ���� ���
                if (callback.GetMessage().Contains("userRank"))
                {
                    UpdateMyRankData(newScore);

                    Debug.Log($"���ο� ��ŷ ������ ���� �� ��� : {callback}");
                }
            }
        });
    }
}

