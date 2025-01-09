using System.Collections.Generic;

public class PostData
{
	public	string	title;						// 우편 제목
	public	string	content;					// 우편 내용
	public	string	inDate;						// 우편 inDate
	public	string	expirationDate;				// 우편 만료 날짜

	public	bool	isCanReceive = false;		// 우편에 받을 수 있는 아이템이 있는지 여부

	// <아이템 이름, 아이템 개수>
	public	Dictionary<string, int>	postReward = new Dictionary<string, int>();

	// 우편 정보를 Debug.Log()로 출력하기 위한 메소드
	public override string ToString()
	{
		string result = string.Empty;
		result += $"title : {title}\n";
		result += $"content : {content}\n";
		result += $"inDate : {inDate}\n";

		if ( isCanReceive )
		{
			result += "우편 아이템\n";

			foreach ( string itemKey in postReward.Keys )
			{
				result += $"| {itemKey} : {postReward[itemKey]}개\n";
			}
		}

		else
		{
			result += "지원하지 않는 아이템입니다.";
		}

		return result;
	}
}

