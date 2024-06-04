using UnityEngine;
using TMPro;

public class Notice : MonoBehaviour
{
	[SerializeField]
	private	BackendGuildSystem	backendGuildSystem;
	[SerializeField]
	private	GameObject			noticeBackground;		// �������� ���̴� ������Ʈ
	[SerializeField]
	private	TextMeshProUGUI		textNotice;				// �������� ���̴� ��������
	[SerializeField]
	private	TMP_InputField		inputFieldNotice;		// ��帶���Ϳ��� ���̴� �������� ������Ʈ
	[SerializeField]
	private	FadeEffect_TMP		textLog;

	public void Setup(bool isMaster=false, bool isOtherGuild=false)
	{
		if ( isOtherGuild == true )
		{
			textNotice.text			= backendGuildSystem.otherGuildData.notice;
			inputFieldNotice.text	= backendGuildSystem.otherGuildData.notice;
		}
		else
		{
			textNotice.text			= backendGuildSystem.myGuildData.notice;
			inputFieldNotice.text	= backendGuildSystem.myGuildData.notice;
		}

		noticeBackground.SetActive(!isMaster);
		inputFieldNotice.gameObject.SetActive(isMaster);
	}

	public void OnClickNoticeRegistration()
	{
		string notice = inputFieldNotice.text;

		if ( notice.Trim().Equals("") )
		{
			textLog.FadeOut("�������� ������ ����ֽ��ϴ�.");
			return;
		}

		backendGuildSystem.SetNotice(notice);
	}
}

