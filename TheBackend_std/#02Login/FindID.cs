using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class FindID : LoginBase
{
	[SerializeField]
	private	Image				imageEmail;			// E-mail �ʵ� ���� ����
	[SerializeField]
	private	TMP_InputField		inputFieldEmail;	// E-mail �ʵ� �ؽ�Ʈ ���� ����

	[SerializeField]
	private	Button				btnFindID;			// "���̵� ã��" ��ư (��ȣ�ۿ� ����/�Ұ���)

	public void OnClickFindID()
	{
		// �Ű������� �Է��� InputField UI�� ����� Message ���� �ʱ�ȭ
		ResetUI(imageEmail);

		// �ʵ� ���� ����ִ��� üũ
		if ( IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "���� �ּ�") )	return;

		// ���� ���� �˻�
		if ( !inputFieldEmail.text.Contains("@") )
		{
			GuideForIncorrectlyEnteredData(imageEmail, "���� ������ �߸��Ǿ����ϴ�.(ex. address@xx.xx)");
			return;
		}

		// "���̵� ã��" ��ư�� ��ȣ�ۿ� ��Ȱ��ȭ
		btnFindID.interactable = false;
		SetMessage("���� �߼����Դϴ�.");

		// �ڳ� ���� ���̵� ã�� �õ�
		FindCustomID();
	}

	/// <summary>
	/// ���̵� ã�⸦ ���� �̸��� �߼� �õ� �� �����κ��� ���޹��� message�� ������� ���� ó��
	/// </summary>
	private void FindCustomID()
	{
		// ���̵� ������ �̸��Ϸ� �߼�
		Backend.BMember.FindCustomID(inputFieldEmail.text, callback =>
		{
			// "���̵� ã��" ��ư ��ȣ�ۿ� Ȱ��ȭ
			btnFindID.interactable = true;

			// ���� �߼� ����
			if ( callback.IsSuccess() )
			{
				SetMessage($"{inputFieldEmail.text} �ּҷ� ������ �߼��Ͽ����ϴ�.");
			}
			// ���� �߼� ����
			else
			{
				string message = string.Empty;

				switch ( int.Parse(callback.GetStatusCode()) )
				{
					case 404:	// �ش� �̸����� ���̸Ӱ� ���� ���
						message = "�ش� �̸����� ����ϴ� ����ڰ� �����ϴ�.";
						break;
					case 429:	// 24�ð� �̳��� 5ȸ �̻� ���� �̸��� ������ ���̵�/��й�ȣ ã�⸦ �õ��� ���
						message = "24�ð� �̳��� 5ȸ �̻� ���̵�/��й�ȣ ã�⸦ �õ��߽��ϴ�.";
						break;
					default:
						// statusCode : 400 => ������Ʈ �� Ư�����ڰ� �߰��� ��� (�ȳ� ���� �̹߼� �� ���� �߻�)
						message = callback.GetMessage();
						break;
				}

				if ( message.Contains("�̸���") )
				{
					GuideForIncorrectlyEnteredData(imageEmail, message);
				}
				else
				{
					SetMessage(message);
				}
			}
		});
	}
}

