using UnityEngine;

public class Meteorite : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;     // ���� ����Ʈ ������
    private GameController gameController;

    public void Setup(GameController gameController)
    {
        this.gameController = gameController;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // ���� ����Ʈ ����
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            // � ����
            Destroy(gameObject);

            gameController.GameOver();
        }
    }
}

