using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System;

public class BackendGuildSystem : MonoBehaviour
{
	[SerializeField]
	private	FadeEffect_TMP		textLog;
	[SerializeField]
	private	GuildCreatePage		guildCreatePage;
	[SerializeField]
	private	GuildApplicantsPage	guildApplicantsPage;

	public void CreateGuild(string guildName, int goodsCount=1)
	{
		Backend.Guild.CreateGuildV3(guildName, goodsCount, callback =>
		{
			if ( !callback.IsSuccess() )
			{
				ErrorLogCreateGuild(callback);

				return;
			}

			Debug.Log($"��尡 �����Ǿ����ϴ�. : {callback}");

			// ��� ������ �������� �� ȣ��
			guildCreatePage.SuccessCreateGuild();
		});
	}

	public void ApplyGuild(string guildName)
	{
		// GetGuildIndateByGuildNameV3() �޼ҵ带 ȣ���� ���ϴ� ���(guildName)�� guildInDate ���� ��ȯ
		string guildInDate = GetGuildInfoBy(guildName);

		// guildInDate ������ ���� ��忡 ���� ��û�� ������.
		Backend.Guild.ApplyGuildV3(guildInDate, callback =>
		{
			if ( !callback.IsSuccess() )
			{
				ErrorLogApplyGuild(callback);

				return;
			}

			Debug.Log($"��� ���� ��û�� �����߽��ϴ�. : {callback}");
		});
	}

	public void GetApplicants()
	{
		Backend.Guild.GetApplicantsV3(callback =>
		{
			if ( !callback.IsSuccess() )
			{
				// ���� ������ 403 �ϳ� �ۿ� ���� ������ ������ �޼ҵ� ���� X
				ErrorLog(callback.GetMessage(), "Guild_Failed_Log", "GetApplicants");

				return;
			}

			// JSON ������ �Ľ� ����
			try
			{
				LitJson.JsonData jsonData = callback.GetFlattenJSON()["rows"];

				if ( jsonData.Count <= 0 )
				{
					Debug.LogWarning("��� ���� ��û ����� ����ֽ��ϴ�.");
					return;
				}

				// ��� ���� ��û ��Ͽ� �ִ� ��� UI ��Ȱ��ȭ
				guildApplicantsPage.DeactivateAll();

				List<TransactionValue>	transactionList		= new List<TransactionValue>();
				List<GuildMemberData>	guildMemberDataList	= new List<GuildMemberData>();

				foreach ( LitJson.JsonData item in jsonData )
				{
					GuildMemberData	guildMember	= new GuildMemberData();

					guildMember.nickname = item["nickname"].ToString().Equals("True") ? "NONAME" : item["nickname"].ToString();
					guildMember.inDate	 = item["inDate"].ToString();

					guildMemberDataList.Add(guildMember);

					// guildMember.inDate�� ������ ģ���� UserGameData ���� �ҷ�����
					Where where = new Where();
					where.Equal("owner_inDate", guildMember.inDate);
					transactionList.Add(TransactionValue.SetGet(Constants.USER_DATA_TABLE, where));
				}

				Backend.GameData.TransactionReadV2(transactionList, callback =>
				{
					if ( !callback.IsSuccess() )
					{
						ErrorLog(callback.GetMessage(), "Guild_Failed_Log", "GetApplicants-TransactionReadV2");
						return;
					}

					LitJson.JsonData userData = callback.GetFlattenJSON()["Responses"];

					if ( userData.Count <= 0 )
					{
						Debug.LogWarning($"�����Ͱ� �������� �ʽ��ϴ�.");
						return;
					}

					for ( int i = 0; i < userData.Count; ++ i )
					{
						guildMemberDataList[i].level = userData[i]["level"].ToString();
						guildApplicantsPage.Activate(guildMemberDataList[i]);
						Debug.Log(guildMemberDataList[i].ToString());
					}
				});
			}
			// JSON ������ �Ľ� ����
			catch ( Exception e )
			{
				// try-catch ���� ���
				Debug.LogError(e);
			}
		});
	}

