public class FriendReceivedRequest : FriendBase
{
	public override void Setup(BackendFriendSystem friendSystem, FriendPageBase friendPage, FriendData friendData)
	{
		base.Setup(friendSystem, friendPage, friendData);
		base.SetExpirationDate();
	}

	public void OnClickAcceptRequest()
	{
		// ģ�� UI ������Ʈ ����
		friendPage.Deactivate(gameObject);
		// ģ�� ��û ���� (Backend Console)
		backendFriendSystem.AcceptFriend(friendData);
	}

	public void OnClickRejectRequest()
	{
		// ģ�� UI ������Ʈ ����
		friendPage.Deactivate(gameObject);
		// ģ�� ��û ���� (Backend Console)
		backendFriendSystem.RejectFriend(friendData);
	}
}

