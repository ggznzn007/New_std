using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class Nickname : LoginBase
{
	[System.Serializable]
	public class NicknameEvent : UnityEngine.Events.UnityEvent { }
	public NicknameEvent		onNicknameEvent = new NicknameEvent();

	[SerializeField]
	private	Image				imageNickname;		// �г��� �ʵ� ���� ����
	[SerializeField]
	private	TMP_InputField		inputFieldNickname;	// �г��� �ʵ� �ؽ�Ʈ ���� ����

	[SerializeField]
	private	Button				btnUpdateNickname;  // "�г��� ����" ��ư (��ȣ�ۿ� ����/�Ұ���)

	private void OnEnable()
	{
		// �г��� ���濡 ������ ���� �޽����� ����� ���¿���
		// �г��� ���� �˾��� �ݾҴٰ� �� �� �ֱ� ������ ���¸� �ʱ�ȭ
		ResetUI(imageNickname);
		SetMessage("�г����� �Է��ϼ���");
	}

	public void OnClickUpdateNickname()
	{
		// �Ű������� �Է��� InputField UI�� ����� Message ���� �ʱ�ȭ
		ResetUI(imageNickname);

		// �ʵ� ���� ����ִ��� üũ
		if ( IsFieldDataEmpty(imageNickname, inputFieldNickname.text, "Nickname") )	return;

		// "�г��� ����" ��ư�� ��ȣ�ۿ� ��Ȱ��ȭ
		btnUpdateNickname.interactable = false;
		SetMessage("�г��� �������Դϴ�..");

		// �ڳ� ���� �г��� ���� �õ�
		UpdateNickname();
	}

	private void UpdateNickname()
	{
		// �г��� ����
		Backend.BMember.UpdateNickname(inputFieldNickname.text, callback =>
		{
			// "�г��� ����" ��ư�� ��ȣ�ۿ� Ȱ��ȭ
			btnUpdateNickname.interactable = true;

			// �г��� ���� ����
			if ( callback.IsSuccess() )
			{
				SetMessage($"{inputFieldNickname.text}(��)�� �г����� ����Ǿ����ϴ�.");

				// �г��� ���濡 �������� �� onNicknameEvent�� ��ϵǾ� �ִ� �̺�Ʈ �޼ҵ� ȣ��
				onNicknameEvent?.Invoke();
			}
			// �г��� ���� ����
			else
			{
				string message = string.Empty;

				switch ( int.Parse(callback.GetStatusCode()) )
				{
					case 400:	// �� �г��� Ȥ�� string.Empty, 20�� �̻��� �г���, �г��� ��/�ڿ� ������ �ִ� ���
						message = "�г����� ����ְų� | 20�� �̻� �̰ų� | ��/�ڿ� ������ �ֽ��ϴ�.";
						break;
					case 409:	// �̹� �ߺ��� �г����� �ִ� ���
						message = "�̹� �����ϴ� �г����Դϴ�.";
						break;
					default:
						message = callback.GetMessage();
						break;
				}

				GuideForIncorrectlyEnteredData(imageNickname, message);
			}
		});
	}
}

