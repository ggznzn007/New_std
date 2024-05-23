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
		// 활성화되지 않은 상태에서도 게임이 계속 진행
		Application.runInBackground = true;

		// 해상도 설정 (9:18.5, 1440x2960, 갤럭시 노트 8)
		int width	= Screen.width;
		int height	= (int)(Screen.width * 18.5f / 9);
		Screen.SetResolution(width, height, true);

		// 화면이 꺼지지 않도록 설정
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		// 로딩 애니메이션 시작, 재생 완료시 OnAfterProgress() 메소드 실행
		progress.Play(OnAfterProgress);
	}

	private void OnAfterProgress()
	{
		Utils.LoadScene(nextScene);
	}
}

