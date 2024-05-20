using System.Collections;
using UnityEngine;

public class Player_Color : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;            // �̵� �ӵ�
    [SerializeField] private float jumpForce = 15;           // �����ϴ� ��
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject playerDieEffect;     // �÷��̾� ��� ȿ��

    private Rigidbody2D rb2D;                                // �ӷ���� ���� ������ٵ�2D
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
            // �÷��̾�� ���� ������ �ٸ��� ���ӿ���
            if (collision.GetComponent<SpriteRenderer>().color != spriteRenderer.color)
            {
                PlayerDie();
            }
            else
            {
                AudioManager.AM.PlaySE("Ball");
                // �÷��̾� X�� ���� ��ȯ
                ReverseDir();
                // ���� ���� ������ �浹�ϴ� ���� �����ϱ� ���� ��� �浹�� ����
                StartCoroutine(ColliderOnOffAnimation());
                // ���� �浹���� �� ������Ʈ�ѷ����� ó��(�� �߰�, ���� ���� ��)
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
        // �÷��̾� ��� ȿ�� ����
        Instantiate(playerDieEffect, transform.position, Quaternion.identity);
        // ���ӿ��� ó��
        gameController.GameOver();
        // �÷��̾� ��Ȱ��ȭ
        gameObject.SetActive(false);
    }

  
}
