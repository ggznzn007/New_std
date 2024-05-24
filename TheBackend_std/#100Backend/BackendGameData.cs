using UnityEngine;
using BackEnd;

public class BackendGameData
{
	[System.Serializable]
	public class GameDataLoadEvent : UnityEngine.Events.UnityEvent { }
	public GameDataLoadEvent onGameDataLoadEvent = new GameDataLoadEvent();

	private	static	BackendGameData	instance = null;
	public	static	BackendGameData	Instance
	{
		get
		{
			if ( instance == null )
			{
				instance = new BackendGameData();
			}

			return instance;
		}
	}

	private	UserGameData userGameData = new UserGameData();
	public	UserGameData UserGameData => userGameData;

	private	string gameDataRowInDate = string.Empty;

	/// <summary>
	/// 뒤끝 콘솔 테이블에 새로운 유저 정보 추가
	/// </summary>
	public void GameDataInsert()
	{
		// 유저 정보를 초기값으로 설정
		userGameData.Reset();
		
		// 테이블에 추가할 데이터로 가공
		Param param = new Param()
		{
			{ "level",		userGameData.level },
			{ "experience",	userGameData.experience },
			{ "gold",		userGameData.gold },
			{ "jewel",		userGameData.jewel },
			{ "heart",		userGameData.heart }
		};

		// 첫 번째 매개변수는 뒤끝 콘솔의 "게임 정보 관리" 탭에 생성한 테이블 이름
		Backend.GameData.Insert("USER_DATA", param, callback =>
		{
			// 게임 정보 추가에 성공했을 때
			if ( callback.IsSuccess() )
			{
				// 게임 정보의 고유값
				gameDataRowInDate = callback.GetInDate();

				Debug.Log($"게임 정보 데이터 삽입에 성공했습니다. : {callback}");
			}
			// 실패했을 때
			else
			{
				Debug.LogError($"게임 정보 데이터 삽입에 실패했습니다. : {callback}");
			}
		});
	}

	/// <summary>
	/// 뒤끝 콘솔 테이블에서 유저 정보를 불러올 때 호출
	/// </summary>
	public void GameDataLoad()
	{
		Backend.GameData.GetMyData("USER_DATA", new Where(), callback =>
		{
			// 게임 정보 불러오기에 성공했을 때
			if ( callback.IsSuccess() )
			{
				Debug.Log($"게임 정보 데이터 불러오기에 성공했습니다. : {callback}");

				// JSON 데이터 파싱 성공
				try
				{
					LitJson.JsonData gameDataJson = callback.FlattenRows();

					// 받아온 데이터의 개수가 0이면 데이터가 없는 것
					if ( gameDataJson.Count <= 0 )
					{
						Debug.LogWarning("데이터가 존재하지 않습니다.");
					}
					else
					{
						// 불러온 게임 정보의 고유값
						gameDataRowInDate		= gameDataJson[0]["inDate"].ToString();
						// 불러온 게임 정보를 userData 변수에 저장
						userGameData.level		= int.Parse(gameDataJson[0]["level"].ToString());
						userGameData.experience	= float.Parse(gameDataJson[0]["experience"].ToString());
						userGameData.gold		= int.Parse(gameDataJson[0]["gold"].ToString());
						userGameData.jewel		= int.Parse(gameDataJson[0]["jewel"].ToString());
						userGameData.heart		= int.Parse(gameDataJson[0]["heart"].ToString());

						onGameDataLoadEvent?.Invoke();
					}
				}
				// JSON 데이터 파싱 실패
				catch ( System.Exception e )
				{
					// 유저 정보를 초기값으로 설정
					userGameData.Reset();
					// try-catch 에러 출력
					Debug.LogError(e);
				}
			}
			// 실패했을 때
			else
			{
				Debug.LogError($"게임 정보 데이터 불러오기에 실패했습니다. : {callback}");
			}
		});
	}
}

