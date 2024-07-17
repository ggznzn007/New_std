using UnityEngine;

public class MainPanel : MonoBehaviour
{
	public void BtnClickGameStart()
	{
		Utils.LoadScene(SceneNames.Game);
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            BtnClickGameStart();
        }      
    }
}

