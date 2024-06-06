using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GuildMemberEdit : MonoBehaviour
{
	[SerializeField]
	private	BackendGuildSystem	backendGuildSystem;
	[SerializeField]
	private	GameObject			overlayBackground;
	[SerializeField]
	private	TextMeshProUGUI		textNickname;
	[SerializeField]
	private	Button				buttonViceMaster;
	[SerializeField]
	private	TextMeshProUGUI		textViceMaster;

	private	GuildMemberData		guildMemberData;

	public void Setup(GuildMemberData memberData)
	{
		guildMemberData		= memberData;
		textNickname.text	= memberData.nickname;

		buttonViceMaster.onClick.RemoveAllListeners();

		if ( guildMemberData.position.Equals("viceMaster") )
		{
			textViceMaster.text = "�ӿ�\n����";
			buttonViceMaster.onClick.AddListener(OnClickReleaseViceMaster);
		}
		else
		{
			textViceMaster.text = "�ӿ�\n�Ӹ�";
			buttonViceMaster.onClick.AddListener(OnClickNominateViceMaster);
		}
	}

	public void OnClickExpelMember()
	{
		backendGuildSystem.ExpelMember(guildMemberData.inDate);
	}

	public void OnClickNominateViceMaster()
	{
		backendGuildSystem.NominateViceMaster(guildMemberData.inDate);
	}

	public void OnClickReleaseViceMaster()
	{
		backendGuildSystem.ReleaseViceMaster(guildMemberData.inDate);
	}

	public void OnClickNominateMaster()
	{
		backendGuildSystem.NominateMaster(guildMemberData.inDate);
	}

	public void SuccessMemberEdit()
	{
		gameObject.SetActive(false);
		overlayBackground.SetActive(false);

		backendGuildSystem.GetMyGuildInfo();
	}
}

