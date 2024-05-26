using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int scorePoint = 100;       // 적을 처치했을 때 획득하는 점수
    [SerializeField]
    private GameObject explosionPrefab;     // 폭발 이펙트 프리팹
    private GameController gameController;

    public void Setup(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void OnDie()
    {
        // 폭발 이펙트 생성
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        // 플레이어의 점수를 scorePoint만큼 증가
        gameController.Score += scorePoint;
        // 적 캐릭터 삭제
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

