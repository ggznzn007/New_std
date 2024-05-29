public class FriendSentRequest : FriendBase
{
	public override void Setup(BackendFriendSystem friendSystem, FriendPageBase friendPage, FriendData friendData)
	{
		base.Setup(friendSystem, friendPage, friendData);
		base.SetExpirationDate();
	}

	public void OnClickCancelRequest()
	{
		// ģ�� UI ������Ʈ ��Ȱ��ȭ
		friendPage.Deactivate(gameObject);
		// ģ�� ��û ���
		backendFriendSystem.RevokeSentRequest(friendData.inDate);
	}
}

