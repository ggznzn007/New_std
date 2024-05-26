using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int scorePoint = 100;       // ���� óġ���� �� ȹ���ϴ� ����
    [SerializeField]
    private GameObject explosionPrefab;     // ���� ����Ʈ ������
    private GameController gameController;

    public void Setup(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void OnDie()
    {
        // ���� ����Ʈ ����
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        // �÷��̾��� ������ scorePoint��ŭ ����
        gameController.Score += scorePoint;
        // �� ĳ���� ����
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnDie();
            gameController.GameOver();
        }
    }
}

