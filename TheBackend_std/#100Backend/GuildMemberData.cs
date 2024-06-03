public class GuildMemberData
{
    public string nickname; // �г���
    public string inDate;       // �ش� ������ inDate
    public string level;        // �ش� ������ level
    public string position; // ��� ��å
    public int goodsCount;  // ���� ����
    public string lastLogin;    // ������ ���� ��¥

    public override string ToString()
    {
        string result = string.Empty;
        result += $"nickname : {nickname}\n";
        result += $"inDate : {inDate}\n";
        result += $"level : {level}\n";
        result += $"position : {position}\n";
        result += $"goodsCount : {goodsCount}\n";
        result += $"lastLogin : {lastLogin}\n";

        return result;
    }
}


/*public class GuildMemberData
{
	public	string	position;	// ��å
	public	string	nickname;	// �г���
	public	string	inDate;		// �ش� ������ inDate
	public	string	level;		// �ش� ������ level
	public	string	lastLogin;  // ������ ���� ��¥

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