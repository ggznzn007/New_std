public class FriendData
{
	public	string	nickname;	// �г���
	public	string	inDate;		// �ش� ������ inDate
	public	string	createdAt;	// ģ�� ��û ���� �ð� / ģ�� ��û ���� �ð� / ģ���� �� �ð�

	public override string ToString()
	{
		string result = string.Empty;
		result += $"�г��� : {nickname}\n";
		result += $"inDate : {inDate}\n";
		result += $"createdAt : {createdAt}\n";

		return result;
	}
}

