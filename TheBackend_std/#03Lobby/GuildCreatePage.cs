using UnityEngine;
using TMPro;

public class GuildCreatePage : MonoBehaviour
{
	[SerializeField]
	private	BackendGuildSystem	backendGuildSystem;
	[SerializeField]
	private	TMP_InputField		inputFieldGuildName;

	public void OnClickCreateGuild()
	{
		string guildName = inputFieldGuildName.text;

		if ( guildName.Trim().Equals("") )
		{
			return;
		}

		inputFieldGuildName.text = "";
		
		// 길드 생성
		backendGuildSystem.CreateGuild(guildName);
	}

	public void SuccessCreateGuild()
	{
		gameObject.SetActive(false);
	}
}