	public void ApproveApplicant(string gamerInDate)
	{
		Backend.Guild.ApproveApplicantV3(gamerInDate, callback =>
		{
			if ( !callback.IsSuccess() )
			{
				ErrorLogApproveApplicant(callback);

				return;
			}

			Debug.Log($"��� ���� ��û ������ �����߽��ϴ�. : {callback}");
		});
	}

	public void RejectApplicant(string gamerInDate)
	{
		Backend.Guild.RejectApplicantV3(gamerInDate, callback =>
		{
			if ( !callback.IsSuccess() )
			{
				// ������Ը� �ش� ��ư�� ���� ��������
				// ���� ������ 404 �ϳ� �ۿ� ���� ������ ������ �޼ҵ� ���� X
				ErrorLog(callback.GetMessage(), "Guild_Failed_Log", "RejectApplicant");

				return;
			}

			Debug.Log($"��� ���� ��û ������ �����߽��ϴ�. : {callback}");
		});
	}

	public string GetGuildInfoBy(string guildName)
	{
		// �ش� ����(guildName)�� ��尡 �����ϴ��� ���δ� ����� ����
		var		bro		= Backend.Guild.GetGuildIndateByGuildNameV3(guildName);
		string	inDate	= string.Empty;

		if ( !bro.IsSuccess() )
		{
			Debug.LogError($"��� �˻� ���� ������ �߻��߽��ϴ�. : {bro}");
			return inDate;
		}

		try
		{
			inDate = bro.GetFlattenJSON()["guildInDate"].ToString();

			Debug.Log($"{guildName}�� inDate ���� {inDate} �Դϴ�.");
		}
		catch ( Exception e )
		{
			Debug.LogError(e);
		}

		return inDate;
	}

	private void ErrorLogCreateGuild(BackendReturnObject callback)
	{
		string message = string.Empty;

		switch ( int.Parse(callback.GetStatusCode()) )
		{
			case 403:	// Backend Console�� ������ ������ �������� ������ ��
				message = "��� ������ ���� ������ �����մϴ�.";
				break;
			case 409:	// �ߺ��� �������� ���� �õ��� ���
				message = "�̹� ������ �̸��� ��尡 �����մϴ�.";
				break;
			default:
				message = callback.GetMessage();
				break;
		}

		ErrorLog(message, "Guild_Failed_Log", "ApplyGuild");
	}

	private void ErrorLogApplyGuild(BackendReturnObject callback)
	{
		string message = string.Empty;

		switch ( int.Parse(callback.GetStatusCode()) )
		{
			case 403:	// Backend Console�� ������ ������ �������� ������ ��
				message = "��� ������ ���� ������ �����մϴ�.";
				break;
			case 409:
				message = "�̹� ���� ��û�� ����Դϴ�.";
				break;
			case 412:
				message = "�̹� �ٸ� ��忡 �ҼӵǾ� �ֽ��ϴ�.";
				break;
			case 429:
				message = "��忡 �� �̻� �ڸ��� �����ϴ�.";
				break;
		}

		ErrorLog(message, "Guild_Failed_Log", "ApplyGuild");
	}

	private void ErrorLogApproveApplicant(BackendReturnObject callback)
	{
		string message = string.Empty;

		switch ( int.Parse(callback.GetStatusCode()) )
		{
			case 412:
				message = "��� ���� ��û�� �����Ϸ��� ������ �̹� �ٸ� ��� �Ҽ��Դϴ�.";
				break;
			case 429:
				message = "��忡 �� �̻� �ڸ��� �����ϴ�.";
				break;
		}

		ErrorLog(message, "Guild_Failed_Log", "ApproveApplicant");
	}

	private void ErrorLog(string message, string behaviorType="", string paramKey="")
	{
		// ���� ������ Console View�� ���
		Debug.LogError($"{paramKey} : {message}");

		// ���� ������ UI�� ���
		textLog.FadeOut(message);

		// ���� ������ Backend Console�� ����
		Param param = new Param() { { paramKey, message } };
		// InsertLogV2(�ൿ ����, Key&Value)
		Backend.GameLog.InsertLogV2(behaviorType, param);
	}
}


// Backend.GameLog.InsertLogV2()�� 5.9.6 �̻� �������� ��� ����