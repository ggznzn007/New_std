using UnityEngine;
using TMPro;

public class PopupUpdateProfileViewer : MonoBehaviour
{
	[SerializeField]
	private	TextMeshProUGUI	textNickname;
	[SerializeField]
	private	TextMeshProUGUI	textGamerID;

	public void UpdateNickname()
	{
		// 닉네임이 없으면 gamer_id를 출력하고, 닉네임이 있으면 닉네임 출력
		textNickname.text = UserInfo.Data.nickname == null ?
							UserInfo.Data.gamerId : UserInfo.Data.nickname;

		// gamer_id 출력
		textGamerID.text = UserInfo.Data.gamerId;
	}
}

