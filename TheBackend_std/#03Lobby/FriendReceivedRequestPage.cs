public class FriendReceivedRequestPage : FriendPageBase
{
	private void OnEnable()
	{
		// [ģ�� ���� ���] ��� �ҷ�����
		backendFriendSystem.GetReceivedRequestList();
	}

	private void OnDisable()
	{
		DeactivateAll();
	}
}

