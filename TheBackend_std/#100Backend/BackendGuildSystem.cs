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

			Debug.Log($"길드가 생성되었습니다. : {callback}");

			// 길드 생성에 성공했을 때 호출
			guildCreatePage.SuccessCreateGuild();
		});
	}

	public void ApplyGuild(string guildName)
	{
		// GetGuildIndateByGuildNameV3() 메소드를 호출해 원하는 길드(guildName)의 guildInDate 정보 반환
		string guildInDate = GetGuildInfoBy(guildName);

		// guildInDate 정보를 가진 길드에 가입 요청을 보낸다.
		Backend.Guild.ApplyGuildV3(guildInDate, callback =>
		{
			if ( !callback.IsSuccess() )
			{
				ErrorLogApplyGuild(callback);

				return;
			}

			Debug.Log($"길드 가입 요청에 성공했습니다. : {callback}");
		});
	}

	public void GetApplicants()
	{
		Backend.Guild.GetApplicantsV3(callback =>
		{
			if ( !callback.IsSuccess() )
			{
				// 실패 사유가 403 하나 밖에 없기 때문에 별도로 메소드 제작 X
				ErrorLog(callback.GetMessage(), "Guild_Failed_Log", "GetApplicants");

				return;
			}

			// JSON 데이터 파싱 성공
			try
			{
				LitJson.JsonData jsonData = callback.GetFlattenJSON()["rows"];

				if ( jsonData.Count <= 0 )
				{
					Debug.LogWarning("길드 가입 요청 목록이 비어있습니다.");
					return;
				}

				// 길드 가입 요청 목록에 있는 모든 UI 비활성화
				guildApplicantsPage.DeactivateAll();

				List<TransactionValue>	transactionList		= new List<TransactionValue>();
				List<GuildMemberData>	guildMemberDataList	= new List<GuildMemberData>();

				foreach ( LitJson.JsonData item in jsonData )
				{
					GuildMemberData	guildMember	= new GuildMemberData();

					guildMember.nickname = item["nickname"].ToString().Equals("True") ? "NONAME" : item["nickname"].ToString();
					guildMember.inDate	 = item["inDate"].ToString();

					guildMemberDataList.Add(guildMember);

					// guildMember.inDate를 가지는 친구의 UserGameData 정보 불러오기
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
						Debug.LogWarning($"데이터가 존재하지 않습니다.");
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
			// JSON 데이터 파싱 실패
			catch ( Exception e )
			{
				// try-catch 에러 출력
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

			Debug.Log($"길드 가입 요청 수락에 성공했습니다. : {callback}");
		});
	}

	public void RejectApplicant(string gamerInDate)
	{
		Backend.Guild.RejectApplicantV3(gamerInDate, callback =>
		{
			if ( !callback.IsSuccess() )
			{
				// 운영진에게만 해당 버튼이 보일 예정으로
				// 실패 사유가 404 하나 밖에 없기 때문에 별도로 메소드 제작 X
				ErrorLog(callback.GetMessage(), "Guild_Failed_Log", "RejectApplicant");

				return;
			}

			Debug.Log($"길드 가입 요청 거절에 성공했습니다. : {callback}");
		});
	}

	public string GetGuildInfoBy(string guildName)
	{
		// 해당 길드명(guildName)의 길드가 존재하는지 여부는 동기로 진행
		var		bro		= Backend.Guild.GetGuildIndateByGuildNameV3(guildName);
		string	inDate	= string.Empty;

		if ( !bro.IsSuccess() )
		{
			Debug.LogError($"길드 검색 도중 에러가 발생했습니다. : {bro}");
			return inDate;
		}

		try
		{
			inDate = bro.GetFlattenJSON()["guildInDate"].ToString();

			Debug.Log($"{guildName}의 inDate 값은 {inDate} 입니다.");
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
			case 403:	// Backend Console에 설정한 조건을 만족하지 못했을 때
				message = "길드 생성을 위한 레벨이 부족합니다.";
				break;
			case 409:	// 중복된 길드명으로 생성 시도한 경우
				message = "이미 동일한 이름의 길드가 존재합니다.";
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
			case 403:	// Backend Console에 설정한 조건을 만족하지 못했을 때
				message = "길드 가입을 위한 레벨이 부족합니다.";
				break;
			case 409:
				message = "이미 가입 요청한 길드입니다.";
				break;
			case 412:
				message = "이미 다른 길드에 소속되어 있습니다.";
				break;
			case 429:
				message = "길드에 더 이상 자리가 없습니다.";
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
				message = "길드 가입 요청을 수락하려는 유저가 이미 다른 길드 소속입니다.";
				break;
			case 429:
				message = "길드에 더 이상 자리가 없습니다.";
				break;
		}

		ErrorLog(message, "Guild_Failed_Log", "ApproveApplicant");
	}

	private void ErrorLog(string message, string behaviorType="", string paramKey="")
	{
		// 에러 내용을 Console View에 출력
		Debug.LogError($"{paramKey} : {message}");

		// 에러 내용을 UI로 출력
		textLog.FadeOut(message);

		// 에러 내용을 Backend Console에 저장
		Param param = new Param() { { paramKey, message } };
		// InsertLogV2(행동 유형, Key&Value)
		Backend.GameLog.InsertLogV2(behaviorType, param);
	}
}


// Backend.GameLog.InsertLogV2()는 5.9.6 이상 버전에서 사용 가능