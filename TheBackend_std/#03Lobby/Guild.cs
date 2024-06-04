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
		// 해당 유저의 UI 오브젝트 삭제
		guildApplicantsPage.Deactivate(gameObject);
		// 해당 유저 길드 가입 요청 수락
		backendGuildSystem.ApproveApplicant(guildMemberData.inDate);
	}

	public void OnClickRejectApplicant()
	{
		// 해당 유저의 UI 오브젝트 삭제
		guildApplicantsPage.Deactivate(gameObject);
		// 해당 유저 길드 가입 요청 거절
		backendGuildSystem.RejectApplicant(guildMemberData.inDate);
	}
}

*/