public class FriendPage : FriendPageBase
{
	private void OnEnable()
	{
		// [ģ��] ��� �ҷ�����
		backendFriendSystem.GetFriendList();
	}

	private void OnDisable()
	{
		DeactivateAll();
	}
}

