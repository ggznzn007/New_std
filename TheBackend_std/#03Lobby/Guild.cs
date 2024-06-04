using UnityEngine;
using TMPro;

public class Guild : MonoBehaviour
{
	[SerializeField]
	private	TextMeshProUGUI		textGuildName;
	[SerializeField]
	private	TextMeshProUGUI		textMasterNickname;
	[SerializeField]
	private	TextMeshProUGUI		textMemberCount;

	private	BackendGuildSystem	backendGuildSystem;
	private	GuildData			guildData;

	public void Setup(BackendGuildSystem guildSystem, GuildData guildData)
	{
		backendGuildSystem		= guildSystem;
		this.guildData			= guildData;

		textGuildName.text		= guildData.guildName;
		textMasterNickname.text	= guildData.master.nickname;
		textMemberCount.text	= $"{guildData.memberCount}/100";
	}

	public void OnClickGuildInfo()
	{
		backendGuildSystem.GetGuildInfo(guildData.guildInDate);
	}
}


/*
	public void OnClickApproveApplicant()
	{
		// �ش� ������ UI ������Ʈ ����
		guildApplicantsPage.Deactivate(gameObject);
		// �ش� ���� ��� ���� ��û ����
		backendGuildSystem.ApproveApplicant(guildMemberData.inDate);
	}

	public void OnClickRejectApplicant()
	{
		// �ش� ������ UI ������Ʈ ����
		guildApplicantsPage.Deactivate(gameObject);
		// �ش� ���� ��� ���� ��û ����
		backendGuildSystem.RejectApplicant(guildMemberData.inDate);
	}
}

*/