using UnityEngine;

public class LogoScenario : MonoBehaviour
{
	[SerializeField]
	private	Progress	progress;
	[SerializeField]
	private	SceneNames	nextScene;

	private void Awake()
	{
		SystemSetup();
	}

	private void SystemSetup()
	{
		// Ȱ��ȭ���� ���� ���¿����� ������ ��� ����
		Application.runInBackground = true;

		// �ػ� ���� (9:18.5, 1440x2960, ������ ��Ʈ 8)
		int width	= Screen.width;
		int height	= (int)(Screen.width * 18.5f / 9);
		Screen.SetResolution(width, height, true);

		// ȭ���� ������ �ʵ��� ����
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		// �ε� �ִϸ��̼� ����, ��� �Ϸ�� OnAfterProgress() �޼ҵ� ����
		progress.Play(OnAfterProgress);
	}

	private void OnAfterProgress()
	{
		Utils.LoadScene(nextScene);
	}
}

