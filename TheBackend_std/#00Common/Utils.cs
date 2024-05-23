using UnityEngine.SceneManagement;

public enum SceneNames { Logo=0, Login, Lobby, }

public static class Utils
{
	public static string GetActiveScene()
	{
		return SceneManager.GetActiveScene().name;
	}

	public static void LoadScene(string sceneName="")
	{
		if ( sceneName == "" )
		{
			SceneManager.LoadScene(GetActiveScene());
		}
		else
		{
			SceneManager.LoadScene(sceneName);
		}
	}

	public static void LoadScene(SceneNames sceneName)
	{
		// SceneNames ���������� �Ű������� �޾ƿ� ��� ToString() ó��
		SceneManager.LoadScene(sceneName.ToString());
	}
}

