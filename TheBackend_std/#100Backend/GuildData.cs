using System.Collections.Generic;

public class GuildData
{
	public	string					guildName;		// ��� �̸�
	public	string					guildInDate;	// ��� InDate
	public	string					notice;			// ��� �������� (��Ÿ ������)
	public	int						memberCount;	// ��� ȸ�� ��
	public	GuildMemberData			master;			// ��� ������
	public	List<GuildMemberData>	viceMasterList;	// �α�� ������ ���
}

