using System.Collections;
using UnityEngine;

public class Player_Color : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;            // 이동 속도
    [SerializeField] private float jumpForce = 15;           // 점프하는 힘
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject playerDieEffect;     // 플레이어 사망 효과

    private Rigidbody2D rb2D;                                // 속력제어를 위한 리지드바디2D
    private CircleCollider2D circleCollider2D;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.isKinematic = true;
        /* rb2D.velocity = new (moveSpeed, jumpForce);

         StartCoroutine(UpdateInput());*/
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5);
        float orginY = transform.position.y;
        float deltaY = 0.5f;
        float moveSpeedY = 2;

        while (true)
        {
            float y = orginY + deltaY * Mathf.Sin(Time.time * moveSpeedY);
            transform.position = new Vector2(transform.position.x, y);

            yield return null;
        }        
    }

    public void GameStart()
    {
        rb2D.isKinematic = false;
        rb2D.velocity = new Vector2(moveSpeed,jumpForce);
        GameController.GC.isPlaying = true;
        StopCoroutine(nameof(Start));
        StartCoroutine(nameof(UpdateInput));
    }

    private IEnumerator UpdateInput()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                JumpTo();
            }

            yield return null;
        }
    }

    private void JumpTo()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
    }

    private void ReverseDir()
    {
        float x = -Mathf.Sign(rb2D.velocity.x);
        rb2D.velocity = new(x * moveSpeed, rb2D.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            // 플레이어와 벽의 색상이 다르면 게임오버
            if (collision.GetComponent<SpriteRenderer>().color != spriteRenderer.color)
            {
                PlayerDie();
            }
            else
            {
                AudioManager.AM.PlaySE("Ball");
                // 플레이어 X축 방향 전환
                ReverseDir();
                // 같은 벽에 여러번 충돌하는 것을 방지하기 위해 잠시 충돌을 꺼둠
                StartCoroutine(ColliderOnOffAnimation());
                // 벽과 충돌했을 때 게임컨트롤러에서 처리(벽 추가, 색상 변경 등)
                gameController.CollosionWithWall();
            }
        }
        else if (collision.CompareTag("DeathWall"))
        {
            PlayerDie();
        }
    }

    private IEnumerator ColliderOnOffAnimation()
    {
        circleCollider2D.enabled = false;

        yield return new WaitForSeconds(.1f);

        circleCollider2D.enabled = true;
    }

    private void PlayerDie()
    {
        GameController.GC.isPlaying = false;
        AudioManager.AM.PlaySE("Hit");
        // 플레이어 사망 효과 생성
        Instantiate(playerDieEffect, transform.position, Quaternion.identity);
        // 게임오버 처리
        gameController.GameOver();
        // 플레이어 비활성화
        gameObject.SetActive(false);
    }

  
}
