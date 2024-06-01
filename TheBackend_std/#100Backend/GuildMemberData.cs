public class GuildMemberData
{
	public	string	nickname;	// 닉네임
	public	string	inDate;		// 해당 유저의 inDate
	public	string	level;		// 해당 유저의 level

	public override string ToString()
	{
		string result = string.Empty;
		result += $"nickname : {nickname}\n";
		result += $"inDate : {inDate}\n";
		result += $"level : {level}\n";

		return result;
	}
}


/*public class GuildMemberData
{
	public	string	position;	// 직책
	public	string	nickname;	// 닉네임
	public	string	inDate;		// 해당 유저의 inDate
	public	string	level;		// 해당 유저의 level
	public	string	lastLogin;  // 마지막 접속 날짜

	public override string ToString()
	{
		string result = string.Empty;
		result += $"position : {position}\n";
		result += $"nickname : {nickname}\n";
		result += $"inDate : {inDate}\n";
		result += $"level : {level}\n";
		result += $"lastLogin : {lastLogin}\n";

		return result;
	}
}

*/