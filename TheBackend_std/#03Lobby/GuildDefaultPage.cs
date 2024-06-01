using UnityEngine;
using TMPro;

public class GuildDefaultPage : MonoBehaviour
{
	[SerializeField]
	private	BackendGuildSystem	backendGuildSystem;
	[SerializeField]
	private	TMP_InputField		inputFieldGuildName;
	[SerializeField]
	private	GuildPage			guildPage;
	[SerializeField]
	private	FadeEffect_TMP		textLog;

	public void OnClickSearchGuild()
	{
		string guildName = inputFieldGuildName.text;

		if ( guildName.Trim().Equals("") )
		{
			return;
		}

		inputFieldGuildName.text = "";

		string inDate = backendGuildSystem.GetGuildInfoBy(guildName);

		if ( inDate.Equals(string.Empty) )
		{
			textLog.FadeOut("존재하지 않는 길드입니다.");
		}
		else
		{
			// 길드 팝업 페이지 활성화
			guildPage.Activate(guildName);
		}
	}
}

