using UnityEngine;
using TMPro;
using BackEnd;
using System;

public class FriendBase : MonoBehaviour
{
	[Header("Friend Base")]
	[SerializeField]
	private	TextMeshProUGUI	textNickname;	// �г���
	[SerializeField]
	private	TextMeshProUGUI	textTime;		// ����ð�, ���ӽð� ���� �ð� ����

	protected	BackendFriendSystem	backendFriendSystem;
	protected	FriendPageBase		friendPage;
	protected	FriendData			friendData;

	public virtual void Setup(BackendFriendSystem friendSystem, FriendPageBase friendPage, FriendData friendData)
	{
		backendFriendSystem	= friendSystem;
		this.friendPage		= friendPage;
		this.friendData		= friendData;

		textNickname.text	= friendData.nickname;
	}

	public void SetExpirationDate()
	{
		// GetServerTime() - ���� �ð� �ҷ�����
		Backend.Utils.GetServerTime(callback =>
		{
			if ( !callback.IsSuccess() )
			{
				Debug.LogError($"���� �ð� �ҷ����⿡ �����߽��ϴ�. : {callback}");
				return;
			}

			// JSON ������ �Ľ� ����
			try
			{
				// createdAt �ð����κ��� 3�� ���� �ð�
				DateTime after3Days = DateTime.Parse(friendData.createdAt).AddDays(Constants.EXPIRATION_DAYS);
				// ���� ���� �ð�
				string serverTime = callback.GetFlattenJSON()["utcTime"].ToString();
				// ������� ���� �ð� = ���� �ð� - ���� ���� �ð�
				TimeSpan timeSpan = after3Days - DateTime.Parse(serverTime);
			
				// timeSpan.TotalHours�� ���� �Ⱓ�� ��(hour)�� ǥ��
				textTime.text = $"{timeSpan.TotalHours:F0}�ð� ����";
			}
			// JSON ������ �Ľ� ����
			catch ( Exception e )
			{
				// try-catch ���� ���
				Debug.LogError(e);
			}
		});
	}
}

