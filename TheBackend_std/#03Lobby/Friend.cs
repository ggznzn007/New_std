using UnityEngine;
using TMPro;

public class Friend : FriendBase
{
	[SerializeField]
	private	TextMeshProUGUI	textLevel;

	public override void Setup(BackendFriendSystem friendSystem, FriendPageBase friendPage, FriendData friendData)
	{
		base.Setup(friendSystem, friendPage, friendData);

		textLevel.text	= friendData.level;
		textTime.text	= System.DateTime.Parse(friendData.lastLogin).ToString();
	}

	public void OnClickDeleteFriend()
	{
		// 模备 UI 坷宏璃飘 昏力
		friendPage.Deactivate(gameObject);
		// 模备 昏力 (Backend Console)
		backendFriendSystem.BreakFriend(friendData);
	}
}

