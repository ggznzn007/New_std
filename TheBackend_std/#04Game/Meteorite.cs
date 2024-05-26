using UnityEngine;

public class Meteorite : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;     // 气惯 捞棋飘 橇府普
    private GameController gameController;

    public void Setup(GameController gameController)
    {
        this.gameController = gameController;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 气惯 捞棋飘 积己
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            // 款籍 昏力
            Destroy(gameObject);

            gameController.GameOver();
        }
    }
}

