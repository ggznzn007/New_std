using UnityEngine;
using TMPro;

public class FriendSentRequestPage : FriendPageBase
{
	[Header("Send Request Friend")]
	[SerializeField]
	private	TMP_InputField		inputFieldNickname;
	[SerializeField]
	private	FadeEffect_TMP		textResult;

	private void OnEnable()
	{
		// [ģ�� ��û ���] ��� �ҷ�����
		backendFriendSystem.GetSentRequestList();
	}

	private void OnDisable()
	{
		DeactivateAll();
	}

	public void OnClickRequestFriend()
	{
		string nickname = inputFieldNickname.text;

		if ( nickname.Trim().Equals("") )
		{
			textResult.FadeOut("ģ�� ��û�� ���� �г����� �Է����ּ���.");
			return;
		}

		inputFieldNickname.text = "";

		backendFriendSystem.SendRequestFriend(nickname);
	}
}

