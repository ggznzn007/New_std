using UnityEngine;
using TMPro;

public class GuildPage : MonoBehaviour
{
	[SerializeField]
	private	BackendGuildSystem	backendGuildSystem;
	[SerializeField]
	private	TextMeshProUGUI		textGuildName;				// Popup ��ܿ� ��µǴ� ��� �̸� Text UI

	private	string				guildName = string.Empty;	// ��� �̸�

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

