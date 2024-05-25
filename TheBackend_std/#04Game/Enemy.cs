using UnityEngine;

public class Enemy : MonoBehaviour
{
	private	GameController	gameController;

	public void Setup(GameController gameController)
	{
		this.gameController = gameController;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ( collision.CompareTag("Player") )
		{
			gameController.GameOver();
		}
	}
}

