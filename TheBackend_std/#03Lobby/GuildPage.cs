using UnityEngine;
using TMPro;

public class GuildPage : MonoBehaviour
{
	[SerializeField]
	private	BackendGuildSystem	backendGuildSystem;
	[SerializeField]
	private	TextMeshProUGUI		textGuildName;				// Popup 상단에 출력되는 길드 이름 Text UI

	private	string				guildName = string.Empty;	// 길드 이름

	public void Activate(string guildName)
	{
		gameObject.SetActive(true);

		textGuildName.text	= guildName;
		this.guildName		= guildName;
	}

	public void OnClickApplyGuild()
	{
		backendGuildSystem.ApplyGuild(guildName);
	}
}

